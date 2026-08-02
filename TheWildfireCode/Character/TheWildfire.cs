using System.Diagnostics.CodeAnalysis;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using TheWildfire.TheWildfireCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using TheWildfire.TheWildfireCode.Cards.Basic;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Character;

[SuppressMessage("Localization", "STS001:Symbol missing localization")]
public class TheWildfire : PlaceholderCharacterModel
{
    public const string CharacterId = "TheWildfire";

    public static readonly Color Color = new("803bb2");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 80;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeWildfire>(),
        ModelDb.Card<StrikeWildfire>(),
        ModelDb.Card<StrikeWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<Combustion>(),
        ModelDb.Card<BlazingWave>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<HeartOfFire>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TheWildfireCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheWildfireRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheWildfirePotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_desha.png".CharacterUiPath();
    public override string? CustomIconOutlineTexturePath =>  "character_icon_desha_outline.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_desha.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_desha_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_desha.png".CharacterUiPath();
    
    public override string CustomCharacterSelectBg => "res://TheWildfire/scenes/char_select_bg_wildfire.tscn";
    public override string CustomEnergyCounterPath => "res://TheWildfire/scenes/wildfire_energy_counter.tscn";
    public override string CustomVisualPath => "res://TheWildfire/scenes/desha.tscn";
    
    public override Color EnergyLabelOutlineColor => new ("562F53");
    public override Color DialogueColor => new ("713980");
    public override VfxColor SpeechBubbleColor => VfxColor.Purple;
    public override Color MapDrawingColor => new ("AD43C8");
    public override Color RemoteTargetingLineColor => new ("c07ceaFF");
    public override Color RemoteTargetingLineOutline => new ("a342b5FF");
}