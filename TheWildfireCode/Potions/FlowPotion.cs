using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Potions;

public class FlowPotion : TheWildfirePotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;
    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(WildfireKeywords.AttackFlow),HoverTipFactory.FromKeyword(WildfireKeywords.SkillFlow),HoverTipFactory.FromKeyword(WildfireKeywords.PowerFlow)];
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("e97b1e"));
        List<CardType> flows = new List<CardType>();
        if (!target.HasPower<AttackFlowPower>())
            flows.Add(CardType.Attack);
        if (!target.HasPower<SkillFlowPower>())
            flows.Add(CardType.Skill);
        if (!target.HasPower<PowerFlowPower>())
            flows.Add(CardType.Power);
        if (flows.Any() && target.CombatState != null && target.Player != null)
        {
            var type = flows.TakeRandom(1, target.CombatState.RunState.Rng.Niche);
            await FlowSingleton.AchieveFlow(choiceContext, target.Player, null, type.FirstOrDefault());
        }
    }
    
    public override string? CustomPackedImagePath => "res://TheWildfire/images/potions/firebreath_potion.png";
    public override string? CustomPackedOutlinePath => "res://TheWildfire/images/potions/firebreath_potion_outline.png";
}