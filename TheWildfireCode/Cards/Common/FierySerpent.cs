using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Common;

public class FierySerpent() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(18, ValueProp.Move), new ExertVar(6)];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ExertTag };
    }
    
    protected override bool IsPlayable
    {
        get
        {
            var ownerPlayerCombatState = this.Owner.PlayerCombatState;
            if (ownerPlayerCombatState != null &&
                FirepowerController.Firepower.Get(ownerPlayerCombatState) >=
                this.DynamicVars["Exert"].IntValue)
                return true;
            if (Owner.HasPower<OverdrivePower>())
                return true;
            return false;
        }
    }

    protected override bool ShouldGlowGoldInternal => IsPlayable;
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        int exert = ResolveExert();
        float scale = 0.6f;
        await CommonActions.CardAttack(this, play).BeforeDamage(() =>
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
        await FirepowerController.Exert(choiceContext, exert, Owner, true);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}