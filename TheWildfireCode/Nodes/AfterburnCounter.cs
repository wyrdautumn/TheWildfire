using Godot;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Status;

namespace TheWildfire.TheWildfireCode.Nodes;

public partial class AfterburnCounter : Control
{
    private Label? _label;
    private NCard? _card;
    private Control? _container;
    
    public override void _Ready()
    {
        this.MouseFilter = MouseFilterEnum.Ignore;

        _label = GetNodeOrNull<Label>("AfterburnCounterVisual/Icon/Label");

        if (GetParent() is Control container)
        {
            _container = container;
        }

        if (_container != null && _container.GetParent() is NCard card)
        {
            _card = card;
        }
    }
    
    public override void _Process(double delta)
    {
        if (_card == null || _card.Model == null || !_card.Model.Tags.Contains(WildfireKeywords.ShowAfterburn) || _label == null)
        {
            this.Visible = false;
            return;
        }

        if (_card.Model.Tags.Contains(WildfireKeywords.ShowAfterburn))
        {
            this.Visible = _card.Visible;
        }

        if (!_card.Model.IsInCombat || _card.Model.Owner.PlayerCombatState == null)
        {
            _label.Text = "0";
            return;
        }
        
        int ignite = Afterburn.AfterburnCount.Get(_card.Model.Owner.PlayerCombatState);
        _label.Text = ignite.ToString();
    }
}