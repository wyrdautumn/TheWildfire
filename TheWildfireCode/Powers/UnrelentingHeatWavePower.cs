using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards.Token;

namespace TheWildfire.TheWildfireCode.Powers;

public class UnrelentingHeatWavePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust),HoverTipFactory.FromPower<HeatExhaustionPower>()];
    
    public override int DisplayAmount
    {
        get => Math.Max(0, Amount - GetInternalData<Data>().cardsExhausted);
    }
    
    protected override object InitInternalData() => new Data();

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        SetCardsExhausted(CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Count(e => e.HappenedThisTurn(Owner.CombatState) && e.Actor == Owner));
        return Task.CompletedTask;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner != Owner.Player)
            return;
        if (GetInternalData<Data>().cardsExhausted >= Amount)
        {
            SetCardsExhausted(GetInternalData<Data>().cardsExhausted + 1);
            return;
        }
        Flash();
        await PowerCmd.Apply<HeatExhaustionPower>(choiceContext, CombatState.HittableEnemies, 1, Owner, null);
        SetCardsExhausted(GetInternalData<Data>().cardsExhausted + 1);
        InvokeDisplayAmountChanged();
    }

    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner))
            return Task.CompletedTask;
        SetCardsExhausted(0);
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
    
    private void SetCardsExhausted(int value)
    {
        GetInternalData<Data>().cardsExhausted = value;
        InvokeDisplayAmountChanged();
    }
    
    private class Data
    {
        public int cardsExhausted;
    }
}