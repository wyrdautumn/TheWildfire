using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Nodes;

public partial class WildfireFireCounter : Control
{
	private Label? _label;
	private Label? _labelSmall;
	private Control? _fireLayers;
	private NEnergyCounter? _energyCounter;
	private Player? _player;

	private int _fireCount = 0;
	private int _damageCount = 0;

	private bool _wasMouseOverCounter;
	private bool _tooltipShown;

	private WildfireParticles? _vfxFront;
	private WildfireParticles? _vfxBack;

	private TextureRect? _layer1;
	private TextureRect? _layer2;
	private TextureRect? _layer3;
	private TextureRect? _layer4;
	private TextureRect? _layer5;
	private TextureRect? _layer6;
	
	public override void _Ready()
	{
		SetMouseFilterRecursive(this, MouseFilterEnum.Ignore);

		_label = GetNodeOrNull<Label>("FirepowerVisual/Label");
		_fireLayers = GetNodeOrNull<Control>("FirepowerVisual/FireLayers");
		_labelSmall = GetNodeOrNull<Label>("FirepowerVisual/10PercentLabel");
		_vfxFront = GetNodeOrNull<WildfireParticles>("FirepowerVisual/FireVfxFront");
		_vfxBack = GetNodeOrNull<WildfireParticles>("FirepowerVisual/FireVfxBack");

		_layer1 = GetNodeOrNull<TextureRect>("FirepowerVisual/FireLayers/Layer1");
		_layer2 = GetNodeOrNull<TextureRect>("FirepowerVisual/FireLayers/Layer2");
		_layer3 = GetNodeOrNull<TextureRect>("FirepowerVisual/FireLayers/Layer3");
		_layer4 = GetNodeOrNull<TextureRect>("FirepowerVisual/FireLayers/Layer4");
		_layer5 = GetNodeOrNull<TextureRect>("FirepowerVisual/FireLayers/Layer5");
		_layer6 = GetNodeOrNull<TextureRect>("FirepowerVisual/FireLayers/Layer6");

		if (GetParent() is NEnergyCounter energyCounter)
		{
			_energyCounter = energyCounter;
			_player = energyCounter._player;
		}

		this.Visible = false;
		RefreshVisibility();

		if (this.Visible == true)
		{
			UpdateFire();
		}
		else
		{
			HideFirepowerTooltip();
		}
		
		GD.Print(
			$"Firepower Counter initialized.");
		if (this.Visible == true)
			GD.Print($"Firepower counter displayed.");
		else
			GD.Print($"Firepower counter not displayed.");
		if (_label != null)
			GD.Print($"Label found");
		if (_fireLayers != null)
			GD.Print($"Layers found");
	}
	
	public override void _ExitTree()
	{
		HideFirepowerTooltip();
	}

	public override void _Process(double delta)
	{
		if (_player == null)
			return;
		RefreshVisibility();
		if (this.Visible == false)
			return;
		UpdateFire();
		UpdateCounterHover();
	}

	private void RefreshVisibility()
	{
		if (_player == null || _player.PlayerCombatState == null)
		{
			this.Visible = false;
		}
		else
		{
			int fire = FirepowerController.Firepower.Get(_player.PlayerCombatState);
			this.Visible = this.Visible || _player.Character is Character.TheWildfire || fire > 0;
		}
	}

	private void UpdateFire()
	{
		if (_player == null || _player.PlayerCombatState == null || _label == null || _labelSmall == null)
			return;
		int fire = FirepowerController.Firepower.Get(_player.PlayerCombatState);
		if (_fireCount == fire)
			return;
		if (_fireCount < fire && _vfxFront != null)
			_vfxFront.Restart();
		if (_fireCount > fire && _vfxBack != null)
			_vfxBack.Restart();
		_fireCount = fire;
		int damage = FirepowerController.CalculateOverheatDamageWithoutMitigation(_player);
		_damageCount = damage;
		_label.Text = _fireCount.ToString();
		_labelSmall.Text = _damageCount.ToString();
		SetLayers();
	}

	private void SetLayers()
	{
		if (_layer1 == null || _layer2 == null || _layer3 == null || _layer4 == null || _layer5 == null || _layer6 == null)
			return;
		_layer1.Visible = true;
		if (_fireCount < 10)
		{
			_layer2.Visible = false;
			_layer3.Visible = false;
			_layer4.Visible = false;
			_layer5.Visible = false;
		}
		else if (_fireCount < 20)
		{
			_layer2.Visible = true;
			_layer3.Visible = false;
			_layer4.Visible = false;
			_layer5.Visible = false;
		}
		else if (_fireCount < 30)
		{
			_layer2.Visible = true;
			_layer3.Visible = true;
			_layer4.Visible = false;
			_layer5.Visible = false;
		}
		else if (_fireCount < 40)
		{
			_layer2.Visible = true;
			_layer3.Visible = true;
			_layer4.Visible = true;
			_layer5.Visible = false;
		}
		else
		{
			_layer2.Visible = true;
			_layer3.Visible = true;
			_layer4.Visible = true;
			_layer5.Visible = true;
		}

		if (_fireCount >= 50)
			_layer6.Visible = true;
		else
			_layer6.Visible = false;
	}
	
	
	private void UpdateCounterHover()
	{
		bool isMouseOverMaterial = IsMouseOverFirepowerCounter();

		if (isMouseOverMaterial == _wasMouseOverCounter)
			return;

		_wasMouseOverCounter = isMouseOverMaterial;

		if (isMouseOverMaterial)
			ShowFirepowerTooltip();
		else
			HideFirepowerTooltip();
	}
	
	public bool IsMouseOverFirepowerCounter()
	{
		Vector2 mouse = GetGlobalMousePosition();

		if (_fireLayers == null || !GodotObject.IsInstanceValid(_fireLayers))
			return false;

		return _fireLayers.GetGlobalRect().Grow(4f).HasPoint(mouse);
	}
	
	private static void SetMouseFilterRecursive(Node node, MouseFilterEnum mouseFilter)
	{
		if (node is Control control)
			control.MouseFilter = mouseFilter;

		foreach (Node child in node.GetChildren())
			SetMouseFilterRecursive(child, mouseFilter);
	}
	
	private void ShowFirepowerTooltip()
	{
		if (_player == null)
			return;

		if (!Visible)
			return;

		if (_tooltipShown)
			return;

		_tooltipShown = true;

		// Important:
		// Clean stale entry for this owner before showing.
		NHoverTipSet.Remove(this);

		// Also remove the energy tooltip, because the material UI overlaps it.
		if (_energyCounter != null && GodotObject.IsInstanceValid(_energyCounter))
			NHoverTipSet.Remove(_energyCounter);

		HoverTip hoverTip = BuildHoverTip();

		NHoverTipSet.CreateAndShow(this, hoverTip)
			?.SetGlobalPosition(GlobalPosition + new Vector2(Size.X * 1.5f, 0));
	}
	
	private void HideFirepowerTooltip()
	{
		_tooltipShown = false;

		NHoverTipSet.Remove(this);

		if (_energyCounter == null || !GodotObject.IsInstanceValid(_energyCounter))
			return;

		if (!IsMouseOverEnergyCounter())
			return;

		// Let Material fully unregister first, then let Energy try to show again.
		_energyCounter.CallDeferred(nameof(NEnergyCounter.OnHovered));
	}

	private bool IsMouseOverEnergyCounter()
	{
		if (_energyCounter == null || !GodotObject.IsInstanceValid(_energyCounter))
			return false;

		return _energyCounter.GetGlobalRect().Grow(4f).HasPoint(GetGlobalMousePosition());
	}
	
	private HoverTip BuildHoverTip()
	{
		LocString title = new(
			"static_hover_tips",
			"THEWILDFIRE_FIREPOWER_COUNTER.title");

		LocString description = new(
			"static_hover_tips",
			"THEWILDFIRE_FIREPOWER_COUNTER.description");

		return new HoverTip(title, description);
	}
}
