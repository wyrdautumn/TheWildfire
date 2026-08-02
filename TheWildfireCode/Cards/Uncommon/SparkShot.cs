using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class SparkShot() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move),
    new CalculationBaseVar(1),
    new CalculationExtraVar(1),
    new CalculatedVar("Sunstrikes").WithMultiplier(Calc)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<SunStrike>(IsUpgraded)];
    
    private static decimal Calc(CardModel card, Creature? target)
    {
        return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count(
            (Func<DamageReceivedEntry, bool>)(e =>
                e.Receiver == target && e.Dealer == card.Owner.Creature && e.Result.Props.IsPoweredAttack() &&
                e.HappenedThisTurn(card.CombatState)));
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        AttackCommand attackCommand = await CommonActions.CardAttack(this, play, vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        if (CombatState == null)
            return;
        IEnumerable<CardModel> inHand = await SunStrike.CreateInHand(Owner, (int) ((CalculatedVar)DynamicVars["Sunstrikes"]).Calculate(play.Target) - attackCommand.Results.Count(),
            CombatState);
        if (!IsUpgraded)
            return;
        foreach (CardModel card in inHand)
            CardCmd.Upgrade(card);
    }

    protected override void OnUpgrade()
    {
        
    }
}