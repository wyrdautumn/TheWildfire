using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Relics;

public class HeartOfSorcery() : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Ignite",3), new PowerVar<ConstitutionPower>(4)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.Firepower),HoverTipFactory.Static(WildfireKeywords.IgniteStatic),HoverTipFactory.FromPower<ConstitutionPower>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        Flash();
        await PowerCmd.Apply<ConstitutionPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["ConstitutionPower"].BaseValue, Owner.Creature,
            null);
    }

    public override async Task AfterEnergyReset(Player player)
    {
        await FirepowerController.Ignite(new ThrowingPlayerChoiceContext(), DynamicVars["Ignite"].IntValue, Owner);
    }
}