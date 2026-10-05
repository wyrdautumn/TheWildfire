using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Token;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class SparkShot() : TheWildfireCard(0,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0),
        new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(Calc)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.AfterburnStatic),HoverTipFactory.FromCard<Afterburn>(),HoverTipFactory.FromCard<Firebolt>(IsUpgraded),HoverTipFactory.FromCard<Burn>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ShowAfterburn };
    }
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card.Owner.PlayerCombatState == null)
            return 0;
        return Afterburn.AfterburnCount.Get(card.Owner.PlayerCombatState);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        if (CombatState == null)
            return;
        CardModel firebolt = CombatState.CreateCard<Firebolt>(Owner);
        if (IsUpgraded)
            CardCmd.Upgrade(firebolt);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(firebolt, PileType.Draw, Owner,
            CardPilePosition.Random));
        await Cmd.Wait(0.5f);
        CardModel burn = CombatState.CreateCard<Burn>(Owner);
        await CardPileCmd.AddGeneratedCardToCombat(burn, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
    }
}