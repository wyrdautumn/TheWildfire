using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
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
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Relics;

public class SparkGuards() : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.AfterburnStatic), HoverTipFactory.FromCard<Afterburn>()];

    public override async Task AfterCardEnteredCombat(CardModel card)
    {
        if (card.Owner == Owner && card is Afterburn)
        {
            if (card.Owner.PlayerCombatState == null || card.CombatState == null)
                return;
            int afterburn = Afterburn.AfterburnCount.Get(card.Owner.PlayerCombatState);
            foreach (Creature target in card.CombatState.HittableEnemies)
            {
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
            }
            await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), card.CombatState.HittableEnemies, afterburn, ValueProp.Unpowered, Owner.Creature, null, null);
        }
    }
}