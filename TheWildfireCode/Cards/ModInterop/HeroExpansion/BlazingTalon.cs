using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.HeroExpansion;

public class BlazingTalon() : TheWildfireHeroExpansionCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BlazingTalonPower>(1), new PowerVar<BlazingTalonUpgradePower>(1)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddHeroExpansion(), HoverTipFactory.FromCard<TalonSurge>(IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        if (IsUpgraded)
        {
            await CommonActions.ApplySelf<BlazingTalonUpgradePower>(choiceContext, this);
            return;
        }
        await CommonActions.ApplySelf<BlazingTalonPower>(choiceContext, this);
    }

    protected override void OnUpgrade()
    {

    }
}