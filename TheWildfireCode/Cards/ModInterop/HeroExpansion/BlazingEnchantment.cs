using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.HeroExpansion;

public class BlazingEnchantment : CustomEnchantmentModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [];
    
    protected override string? CustomIconPath => "res://TheWildfire/images/enchantments/blazing.png";

    private IHoverTip ExertTip()
    {
        LocString title = new LocString("static_hover_tips", "THEWILDFIRE-EXERT.title");
        LocString description = new LocString("static_hover_tips", "THEWILDFIRE-EXERT.description");
        description.Add("Exert", Amount);
        return new HoverTip(title, description);
    }
    
    public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;
    
    public override bool ShowAmount => true;

    public override bool CanEnchant(CardModel card)
    {
        base.CanEnchant(card);
        return !card.Tags.Contains(WildfireKeywords.ExertTag);
    }
    
    public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
    {
        if (!props.IsPoweredAttack())
            return 0M;
        if (Card.Owner.PlayerCombatState == null)
            return 0M;
        int fire = FirepowerController.Firepower.Get(Card.Owner.PlayerCombatState);
        int exert = Amount;
        if (Card.Owner.HasPower<OverdrivePower>() || fire > exert || (Card.TemporaryStarCost != null && Card.TemporaryStarCost.Cost == 0))
        {
            return exert * 2;
        }
        return fire * 2;
    }
    
    public override bool HasExtraCardText => true;

    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        if (Card.Owner.PlayerCombatState == null)
            return;
        int fire = FirepowerController.Firepower.Get(Card.Owner.PlayerCombatState);
        int exert = Amount;
        bool fullExert = false;
        int spend;
        if (Card.Owner.HasPower<OverdrivePower>() || fire >= exert || (Card.TemporaryStarCost != null && Card.TemporaryStarCost.Cost == 0))
        {
            spend = exert;
            fullExert = true;
        }
        else
        {
            spend = fire;
        }
        await FirepowerController.Exert(choiceContext, spend, Card.Owner, fullExert, cardPlay);
    }
}