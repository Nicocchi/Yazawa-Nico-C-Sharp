﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Yazawa_Nico.Core;
using Yazawa_Nico.Core.GuildAccounts;
using Yazawa_Nico.Core.LevelingSystem;

namespace Yazawa_Nico
{
    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;

        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.botToken == "" || Config.bot.botToken == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig 
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Logger;
            //_client.Ready += RepeatingTimer.StartTimer;
            //_client.ReactionAdded += OnReactionAdded;

            await _client.LoginAsync(TokenType.Bot, Config.bot.botToken);
            await _client.StartAsync();

            Global.client = _client;
            _handler = new CommandHandler();
            
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);

        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.MessageId == Global.MessageToTrack)
            {
                if (reaction.Emote.Name == "👌")
                {
                    await channel.SendMessageAsync(reaction.User.Value.Username + " says OK.");
                }
            }
        }

        private static Task Logger(LogMessage lmsg)
        {
            var cc = Console.ForegroundColor;
            switch (lmsg.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            Console.WriteLine($"{DateTime.Now} [{lmsg.Severity,8}] {lmsg.Source}: {lmsg.Message}");
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }
    }
}
