using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Relics;

public class ElementalInfuser : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ConstitutionPower>(3)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ConstitutionPower>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return;
        Flash();
        await PowerCmd.Apply<ConstitutionPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["ConstitutionPower"].BaseValue, Owner.Creature,
            null);
    }
}