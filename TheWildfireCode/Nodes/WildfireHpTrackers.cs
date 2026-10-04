using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Nodes;

public partial class WildfireHpTrackers : Control
{
	private Control? _overheat;
	private Label? _overheatLabel;

	private NHealthBar? _healthBar;
	private Creature? _creature;

	private int _overheatCount;
	
	private bool _wasMouseOverOverheatCounter;
	private bool _tooltipShown;

	public override void _Ready()
	{
		SetMouseFilterRecursive(this, MouseFilterEnum.Ignore);
		
		_overheat = GetNodeOrNull<Control>("OverheatContainer");
		_overheatLabel = GetNodeOrNull<Label>("OverheatContainer/OverheatLabel");

		if (GetParent() is NHealthBar healthBar)
			_healthBar = healthBar;

		if (GetParent().Owner.Owner is NCreature creature)
		{
			_creature = creature.Entity;
		}

		this.Visible = true;
		if (_creature != null && _creature.Player != null && _creature.Player.Character is Character.TheWildfire)
		{
			_overheat.Visible = true;
			GD.Print("overheat visible");
		}
		else
		{
			_overheat.Visible = false;
		}

		GD.Print("hp bar counters initialized");
		if (_creature != null)
			GD.Print("creature found");
		if (_overheat != null)
			GD.Print("overheat counter found");
	}

	public override void _Process(double delta)
	{
		if (_creature == null || _creature.Player == null)
			return;

		if (_overheat != null && _overheat.Visible == false && _creature.Player != null &&
			_creature.Player.PlayerCombatState != null &&
			FirepowerController.Firepower.Get(_creature.Player.PlayerCombatState) > 0)
		{
			_overheat.Visible = true;
			GD.Print("overheat visible");
		}
		
		if (_overheat != null && _overheatLabel != null && _overheat.Visible && _creature.Player != null)
		{
			_overheatCount = FirepowerController.CalculateOverheatDamage(_creature.Player);
			_overheatLabel.Text = _overheatCount.ToString();
		}
		
		UpdateCounterHover();
	}

	public void AdjustBarPositions()
	{
		Control? overheat = GetNodeOrNull<Control>("%OverheatContainer");
		Control? block;
		if (_healthBar != null)
			block = _healthBar.GetNodeOrNull<Control>("%BlockContainer");
		else
			block = null;
		if (overheat != null && block != null && _healthBar != null)
		{
			overheat.Size = block.Size;
			float adjust2 = overheat.Size.X * 0.8f;
			overheat.GlobalPosition =
				new Vector2(block.GlobalPosition.X + _healthBar.HpBarContainer.Size.X - adjust2,
					block.GlobalPosition.Y);
		}
	}

	private void UpdateCounterHover()
	{
		bool isMouseOverOverheat = IsMouseOverOverheatCounter();

		if (isMouseOverOverheat == _wasMouseOverOverheatCounter)
			return;

		_wasMouseOverOverheatCounter = isMouseOverOverheat;
		
		if (isMouseOverOverheat)
			ShowOverheatTooltip();
		else
			HideTooltips();
	}
	
	private bool IsMouseOverOverheatCounter()
	{
		Vector2 mouse = GetGlobalMousePosition();
		
		if (_overheat == null || !_overheat.Visible || !GodotObject.IsInstanceValid(_overheat))
			return false;
		
		return _overheat.GetGlobalRect().Grow(4f).HasPoint(mouse);
	}
	
	private static void SetMouseFilterRecursive(Node node, MouseFilterEnum mouseFilter)
	{
		if (node is Control control)
			control.MouseFilter = mouseFilter;

		foreach (Node child in node.GetChildren())
			SetMouseFilterRecursive(child, mouseFilter);
	}
	
	private void ShowOverheatTooltip()
	{
		if (_creature == null || _overheat == null)
			return;

		if (!_overheat.Visible)
			return;

		if (_tooltipShown)
			return;

		_tooltipShown = true;

		// Important:
		// Clean stale entry for this owner before showing.
		NHoverTipSet.Remove(this);

		HoverTip hoverTip = BuildOverheatHoverTip();

		NHoverTipSet.CreateAndShow(this, hoverTip)
			?.SetGlobalPosition(_overheat.GlobalPosition + new Vector2(0, _overheat.Size.Y));
	}
	
	private void HideTooltips()
	{
		_tooltipShown = false;

		NHoverTipSet.Remove(this);
	}
	
	private HoverTip BuildOverheatHoverTip()
	{
		LocString title = new(
			"static_hover_tips",
			"THEWILDFIRE_OVERHEAT_COUNTER.title");

		LocString description = new(
			"static_hover_tips",
			"THEWILDFIRE_OVERHEAT_COUNTER.description");
		
		description.Add("overheat", _overheatCount.ToString());

		return new HoverTip(title, description);
	}
}
