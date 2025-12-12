using HarmonyLib;
using System.Collections.Generic;
using challenge_mode.Managers;
using UnityEngine;
using MelonLoader;

#if MONO
using ScheduleOne.Economy;
using ScheduleOne.Product;
using ScheduleOne.ItemFramework;
using ScheduleOne.UI.Handover;
#endif

namespace challenge_mode.Patches
{

    [HarmonyPatch(typeof(Customer), "GetWeightedRandomProduct")]
    public class TrackProductRequest_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Customer __instance, ref ProductDefinition __result, ref float appeal)
        {
            if (__instance?.NPC?.ID == null)
                return;

            if (__result != null)
            {
                var quality = __instance.CustomerData.Standards.GetCorrespondingQuality();
                float enjoyment = __instance.GetProductEnjoyment(__result, quality);
                float priceRatio = __result.Price / __result.MarketValue;
                
                // MelonLogger.Msg($"[ProductSearch] {__instance.NPC.fullName} considering {__result.Name}: " +
                //                $"enjoyment={enjoyment:F3}, price={__result.Price:F0}, " +
                //                $"market={__result.MarketValue:F0}, ratio={priceRatio:F2}, appeal={appeal:F3}");
            }

            bool foundGoodProduct = __result != null && appeal >= ChallengeConfig.MIN_APPEAL_FOR_SUCCESS;

            if (!foundGoodProduct)
            {
                ProductRequestTracker.RecordFailedRequest(__instance);
                string reason = __result == null ? "no products available" : $"low appeal ({appeal:F3})";
                MelonLogger.Msg($"[ProductRequest] {__instance.NPC.fullName} FAILED: {reason}");
            }
        }
    }
}

