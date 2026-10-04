using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Nodes;

namespace TheWildfire.TheWildfireCode.Cards.Status;

[Pool(typeof(StatusCardPool))]
public class Afterburn() : TheWildfireCard(1,
    CardType.Status, CardRarity.Status,
    TargetType.None)
{
    public static readonly SpireField<PlayerCombatState, int> AfterburnCount = new(() => 0);
    public static readonly AddedNode<NCard, AfterburnCounter> FireCounterNode =
        new((card) =>
        {
            var counter = new AfterburnCounter
            {
                Name = "AfterburnCounter",
                MouseFilter = Control.MouseFilterEnum.Ignore
            };

            counter.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
            counter.Position = Vector2.Zero;
            counter.Size = new Vector2(64, 64);
            counter.ZIndex = 0;

            var visualScene = ResourceLoader.Load<PackedScene>(
                "res://TheWildfire/scenes/AfterburnCounter.tscn");

            var visual = visualScene.Instantiate<Control>();
            visual.Name = "AfterburnCounterVisual";
            visual.MouseFilter = Control.MouseFilterEnum.Ignore;

            counter.AddChild(visual);
            
            var cardContainer = card.GetChild(0);
            if (cardContainer == null)
                return counter;
            cardContainer.AddChild(counter);
            
            return counter;
        });
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(0),
    new CalculationExtraVar(1),
    new CalculatedVar("Ignite").WithMultiplier(Calc)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ShowAfterburn };
    }
    
    public override bool HasTurnEndInHandEffect => true;
    
    public override int MaxUpgradeLevel => 0;
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card.Owner.PlayerCombatState == null)
            return 0;
        return AfterburnCount.Get(card.Owner.PlayerCombatState);
    }
    
    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        NCombatRoom? instance = NCombatRoom.Instance;
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(Owner.Creature, VfxColor.Purple));
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        await CreatureCmd.Damage(choiceContext, Owner.Creature, ((CalculatedVar)DynamicVars["Ignite"]).Calculate(Owner.Creature),
            ValueProp.Unpowered | ValueProp.Move, this, null);
    }

    public static async Task CreateAfterburn(int afterburn, Player owner, ICombatState combatState)
    {
        if (owner.PlayerCombatState == null)
            return;
        int val = AfterburnCount.Get(owner.PlayerCombatState);
        AfterburnCount.Set(owner.PlayerCombatState, val + afterburn);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(combatState.CreateCard<Afterburn>(owner), PileType.Discard, owner));
        await Cmd.Wait(0.5f);
    }
}