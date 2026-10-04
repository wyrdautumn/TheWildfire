using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class FistOfTheThousandthSun() : TheWildfireCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    private int _hits = 1;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move), new DynamicVar("hits", 1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<SolarPower>(IsUpgraded)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars["hits"].IntValue, vfx: "vfx/vfx_attack_blunt").Execute(choiceContext);
        DynamicVars["hits"].BaseValue += 1;
        _hits += 1;
        if (CombatState == null)
            return;
        var solarPowers = await SolarPower.CreateInHand(Owner, 1, CombatState);
        foreach (CardModel card in solarPowers)
            CardCmd.Upgrade(card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
    
    protected override void AfterDowngraded()
    {
        DynamicVars["hits"].BaseValue = _hits;
    }
}