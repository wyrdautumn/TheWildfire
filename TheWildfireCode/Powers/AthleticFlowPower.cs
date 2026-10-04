using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Powers;

public class AthleticFlowPower() : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FlowStatic),HoverTipFactory.FromPower<StrengthPower>(),HoverTipFactory.FromPower<DexterityPower>(),HoverTipFactory.FromPower<ConstitutionPower>()];

    public override async Task FlowAchieved(PlayerChoiceContext choiceContext, Player achiever, CardType type)
    {
        if (achiever.Creature != Owner)
            return;
        if (type == CardType.Attack)
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, Amount, Owner, null);
        if (type == CardType.Skill)
            await PowerCmd.Apply<ConstitutionPower>(choiceContext, Owner, Amount, Owner, null);
        if (type == CardType.Power)
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner, Amount, Owner, null);
    }
}