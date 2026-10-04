using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;

namespace TheWildfire.TheWildfireCode.Powers;

public class HeatVentPower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust),HoverTipFactory.Static(WildfireKeywords.AfterburnStatic), HoverTipFactory.FromCard<Afterburn>()];

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner.Creature == Owner && card.Type == CardType.Status && card.Owner.PlayerCombatState != null)
        {
            for (int i = Amount; i > 0; i--)
            {
                int afterburn = Afterburn.AfterburnCount.Get(card.Owner.PlayerCombatState);
                Creature? target = card.Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
                if (target == null)
                    continue;
                NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
                if (creatureNode != null)
                {
                    NFireBurningVfx? child =
                        NFireBurningVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, true, new Color("7a37a8"));
                    if (child == null)
                        continue;
                    SfxCmd.Play("event:/sfx/characters/attack_fire");
                    NCombatRoom? instance = NCombatRoom.Instance;
                    if (instance != null)
                        instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                }
                await CreatureCmd.Damage(choiceContext, target, afterburn, ValueProp.Unpowered, null, null);
            }
        }
    }
}