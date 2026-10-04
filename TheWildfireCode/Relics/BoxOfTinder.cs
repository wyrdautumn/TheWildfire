using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Relics;

public class BoxOfTinder : TheWildfireRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Common;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.ExertStatic)];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Unpowered)];
    
    public override string FlashSfx => "event:/sfx/characters/attack_fire";

    public override async Task AfterExert(PlayerChoiceContext choiceContext, int amount, Player exerter, bool fullExert)
    {
        if (exerter == Owner && Owner.Creature.CombatState != null)
        {
            Flash();
            Creature? target = Owner.RunState.Rng.CombatTargets.NextItem(Owner.Creature.CombatState.HittableEnemies);
            if (target != null)
                await CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage, Owner.Creature);
        }
    }
}