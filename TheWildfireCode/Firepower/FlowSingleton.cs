using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Firepower;

public class FlowSingleton() : CustomSingletonModel(HookType.Combat)
{
    private static Dictionary<CardModel, int> _flowCards = new Dictionary<CardModel, int>();

    public static bool WillTriggerFlow(CardModel card)
    {
        Dictionary<CardModel, int> flowCards = _flowCards;
        if (flowCards.ContainsKey(card))
        {
            int type = flowCards.GetValueOrDefault(card);
            if (type == 0 && !card.Owner.HasPower<AttackFlowPower>())
                return true;
            if (type == 1 && !card.Owner.HasPower<SkillFlowPower>())
                return true;
            if (type == 2 && !card.Owner.HasPower<PowerFlowPower>())
                return true;
        }
        return false;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(WildfireKeywords.AttackFlow) && !cardPlay.Card.Owner.HasPower<AttackFlowPower>() && CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e =>
                    e.CardPlay.Card.Owner == cardPlay.Card.Owner &&
                    e.CardPlay != cardPlay && e.HappenedThisTurn(cardPlay.Card.CombatState))?.CardPlay.Card.Type == CardType.Attack)
        {
            _flowCards.Add(cardPlay.Card, 0);
        }
        else if (cardPlay.Card.Keywords.Contains(WildfireKeywords.SkillFlow) && !cardPlay.Card.Owner.HasPower<SkillFlowPower>() && CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e =>
                    e.CardPlay.Card.Owner == cardPlay.Card.Owner &&
                    e.CardPlay != cardPlay && e.HappenedThisTurn(cardPlay.Card.CombatState))?.CardPlay.Card.Type == CardType.Skill)
        {
            _flowCards.Add(cardPlay.Card, 1);
        }
        else if (cardPlay.Card.Keywords.Contains(WildfireKeywords.PowerFlow) && !cardPlay.Card.Owner.HasPower<PowerFlowPower>() && CombatManager.Instance.History
                .CardPlaysStarted
                .LastOrDefault(e =>
                    e.CardPlay.Card.Owner == cardPlay.Card.Owner &&
                    e.CardPlay != cardPlay && e.HappenedThisTurn(cardPlay.Card.CombatState))?.CardPlay.Card.Type == CardType.Power)
        {
            _flowCards.Add(cardPlay.Card, 2);
        }
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (_flowCards.ContainsKey(cardPlay.Card))
        {
            _flowCards.Remove(cardPlay.Card, out int type);
            switch (type)
            {
                case 0:
                    if (!cardPlay.Card.Owner.HasPower<AttackFlowPower>())
                        await AchieveFlow(choiceContext, cardPlay.Card.Owner, cardPlay.Card, CardType.Attack);
                    break;
                case 1:
                    if (!cardPlay.Card.Owner.HasPower<SkillFlowPower>())
                        await AchieveFlow(choiceContext, cardPlay.Card.Owner, cardPlay.Card, CardType.Skill);
                    break;
                case 2:
                    if (!cardPlay.Card.Owner.HasPower<PowerFlowPower>())
                        await AchieveFlow(choiceContext, cardPlay.Card.Owner, cardPlay.Card, CardType.Power);
                    break;
            }
        }
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        _flowCards.Clear();
        return Task.CompletedTask;
    }

    public static async Task AchieveFlow(PlayerChoiceContext choiceContext, Player player, CardModel? card, CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Attack:
                await PlayerCmd.GainEnergy(1, player);
                await CardPileCmd.Draw(choiceContext, player);
                await PowerCmd.Apply<AttackFlowPower>(choiceContext, player.Creature, 1, player.Creature, card, true);
                break;
            case CardType.Skill:
                await PlayerCmd.GainEnergy(1, player);
                await CardPileCmd.Draw(choiceContext, player);
                await PowerCmd.Apply<SkillFlowPower>(choiceContext, player.Creature, 1, player.Creature, card, true);
                break;
            case CardType.Power:
                await PlayerCmd.GainEnergy(1, player);
                await CardPileCmd.Draw(choiceContext, player);
                await PowerCmd.Apply<PowerFlowPower>(choiceContext, player.Creature, 1, player.Creature, card, true);
                break;
        }
        if (player.Creature.CombatState == null)
            return;
        await FirepowerHooks.FlowAchieved(player.Creature.CombatState, choiceContext, player, cardType);
    }
}