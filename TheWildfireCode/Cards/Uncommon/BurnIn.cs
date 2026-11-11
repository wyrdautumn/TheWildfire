using System.Buffers;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class BurnIn() : TheWildfireCard(0,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("SelfDamage",4),
        new CalculationBaseVar(0),
        new ExtraDamageVar(1),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(Calc)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>()
            .Count(d => d.Receiver == card.Owner.Creature && d.Result.TotalDamage > 0);
    }
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (play.Target == null)
            return;
        NCombatRoom? instance = NCombatRoom.Instance; 
        await CommonActions.CardAttack(this, play).BeforeDamage(() =>
        {
            NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(play.Target);
            if (creatureNode != null)
            {
                NFireBurstVfx? child =
                    NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("7a37a8"));
                if (child == null)
                    return Task.CompletedTask;
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely(child);
            }
            return Task.CompletedTask;
        }).Execute(choiceContext);
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(Owner.Creature, VfxColor.Purple));
        SfxCmd.Play("event:/sfx/characters/attack_fire");
        await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars["SelfDamage"].BaseValue, ValueProp.Unpowered, this, play);
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        CardModel? card = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (card == null)
            return;
        await CardCmd.Exhaust(choiceContext, card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["SelfDamage"].UpgradeValueBy(-3);
    }
}