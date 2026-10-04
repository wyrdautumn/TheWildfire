using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Relics;

public class HeartOfFire() : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IgniteVar(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.Firepower)];

    public override async Task AfterEnergyReset(Player player)
    {
        await FirepowerController.Ignite(new BlockingPlayerChoiceContext(), DynamicVars["Ignite"].IntValue, Owner);
    }
    
    public override RelicModel? GetUpgradeReplacement()
    {
        return ModelDb.Relic<HeartOfSorcery>();
    }
}