using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using BaseLib.Abstracts;
using BaseLib.Patches.UI;
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
        ModelDb.Card<StrikeWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<DefendWildfire>(),
        ModelDb.Card<EternalTorch>(),
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
    public override string CustomMerchantAnimPath => "res://TheWildfire/scenes/wildfire_merchant.tscn";
    public override string CustomRestSiteAnimPath => "res://TheWildfire/scenes/wildfire_rest_site.tscn";
    public override string CustomTrailPath => "res://TheWildfire/scenes/card_trail_wildfire.tscn";

    public override float DeathAnimTime => 1.83f;

    public override string CustomCharacterSelectTransitionPath =>
        "res://TheWildfire/images/shaders/wildfire_screen_transition.tres";

    public override RelicIconData? CustomYummyCookie => new RelicIconData("res://TheWildfire/images/relics/big/yummy_cookie_wildfire.png","res://TheWildfire/images/relics/yummy_cookie_wildfire.png","res://TheWildfire/images/relics/yummy_cookie_wildfire_outline.png");
    
    public override string CharacterTransitionSfx => "res://TheWildfire/audio/wildfire_transition.wav";
    public override Color EnergyLabelOutlineColor => new ("562F53");
    public override Color DialogueColor => new ("713980");
    public override VfxColor SpeechBubbleColor => VfxColor.Purple;
    public override Color MapDrawingColor => new ("AD43C8");
    public override Color RemoteTargetingLineColor => new ("c07ceaFF");
    public override Color RemoteTargetingLineOutline => new ("a342b5FF");
    
    public override List<string> GetArchitectAttackVfx()
    {
        int num = 5;
        List<string> list = new List<string>(num);
        CollectionsMarshal.SetCount<string>(list, num);
        Span<string> span = CollectionsMarshal.AsSpan<string>(list);
        int index1 = 0;
        span[index1] = "vfx/vfx_attack_slash";
        int index2 = index1 + 1;
        span[index2] = "vfx/vfx_fire_burst";
        int index3 = index2 + 1;
        span[index3] = "vfx/vfx_rock_shatter";
        int index4 = index3 + 1;
        span[index4] = "vfx/vfx_bloody_impact";
        int index5 = index4 + 1;
        span[index5] = "vfx/vfx_fire_burst";
        return list;
    }
}