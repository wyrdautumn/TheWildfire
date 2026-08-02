using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheWildfire.TheWildfireCode.Nodes;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch(typeof(NHealthBar), nameof(NHealthBar.UpdateLayoutForCreatureBounds))]
public class NHealthBarPositionPatch
{
    [HarmonyPostfix]
    public static void UpdateWildfireTrackerPositions(NHealthBar __instance, Godot.Control bounds)
    {
        Control? wildfire = __instance.GetNodeOrNull<Control>("WildfireHPTrackers");
        if (wildfire != null && wildfire is WildfireHpTrackers node)
        {
            node.AdjustBarPositions();
        }
    }
}