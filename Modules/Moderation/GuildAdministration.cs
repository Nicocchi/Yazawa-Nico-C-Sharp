using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Web;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;
using Newtonsoft.Json;
using Yazawa_Nico.Core.UserProfiles;
using Yazawa_Nico.Core.GuildAccounts;

namespace Yazawa_Nico.Modules.Moderation
{
    public class GuildAdministration : ModuleBase<SocketCommandContext>
    {
        public static WarnConfig conf;

        // Set Command prefix
        [Command("setprefix")]
        [Summary("setprefix | Sets a new prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task SetPrefix(string prefix)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            guildProfile.CmdPrefix = prefix;
            GuildProfiles.SaveProfiles();

            await Context.Channel.SendMessageAsync($"Command prefix has been set to `{prefix}`");
        }

        // Ban a user
        [Command("ban")]
        [Summary("ban <user> | Bans a user. Requires User Administrator & Ban privileges. Requires Bot Ban privileges.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task BanUser(IGuildUser user, [Remainder]string reason = "No reason provided.")
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.BanModule)
            {
                string avatarUrl = user.GetAvatarUrl();

                // Display embed
                var embed = new EmbedBuilder();
                embed.WithTitle("BAN");
                embed.WithColor(255,73,167);
                embed.WithDescription($"**{user.Username}#{user.Discriminator}** has been banned from **{Context.Guild.Name}** with the reason:\n\n **{reason}**");
                embed.WithThumbnailUrl(avatarUrl);
                embed.WithFooter(footer =>
                {
                    footer
                        .WithText($"Banned by {Context.User.Username}#{Context.User.Discriminator} on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToShortTimeString()}")
                        .WithIconUrl(Context.Client.GetUser(Context.User.Id).GetAvatarUrl());
                });
                await Context.Channel.SendMessageAsync("", embed: embed.Build());

                // Send Direct Message to user stating he/she has been kicked
                var dmChannel = await user.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync($"You have been banned from **{Context.Guild.Name}** with the reason:\n\n **{reason}**");

                // Ban the user
                await user.Guild.AddBanAsync(user, 7, reason);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Ban Module has not been enabled yet.");
            }
            
        }

        // Kick a user
        [Command("kick")]
        [Summary("kick <user> | Kicks a user. Requires User Administrator & Kick privileges. Requires Bot Kick privileges.")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task KickUser(IGuildUser user, [Remainder]string reason = "No reason provided.")
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.KickModule)
            {
                string avatarUrl = user.GetAvatarUrl();
                string dateTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();

                // Display embed
                var embed = new EmbedBuilder();
                embed.WithTitle("KICK");
                embed.WithColor(255,73,167);
                embed.WithDescription($"**{user.Username}#{user.Discriminator}** has been kicked from **{Context.Guild.Name}** with the reason:\n\n **{reason}**");
                embed.WithThumbnailUrl(avatarUrl);
                embed.WithFooter(footer =>
                {
                    footer
                        .WithText($"Kicked by {Context.User.Username}#{Context.User.Discriminator} on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToShortTimeString()}")
                        .WithIconUrl(Context.Client.GetUser(Context.User.Id).GetAvatarUrl());
                });
                await Context.Channel.SendMessageAsync("", embed: embed.Build());

                // Send Direct Message to user stating he/she has been kicked
                var dmChannel = await user.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync($"You have been kicked from **{Context.Guild.Name}** with the reason:\n\n **{reason}**");

                // Kick the user
                await user.KickAsync(reason);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Kick Module has not been enabled yet.");
            }
            
        }

        // Warn a user for [x] amount of times before invoking kick/ban
        [Command("warn")]
        [Summary("warn <user> [reason] | Warns a user with a reason. Requires User Administrator privileges. Requires Bot Ban privileges.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnUser(IGuildUser user, [Remainder] string reason = "No Reason")
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;

            string guildId = Context.Guild.Id.ToString(); // Guild ID

            string warnFolder = "Resources"; // Folder to place the warnFile inside of
            string warnFile = $"{guildId}warn.json"; // Warning file : guildId + warn.json

            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile
            
            
            if(!Directory.Exists(warnFolder)) Directory.CreateDirectory(warnFolder);
            // warnFile does not exist, create it and add in the userId and 1 warning to the user.
            // If warnFile does exist, load the user's warnings and and 1 warning to the existing warning.
            if (!File.Exists(warnFolder + "/" + warnFile))
            {
                conf.UserId = user.Id;
                conf.NumberOfWarnings = 1;
                conf.WarningMessage = reason;
                conf.UserUsername = user.Username;
                conf.ModUsername = Context.User.Username;

                string json = JsonConvert.SerializeObject(conf, Formatting.Indented);
                File.WriteAllText(warnFolder + "/" + warnFile, json);
            }
            else
            {
                ulong currentGuild = Context.Guild.Id;

                string json = File.ReadAllText(warnFolder + "/" + warnFile);
                conf = JsonConvert.DeserializeObject<WarnConfig>(json);

                conf.UserId = user.Id;
                conf.NumberOfWarnings++;
                conf.WarningMessage = reason;
                conf.UserUsername = user.Username;
                conf.ModUsername = Context.User.Username;

                string json1 = JsonConvert.SerializeObject(conf, Formatting.Indented);
                File.WriteAllText(warnFolder + "/" + warnFile, json1);
            }

            var dmChannel = await user.GetOrCreateDMChannelAsync();
            await dmChannel.SendMessageAsync($"You have been warned in **{Context.Guild.Name}** with the reason:\n\n **{reason}**\n\n This makes {conf.NumberOfWarnings} warning(s).");
            await Context.Channel.SendMessageAsync($":white_check_mark: {user.Username}#{user.Discriminator} has been warned.");

            // Check warnings
            if (conf.NumberOfWarnings == guildProfile.NumberofWarnings)
            {
                if (guildProfile.WarningsBan) 
                {
                    await dmChannel.SendMessageAsync($"You have been banned from **{Context.Guild.Name}** with the reason:\n\n Warned **{conf.NumberOfWarnings}** time(s)");
                    await user.Guild.AddBanAsync(user, 7, $"Banned for having {guildProfile.NumberofWarnings} warning(s)");
                    conf.NumberOfWarnings = 0;
                }
                else if (guildProfile.WarningsKick) 
                {
                    await dmChannel.SendMessageAsync($"You have been kicked from **{Context.Guild.Name}** with the reason:\n\n Warned **{conf.NumberOfWarnings}** time(s)");
                    await user.KickAsync("reason");
                    conf.NumberOfWarnings = 0;
                }
                else if (guildProfile.WarningsMute) 
                {
                    // Mute
                }
                else { }
            }

            string json2 = JsonConvert.SerializeObject(conf, Formatting.Indented);
            File.WriteAllText(warnFolder + "/" + warnFile, json2);
        }

        // Clear a warning
        [Command("clearwarn")]
        [Summary("clearwarn <user> | Clears a warning off a user. Requires User Administrator privileges.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task ClearWarning(IGuildUser user)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;

            string guildId = Context.Guild.Id.ToString(); // Guild ID

            string warnFolder = "Resources"; // Folder to place the warnFile inside of
            string warnFile = $"{guildId}warn.json"; // Warning file : guildId + warn.json

            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if(!Directory.Exists(warnFolder)) Directory.CreateDirectory(warnFolder);
            // warnFile does not exist, create it and add in the userId and 1 warning to the user.
            // If warnFile does exist, load the user's warnings and and 1 warning to the existing warning.
            if (!File.Exists(warnFolder + "/" + warnFile))
            {
                conf.UserId = user.Id;
                conf.NumberOfWarnings = 0;
                conf.WarningMessage = "";
                conf.UserUsername = user.Username;
                conf.ModUsername = Context.User.Username;

                string json = JsonConvert.SerializeObject(conf, Formatting.Indented);
                File.WriteAllText(warnFolder + "/" + warnFile, json);
            }
            else
            {
                ulong currentGuild = Context.Guild.Id;

                string json = File.ReadAllText(warnFolder + "/" + warnFile);
                conf = JsonConvert.DeserializeObject<WarnConfig>(json);

                conf.UserId = user.Id;
                if (conf.NumberOfWarnings <= 0) conf.NumberOfWarnings = 0;
                else { conf.NumberOfWarnings--; }
                conf.WarningMessage = "";
                conf.UserUsername = user.Username;
                conf.ModUsername = Context.User.Username;

                string json1 = JsonConvert.SerializeObject(conf, Formatting.Indented);
                File.WriteAllText(warnFolder + "/" + warnFile, json1);
            }

            await Context.Channel.SendMessageAsync($":white_check_mark: {user.Username}#{user.Discriminator} warning has been cleared. He now has {conf.NumberOfWarnings} warning(s).");
        }

        /*
        [Command("warnings")]
        [Summary("View all warnings")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AllWarnings()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;

            string guildId = Context.Guild.Id.ToString(); // Guild ID

            string warnFolder = "Resources"; // Folder to place the warnFile inside of
            string warnFile = $"{guildId}warn.json"; // Warning file : guildId + warn.json

            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if(!Directory.Exists(warnFolder)) Directory.CreateDirectory(warnFolder);
            // warnFile does not exist, create it and add in the userId and 1 warning to the user.
            // If warnFile does exist, load the user's warnings and and 1 warning to the existing warning.
            if (!File.Exists(warnFolder + "/" + warnFile))
            {
                //conf.UserId = user.Id;
                conf.NumberOfWarnings = 0;
                conf.WarningMessage = "";
                conf.UserUsername = user.Username;
                conf.ModUsername = Context.User.Username;

                string json = JsonConvert.SerializeObject(conf, Formatting.Indented);
                File.WriteAllText(warnFolder + "/" + warnFile, json);
            }
            else
            {
                ulong currentGuild = Context.Guild.Id;

                string json = File.ReadAllText(warnFolder + "/" + warnFile);
                conf = JsonConvert.DeserializeObject<WarnConfig>(json);

                conf.UserId = user.Id;
                if (conf.NumberOfWarnings <= 0) conf.NumberOfWarnings = 0;
                else { conf.NumberOfWarnings--; }
                conf.WarningMessage = "";

                string json1 = JsonConvert.SerializeObject(conf, Formatting.Indented);
                File.WriteAllText(warnFolder + "/" + warnFile, json1);
            }

        }
        */

        [Command("purge")]
        [Summary("purge [amount] | Purge [x] amount of messages. Requires User Administrator and ManageMessage privileges. Requires Bot ManageMessages privileges.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireContext(ContextType.Guild)]
        public async Task Purge([Remainder] int del = 0)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.PurgeModule)
            {
                if (del != 100)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(222, 77, 156));

                    if (del != 0)
                    {
                        var messages = await (Context.Channel as ITextChannel).GetMessagesAsync(del + 1).FlattenAsync();
                        await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);
                    }
                    else
                    {
                        embed.AddField("Purge", $"**{Context.User.Username}** Invalid command syntax. Usage => `{guildProfile.CmdPrefix}purge [--amount]`");
                        await ReplyAsync("", false, embed.Build());
                    }
                }
                
            }
            

        }

        [Command("purgeu")]
        [Summary("purgeu <user> [amount] | Purge [x] amount of messages from a specific user. Requires User Administrator and ManageMesseges privileges. Requires Bot ManageMesseges privileges.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireContext(ContextType.Guild)]
        public async Task Cleard(SocketGuildUser user = null, [Remainder] int del = 0)
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.PurgeModule)
            {
                if (del != 100)
                {
                    var embed = new EmbedBuilder();
                    embed.WithColor(new Color(222, 77, 156));

                    if (del != 0)
                    {
                        if (user != null)
                        {
                            var Items = await (Context.Channel as ITextChannel).GetMessagesAsync().FlattenAsync();

                            int y = 0;
                            var messages = await (Context.Channel as ITextChannel).GetMessagesAsync(y + 1).FlattenAsync();
                            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);

                            var usermessages = Items.Where(x => x.Author == user).Take(del + 1);

                            await (Context.Channel as ITextChannel).DeleteMessagesAsync(usermessages);
                        }
                    }
                    else
                    {
                        embed.AddField("Purge", $"**{Context.User.Username}** Invalid command syntax. Usage => `{guildProfile.CmdPrefix}purge <user> [--amount]`");
                        await ReplyAsync("", false, embed.Build());
                    }
                }
                
            }
            
            
        }

        #region Enable/Disable Modules
        // Enable Ban Module
        [Command("ban enable")]
        [Summary("banenable | Enable the ban module. Requires User Administrator privileges.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task BanEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.BanModule)
            {
                guildProfile.BanModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Mute Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Mute Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disabled Ban Module
        [Command("ban disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task BanDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.BanModule)
            {
                guildProfile.BanModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark:  Ban Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Ban Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable Kick Module
        [Command("kick enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task KickEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.KickModule)
            {
                guildProfile.KickModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Kick Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Kick Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Kick Module
        [Command("kick disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task KickDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.KickModule)
            {
                guildProfile.KickModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Kick Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Kick Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable Warn Module
        [Command("warn enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.WarnModule)
            {
                guildProfile.WarnModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Warn Module
        [Command("warn disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task WarnDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.WarnModule)
            {
                guildProfile.WarnModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Warn Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable Greet Module
        [Command("greet enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task GreetEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.GreetModule)
            {
                guildProfile.GreetModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Greet Module
        [Command("greet disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task GreetDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.GreetModule)
            {
                guildProfile.GreetModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Greet Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable Leave Module
        [Command("leave enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task LeaveEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.LeaveModule)
            {
                guildProfile.LeaveModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Leave Module
        [Command("leave disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task LeaveDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.LeaveModule)
            {
                guildProfile.LeaveModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Leave Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable Afk Module
        [Command("afk enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task AfkEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.AfkModule)
            {
                guildProfile.AfkModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Afk Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Afk Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Afk Module
        [Command("afk disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task AfkDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.AfkModule)
            {
                guildProfile.AfkModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Afk Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Afk Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable Purge Module
        [Command("purge enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task PurgeEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.PurgeModule)
            {
                guildProfile.PurgeModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Purge Module has been enabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Purge Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable Purge Module
        [Command("purge disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task PurgeDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.PurgeModule)
            {
                guildProfile.PurgeModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Purge Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Purge Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Enable LevelUp Messages
        [Command("levelmsg enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task LevelEnable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (!guildProfile.LevelModule)
            {
                guildProfile.LevelModule = true;
                await Context.Channel.SendMessageAsync(":white_check_mark: Level Module has been enabled. Members will still get experince, but the message will not appear.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Level Module has already been enabled.");
            }
            GuildProfiles.SaveProfiles();
        }

        // Disable LevelUp Messages
        [Command("levelmsg disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task LevelDisable()
        {
            // Create a target of type SocketGuild so we can retreive information from the GuildProfiles
            SocketGuild target = null;
            target = Context.Guild;
            var guildProfile = GuildProfiles.GetAccount(target); // Guild Profile

            if (guildProfile.LevelModule)
            {
                guildProfile.LevelModule = false;
                await Context.Channel.SendMessageAsync(":white_check_mark: Level Module has been disabled.");
            }
            else
            {
                await Context.Channel.SendMessageAsync(":white_check_mark: Level Module has already been disabled.");
            }
            GuildProfiles.SaveProfiles();
        }


        #endregion

        

        public struct WarnConfig
        {
            public ulong UserId;
            public ulong NumberOfWarnings;

            public string WarningMessage;

            public string UserUsername;

            public string ModUsername;
        }
    }
}