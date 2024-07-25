using System;
using System.Diagnostics;
using GorillaExtensions;
using GorillaNetworking;
using HarmonyLib;
using UnityEngine;

namespace ColorsExpanded.Patches
{
    [HarmonyPatch(typeof(VRRig), "InitializeNoobMaterial")]
    [HarmonyWrapSafe]
    internal class InitializeNoobMaterialPatch
    {
        private static bool Prefix(ref float red, ref float green, ref float blue, ref PhotonMessageInfoWrapped info, ref VRRig __instance)
        {
            __instance.IncrementRPC(info, "InitializeNoobMaterial");
            NetworkSystem.Instance.GetPlayer(info.senderID);
            string userID = NetworkSystem.Instance.GetUserID(info.senderID);
            if (info.senderID == NetworkSystem.Instance.GetOwningPlayerID(__instance.rigSerializer.gameObject) && (!__instance.initialized || (__instance.initialized && GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(userID)) || (__instance.initialized && CosmeticWardrobeProximityDetector.IsUserNearWardrobe(userID))))
            {
                __instance.initialized = true;
                __instance.InitializeNoobMaterialLocal(red, green, blue);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(VRRig), "InitializeNoobMaterialLocal")]
    [HarmonyWrapSafe]
    internal class InitializeNoobMaterialLocalPatch
    {
        private static bool Prefix(ref float red, ref float green, ref float blue, ref VRRig __instance)
        {
            Color color = new Color(red, green, blue);
            __instance.EnsureInstantiatedMaterial();
            if (__instance.myDefaultSkinMaterialInstance != null)
            {
                __instance.myDefaultSkinMaterialInstance.color = color;
            }
            __instance.SetColor(color);
            __instance.UpdateName(PlayFabAuthenticator.instance.GetSafety());
            return false;
        }
    }
}
