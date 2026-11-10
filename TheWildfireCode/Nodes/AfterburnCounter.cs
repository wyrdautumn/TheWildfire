using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;
using TheWildfire.TheWildfireCode.Cards.Uncommon;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Nodes;

public partial class AfterburnCounter : Control
{
    private Label? _label;
    private NCard? _card;
    private Control? _container;
    private TextureRect? _afterburnIcon;
    private TextureRect? _exertIcon;
    private RichTextLabel? _exertLower;
    private RichTextLabel? _exertUpper;
    private TextureRect? _exertGreen;
    private TextureRect? _exertPurple;
    
    public override void _Ready()
    {
        this.MouseFilter = MouseFilterEnum.Ignore;

        _label = GetNodeOrNull<Label>("AfterburnCounterVisual/Icon/Label");
        _afterburnIcon = GetNodeOrNull<TextureRect>("AfterburnCounterVisual/Icon");
        _exertIcon = GetNodeOrNull<TextureRect>("AfterburnCounterVisual/ExertIcon");
        _exertLower = GetNodeOrNull<RichTextLabel>("AfterburnCounterVisual/ExertIcon/ExertLower");
        _exertUpper = GetNodeOrNull<RichTextLabel>("AfterburnCounterVisual/ExertIcon/ExertUpper");
        _exertGreen = GetNodeOrNull<TextureRect>("AfterburnCounterVisual/ExertIcon/ExertIconGreen");
        _exertPurple = GetNodeOrNull<TextureRect>("AfterburnCounterVisual/ExertIcon/ExertIconPurple");

        if (_label != null)
            _label.AddThemeFontOverride(ThemeConstants.Label.Font, PreloadManager.Cache.GetAsset<Font>("res://themes/kreon_bold_shared.tres"));
        if (_exertLower != null)
            _exertLower.AddThemeFontOverride(ThemeConstants.RichTextLabel.NormalFont, PreloadManager.Cache.GetAsset<Font>("res://themes/kreon_bold_shared.tres"));
        if (_exertUpper != null)
            _exertUpper.AddThemeFontOverride(ThemeConstants.RichTextLabel.NormalFont, PreloadManager.Cache.GetAsset<Font>("res://themes/kreon_bold_shared.tres"));


        if (GetParent() is Control container)
        {
            _container = container;
        }

        if (_container != null && _container.GetParent() is NCard card)
        {
            _card = card;
        }

        if (_afterburnIcon != null)
            _afterburnIcon.Visible = false;
        if (_exertIcon != null)
            _exertIcon.Visible = false;
        
        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        if (_card == null || _card.Model == null)
            return;

        CardModel card = _card.Model;
        PlayerCombatState? combatState = null;
        if (card.IsInCombat)
            combatState = card.Owner.PlayerCombatState;

        if (_afterburnIcon != null)
        {
            if (!card.Tags.Contains(WildfireKeywords.ShowAfterburn))
            {
                _afterburnIcon.Visible = false;
            }

            if (card.Tags.Contains(WildfireKeywords.ShowAfterburn))
            {
                _afterburnIcon.Visible = _card.Visible;
            }

            if (_afterburnIcon != null && _label != null && _afterburnIcon.Visible)
            {
                if (!card.IsInCombat || combatState == null)
                {
                    _label.Text = "0";
                }

                else
                {
                    int ignite = Afterburn.AfterburnCount.Get(combatState);
                    _label.Text = ignite.ToString();
                }
            }
        }

        if (_exertIcon == null || _exertLower == null || _exertUpper == null || _exertGreen == null || _exertPurple == null)
            return;

        if (!card.Tags.Contains(WildfireKeywords.ExertTag))
        {
            _exertIcon.Visible = false;
            return;
        }

        _exertIcon.Visible = _card.Visible;

        if (!_exertIcon.Visible)
            return;
        
        _exertGreen.Visible = false;
        _exertPurple.Visible = false;

        string numerator;
        string denominator;
        bool isFree = false;
        if (card.IsInCombat && card.Owner.HasPower<OverdrivePower>() ||
            (card.TemporaryStarCost != null && card.TemporaryStarCost.Cost == 0))
        {
            isFree = true;
        }

        if (card.DynamicVars.TryGetValue("Exert", out var var))
        {
            int exert = var.IntValue;
            denominator = "[center]" + exert + "[/center]";
            if (!card.IsInCombat || combatState == null)
            {
                _exertUpper.Text = "[center]0[/center]";
                if (card.UpgradePreviewType.IsPreview() && card.DynamicVars["Exert"].WasJustUpgraded)
                    _exertLower.Text = "[color=7FFF00][outline_color=1F5923]" + denominator + "[/outline_color][/color]";
                else
                    _exertLower.Text = denominator;
                return;
            }

            if (isFree)
            {
                _exertGreen.Visible = true;
                numerator = "[color=7FFF00][outline_color=1F5923]" + denominator + "[/outline_color][/color]";
                denominator = numerator;
                _exertUpper.Text = numerator;
                _exertLower.Text = denominator;
                return;
            }

            int firepower = Math.Min(exert, FirepowerController.Firepower.Get(combatState));
            if (firepower >= exert)
                _exertPurple.Visible = true;
            numerator = "[center]" + firepower + "[/center]";
            _exertUpper.Text = numerator;
            if (card.UpgradePreviewType.IsPreview() && card.DynamicVars["Exert"].WasJustUpgraded)
                _exertLower.Text = "[color=7FFF00][outline_color=1F5923]" + denominator + "[/outline_color][/color]";
            else
                _exertLower.Text = denominator;
            return;
        }

        denominator = "[center]∞[/center]";
        if (!card.IsInCombat || combatState == null)
        {
            _exertUpper.Text = "[center]0[/center]";
            _exertLower.Text = denominator;
            return;
        }

        numerator = "[center]" + FirepowerController.Firepower.Get(combatState) + "[/center]";
        if (isFree)
        {
            _exertGreen.Visible = true;
            numerator = "[color=7FFF00][outline_color=1F5923]" + numerator + "[/outline_color][/color]";
            denominator = "[color=7FFF00][outline_color=1F5923]" + denominator + "[/outline_color][/color]";
        }

        if (!_exertGreen.Visible && card is not DeepBreath)
            _exertPurple.Visible = true;
        
        _exertUpper.Text = numerator;
        _exertLower.Text = denominator;
    }
}