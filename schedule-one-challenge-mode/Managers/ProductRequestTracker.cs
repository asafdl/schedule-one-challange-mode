using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MelonLoader;

#if MONO
using ScheduleOne.Economy;
using ScheduleOne.GameTime;
using ScheduleOne.Messaging;
using ScheduleOne.NPCs;
using ScheduleOne.Product;
using ScheduleOne.Effects;
using ScheduleOne.DevUtilities;
#endif

namespace challenge_mode.Managers
{
    /// <summary>
    /// Tracks failed product requests per customer and manages messaging cooldowns
    /// </summary>
    public static class ProductRequestTracker
    {
        private static Dictionary<string, DateTime> lastMessageTime = new Dictionary<string, DateTime>();


        private static readonly string[] ProductRequestTemplates = new string[]
        {
            "Hi, can't find any {0} with {1}, text me when you have some",
            "Looking for {0} that's {1}. Hit me up if you get any",
            "Need some {0} with {1} effects. Let me know when you're stocked",
            "Can't find {0} with {1} anywhere. You got hookup?",
            "Yo, need {0} that has {1}. Message me when available"
        };

        /// <summary>
        /// Record a failed product search attempt for a customer
        /// </summary>
        public static void RecordFailedRequest(Customer customer)
        {
            if (customer == null || customer.NPC == null)
                return;

            if (!ChallengeConfig.ENABLE_PRODUCT_REQUEST_MESSAGES)
                return;

            string customerId = customer.NPC.ID;


            if (ShouldSendMessage(customerId))
            {
                MelonLogger.Msg($"[ProductRequestTracker] Sending message to {customer.NPC.fullName} failure");
                SendProductRequestMessage(customer);
                lastMessageTime[customerId] = DateTime.Now;
            }
        }

        private static bool ShouldSendMessage(string customerId)
        {
            if (!lastMessageTime.ContainsKey(customerId))
                return true;

            var daysSinceLastMessage = (DateTime.Now - lastMessageTime[customerId]).TotalDays;
            return daysSinceLastMessage >= ChallengeConfig.MESSAGE_COOLDOWN_DAYS;
        }

        private static void SendProductRequestMessage(Customer customer)
        {
            try
            {
                var messagingManager = NetworkSingleton<MessagingManager>.Instance;
                if (messagingManager == null)
                {
                    Debug.LogWarning("[ProductRequestTracker] MessagingManager not found");
                    return;
                }

                var conversation = messagingManager.GetConversation(customer.NPC);
                if (conversation == null)
                {
                    Debug.LogWarning($"[ProductRequestTracker] No conversation found for {customer.NPC.fullName}");
                    return;
                }

                string messageText = GenerateMessageText(customer);
                
                var message = new Message(messageText, Message.ESenderType.Other, _endOfGroup: true);
                conversation.SendMessage(message, notify: true, network: true);

            }
            catch (Exception ex)
            {
                Debug.LogError($"[ProductRequestTracker] Error sending message: {ex.Message}");
            }
        }

        private static string GenerateMessageText(Customer customer)
        {
            var customerData = customer.CustomerData;

            string likedProduct = null;
            string effectName = null;

            if (customerData.DefaultAffinityData != null)
            {
                var affinities = customerData.DefaultAffinityData.ProductAffinities;
                if (affinities != null && affinities.Count > 0)
                {
                    var topAffinity = affinities.OrderByDescending(a => a.Affinity).First();
                    likedProduct = topAffinity.DrugType.ToString().ToLower();
                }
            }

            if (customerData.PreferredProperties != null && customerData.PreferredProperties.Count > 0)
            {
                var preferredEffect = customerData.PreferredProperties[
                    UnityEngine.Random.Range(0, customerData.PreferredProperties.Count)];
                effectName = preferredEffect.name.Replace("Effect_", "").ToLower();
            }

            // Only send specific request if we have both pieces of info
            if (likedProduct != null && effectName != null)
            {
                string template = ProductRequestTemplates[
                    UnityEngine.Random.Range(0, ProductRequestTemplates.Length)];
                return string.Format(template, likedProduct, effectName);
            }

            return "Looking for something specific. Let me know when you restock.";
        }

        /// <summary>
        /// Save tracking data to persistent storage
        /// </summary>
        public static void SaveData()
        {
            ProductRequestPersistence.SaveData(lastMessageTime);
        }

        /// <summary>
        /// Load tracking data from persistent storage
        /// </summary>
        public static void LoadData()
        {
            ProductRequestPersistence.LoadData(out lastMessageTime);
        }

        /// <summary>
        /// Get internal dictionaries for persistence (used by persistence system)
        /// </summary>
        internal static Dictionary<string, DateTime> GetLastMessageTimes()
        {
            return lastMessageTime;
        }
    }
}

