using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch (typeof(TalkCmd), "GetDuration")]
public class TalkCmdPostfix
{
    [HarmonyPostfix]
    public static void ExtraVeryLongDurationPostfix(VfxDuration duration, ref double __result)
    {
        if (duration == WildfireKeywords.WildfireVeryLong)
            __result = 5.0;
    }
}