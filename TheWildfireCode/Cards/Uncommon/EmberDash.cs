using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class EmberDash() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(2, ValueProp.Move | ValueProp.Unblockable | ValueProp.Unpowered),
    new DynamicVar("hits",4),
    new IgniteVar(3)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        List<Creature> targets = new List<Creature>();
        for (int i = DynamicVars["hits"].IntValue; i > 0; i--)
        {
            Creature target = this.CombatState.HittableEnemies.TakeRandom(1, this.Owner.RunState.Rng.CombatTargets)
                .First();
            await CreatureCmd.Damage(choiceContext, target, DynamicVars.Damage, this, play);
            if (!targets.Contains(target))
                targets.Add(target);
        }
        int ignite = DynamicVars["Ignite"].IntValue * targets.Count;
        await FirepowerController.Ignite(choiceContext, ignite, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["hits"].UpgradeValueBy(1);
        DynamicVars["Ignite"].UpgradeValueBy(2);
    }
}