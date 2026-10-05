using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Hooks;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Nodes;
using TheWildfire.TheWildfireCode.Powers;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Firepower;

public class FirepowerController() : CustomSingletonModel(HookType.Combat)
{
    public static readonly SpireField<PlayerCombatState, int> Firepower = new(() => 0);
    
    public static readonly AddedNode<NEnergyCounter, WildfireFireCounter> FireCounterNode =
        new(parent =>
        {
            var counter = new WildfireFireCounter
            {
                Name = "FireCounter",
                MouseFilter = Control.MouseFilterEnum.Ignore
            };

            counter.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
            counter.Position = Vector2.Zero;
            counter.Size = new Vector2(128, 128);
            counter.ZIndex = 0;

            var visualScene = ResourceLoader.Load<PackedScene>(
                "res://TheWildfire/scenes/fire counter.tscn");

            var visual = visualScene.Instantiate<Control>();
            visual.Name = "FirepowerVisual";
            visual.MouseFilter = Control.MouseFilterEnum.Ignore;

            counter.AddChild(visual);
            
            return counter;
        });

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (cardSource != null && cardSource.Tags.Contains(WildfireKeywords.ShowAfterburn) && cardSource.Owner.PlayerCombatState != null && (props.IsPoweredAttack() || cardSource.Type == CardType.Skill))
            return Afterburn.AfterburnCount.Get(cardSource.Owner.PlayerCombatState);
        return 0;
    }

    public static async Task Ignite(PlayerChoiceContext choiceContext, int ignite, Player player)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null || ignite <= 0)
            return;
        int val = Firepower.Get(player.PlayerCombatState);
        int increase = val + ignite;
        SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_select");
        Firepower.Set(player.PlayerCombatState, increase);
        await FirepowerHooks.AfterIgnite(player.Creature.CombatState, choiceContext, increase, player);
    }

    public static async Task Scorch(PlayerChoiceContext choiceContext, Player player, bool damage = true, int repeat = 1)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null)
            return;
        int val = Firepower.Get(player.PlayerCombatState);
        if (val < 1)
            return;
        for(int i = repeat; i > 0; i--)
        {
            NCombatRoom? instance = NCombatRoom.Instance;
            NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(player.Creature);
            if (instance != null && creatureNode != null)
                instance.CombatVfxContainer.AddChildSafely(NFireBurningVfx.Create(creatureNode.GetBottomOfHitbox(), 0.75f, false, Color.FromHtml("#ff97fe")));
            SfxCmd.Play("event:/sfx/characters/attack_fire");
            await CreatureCmd.Damage(choiceContext, player.Creature.CombatState.HittableEnemies, val,
                ValueProp.Unpowered, player.Creature);
            if (damage)
                await DealOverheatDamage(choiceContext, player, true);
            await FirepowerHooks.AfterScorch(player.Creature.CombatState, choiceContext, val, player);
        }
    }

    public static async Task Exert(PlayerChoiceContext choiceContext, int amount, Player player, bool fullExert, bool fromCard = true)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null)
            return;
        int spent = amount;
        PowerModel? overdrive = player.Creature.GetPower<OverdrivePower>();
        if (overdrive != null && fromCard)
            await PowerCmd.Decrement(overdrive);
        else
        {
            int val = Firepower.Get(player.PlayerCombatState);
            int result = val - amount;
            if (result < 0)
            {
                result = 0;
                spent = val;
            }
            Firepower.Set(player.PlayerCombatState, result);
        }
        await FirepowerHooks.AfterExert(player.Creature.CombatState, choiceContext, spent, player, fullExert);
    }

    public static async Task<int> ExertAll(PlayerChoiceContext choiceContext, Player player, bool fullExert)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null)
            return 0;
        int val = Firepower.Get(player.PlayerCombatState);
        PowerModel? overdrive = player.Creature.GetPower<OverdrivePower>();
        if (overdrive != null)
            await PowerCmd.Decrement(overdrive);
        else
            Firepower.Set(player.PlayerCombatState, 0);
        await FirepowerHooks.AfterExert(player.Creature.CombatState, choiceContext, val, player, fullExert);
        return val;
    }
    
    public static async Task DealOverheatDamage(PlayerChoiceContext choiceContext, Player player, bool skipEffect = false)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null)
            return;
        int hpLoss = CalculateOverheatDamage(player);
        if (hpLoss > 0)
        {
            if (hpLoss > 2 && skipEffect == false)
            {
                NCombatRoom? instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(player.Creature,
                        VfxColor.Purple));
                SfxCmd.Play("event:/sfx/characters/attack_fire");
            }
            await CreatureCmd.Damage(choiceContext, player.Creature, hpLoss, ValueProp.Unpowered,
                player.Creature, null, null);
        }
        await FirepowerHooks.AfterOverheatDamage(player.Creature.CombatState, choiceContext, hpLoss,
            CalculateOverheatDamageWithoutMitigation(player), player);
    }

    public static int CalculateOverheatDamage(Player player)
    {
        if (player.PlayerCombatState == null)
            return 0;
        int val = Firepower.Get(player.PlayerCombatState) / 10;
        return val;
    }
    
    public static int CalculateOverheatDamageWithoutMitigation(Player player)
    {
        if (player.PlayerCombatState == null)
            return 0;
        int val = Firepower.Get(player.PlayerCombatState) / 10;
        return val;
    }
}