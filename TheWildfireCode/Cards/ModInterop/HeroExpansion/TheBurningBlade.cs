using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.HeroExpansion;

public class TheBurningBlade : TheWildfireCard
{
    public TheBurningBlade() : base(1,
        CardType.Attack, CardRarity.Event,
        TargetType.AnyEnemy)
    {
        var brokenBladeType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomBrokenBladeExtension");
        if (brokenBladeType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(brokenBladeType, "AddBladeCardForCustomCharacter");
            addMethod.Invoke(null, [this, ModelDb.Character<Character.TheWildfire>()]);
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new PowerVar<BurningBladePower>(10)];

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
        if (play.Target == null)
            return;
        await CommonActions.Apply<BurningBladePower>(choiceContext, play.Target, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
        DynamicVars["BurningBladePower"].UpgradeValueBy(2);
    }
}