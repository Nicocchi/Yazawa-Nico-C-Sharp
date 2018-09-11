using Discord;
using Discord.Commands;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yazawa_Nico.Core.GuildAccounts;

namespace Yazawa_Nico.Modules.Utils
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        string titleInfo;
        string messageInfo;
        EmbedBuilder embed = new EmbedBuilder();

        [Command("help", RunMode = RunMode.Async)]
        [Summary("Display the help categories")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task HelpInfo()
        {
            LoadHelp();
            await ReplyAsync("", false, embed.Build());

        }

        [Command("helpdm", RunMode = RunMode.Async)]
        [Summary("Display the help categories in a direct message")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpInfoDm()
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync(); /* A channel is created so that the commands will be privately sent to the user, and not flood the chat. */

            LoadHelp();
            await dmChannel.SendMessageAsync("", false, embed.Build());

        }

        // Create the help information inside the EmbedBuilder
        public void LoadHelp()
        {
            var gp = GuildProfiles.GetAccount(Context.Guild);
            string prefix = gp.CmdPrefix;
            embed.WithDescription("Nico Nico Nii~ Here is a list of all the commands I can do~");
            embed.WithColor(new Color(255,73,167));
            embed.WithAuthor(author => {
                author
                    .WithName("Yazawa Nico's Commands")
                    .WithUrl("https://discord.gg/uSaxHgr")
                    .WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl());
            });
            embed.WithFooter(footer =>
            {
                footer
                    .WithText($"Requested by {Context.User.Username}#{Context.User.Discriminator} on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToShortTimeString()}")
                    .WithIconUrl(Context.Client.GetUser(Context.User.Id).GetAvatarUrl());
            });
            embed.AddField("Prefix", $"{prefix}", true);
            embed.AddField("Support Server", $"[Support Server](https://discord.gg/hMXMpqv)", true);
            embed.AddField("Version", $"`v2.1.0`", true);
            embed.AddField("Core", "`help`, `helpdm`");
            embed.AddField("Images", "`angry`, `bad`, `badass`, `bite`, `blush`, `congrats`, `cry`, `cuddle`, `evil`, `excited`, `highfive`, `hug`, `kiss`, `laugh`, `lewd`, `lick`, `niconii`, `no`, `nosebleed`, `pat`, `peek`, `poke`, `sad`, `scared`, `slap`, `sleepy`, `tantrum`, `washi`, `wasted`, `wow`, `yes`");
            embed.AddField("Currency", "`daily`, `profile`, `send`");
            embed.AddField("Fun", "`choose`, `cucumber`, `give`, `marry`, `notice`, `rip`, `say`, `senpai` `wisdom`");
            embed.AddField("Games", "`8ball`, `rps`");
            embed.AddField("Moderation", "`ban`, `kick`, `purge`, `purgeu` `greetmsg`, `leavemsg`. `greetmsg settext`, `greetmsg setchannel`, `leavemsg settext`, `leavemsg setchannel`, `greet enable/disable`, `leave enable/diable`, `ban enable/disable`, `kcik enable/disable`, `warn enable/disable`, `levelmsg enable`, `levelmsg disable`, `setprefix`");
            embed.AddField("Info", "`afk`, `avatar`, `botinfo`, `userinfo`, `serverinfo`");
            embed.AddField("Examples", "`!help [command]` for detailed information on the command.");
        }

        /// <summary>
        /// Specific Help Command Information
        /// </summary>
        /// <returns></returns>
        #region Specific Help Command Information
        // <- Help Commands ->
        #region Help Categories
        [Command("help nico", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpNico()
        {
            titleInfo = "Nico Help Commands";
            messageInfo = "botinfo:    Display information about this bot\nhelp:         Display a list of all commands";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help fun", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpFun()
        {
            titleInfo = "Fun Help Commands";
            messageInfo = "8ball:    Play with an 8ball\nbad:      Display you/user is bad\ncry:       Display you/user is crying\nhug:       Display hugging an user\nlewd:     Display you/user is lewd\nniconii:  nico\nping:     pong\nrip:        Rest in peace\nsad:      Display you/user is sad\nslap:     Display you/user is slapping an user\ntantrum:  Display you/user is throwing a tantrum\nwasted:   Display you/user is wasted";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help games", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpGames()
        {
            titleInfo = "Games Help Commands";
            messageInfo = "rps:    Play Rock Paper Scissors";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help guild", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpGuild()
        {
            titleInfo = "Guild Help Commands";
            messageInfo = "avatar:    Display the user's avatar\nafk:       Display that you are afk\nban:       Ban a user\nkick:      Kick a user\nclear:     Delete [x] messages\ngreetmsg:     Change join message.\nleavemsg:     Change leave message.";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help util", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpUtil()
        {
            titleInfo = "Util Help Commands";
            messageInfo = "serverinfo:    Display information about this server\nuserinfo:      Display information about a user";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help points", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpPoints()
        {
            titleInfo = "Points Help Commands";
            messageInfo = "daily:    Receive daily Nico Nii's every 24 hours!";
            await SendMessage(titleInfo, messageInfo);
        }
        #endregion

        #region Command Help
        // <- Nico help Commands ->
        #region Nico
        [Command("help botinfo", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpBotInfo()
        {
            titleInfo = "Bot Info";
            messageInfo = "Show bot info.   Usage => `!botinfo`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help help", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpHelp()
        {
            titleInfo = "Help";
            messageInfo = "Show all help info.   Usage => `!help`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help helpdm", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpHelpDm()
        {
            titleInfo = "Help";
            messageInfo = "Show all help info in direct message.   Usage => `!helpdm`";
            await SendMessage(titleInfo, messageInfo);
        }
        #endregion
        // <- /Nico help Commands ->
        // <- Points Help Commands ->
        #region Points
        [Command("help daily", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpDaily()
        {
            titleInfo = "Daily";
            messageInfo = "Receive daily Nico Nii's every 24 hours!    Usage => `!daily`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help status", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpStatus()
        {
            titleInfo = "Status";
            messageInfo = "View your global status!    Usage => `!status` and `!s`";
            await SendMessage(titleInfo, messageInfo);
        }
        #endregion
        // <- Points Help Commands ->
        //
        // <- Fun Help Commands ->
        #region Fun
        [Command("help 8ball", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task Help8ball()
        {
            titleInfo = "8Ball";
            messageInfo = "Ask an 8Ball!    Usage => `!8ball [question]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help bad", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpBad()
        {
            titleInfo = "Bad";
            messageInfo = "Punish a user.   Usage => `!bad [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help cry", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpCry()
        {
            titleInfo = "Cry";
            messageInfo = "Cry at yourself/user!    Usage => `!cry` or `!cry [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help cucumber", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpCucumber()
        {
            titleInfo = "Cucumber";
            messageInfo = "Give a cucumber to a user.   Usage => `!cucumber [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help give", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpGive()
        {
            titleInfo = "Give";
            messageInfo = "Give any item to a user.     Usage => `!give [\"item\"] [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help hug", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpHug()
        {
            titleInfo = "Hug";
            messageInfo = "Hug a user.     Usage => `!hug [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help lewd", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpLewdy()
        {
            titleInfo = "Lewd";
            messageInfo = "Lewd a user!     Usage => `!lewd [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help niconii", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpNiconii()
        {
            titleInfo = "Nico Nii";
            messageInfo = "Nico Nii.    Usage => `!niconii` or `!niconii [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help ping", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpPing()
        {
            titleInfo = "Ping";
            messageInfo = "Get ping information of site.    Usage => `!ping [site]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help rip", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpRip()
        {
            titleInfo = "Rip";
            messageInfo = "Rest in Peace.   Usage => `!rip` or `rip [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help sad", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpSad()
        {
            titleInfo = "Sad";
            messageInfo = "Sad... Usage => `!sad` or `!sad [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help say", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpSay()
        {
            titleInfo = "Say";
            messageInfo = "Tell the bot to say something.   Usage => `!say [message]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help slap", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpSlap()
        {
            titleInfo = "Slap";
            messageInfo = "Slap a user!     Usage => `!slap [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help tantrum", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpTantrum()
        {
            titleInfo = "Tantrum";
            messageInfo = "Throw a tantrum.     Usage => `!tantrum`  or `!tantrum [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help washi", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpWashi()
        {
            titleInfo = "Washi";
            messageInfo = "Washi washi!     Usage => `!washi [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help wasted", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpWasted()
        {
            titleInfo = "Wasted";
            messageInfo = "Become wasted!     Usage => `!wasted` or `!wasted [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help poke", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpPoke()
        {
            titleInfo = "Wasted";
            messageInfo = "Poke a user!     Usage => `!poke [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help cuddle", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpCuddle()
        {
            titleInfo = "Wasted";
            messageInfo = "Cuddle a user!     Usage => `!cuddle [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help wow", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpWow()
        {
            titleInfo = "Wasted";
            messageInfo = "Woooow!     Usage => `!wow`";
            await SendMessage(titleInfo, messageInfo);
        }
        #endregion
        // <- /Fun Help Commands ->
        //
        // <- Games Help Commands ->
        #region Games
        [Command("help rps", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpRps()
        {
            titleInfo = "Rock Paper Scissors";
            messageInfo = "Play Rock Paper Scissors with Yazawa Nico.     Usage => `!rps` or `!rps [amount]`";
            await SendMessage(titleInfo, messageInfo);
        }
        #endregion
        // <- /Games Help Commands ->
        //
        // <- Guild Help Commands ->
        #region Guild
        [Command("help avatar", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpAvatar()
        {
            titleInfo = "Avatar";
            messageInfo = "Show user's avatar.     Usage => `!avatar [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help afk", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpAfk()
        {
            titleInfo = "AFK";
            messageInfo = "Go AFK.     Usage => `!afk [--message]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help ban", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpBan()
        {
            titleInfo = "Ban";
            messageInfo = "Ban user. Must have `Ban` permissions.     Usage => `!ban [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help kick", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpKick()
        {
            titleInfo = "Kick";
            messageInfo = "Kick user. Must have `Kick` permissions.     Usage => `!kick [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help purge", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpClear()
        {
            titleInfo = "Purge";
            messageInfo = "Delete [x] amount of messages. Must have `Administrator` permissions.     Usage => `!purge [--amount]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help purgeu", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpClearD()
        {
            titleInfo = "Purge user specific messages";
            messageInfo = "Delete [x] amount of messages from a specific user. Must have `Administrator` permissions.     Usage => `!purgeu <user> [--amount]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help greetmsg", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpJoinMessage()
        {
            titleInfo = "Change join message";
            messageInfo = "Change the channel, and enable/disable and set the text of the greet messages. Must have `Manage Messages` permissions.\nUsage:\n`!greet enable` to enable greet messages.\n`!greet disable` to disable the greet messages\n`!greetmsg settext` to change the greet message.\n`!greetmsg setchannel` to set the greet channel.";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help leavemmsg", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpLeaveMessage()
        {
            titleInfo = "Change leave message";
            messageInfo = "Change the channel, and enable/disable and set the text of the leave messages. Must have `Manage Messages` permissions.\nUsage:\n`!leave enable` to enable greet messages.\n`!leave disable` to disable the greet messages\n`!leavemsg settext` to change the greet message.\n`!leavemsg setchannel` to set the greet channel.";
            await SendMessage(titleInfo, messageInfo);
        }
        #endregion
        // <- /Guild Help Commands ->
        //
        // <- Util Help Commands ->
        #region Util
        [Command("help serverinfo", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpServerInfo()
        {
            titleInfo = "Server Info";
            messageInfo = "Show server info.     Usage => `!serverinfo`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help userinfo", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpUserInfo()
        {
            titleInfo = "User Info";
            messageInfo = "Show user info.     Usage => `!userinfo [user]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help setprefix", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpSetPrefix()
        {
            titleInfo = "Set Prefix";
            messageInfo = "Set command prefix.     Usage => `!setprefix <prefix>`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help send", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpSend()
        {
            titleInfo = "Send";
            messageInfo = "Send x amount of Nico Nii's to someone.     Usage => `!send <user> [amount]`";
            await SendMessage(titleInfo, messageInfo);
        }
        [Command("help marry", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        public async Task HelpMarry()
        {
            titleInfo = "Marry";
            messageInfo = "Marry a user. Max number of users to marry is 10.     Usage => `!marry <user>`";
            await SendMessage(titleInfo, messageInfo);
        }
        #endregion
        // <- /Util Help Commands ->
        //
        // <- /Help Commands ->
        #endregion
        #endregion

        public async Task SendMessage(string title, string message)
        {
            title = titleInfo;
            message = messageInfo;
            embed.WithColor(new Color(222, 77, 156));
            embed.AddField(title, message);
            await ReplyAsync("", false, embed.Build());
        }


    }
}
