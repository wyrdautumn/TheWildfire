using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Hooks;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
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

public class FirepowerController() : CustomSingletonModel(HookType.Combat),IHealthBarForecastSource
{
    public static readonly SpireField<PlayerCombatState, int> Firepower = new(() => 0);
    public static readonly SpireField<PlayerCombatState, bool> FirstFlare = new(() => true);

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
    
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        int amount;
        if (context.Creature.Player == null || context.Creature.Player.PlayerCombatState == null || !context.Creature.IsAlive)
            amount = 0;
        else
        {
            Player player = context.Creature.Player;
            amount = CalculateOverheatDamage(player);
            ConstitutionPower? constitution = player.Creature.GetPower<ConstitutionPower>();
            if (constitution != null)
                amount -= constitution.GetAbsorbRemaining();
            if (amount < 0)
                amount = 0;
        }
        return [new HealthBarForecastSegment(amount, Color.FromHtml("#fa95ff"), HealthBarForecastDirection.FromRight)];
    }
    
    public static async Task Ignite(PlayerChoiceContext choiceContext, int ignite, Player player)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null)
            return;
        int val = Firepower.Get(player.PlayerCombatState);
        int increase = val + ignite;
        if (player.GetRelic<BoxOfTinder>() != null)
            increase += 1;
        NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(player.Creature);
        if (creatureNode != null)
        {
            NFireBurstVfx? child = NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("b18aff"));
            if (child != null)
            {
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                NCombatRoom? instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
            }
        }
        Firepower.Set(player.PlayerCombatState, increase);
        await FirepowerHooks.AfterIgnite(player.Creature.CombatState, choiceContext, increase, player);
    }

    public static async Task Scorch(PlayerChoiceContext choiceContext, Player player, int repeat = 1)
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
                ValueProp.Unblockable | ValueProp.Unpowered, player.Creature);
            await DealOverheatDamage(choiceContext, player, true);
            await FirepowerHooks.AfterScorch(player.Creature.CombatState, choiceContext, val, player);
        }
    }

    public static async Task Exert(PlayerChoiceContext choiceContext, int amount, Player player)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null)
            return;
        int val = Firepower.Get(player.PlayerCombatState);
        int spent = amount;
        int result = val - amount;
        if (result < 0)
        {
            result = 0;
            spent = val;
        }
        Firepower.Set(player.PlayerCombatState, result);
        await FirepowerHooks.AfterExert(player.Creature.CombatState, choiceContext, spent, player);
    }

    public static async Task Flare(PlayerChoiceContext choiceContext, int amount, Player player)
    {
        if (player.PlayerCombatState == null || player.Creature.CombatState == null)
            return;
        int spent = 0;
        if (player.HasPower<OverdrivePower>())
            await PowerCmd.Decrement(player.Creature.GetPower<OverdrivePower>() ?? throw new InvalidOperationException());
        else
        {
            spent = Firepower.Get(player.PlayerCombatState);
            if (player.GetRelic<FireflowWraps>() != null)
            {
                int val = Firepower.Get(player.PlayerCombatState);
                int result = val - amount;
                if (result < 0)
                    result = 0;
                Firepower.Set(player.PlayerCombatState, result);
            }
            else
            {
                Firepower.Set(player.PlayerCombatState, 0);
            }
        }
        if (FirstFlare.Get(player.PlayerCombatState))
        {
            int energy = player.Creature.GetPowerAmount<OverflowMasteryPower>();
            if (energy > 0)
            {
                await PlayerCmd.GainEnergy(energy, player);
            }
            FirstFlare.Set(player.PlayerCombatState, false);
        }
        await FirepowerHooks.AfterFlare(player.Creature.CombatState, choiceContext, player, spent);
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
            await CreatureCmd.Damage(choiceContext, player.Creature, hpLoss, ValueProp.Unpowered | ValueProp.Unblockable,
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

    public override Task AfterEnergyReset(Player player)
    {
        if (player.PlayerCombatState != null)
            FirstFlare.Set(player.PlayerCombatState, true);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = cardPlay.Card;
        Player owner = cardPlay.Card.Owner;
        if (!card.Keywords.Contains(WildfireKeywords.Afterburn) || card.CombatState == null || owner.Creature.CombatState == null)
            return;
        CardModel afterburn = card.CombatState.CreateCard<Afterburn>(owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(afterburn, PileType.Draw, owner,CardPilePosition.Random), 1.5F);
        await FirepowerHooks.AfterburnPlayed(owner.Creature.CombatState, choiceContext, owner);
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (dealer == null || dealer.Player == null || dealer.Player.PlayerCombatState == null || !props.IsPoweredAttack() || cardSource == null ||
            !cardSource.Keywords.Contains(WildfireKeywords.Discipline))
            return 0;
        decimal val = Firepower.Get(dealer.Player.PlayerCombatState);
        val -= dealer.GetPowerAmount<WalkThroughTheConflagrationPower>();
        if (val <= 0)
            return 0;
        return -val;
    }

    public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (cardSource == null || cardSource.Owner.PlayerCombatState == null || !cardSource.Keywords.Contains(WildfireKeywords.Discipline) ||
            !props.IsPoweredCardOrMonsterMoveBlock() || cardSource.Owner.Creature != target)
            return 0;
        decimal val = Firepower.Get(cardSource.Owner.PlayerCombatState);
        val -= cardSource.Owner.Creature.GetPowerAmount<WalkThroughTheConflagrationPower>();
        if (val <= 0)
            return 0;
        return -val;
    }


    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        foreach (Creature creature in participants.Where(c => c.IsPlayer).ToList())
        {
            Player? player = creature.Player;
            if (player == null || player.PlayerCombatState == null)
                continue;
            PowerModel? nimbus = creature.GetPower<SustainingFlamePower>();
            int val = Firepower.Get(player.PlayerCombatState);
            if (val >= 10)
            {
                if (nimbus != null)
                {
                    await Scorch(choiceContext, player, nimbus.Amount);
                }
                else
                {
                    await DealOverheatDamage(choiceContext, player);
                }
            }
        }
    }
}