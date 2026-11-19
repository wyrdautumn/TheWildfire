using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Character;

public class WildfireCombatDialogSingleton() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SavedSpireField<RunState, int> WildfireCultistBanter = new(() => 0, "WildfireCultistsBantered");
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != CombatSide.Player)
            return;
        EncounterModel? encounter = combatState.Encounter;
        if (encounter == null)
            return;
        if (combatState.RoundNumber > 1)
            return;
        Player? wildfire = combatState.Players.FirstOrDefault(p => p.Character is Character.TheWildfire);
        if (wildfire == null)
            return;
        LocString? banter = LocString.GetIfExists("combat_messages", "WILDFIREBANTER." + encounter.Id.Entry);
        if (banter != null)
        {
            TalkCmd.Play(banter, wildfire.Creature, VfxColor.Purple, WildfireKeywords.WildfireVeryLong);
            LocString? banter2 = LocString.GetIfExists("combat_messages", "WILDFIREBANTER." + encounter.Id.Entry + ".reply");
            Creature? monster = GetTalkingMonster(encounter, combatState);
            if (banter2 != null && monster != null)
            {
                await Cmd.CustomScaledWait(0.4f, 1);
                TalkCmd.Play(banter2, monster, GetMonsterVfxColor(encounter), WildfireKeywords.WildfireVeryLong);
            }
        }
    }

    private VfxColor GetMonsterVfxColor(EncounterModel encounter)
    {
        return VfxColor.Blue;
    }

    private Creature? GetTalkingMonster(EncounterModel encounter, ICombatState combatState)
    {
        if (encounter is SeapunkNormal)
            return combatState.Enemies.Where(c => c.Monster != null && c.Monster is CalcifiedCultist).FirstOrDefault();
        return combatState.Enemies.FirstOrDefault();
    }
    
        [HarmonyPatch(typeof(DampCultist), nameof(DampCultist.IncantationMove))]
    public static class DampDialogPatch
    {
        [HarmonyPostfix]
        private static async void AddDampCultistDialog(Task __result, CalcifiedCultist __instance)
        {
            await __result;
            Player? wildfire = __instance.CombatState.Players.FirstOrDefault(p => p.Character is Character.TheWildfire);
            RunState? runState = __instance.CombatState.RunState as RunState;
            if (wildfire == null || runState == null)
                return;
            int num = WildfireCultistBanter.Get(runState);
            if (num == 0)
            {
                WildfireCultistBanter.Set(runState, 1);
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.CAWCAW.1"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
            }
            else if (num == 1)
            {
                WildfireCultistBanter.Set(runState, 2);
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.CAWCAW.2"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
            }
        }
    }
    
    [HarmonyPatch(typeof(DevotedSculptor), nameof(DevotedSculptor.ForbiddenIncantationMove))]
    public static class SculptorDialogPatch
    {
        [HarmonyPostfix]
        private static async void AddSculptorDialog(Task __result, CalcifiedCultist __instance)
        {
            await __result;
            Player? wildfire = __instance.CombatState.Players.FirstOrDefault(p => p.Character is Character.TheWildfire);
            RunState? runState = __instance.CombatState.RunState as RunState;
            if (wildfire == null || runState == null)
                return;
            int num = WildfireCultistBanter.Get(runState);
            if (num == 0)
            {
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.1"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
            }
            else if (num == 1)
            {
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.2"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
            }
            else if (num == 2)
            {
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.3"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
            }
        }
    }
    
    [HarmonyPatch(typeof(ThievingHopper), nameof(ThievingHopper.ThieveryMove))]
    public static class HopperDialogPatch
    {
        [HarmonyPostfix]
        private static async void AddHopperDialog(Task __result, CalcifiedCultist __instance)
        {
            await __result;
            Player? wildfire = __instance.CombatState.Players.FirstOrDefault(p => p.Character is Character.TheWildfire);
            if (wildfire == null)
                return;
            TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.HOPPER"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
        }
    }
}