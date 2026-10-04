using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Powers;

public class ThermalNimbusPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.IgniteStatic)];

    public override async Task AfterIgnite(PlayerChoiceContext choiceContext, int amount, Player igniter)
    {
        for (int i = Amount; i > 0; i--)
        {
            NCombatRoom? instance = NCombatRoom.Instance;
            foreach (Creature creature in CombatState.HittableEnemies)
            {
                NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(creature);
                if (instance != null && creatureNode != null)
                    instance.CombatVfxContainer.AddChildSafely(NFireBurningVfx.Create(creatureNode.GetBottomOfHitbox(),
                        0.75f, false, Color.FromHtml("#ff97fe")));
            }
            await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, amount, ValueProp.Unpowered | ValueProp.Unblockable, Owner);
        }
    }
}