using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace challange_mode.Managers
{
    [Serializable]
    public class ProductRequestSaveData
    {
        [Serializable]
        public class CustomerTrackingEntry
        {
            public string customerId;
            public string lastMessageTime;
        }

        public List<CustomerTrackingEntry> trackingData = new List<CustomerTrackingEntry>();
    }

    public static class ProductRequestPersistence
    {
        private const string SAVE_FILE_NAME = "product_request_tracking.json";
        
        private static string GetSaveFilePath()
        {
            string directory = Path.Combine(Application.persistentDataPath, "ChallengeMod");
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            return Path.Combine(directory, SAVE_FILE_NAME);
        }

        public static void SaveData(Dictionary<string, DateTime> lastMessageTimes)
        {
            try
            {
                var saveData = new ProductRequestSaveData();
                var allCustomerIds = new HashSet<string>(lastMessageTimes.Keys);

                foreach (var customerId in allCustomerIds)
                {
                    var entry = new ProductRequestSaveData.CustomerTrackingEntry
                    {
                        customerId = customerId,
                        lastMessageTime = lastMessageTimes.ContainsKey(customerId) 
                            ? lastMessageTimes[customerId].ToString("O") 
                            : string.Empty,
                    };

                    saveData.trackingData.Add(entry);
                }

                string json = JsonUtility.ToJson(saveData, prettyPrint: true);
                string filePath = GetSaveFilePath();
                File.WriteAllText(filePath, json);

                Debug.Log($"[ProductRequestPersistence] Saved tracking data to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ProductRequestPersistence] Error saving data: {ex.Message}");
            }
        }

        public static void LoadData(out Dictionary<string, DateTime> lastMessageTimes)
        {
            lastMessageTimes = new Dictionary<string, DateTime>();

            try
            {
                string filePath = GetSaveFilePath();

                if (!File.Exists(filePath))
                {
                    Debug.Log("[ProductRequestPersistence] No save file found, starting fresh");
                    return;
                }

                string json = File.ReadAllText(filePath);
                var saveData = JsonUtility.FromJson<ProductRequestSaveData>(json);

                if (saveData?.trackingData == null)
                {
                    Debug.LogWarning("[ProductRequestPersistence] Invalid save data");
                    return;
                }

                foreach (var entry in saveData.trackingData)
                {
                    if (!string.IsNullOrEmpty(entry.lastMessageTime))
                    {
                        if (DateTime.TryParse(entry.lastMessageTime, out DateTime parsedTime))
                        {
                            lastMessageTimes[entry.customerId] = parsedTime;
                        }
                    }
                }

                Debug.Log($"[ProductRequestPersistence] Loaded tracking data for {saveData.trackingData.Count} customers");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ProductRequestPersistence] Error loading data: {ex.Message}");
            }
        }

        public static void ClearSaveData()
        {
            try
            {
                string filePath = GetSaveFilePath();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Debug.Log("[ProductRequestPersistence] Cleared save data");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[ProductRequestPersistence] Error clearing data: {ex.Message}");
            }
        }
    }
}

