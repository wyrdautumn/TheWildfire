using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class WildfireUnleashed() : TheWildfireCard(0,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(45, ValueProp.Move)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.FlowStatic), HoverTipFactory.Static(WildfireKeywords.Rekindle)];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (combatState.RoundNumber != 1 || player != Owner)
            return;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(this, PileType.Exhaust));
    }

    public override async Task FlowAchieved(PlayerChoiceContext choiceContext, Player achiever, CardType type)
    {
        if (achiever == Owner && Owner.HasPower<AttackFlowPower>() && Owner.HasPower<SkillFlowPower>() &&
                       Owner.HasPower<PowerFlowPower>())
            await CardPileCmd.Add(this, PileType.Hand);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        Color color = new Color("a85bdd80");
        double num2 = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3;
        NCombatRoom? instance1 = NCombatRoom.Instance;
        if (instance1 != null)
            instance1.CombatVfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(color, 0.8 + num2));
        SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_whirlwind");
        NRun? instance2 = NRun.Instance;
        if (instance2 != null)
            instance2.GlobalUi.AddChildSafely(NSmokyVignetteVfx.Create(color, color)); 
        await CommonActions.CardAttack(this, play).BeforeDamage(() =>
        {
            NCreature? creatureNode = NCombatRoom.Instance?.GetCreatureNode(play.Target);
            if (creatureNode != null)
            {
                NFireBurstVfx? child = NFireBurstVfx.Create(creatureNode.GetBottomOfHitbox(), 1f, new Color("7a37a8"));
                if (child == null)
                    return Task.CompletedTask;
                SfxCmd.Play("event:/sfx/characters/attack_fire");
                NCombatRoom? instance = NCombatRoom.Instance;
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely((Godot.Node)child);
            }
            return Task.CompletedTask;
        }).Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(15);
    }
}