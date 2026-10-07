using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;

namespace TheWildfire.TheWildfireCode.Nodes;

public partial class AfterburnDiscard : Control
{
    private Label? _label;
    private NCombatCardPile? _button;
    
    public override void _Ready()
    {
        this.MouseFilter = MouseFilterEnum.Ignore;

        _label = GetNodeOrNull<Label>("AfterburnDiscardVisual/Icon/Label");
        
        if (GetParent() is NCombatCardPile button)
        {
            _button = button;
        }

        Visible = false;
    }
    
    public override void _Process(double delta)
    {
        if (_button == null || _button._pile == null || !_button._pile.Cards.Any(c => c is Afterburn))
        {
            this.Visible = false;
            return;
        }
        
        Visible = _button.Visible;

        if (_label != null && Visible && _button._localPlayer != null && _button._localPlayer.PlayerCombatState != null)
        {
            _label.Text = Afterburn.AfterburnCount.Get(_button._localPlayer.PlayerCombatState).ToString();
        }
    }
}