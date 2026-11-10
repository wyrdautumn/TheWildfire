using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class EverBurningSoul() : TheWildfireCard(1,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<EverBurningSoulPower>(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        await CommonActions.ApplySelf<EverBurningSoulPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["EverBurningSoulPower"].UpgradeValueBy(1);
    }
}