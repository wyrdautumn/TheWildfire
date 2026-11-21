using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Character;

public class WildfireCombatDialogSingleton() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SavedSpireField<RunState, int> WildfireCultistBanter = new(() => 0, "WildfireCultistsBantered");
    private bool wildfireAttackBantered = false;
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != CombatSide.Player)
            return;
        EncounterModel? encounter = combatState.Encounter;
        if (encounter == null)
            return;
        if (combatState.RoundNumber > 1)
            return;
        wildfireAttackBantered = false;
        Player? wildfire = combatState.Players.FirstOrDefault(p => p.Character is Character.TheWildfire);
        Player? sorceress = combatState.Players.FirstOrDefault(p => p.Character.Id.Entry == "THESORCERESSMOD-THE_SORCERESS_MOD");
        if (wildfire == null)
            return;
        if ((encounter is CultistsNormal || encounter is PunchOffEventEncounter || encounter is MysteriousKnightEventEncounter || encounter is QueenBoss) && sorceress != null)
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
    
    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer == null || dealer.Player == null || dealer.Player.Character is not Character.TheWildfire || wildfireAttackBantered)
            return Task.CompletedTask;
        EncounterModel? encounter = dealer.CombatState?.Encounter;
        if (dealer.CombatState == null || encounter == null || encounter.RoomType != RoomType.Boss)
            return Task.CompletedTask;
        if (encounter is CeremonialBeastBoss)
        {
            if (result.TotalDamage >= 45)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_BEAST_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_BEAST_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is TheKinBoss)
        {
            if (result.TotalDamage >= 45)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_KIN_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_KIN_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is VantomBoss)
        {
            if (result.TotalDamage >= 45)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_VANTOM_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_VANTOM_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is WaterfallGiantBoss)
        {
            if (result.TotalDamage >= 45)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25 || (double)target.CurrentHp / target.MaxHp > .5)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_WATERFALL_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_WATERFALL_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is SoulFyshBoss)
        {
            if (result.TotalDamage >= 45)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_FISH_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_FISH_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is LagavulinMatriarchBoss)
        {
            if (result.TotalDamage >= 45)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_MATRIARCH_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_MATRIARCH_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is KnowledgeDemonBoss)
        {
            if (result.TotalDamage >= 65)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_DEMON_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_DEMON_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is KaiserCrabBoss)
        {
            if (result.TotalDamage >= 65)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_CRAB_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_CRAB_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is TheInsatiableBoss)
        {
            if (result.TotalDamage >= 65)
            {
                SandpitPower? sandpitPower = target.Powers.OfType<SandpitPower>().FirstOrDefault(s => s.Target == dealer);
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25 && sandpitPower == null || (sandpitPower != null && sandpitPower.Amount > 1))
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_INSATIABLE_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_INSATIABLE_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is QueenBoss)
        {
            if (result.TotalDamage >= 80)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_QUEEN_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_QUEEN_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is TestSubjectBoss)
        {
            if (result.TotalDamage >= 80)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_SUBJECT_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_SUBJECT_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
            return Task.CompletedTask;
        }
        if (encounter is AeonglassBoss)
        {
            if (result.TotalDamage >= 80)
            {
                if ((double)dealer.CurrentHp / dealer.MaxHp > .25)
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_GLASS_BOSS_ATTACK_HIGH"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                else
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE_GLASS_BOSS_ATTACK_LOW"),dealer,VfxColor.Purple,WildfireKeywords.WildfireVeryLong);
                wildfireAttackBantered = true;
            }
        }
        return Task.CompletedTask;
    }
    
    [HarmonyPatch(typeof(DampCultist), nameof(DampCultist.IncantationMove))]
    public static class DampDialogPatch
    {
        [HarmonyPostfix]
        private static async void AddDampCultistDialog(Task __result, CalcifiedCultist __instance)
        {
            await __result;
            Player? wildfire = __instance.CombatState.Players.FirstOrDefault(p => p.Character is Character.TheWildfire);
            Player? sorceress = __instance.CombatState.Players.FirstOrDefault(p => p.Character.Id.Entry == "THESORCERESSMOD-THE_SORCERESS_MOD");
            RunState? runState = __instance.CombatState.RunState as RunState;
            if (wildfire == null || runState == null)
                return;
            int num = WildfireCultistBanter.Get(runState);
            if (num == 0)
            {
                WildfireCultistBanter.Set(runState, 1);
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.CAWCAW.1"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
                if (sorceress != null)
                {
                    await Cmd.CustomScaledWait(0.4f, 1);
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.CAWCAW.1.reply"), sorceress.Creature,
                        VfxColor.Purple, VfxDuration.Standard);
                }
            }
            else if (num == 1)
            {
                WildfireCultistBanter.Set(runState, 2);
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.CAWCAW.2"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
                if (sorceress != null)
                {
                    await Cmd.CustomScaledWait(0.4f, 1);
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.CAWCAW.2.reply"), sorceress.Creature,
                        VfxColor.Purple, VfxDuration.Standard);
                }
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
            Player? sorceress = __instance.CombatState.Players.FirstOrDefault(p => p.Character.Id.Entry == "THESORCERESSMOD-THE_SORCERESS_MOD");
            RunState? runState = __instance.CombatState.RunState as RunState;
            if (wildfire == null || runState == null)
                return;
            int num = WildfireCultistBanter.Get(runState);
            if (num == 0)
            {
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.1"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
                if (sorceress != null)
                {
                    await Cmd.CustomScaledWait(0.4f, 1);
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.1.reply"), sorceress.Creature,
                        VfxColor.Purple, VfxDuration.Standard);
                }
            }
            else if (num == 1)
            {
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.2"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
                if (sorceress != null)
                {
                    await Cmd.CustomScaledWait(0.4f, 1);
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.2.reply"), sorceress.Creature,
                        VfxColor.Purple, VfxDuration.Standard);
                }
            }
            else if (num == 2)
            {
                TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.3"), wildfire.Creature, VfxColor.Purple, VfxDuration.Standard);
                if (sorceress != null)
                {
                    await Cmd.CustomScaledWait(0.4f, 1);
                    TalkCmd.Play(new LocString("combat_messages", "WILDFIRE.BIGCAW.3.reply"), sorceress.Creature,
                        VfxColor.Purple, VfxDuration.Standard);
                }
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