using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheWildfire.TheWildfireCode.Character;

namespace TheWildfire.TheWildfireCode.Patches;

[HarmonyPatch]
public class WildfireEventPatches
{
    [HarmonyPatch(typeof(ColorfulPhilosophers))]
    public static class WildfirePhilosophersPatch
    {
        [HarmonyPatch("CardPoolColorOrder", MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(ref IEnumerable<CardPoolModel> __result)
        {
            __result = __result.Append(ModelDb.CardPool<TheWildfireCardPool>());
        }
    }

    [HarmonyPatch(typeof(ByrdonisNest), "Eat", MethodType.Async)]
    public class WildfireNestEatPatch
    {
        private const string NewKey = "BYRDONIS_NEST.pages.EAT.wildfireDescription";
    
        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(WildfireNestEatPatch), nameof(ReplaceEventText))
                ]);
        }
        
        private static string ReplaceEventText(string orig, ByrdonisNest instance)
        {
            return instance.Owner?.Character is Character.TheWildfire ? NewKey : orig;
        }
    }
    
    [HarmonyPatch(typeof(ByrdonisNest),"Take", MethodType.Async)]
    public class WildfireNestTakePatch
    {
        private const string NewKey = "BYRDONIS_NEST.pages.TAKE.wildfireDescription";
    
        [HarmonyTranspiler]
        private static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(typeof(EventModel), nameof(EventModel.L10NLookup), [typeof(string)])
                ).Step(-1).Insert([
                    CodeInstruction.LoadLocal(1),
                    CodeInstruction.Call(typeof(WildfireNestTakePatch), nameof(ReplaceEventText))
                ]);
        }
        
        private static string ReplaceEventText(string orig, ByrdonisNest instance)
        {
            return instance.Owner?.Character is Character.TheWildfire ? NewKey : orig;
        }
    }
    
    [HarmonyPatch(typeof(EventModel),"SetInitialEventState")]
    public static class WildfireByrdonisNestInitialPatch
    {
        [HarmonyPostfix]
        public static void Postfix(EventModel __instance)
        {
            if (__instance is ByrdonisNest && __instance.Owner != null && __instance.Owner.Character is Character.TheWildfire)
            {
                __instance.Description = new LocString("events", "BYRDONIS_NEST.pages.INITIAL.wildfireDescription");
            }
        }
    }

}