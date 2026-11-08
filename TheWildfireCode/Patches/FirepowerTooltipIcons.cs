using HarmonyLib;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch(typeof(HoverTipFactory), nameof(HoverTipFactory.Static))]
public class FirepowerTooltipIcons
{
    [HarmonyPostfix]
    static void AddFirepowerIcon(StaticHoverTip tip, ref IHoverTip __result)
    {
        if (tip == WildfireKeywords.Firepower)
            __result = new HoverTip(new LocString("static_hover_tips", "THEWILDFIRE-FIREPOWER.title"), new LocString("static_hover_tips", "THEWILDFIRE-FIREPOWER.description"), PreloadManager.Cache.GetTexture2D("res://TheWildfire/images/charui/firepower_text_icon.png"));
        else if (tip == WildfireKeywords.FullExert)
            __result = new HoverTip(new LocString("static_hover_tips", "THEWILDFIRE-FULL_EXERT.title"), new LocString("static_hover_tips", "THEWILDFIRE-FULL_EXERT.description"), PreloadManager.Cache.GetTexture2D("res://TheWildfire/images/charui/firepower_text_icon.png"));

    }
}