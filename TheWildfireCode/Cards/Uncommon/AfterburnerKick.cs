using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class AfterburnerKick() : TheWildfireCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(18, ValueProp.Move), new IgniteVar(5), new AfterburnVar(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Afterburn>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ShowAfterburn };
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").Execute(choiceContext);
        await FirepowerController.Ignite(choiceContext, DynamicVars["Ignite"].IntValue, Owner);
        await Afterburn.CreateAfterburn(DynamicVars["Afterburn"].IntValue, Owner, CombatState);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars["Ignite"].UpgradeValueBy(2);
    }
}