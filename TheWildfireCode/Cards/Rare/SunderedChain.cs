using BaseLib.Patches.Localization;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class SunderedChain() : TheWildfireCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new OverflowVar(4)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromPower<VulnerablePower>(),HoverTipFactory.FromPower<FrailPower>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.OverflowTag };
    }

    protected override bool IsPlayable
    {
        get
        {
            var ownerPlayerCombatState = this.Owner.PlayerCombatState;
            return ownerPlayerCombatState != null &&
                   FirepowerController.Firepower.Get(ownerPlayerCombatState) >=
                   this.DynamicVars["Overflow"].IntValue;
        }
    }

    protected override bool ShouldGlowGoldInternal => IsPlayable;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (Owner.PlayerCombatState == null)
            return;
        PowerModel? weak = Owner.Creature.GetPower<WeakPower>();
        PowerModel? vulnerable = Owner.Creature.GetPower<VulnerablePower>();
        PowerModel? frail = Owner.Creature.GetPower<FrailPower>();
        if (weak != null)
            await PowerCmd.Remove(weak);
        if (vulnerable != null)
            await PowerCmd.Remove(vulnerable);
        if (frail != null)
            await PowerCmd.Remove(frail);
        foreach (CardModel status in Owner.PlayerCombatState.AllCards.Where(c =>
                     c.Type == CardType.Status && c.Pile != null && c.Pile.Type != PileType.Exhaust))
        {
            await CardCmd.Exhaust(choiceContext, status);
        }
        await FirepowerController.Overflow(choiceContext, DynamicVars["Overflow"].IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
        List<IHoverTip> hoverTips = new List<IHoverTip>();
        hoverTips.Add(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
        ExtraTooltips.AddTips(hoverTips, this);
    }
}