using HarmonyLib;

namespace GanExtendDisplay
{
    internal static class NotificationPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(NotificationCondition), nameof(NotificationCondition.OnRefresh))]
        private static bool RefreshCondition(NotificationCondition __instance)
        {
            if (!ModState.Enabled || !ModConfig.NotificationDisplay.IsEnabled)
                return true;

            __instance.text = __instance.condition.GetText() + " " + __instance.condition.value;
            __instance.item.button.mainText.color = __instance.condition.GetColor(__instance.item.button.skinRoot.GetButton().colorProf);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NotificationStats), nameof(NotificationStats.OnRefresh))]
        private static bool RefreshStats(NotificationStats __instance)
        {
            if (!ModState.Enabled || !ModConfig.NotificationDisplay.IsEnabled)
                return true;

            BaseStats stats = __instance.stats();
            string text = stats.GetText();
            __instance.text = text + (!text.IsEmpty() ? "(" + stats.GetValue() + ")" : string.Empty);
            __instance.item.button.mainText.color = stats.GetColor(__instance.item.button.skinRoot.GetButton().colorProf);
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NotificationBuff), nameof(NotificationBuff.OnRefresh))]
        private static bool RefreshBuff(NotificationBuff __instance)
        {
            if (!ModState.Enabled || !ModConfig.NotificationDisplay.IsEnabled)
                return true;

            if (__instance.item.button.icon.sprite == EClass.core.refs.spriteDefaultCondition)
                __instance.OnInstantiate();

            __instance.text = __instance.condition.GetText() + " " + __instance.condition.value;
            __instance.item.textDuration.SetText(__instance.condition.TextDuration);
            __instance.item.textDuration.SetActive(__instance.condition.HasDuration);
            return false;
        }
    }
}
