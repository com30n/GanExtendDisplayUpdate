using HarmonyLib;

namespace GanExtendDisplay
{
    internal static class CharacterHoverPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Chara), nameof(Chara.GetHoverText))]
        private static void AppendCharacterHoverText(Chara __instance, ref string __result)
        {
            if (!ModState.Enabled || !ModConfig.CharacterDisplay.IsEnabled)
                return;

            // vanilla returns the item text for a disguised mimic and stops; do not append on top of it
            if (__instance.mimicry != null && __instance.mimicry.IsThing)
                return;

            __result = CharacterHoverFormatter.AppendMainHoverText(__instance, __result);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Chara), nameof(Chara.GetHoverText2))]
        private static bool ReplaceSecondaryHoverText(Chara __instance, ref string __result)
        {
            if (!ModState.Enabled || !ModConfig.CharacterDisplay.IsEnabled)
                return true;

            // let vanilla handle a disguised mimic itself
            if (__instance.mimicry != null && __instance.mimicry.IsThing)
                return true;

            __result = CharacterHoverFormatter.BuildSecondaryHoverText(__instance);
            return false;
        }
    }
}
