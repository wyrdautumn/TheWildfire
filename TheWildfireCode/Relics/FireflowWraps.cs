using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Relics;

public class FireflowWraps() : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IgniteVar(4)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FlowStatic)];

    public override async Task FlowAchieved(PlayerChoiceContext choiceContext, Player achiever, CardType type)
    {
        if (achiever == Owner)
            await FirepowerController.Ignite(choiceContext, DynamicVars["Ignite"].IntValue, Owner);
    }
}