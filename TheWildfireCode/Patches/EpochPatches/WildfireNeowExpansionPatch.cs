using HarmonyLib;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using TheWildfire.TheWildfireCode.Epochs;

namespace TheWildfire.TheWildfireCode.Patches.EpochPatches;

[HarmonyPatch(typeof(NeowEpoch), "GetTimelineExpansion")]
public class WildfireNeowExpansionPatch
{
    [HarmonyPostfix]
    public static void Postfix(ref EpochModel[] __result)
    {
        var list = new List<EpochModel>(__result);
        
        var epoch = (EpochModel)Activator.CreateInstance(typeof (Wildfire2Epoch))!;
        if (!list.Any(e => e.Id == epoch.Id))
            list.Add(epoch);

        __result = list.ToArray();
    }
}