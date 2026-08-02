using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Nodes;

namespace TheWildfire.TheWildfireCode.Cards.Status;

[Pool(typeof(StatusCardPool))]
public class Afterburn() : TheWildfireCard(-1,
    CardType.Status, CardRarity.Status,
    TargetType.None)
{
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
            
            var cardContainer = card.GetChild(0)!;
            cardContainer.AddChild(counter);
            
            return counter;
        });
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
    new CalculationBaseVar(0),
    new CalculationExtraVar(1),
    new CalculatedVar("Ignite").WithMultiplier(Calc)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.IgniteStatic),HoverTipFactory.Static(WildfireKeywords.Overheat)];
    public override int MaxUpgradeLevel => 0;
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        return CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>()
            .Count<CardDrawnEntry>((Func<CardDrawnEntry, bool>)(e => e.Actor == card.Owner.Creature && e.Card is Afterburn));
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this || Owner.Creature.CombatState == null)
            return;
        await Cmd.Wait(0.25f);
        int ignite = (int)((CalculatedVar)DynamicVars["Ignite"]).Calculate(Owner.Creature);
        await FirepowerController.Ignite(choiceContext, ignite, Owner);
        await FirepowerController.DealOverheatDamage(choiceContext, Owner);
        await FirepowerHooks.AfterBurnStatusTrigger(Owner.Creature.CombatState, choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {

    }
}