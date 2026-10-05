using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Token;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class AllShallEndInFlames() : TheWildfireCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(32, ValueProp.Move), new IgniteVar(24), new CardsVar(5)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<Burn>()];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        await CommonActions.CardAttack(this, play).BeforeDamage(() =>
        {
            foreach (Creature target in CombatState.HittableEnemies)
            {
                NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
                if (creatureNode != null)
                {
                    NFireBurstVfx? child =
                        NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("7a37a8"));
                    if (child == null)
                        return Task.CompletedTask;
                    SfxCmd.Play("event:/sfx/characters/attack_fire");
                    NCombatRoom? instance = NCombatRoom.Instance;
                    if (instance != null)
                        instance.CombatVfxContainer.AddChildSafely(child);
                }
            }
            return Task.CompletedTask;
        }).Execute(choiceContext);
        await FirepowerController.Ignite(choiceContext, DynamicVars["Ignite"].IntValue, Owner);
        List<CardModel> burns = new List<CardModel>();
        for (int index = 0; index < DynamicVars.Cards.IntValue; ++index)
            burns.Add(CombatState.CreateCard<Burn>(Owner));
        await CardPileCmd.AddGeneratedCardsToCombat(burns, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6);
        DynamicVars["Ignite"].UpgradeValueBy(6);
    }
}