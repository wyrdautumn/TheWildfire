using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Relics;

public class SparkGuards : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ExertVar(3)];

    public override async Task BeforeSideTurnEndEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner.Creature))
            return;
        int exert = ResolveExert();
        if (exert <= 0)
            return;
        await CreatureCmd.GainBlock(Owner.Creature, exert, ValueProp.Unpowered, null);
        await FirepowerController.Exert(choiceContext, exert, Owner);
    }

    public int ResolveExert()
    {
        if (Owner.PlayerCombatState == null)
            return 0;
        int fire = FirepowerController.Firepower.Get(Owner.PlayerCombatState);
        int exert = DynamicVars["Exert"].IntValue;
        if (fire > exert)
        {
            return exert;
        }
        return fire;
    }
}