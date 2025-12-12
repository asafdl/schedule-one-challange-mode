using HarmonyLib;
using UnityEngine;
using TMPro;
using System.Text;

#if MONO
using ScheduleOne.UI.Handover;
using ScheduleOne.Economy;
using ScheduleOne.Product;
#endif

namespace challenge_mode.Patches
{

    public static class AffinityDisplayConfig
    {
        public const float AFFINITY_VERY_HIGH = 0.6f;
        public const float AFFINITY_HIGH = 0.2f;
        public const float AFFINITY_NEUTRAL = -0.2f;
        public const float AFFINITY_LOW = -0.6f;
        
        public const string INDICATOR_VERY_HIGH = "[+++]";
        public const string INDICATOR_HIGH = "[++]";
        public const string INDICATOR_NEUTRAL = "[+]";
        public const string INDICATOR_LOW = "[-]";
        public const string INDICATOR_VERY_LOW = "[--]";
        
        public const string COLOR_VERY_HIGH = "4CAF50";
        public const string COLOR_HIGH = "8BC34A";
        public const string COLOR_NEUTRAL = "FFC107";
        public const string COLOR_LOW = "FF9800";
        public const string COLOR_VERY_LOW = "F44336";
        
        public const string SECTION_HEADER = "\n\n<b>Drug Affinities:</b>";
        public const string DUPLICATE_CHECK_TEXT = "Drug Affinities:";
    }


    public static class AffinityDisplayHelpers
    {
        public struct AffinityDisplay
        {
            public string Indicator;
            public string ColorHex;
        }


        public static AffinityDisplay GetAffinityDisplay(float affinity)
        {
            if (affinity > AffinityDisplayConfig.AFFINITY_VERY_HIGH)
                return new AffinityDisplay 
                { 
                    Indicator = AffinityDisplayConfig.INDICATOR_VERY_HIGH,
                    ColorHex = AffinityDisplayConfig.COLOR_VERY_HIGH
                };
            
            if (affinity > AffinityDisplayConfig.AFFINITY_HIGH)
                return new AffinityDisplay
                {
                    Indicator = AffinityDisplayConfig.INDICATOR_HIGH,
                    ColorHex = AffinityDisplayConfig.COLOR_HIGH
                };
            
            if (affinity > AffinityDisplayConfig.AFFINITY_NEUTRAL)
                return new AffinityDisplay
                {
                    Indicator = AffinityDisplayConfig.INDICATOR_NEUTRAL,
                    ColorHex = AffinityDisplayConfig.COLOR_NEUTRAL
                };
            
            if (affinity > AffinityDisplayConfig.AFFINITY_LOW)
                return new AffinityDisplay
                {
                    Indicator = AffinityDisplayConfig.INDICATOR_LOW,
                    ColorHex = AffinityDisplayConfig.COLOR_LOW
                };
            
            return new AffinityDisplay
            {
                Indicator = AffinityDisplayConfig.INDICATOR_VERY_LOW,
                ColorHex = AffinityDisplayConfig.COLOR_VERY_LOW
            };
        }

        public static string BuildAffinitySection(CustomerAffinityData affinityData)
        {
            var builder = new StringBuilder(AffinityDisplayConfig.SECTION_HEADER);

            foreach (var affinityEntry in affinityData.ProductAffinities)
            {
                var display = GetAffinityDisplay(affinityEntry.Affinity);
                string drugName = affinityEntry.DrugType.ToString();
                
                builder.Append($"\n<color=#{display.ColorHex}>\u2022  {drugName} {display.Indicator}</color>");
            }

            return builder.ToString();
        }
    }

    [HarmonyPatch(typeof(HandoverScreenDetailPanel), "Open")]
    public class HandoverScreenDetailPanel_Open_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(HandoverScreenDetailPanel __instance, Customer customer)
        {
            if (!ValidateCustomerData(customer, out var affinityData))
                return;

            if (!ValidateUIElements(__instance, out var effectsLabel))
                return;

            if (effectsLabel.text.Contains(AffinityDisplayConfig.DUPLICATE_CHECK_TEXT))
                return;

            string affinitySection = AffinityDisplayHelpers.BuildAffinitySection(affinityData);
            effectsLabel.text += affinitySection;
        }

        private static bool ValidateCustomerData(Customer customer, out CustomerAffinityData affinityData)
        {
            affinityData = null;

            if (customer?.CustomerData?.DefaultAffinityData == null)
                return false;

            affinityData = customer.CustomerData.DefaultAffinityData;

            if (affinityData.ProductAffinities == null || affinityData.ProductAffinities.Count == 0)
                return false;

            return true;
        }

        private static bool ValidateUIElements(HandoverScreenDetailPanel panel, out TextMeshProUGUI effectsLabel)
        {
            effectsLabel = panel?.EffectsLabel;
            return effectsLabel != null;
        }
    }
}

