using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
public class Afterburn() : TheWildfireCard(2,
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
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.Held)];
    public override int MaxUpgradeLevel => 0;
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        return CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>()
            .Count<CardDrawnEntry>((Func<CardDrawnEntry, bool>)(e => e.Actor == card.Owner.Creature && e.Card is Afterburn));
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

    protected override void OnUpgrade()
    {

    }
}