using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Multiplayer;

public class BurnTogether() : TheWildfireCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllAllies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IgniteVar(5)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..HoverTipFactory.FromCardWithCardHoverTips<Incinerate>()];
    
    public override CardMultiplayerConstraint MultiplayerConstraint
    {
        get => CardMultiplayerConstraint.MultiplayerOnly;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        await FirepowerController.Ignite(choiceContext, DynamicVars["Ignite"].IntValue, Owner);
        foreach (Creature teammate in CombatState.GetTeammatesOf(Owner.Creature)
                     .Where<Creature>((Func<Creature, bool>)(c => c != null && c.IsAlive && c.IsPlayer)))
        {
            if (teammate.Player == null)
                continue;
            CardModel incinerate = CombatState.CreateCard<Incinerate>(teammate.Player);
            await CardPileCmd.AddGeneratedCardToCombat(incinerate, PileType.Hand, teammate.Player);
        }

    }

    protected override void OnUpgrade()
    {
        DynamicVars["Ignite"].UpgradeValueBy(3);
    }
}