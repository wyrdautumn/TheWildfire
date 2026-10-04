using BaseLib.Extensions;
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Uncommon;

public class LavaLeap() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9, ValueProp.Move), new DynamicVar("Flare", 8), new PowerVar<VulnerablePower>(2)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.ExertAll), HoverTipFactory.FromPower<VulnerablePower>()];
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { WildfireKeywords.ExertTag };
    }

    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            var ownerPlayerCombatState = this.Owner.PlayerCombatState;
            if (ownerPlayerCombatState != null &&
                FirepowerController.Firepower.Get(ownerPlayerCombatState) >=
                this.DynamicVars["Flare"].IntValue)
                return true;
            if (Owner.HasPower<OverdrivePower>())
                return true;
            return false;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        int exert = ResolveExertAll();
        bool hasOverdrive = Owner.HasPower<OverdrivePower>();
        bool fullExert = false;
        float scale = 0.8f;
        await CommonActions.CardAttack(this, play).BeforeDamage(() =>
        {
            foreach (Creature target in CombatState.HittableEnemies)
            {
                NGroundFireVfx? child = NGroundFireVfx.Create(target, VfxColor.Purple);
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
        if (exert >= DynamicVars["Flare"].IntValue || hasOverdrive)
        {
            await CommonActions.Apply<VulnerablePower>(choiceContext, CombatState.HittableEnemies, this);
            fullExert = true;
        }
        await FirepowerController.ExertAll(choiceContext, Owner, fullExert);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.Vulnerable.UpgradeValueBy(1);
    }
}