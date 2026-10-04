using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Patches;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class TwoTalons() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WildfireKeywords.AttackFlow];

    protected override bool ShouldGlowGoldInternal => OverdriveGlowPatch.CheckFlow(this, CardType.Attack);

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int hits = 2;
        if (FlowSingleton.WillTriggerFlow(this))
            hits += 2;
        await CommonActions.CardAttack(this, play, hits, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}