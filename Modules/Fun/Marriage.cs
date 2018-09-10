using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Yazawa_Nico.Core.UserProfiles;
using Yazawa_Nico.Core.GuildAccounts;

namespace Yazawa_Nico.Modules.Fun
{
    public class Marriage : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        /// Propose a marriage to a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Command("marry")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task MarryUser(SocketUser user)
        {
            var gp = GuildProfiles.GetAccount(Context.Guild);
            string prefix = gp.CmdPrefix;
            if (user != Context.User)
            {
                if (UserProfiles.IsMarried(Context.User, user) == false)
                {
                    if (UserProfiles.HasMarriageSlots(Context.User))
                    {
                        UserProfiles.AddMarriageProposal(Context.User, user);

                        var embed = new EmbedBuilder()
                            .WithColor(255,73,167)
                            .AddField($"{Context.User.Username} has proposed to {user.Username}", $"A marriage is a voluntary and full commitment. It is made in the deepest sense to the exclusion of all others. Before you declare your vows to one another, I want to hear you confirm that it is your intention to be married today. **{user.Username}**, do you come here freely to give yourself to **{Context.User.Username}** in marriage?", false)
                            .AddField(":white_check_mark: To accpet", $"{prefix}acceptmarriage @user", true)
                            .AddField(":negative_squared_cross_mark: To decline", $"{prefix}declinemarriage @user", true)
                            .WithFooter(footer =>
                                {
                                    footer
                                        .WithText($"Take your time though! This proposal won't disappear");
                                });

                        await Context.Channel.SendMessageAsync("", embed: embed.Build());
                    }
                    else
                    {
                        await ReplyAsync($"{Context.User.Mention}, don't have enough marriage slots available.");
                    }
                }
                else
                {
                    await ReplyAsync($"{Context.User.Mention}, you are already married to this user.");
                }
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention}, you can't marry yourself.");
            }
            
        }

        /// <summary>
        /// Accept marriage command and marry the two users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Command("acceptmarriage")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task AcceptUserMarriage(SocketUser user)
        {
            var userPf = UserProfiles.GetAccount(Context.User);
            
            if (userPf.MarriageProposals.Contains(user.Id))
            {
                UserProfiles.AcceptMarriage(Context.User, user);

                await ReplyAsync($":tada: Congrulations! You and {user.Username} are married!");
            }
            else
            {
                await ReplyAsync($"You have not proposed to {user.Username}!");
            }

        }

        /// <summary>
        /// Decline marriage command and do not marry the two users
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Command("declinemarriage")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task DeclineUserMarriage(SocketUser user)
        {
            var userPf = UserProfiles.GetAccount(Context.User);
            
            if (userPf.MarriageProposals.Contains(user.Id))
            {
                UserProfiles.DeclineMarriage(Context.User, user);

                await ReplyAsync($"You have declined {user.Username}'s marriage proposal.");
            }
            else
            {
                await ReplyAsync($"You have not proposed to {user.Username}!");
            }

        }

        /// <summary>
        /// Divorce a specified user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Command("divorce")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task DivorceUser(SocketUser user)
        {
            var userPf = UserProfiles.GetAccount(Context.User); // You
            var proposedUser = UserProfiles.GetAccount(user); // Proposer

            if (proposedUser.Marriages.Contains(Context.User.Id) && userPf.Marriages.Contains(user.Id))
            {
                UserProfiles.DivorceUser(Context.User, user);

                await ReplyAsync($"{Context.User.Username} have divorced {user.Username}!");
            }

        }


        [Command("buy marriageslot")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task BuyMarriageSlot()
        {
            var userPf = UserProfiles.GetAccount(Context.User); // You
            var gp = GuildProfiles.GetAccount(Context.Guild);
            string prefix = gp.CmdPrefix;

            var slots = userPf.MarriageSlots;
            int currency = 0;

            switch(slots)
            {
                case 5:
                    currency = 2500;
                    userPf.IsBuyingSlot = true;
                break;
                case 6:
                    currency = 4500;
                    userPf.IsBuyingSlot = true;
                break;
                case 7:
                    currency = 6500;
                    userPf.IsBuyingSlot = true;
                break;
                case 8:
                    currency = 8500;
                    userPf.IsBuyingSlot = true;
                break;
                case 9:
                    currency = 10000;
                    userPf.IsBuyingSlot = true;
                break;
                case 10:
                    await ReplyAsync($"You can't buy anymore slots.");
                    userPf.IsBuyingSlot = false;
                break;
            }

            if (userPf.IsBuyingSlot)
            {
                await ReplyAsync($"You can buy a marriage slot for {currency} Nico Nii's. Would you like to buy it?\n\n Type {prefix}buy slot yes or {prefix}buy slot no");
            }
            UserProfiles.SaveProfiles();

        }

        /// <summary>
        /// User buys a marriage slot
        /// </summary>
        /// <returns></returns>
        [Command("buyslot yes")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task ConfirmMarriageSlot()
        {
            var userPf = UserProfiles.GetAccount(Context.User); // You
            var marriageCount = UserProfiles.GetMarriageCount(Context.User);
            var currency = UserProfiles.GetCurrency(Context.User);

            if (userPf.IsBuyingSlot)
            {
                if (marriageCount == 5)
                {
                    if (currency >= 1000)
                    {
                        UserProfiles.BuyMarriageSlot(Context.User, 1000);
                        await ReplyAsync($"You purchased one slot!");
                    }
                    else
                    {
                        await ReplyAsync($"You do not have enough Nico Nii's to buy a slot.");
                    }
                }
                if (marriageCount == 6)
                {
                    if (currency >= 1500)
                    {
                        UserProfiles.BuyMarriageSlot(Context.User, 1500);
                        await ReplyAsync($"You purchased one slot!");
                    }
                    else
                    {
                        await ReplyAsync($"You do not have enough Nico Nii's to buy a slot.");
                    }
                }
                if (marriageCount == 7)
                {
                    if (currency >= 2000)
                    {
                        UserProfiles.BuyMarriageSlot(Context.User, 2000);
                        await ReplyAsync($"You purchased one slot!");
                    }
                    else
                    {
                        await ReplyAsync($"You do not have enough Nico Nii's to buy a slot.");
                    }
                }
                if (marriageCount == 8)
                {
                    if (currency >= 2500)
                    {
                        UserProfiles.BuyMarriageSlot(Context.User, 2500);
                        await ReplyAsync($"You purchased one slot!");
                    }
                    else
                    {
                        await ReplyAsync($"You do not have enough Nico Nii's to buy a slot.");
                    }
                }
                if (marriageCount == 9)
                {
                    if (currency >= 3000)
                    {
                        UserProfiles.BuyMarriageSlot(Context.User, 3000);
                        await ReplyAsync($"You purchased one slot!");
                    }
                    else
                    {
                        await ReplyAsync($"You do not have enough Nico Nii's to buy a slot.");
                    }
                }
                if (marriageCount == 10)
                {
                    if (currency >= 3500)
                    {
                        UserProfiles.BuyMarriageSlot(Context.User, 3500);
                        await ReplyAsync($"You purchased one slot!");
                    }
                    else
                    {
                        await ReplyAsync($"You do not have enough Nico Nii's to buy a slot.");
                    }
                }
                UserProfiles.SaveProfiles();
            }
            
        }

        /// <summary>
        /// User does not buy a marriage slot
        /// </summary>
        /// <returns></returns>
        [Command("buyslot no")]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task DenyMarriageSlot()
        {
            var userPf = UserProfiles.GetAccount(Context.User); // You

            if (userPf.IsBuyingSlot)
            {
                userPf.IsBuyingSlot = false;
                UserProfiles.SaveProfiles();
                await ReplyAsync($"You canceled the transaction.");
            }
            
        }
        
    }
}