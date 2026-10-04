using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Variables;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class SolarDetonation() : TheWildfireCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(32, ValueProp.Move | ValueProp.Unpowered | ValueProp.Unblockable), new PowerVar<WeakPower>(2), new PowerVar<VulnerablePower>(2), new AfterburnVar(5)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Afterburn>(),HoverTipFactory.FromPower<WeakPower>(),HoverTipFactory.FromPower<VulnerablePower>()];
    
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ShowAfterburn };
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
        {
            return;
        }
        foreach (Creature target in CombatState.HittableEnemies)
        {
            NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
            if (creatureNode != null)
            {
                NFireBurningVfx? child =
                    NFireBurningVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, true, new Color("7a37a8"));
                if (child != null)
                {
                    SfxCmd.Play("event:/sfx/characters/attack_fire");
                    NCombatRoom? instance = NCombatRoom.Instance;
                    if (instance != null)
                        instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                }
            }
        }
        await CreatureCmd.Damage(choiceContext, CombatState.HittableEnemies, DynamicVars.Damage, Owner.Creature, this,
            play);
        await CommonActions.Apply<WeakPower>(choiceContext, CombatState.HittableEnemies, this);
        await CommonActions.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies, this);
        await Afterburn.CreateAfterburn(DynamicVars["Afterburn"].IntValue, Owner, CombatState);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(10);
    }
}