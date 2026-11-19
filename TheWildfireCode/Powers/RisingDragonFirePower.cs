using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Token;

namespace TheWildfire.TheWildfireCode.Powers;

public class RisingDragonFirePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Afterburn>(), HoverTipFactory.FromCard<Firebolt>(),HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    public override int DisplayAmount
    {
        get => Math.Max(0, Amount - GetInternalData<Data>().AfterburnsPlayed);
    }
    protected override object InitInternalData() => new Data();

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        int afterburns = GetInternalData<Data>().AfterburnsPlayed;
        if (card is Afterburn && card.Owner.Creature == Owner && Amount > afterburns)
        {
            await Firebolt.CreateInHand(card.Owner, Amount, CombatState);
            Flash();
            SetAfterburnsPlayed(afterburns + 1);
        }
    }
    
    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return Task.CompletedTask;
        SetAfterburnsPlayed(0);
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
    
    public void SetAfterburnsPlayed(int value)
    {
        GetInternalData<Data>().AfterburnsPlayed = value;
        InvokeDisplayAmountChanged();
    }
    
    public class Data
    {
        public int AfterburnsPlayed;
    }
}