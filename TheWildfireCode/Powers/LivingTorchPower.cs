using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;

namespace TheWildfire.TheWildfireCode.Powers;

public class LivingTorchPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Burn>(),HoverTipFactory.FromCard<Afterburn>(),HoverTipFactory.FromKeyword(CardKeyword.Ethereal)];

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (Owner.Player == null || Owner.Player.PlayerCombatState == null)
            return Task.CompletedTask;
        foreach (CardModel card in Owner.Player.PlayerCombatState.AllCards.Where(c => c is Burn or Afterburn))
        {
            CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
        }
        return Task.CompletedTask;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner.Creature == Owner && card is Burn or Afterburn)
            CardCmd.ApplyKeyword(card, CardKeyword.Ethereal);
        return Task.CompletedTask;
    }
}