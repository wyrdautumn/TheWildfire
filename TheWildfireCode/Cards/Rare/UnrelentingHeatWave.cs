using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class UnrelentingHeatWave() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust),HoverTipFactory.FromCard<Firebolt>(),HoverTipFactory.FromCard<Firebolt>(true)];


    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 0, 2);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (card == null)
            return;
        await CardCmd.Exhaust(choiceContext, card);
        CardPile? draw = CardPile.Get(PileType.Draw, Owner);
        CardPile? exhaust = CardPile.Get(PileType.Exhaust, Owner);
        if (draw == null || exhaust == null || CombatState == null)
            return;
        var firebolt = await Firebolt.CreateInHand(Owner, 1, CombatState);
        if (draw.Cards.Count < exhaust.Cards.Count)
        {
            foreach (CardModel card2 in firebolt)
                CardCmd.Upgrade(card2);
        }
            
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}