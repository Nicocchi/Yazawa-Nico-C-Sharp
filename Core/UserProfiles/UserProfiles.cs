using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;

namespace Yazawa_Nico.Core.UserProfiles
{
    public static class UserProfiles
    {
        private static List<UserProfile> profiles;
        private static string profilesFile = "Resources/profiles.json";

        static UserProfiles()
        {
            if (DataStorage.SaveExists(profilesFile))
            {
                profiles = DataStorage.LoadUserProfiles(profilesFile).ToList();
            }
            else
            {
                profiles = new List<UserProfile>();
                SaveProfiles();
            }
        }

        /// <summary>
        /// Saves the user profiles
        /// </summary>
        public static void SaveProfiles()
        {
            DataStorage.SaveUserProfiles(profiles, profilesFile);
        }

        /// <summary>
        /// Returns the user's profile
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserProfile GetAccount(SocketUser user)
        {
            return GetorCreateProfile(user.Id);
        }

        /// <summary>
        /// Returns the user's profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static UserProfile GetAccount(ulong id)
        {
            return GetorCreateProfile(id);
        }

        /// <summary>
        /// Returns the user's profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static UserProfile GetAccountByID(SocketUser user)
        {
            return GetorCreateProfile(user.Id);
        }

        /// <summary>
        /// Returns all profiles
        /// </summary>
        /// <returns></returns>
        public static List<UserProfile> GetAllAccounts()
        {
            return profiles;
        }

        /// <summary>
        /// Returns the user's ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static ulong GetId(SocketUser user)
        {
            var profile = UserProfiles.GetAccount(user);
            return profile.ID;
        }

        /// <summary>
        /// Returns the user's currency amount
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static uint GetCurrency(SocketUser user)
        {
            var profile = UserProfiles.GetAccount(user);
            return profile.Points;
        }

        /// <summary>
        /// Returns the user's current level
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static uint GetLevel(SocketUser user)
        {
            var profile = UserProfiles.GetAccount(user);
            return profile.LevelNumber;
        }

        /// <summary>
        /// Returns the user's current XP
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static uint GetXP(SocketUser user)
        {
            var profile = GetAccount(user);
            return (uint)Math.Pow(profile.LevelNumber + 1, 2) * 50;
        }

        /// <summary>
        /// Returns how many marriages the user is in
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int GetMarriageCount(SocketUser user)
        {
            var profile = GetAccount(user);
            return profile.Marriages.Count;
        }

        /// <summary>
        /// Returns the user's marriages
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static String GetMarriages(SocketUser user)
        {
            return GetUserMarriages(user);
        }

        /// <summary>
        /// Returns true or false if the user has available marriage slots
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool HasMarriageSlots(SocketUser user)
        {
            var profile = GetAccount(user);
            if (profile.Marriages.Count <= profile.MarriageSlots - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set the user's currency amount
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        public static void SetCurrency(SocketUser user, uint amount)
        {
            var profile = GetAccount(user);
            profile.Points = amount;
            SaveProfiles();
        }

        /// <summary>
        /// Adds another marriage slot to the user and decreases their currency
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        public static void BuyMarriageSlot(SocketUser user, uint amount)
        {
            var profile = GetAccount(user);
            profile.Points -= amount;
            profile.MarriageSlots += 1;
            SaveProfiles();
        }

        public static void AddMarriageProposal(SocketUser contextUser, SocketUser proposedUser)
        {
            var userPf = UserProfiles.GetAccount(contextUser);
            var proposedUserPf = UserProfiles.GetAccount(proposedUser);
            
            userPf.SentMarriageProposals.Add(proposedUserPf.ID);
            proposedUserPf.MarriageProposals.Add(userPf.ID);
            SaveProfiles();
        }

        public static void RemoveMarriageProposal(SocketUser contextUser, SocketUser proposedUser)
        {
            var userPf = UserProfiles.GetAccount(contextUser);
            var proposedUserPf = UserProfiles.GetAccount(proposedUser);
            
            proposedUserPf.MarriageProposals.Remove(userPf.ID);
            userPf.SentMarriageProposals.Remove(proposedUserPf.ID);
            SaveProfiles();
        }

        /// <summary>
        /// Accept the marriage between two users and marry them
        /// </summary>
        /// <param name="contextUser"></param>
        /// <param name="proposedUser"></param>
        public static void AcceptMarriage(SocketUser contextUser, SocketUser proposedUser)
        {
            var userPf = UserProfiles.GetAccount(contextUser); // You
            var proposedUserPf = UserProfiles.GetAccount(proposedUser); // Proposer
            
            userPf.Marriages.Add(proposedUserPf.ID);
            proposedUserPf.Marriages.Add(userPf.ID);

            RemoveMarriageProposal(contextUser, proposedUser);
            SaveProfiles();
        }

        /// <summary>
        /// Decline the marriage between two users and marry them
        /// </summary>
        /// <param name="contextUser"></param>
        /// <param name="proposedUser"></param>
        public static void DeclineMarriage(SocketUser contextUser, SocketUser proposedUser)
        {
            RemoveMarriageProposal(contextUser, proposedUser);
            SaveProfiles();
        }

        /// <summary>
        /// Divorce's a specified user
        /// </summary>
        /// <param name="contextUser"></param>
        /// <param name="proposedUser"></param>
        public static void DivorceUser(SocketUser contextUser, SocketUser proposedUser)
        {
            var userPf = UserProfiles.GetAccount(contextUser); // You
            var proposedUserPf = UserProfiles.GetAccount(proposedUser); // Proposer

            proposedUserPf.Marriages.Remove(userPf.ID);
            userPf.Marriages.Remove(proposedUserPf.ID);
            SaveProfiles();
        }

        /// <summary>
        /// Returns if a user is already married to user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="proprosedUser"></param>
        /// <returns></returns>
        public static bool IsMarried(SocketUser user, SocketUser proprosedUser)
        {
            var profile = GetAccount(user);
            var profile1 = GetAccount(proprosedUser);
            if (profile.Marriages.Contains(profile1.ID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets and returns all the marriages from the current user into a string
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static String GetUserMarriages(SocketUser user)
        {
            var profile = UserProfiles.GetAccount(user);

            List<string> marriages = new List<string>();

            // If the marriages is not empty or null add them to the marriages List and return
            // a formatted string of the married users, otherwise return a string of 'no marriages'
            if (profile.Marriages.Count != 0)
            {
                foreach (var item in profile.Marriages)
                {
                    var marriedUser = GetAccount(item);
                    marriages.Add(marriedUser.Username);
                }

                return string.Join("\n", marriages);
            }
            else
            {
                return "No Marriages";
            }
        }

        /// <summary>
        /// Returns an existing profile, otherwise create new profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static UserProfile GetorCreateProfile(ulong id)
        {
            var result = from p in profiles
                        where p.ID == id
                        select p;

            var profile = result.FirstOrDefault();
            if (profile == null) profile = CreateUserProfile(id);
            return profile;
        }

        /// <summary>
        /// Create's a new user profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private static UserProfile CreateUserProfile(ulong id)
        {
            var newProfile = new UserProfile()
            {
                ID = id,
                Username = "",
                Points = 0,
                XP = 0,
                dailyB = true,
                Afk = false,
                AfkMessage = "",
                IsRPS = false,
                IsRPSGamble = false,
                GambleAmount = 0,
                MarriageProposals = new List<ulong>(),
                SentMarriageProposals = new List<ulong>(),
                Marriages = new List<ulong>(),
                MarriageSlots = 5,
                IsBuyingSlot = false
            };

            profiles.Add(newProfile);
            SaveProfiles();
            return newProfile;
        }
    }
}