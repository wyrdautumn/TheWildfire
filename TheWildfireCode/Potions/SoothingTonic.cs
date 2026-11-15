using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Potions;

public class SoothingTonic : TheWildfirePotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, ValueProp.Unpowered), new PowerVar<ConstitutionPower>(6)];
    public override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ConstitutionPower>()];
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
        if (target.Player == null)
            return;
        await CreatureCmd.GainBlock(target, DynamicVars.Block, null);
        await PowerCmd.Apply<ConstitutionPower>(choiceContext, target, DynamicVars["RegenPower"].BaseValue, Owner.Creature, null);
    }
    
    public override string? CustomPackedImagePath => "res://TheWildfire/images/potions/firebreath_potion.png";
    public override string? CustomPackedOutlinePath => "res://TheWildfire/images/potions/firebreath_potion_outline.png";
}