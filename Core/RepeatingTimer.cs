using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Yazawa_Nico.Core
{
    internal static class RepeatingTimer
    {
        /* 
        private static Timer loopingTimer;
        //private static SocketTextChannel channel;

        public static SocketUser socketUsr;

        internal static Task StartTimer()
        {
            //Console.WriteLine("RpeatingTimer: Start Timer");
            loopingTimer = new Timer()
            {
                Interval = 10000, // Set to 30 seconds (30000) instead of 5 seconds
                AutoReset = true,
                Enabled = true
            };
            loopingTimer.Elapsed += OnTimerTicked;

            return Task.CompletedTask;
        }

        private static void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            var pdd = UserProfiles.UserProfiles.GetAccount(socketUsr);
            pdd.dailyB = true;
            UserProfiles.UserProfiles.SaveProfiles();
            //Console.WriteLine("Reapting Timer: " + pdd.dailyB.ToString());
        }
        */

    }

}