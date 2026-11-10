using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class FlintStrike() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    private int _currentIgnite = 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new IgniteVar(5), new DynamicVar("Increase", 4)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        float scale = 0.6f;
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").BeforeDamage(() =>
        {
            if (play.Target != null){
                NGroundFireVfx? child = NGroundFireVfx.Create(play.Target);
                if (child == null)
                    return Task.CompletedTask;
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                child.Scale = Vector2.One * scale;
                NCombatRoom? instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
                scale += 0.1f;
            }
            return Task.CompletedTask;
        }).Execute(choiceContext);
        await FirepowerController.Ignite(choiceContext, DynamicVars["Ignite"].IntValue, Owner);
        DynamicVars["Ignite"].BaseValue += DynamicVars["Increase"].IntValue;
        _currentIgnite = DynamicVars["Ignite"].IntValue;
    }
    
    protected override void AfterDowngraded()
    {
        if (_currentIgnite > DynamicVars["Ignite"].IntValue)
            DynamicVars["Ignite"].BaseValue = _currentIgnite;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}