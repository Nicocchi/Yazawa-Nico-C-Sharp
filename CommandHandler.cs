using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Timers;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using DiscordBotsList.Api;
using Yazawa_Nico.Core.LevelingSystem;
using Yazawa_Nico.Core.UserProfiles;
using Yazawa_Nico.Core.GuildAccounts;

namespace Yazawa_Nico
{
    public class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
            _client.UserJoined += AnnounceUserJoined;
            _client.UserLeft += AnnounceUserLeave;
            _client.Ready += SetGame;
            // _client.Ready += DBLGet;

            // Timer timer = new Timer(30 * 60 * 1000);
            // timer.Enabled = true;
            // timer.Elapsed += OnTick;
            //timer.Start();
            
        }

        public async Task SetGame()
        {
            await _client.SetGameAsync("!help | Nico Nico Nii~");
        }

        // Update the bot shard and guild count for DiscordBotList
        public async Task DBLGet()
        {
            AuthDiscordBotListApi DblApi = new AuthDiscordBotListApi(Config.bot.selfID, Config.bot.dblToken);
            IDblSelfBot me = await DblApi.GetMeAsync();
            // Update stats sharded   indexShard shardCount shards
            //await me.UpdateStatsAsync(24,        50,        new[] { 12, 421, 62, 241, 524, 534 });

            // Update stats           guildCount
            await me.UpdateStatsAsync(_client.Guilds.Count);
        }

        private async void OnTick(object source, ElapsedEventArgs e)
        { 
            await DBLGet();
        }

        public async Task AnnounceUserJoined(IGuildUser user)
        {
            var guildProfile = GuildProfiles.GetAccount((SocketGuild)user.Guild); // Guild Profile

            if (guildProfile.GreetModule)
            {
                ulong channelId = (guildProfile.GreetChannel); 
                var channel = _client.GetChannel(channelId) as SocketTextChannel; /* Set the channel ID to the database's channel ID */
                var gMsg = guildProfile.GreetMessage;
                string greetMes = gMsg.Replace("@user", user.Mention);
                await channel.SendMessageAsync($"{greetMes}"); //Welcomes the new user
            }
            
        }

        public async Task AnnounceUserLeave(IGuildUser user)
        {
            var guildProfile = GuildProfiles.GetAccount((SocketGuild)user.Guild); // Guild Profile

            if (guildProfile.LeaveModule)
            {
                ulong channelId = (guildProfile.LeaveChannel); 
                var channel = _client.GetChannel(channelId) as SocketTextChannel; /* Set the channel ID to the database's channel ID */
                var lMsg = guildProfile.LeaveMessage;
                string leaveMes = lMsg.Replace("@user", user.Username);
                await channel.SendMessageAsync($"{leaveMes}"); //Welcomes the new user
            }
            
        }

        // Happens when the bot receives a message
        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage; // Covnert s to a user message
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            if (context.User.IsBot) return;

            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            int argPos = 0; // Gets rid of the prefix at which position the prefix ends and the msg begins
            if (!File.Exists("Resources/guildProfiles.json"))
            {
                if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    var result = await _service.ExecuteAsync(context, argPos); // Executes the commands
                    // Check the result
                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }
            }
            else
            {
                if (msg.HasStringPrefix(guildProfile.CmdPrefix, ref argPos)
                || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    var result = await _service.ExecuteAsync(context, argPos); // Executes the commands
                    // Check the result
                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }
            }
            
            if (context.Guild.CurrentUser.GetPermissions(context.Guild.GetChannel(context.Channel.Id)).SendMessages == true)
            {
                //Console.WriteLine(context.Guild.CurrentUser.Username);
                

                // Mute check
                var userProfile = UserProfiles.GetAccount(context.User);
                if (userProfile.IsMuted)
                {
                    await context.Message.DeleteAsync();
                    return;
                }

                // Leveling up
                Leveling.UserSentMessage((SocketGuildUser)context.User, (SocketTextChannel)context.Channel);

                // RPS Game
                #region RPS
                if (userProfile.IsRPS)
                {
                    Random rnd = new Random();
                    int randomInt = rnd.Next(1, 4);
                    string userAns = msg.Content.ToLower();

                    switch (randomInt)
                    {
                        case 1:
                            // CPU chose rock
                            if (userAns.Contains($"rock"))
                            {
                                await msg.Channel.SendMessageAsync("You chose rock. I chose rock.\nHey! We came to a draw!\n\n**╮( ˘ ､ ˘ )╭**");
                                if (userProfile.IsRPSGamble)
                                {
                                    userProfile.Points += userProfile.GambleAmount / 2;
                                    await msg.Channel.SendMessageAsync($"You gained {userProfile.GambleAmount / 2} Nico Nii's");
                                }

                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            else if (userAns.Contains($"paper"))
                            {
                                await msg.Channel.SendMessageAsync("You chose paper. I chose rock.\nAww, you won!\n\n**｡ﾟ･ (>﹏<) ･ﾟ｡**");
                                if (userProfile.IsRPSGamble)
                                {
                                    userProfile.Points += userProfile.GambleAmount * 2;
                                    await msg.Channel.SendMessageAsync($"You gained {userProfile.GambleAmount * 2} Nico Nii's");
                                }
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            else if (userAns.Contains($"scissors"))
                            {
                                await msg.Channel.SendMessageAsync("You chose scissors. I chose rock.\nYay! I won!\n\n**(๑˃ᴗ˂)ﻭ**");
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            break;
                        case 2:
                            // CPU chose paper
                            if (userAns.Contains($"paper"))
                            {
                                await msg.Channel.SendMessageAsync("You chose paper. I chose paper.\nHey! We came to a draw!\n\n**╮( ˘ ､ ˘ )╭**");
                                if (userProfile.IsRPSGamble)
                                {
                                    userProfile.Points += userProfile.GambleAmount / 2;
                                    await msg.Channel.SendMessageAsync($"You gained {userProfile.GambleAmount / 2} Nico Nii's");
                                }
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            else if (userAns.Contains($"rock"))
                            {
                                await msg.Channel.SendMessageAsync("You chose rock. I chose paper.\nYay! I won!\n\n**(๑˃ᴗ˂)ﻭ**");
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            else if (userAns.Contains($"scissors"))
                            {
                                await msg.Channel.SendMessageAsync("You chose scissors. I chose paper.\nAww, you won!\n\n**｡ﾟ･ (>﹏<) ･ﾟ｡**");
                                if (userProfile.IsRPSGamble)
                                {
                                    userProfile.Points += userProfile.GambleAmount * 2;
                                    await msg.Channel.SendMessageAsync($"You gained {userProfile.GambleAmount * 2} Nico Nii's");
                                }
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            break;
                        case 3:
                            // CPU chose scissors
                            if (userAns.Contains($"scissors"))
                            {
                                await msg.Channel.SendMessageAsync("You chose scissors. I chose scissors.\nHey! We came to a draw!\n\n**╮( ˘ ､ ˘ )╭**");
                                if (userProfile.IsRPSGamble)
                                {
                                    userProfile.Points += userProfile.GambleAmount / 2;
                                    await msg.Channel.SendMessageAsync($"You gained {userProfile.GambleAmount / 2} Nico Nii's");
                                }
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            else if (userAns.Contains($"rock"))
                            {
                                await msg.Channel.SendMessageAsync("You chose rock. I chose scissors.\nAww, you won!\n\n**｡ﾟ･ (>﹏<) ･ﾟ｡**");
                                if (userProfile.IsRPSGamble)
                                {
                                    userProfile.Points += userProfile.GambleAmount * 2;
                                    await msg.Channel.SendMessageAsync($"You gained {userProfile.GambleAmount * 2} Nico Nii's");
                                }
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            else if (userAns.Contains($"paper"))
                            {
                                await msg.Channel.SendMessageAsync("You chose paper. I chose scissors.\nYay! I won!\n\n**(๑˃ᴗ˂)ﻭ**");
                                userProfile.IsRPS = false;
                                userProfile.IsRPSGamble = false;
                                userProfile.GambleAmount = 0;
                                UserProfiles.SaveProfiles();
                            }
                            break;
                        default:

                            break;
                    }
                }
                #endregion

                if (context.Guild.CurrentUser.GetPermissions(context.Guild.GetChannel(context.Channel.Id)).EmbedLinks == true)
                {
                    // AFK
                    #region AFK
                    if (guildProfile.AfkModule)
                    {
                        // Set the user back from being AFK if they are AFK
                        if (!s.Author.IsBot && !s.Content.Contains($"{guildProfile.CmdPrefix}afk"))
                        {
                            var afkUser = UserProfiles.GetAccount(s.Author);
                            if (s.Author.Id == afkUser.ID)
                            {
                                if (afkUser.Afk)
                                {
                                    afkUser.Afk = false;
                                    afkUser.AfkMessage = "";
                                    UserProfiles.SaveProfiles();

                                    var embed = new EmbedBuilder();
                                    embed.Color = new Color(222, 77, 156);
                                    embed.Title = $"You are now set as back **{s.Author.Username}**!";
                                    await s.Channel.SendMessageAsync("", embed: embed.Build());
                                }
                            }
                        }

                        // If the user pings an AFK person, give them a notice
                        var userMentioned = s.MentionedUsers.FirstOrDefault();
                        if (userMentioned != null)
                        {
                            var afkUser = UserProfiles.GetAccount(userMentioned); // Get the mentioned user
                            string avatarUrl = userMentioned.GetAvatarUrl();

                            if (afkUser.Afk)
                            {
                                var embed = new EmbedBuilder();
                                embed.Color = new Color(222, 77, 156);
                                embed.Title = $"AFK";
                                embed.WithDescription($"**{userMentioned.Username}** is currently away. They left this message:\n\n **{afkUser.AfkMessage}**");
                                embed.WithThumbnailUrl(avatarUrl);
                                await s.Channel.SendMessageAsync("", embed: embed.Build());
                            }
                        }
                    }
                    #endregion
                }

                
            }
            
        }
    }
}