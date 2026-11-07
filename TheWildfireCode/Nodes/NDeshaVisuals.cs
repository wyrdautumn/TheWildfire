using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheWildfire.TheWildfireCode.Firepower;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Nodes;

[GlobalClass]
public partial class NDeshaVisuals : NCreatureVisuals, IFirepowerNode
{
	private Sprite2D? _attackShadow;
	private Sprite2D? _skillShadow;
	private Sprite2D? _powerShadow;
	private WildfireParticles? _attackRing;
	private WildfireParticles? _skillRing;
	private WildfireParticles? _powerRing;
	private WildfireParticles? _exertRing;
	private WildfireParticles? _igniteEffect;
	private GpuParticles2D? _frontFire;
	private GpuParticles2D? _backFire;
	private int _firepower;

	public override void _Ready()
	{
		base._Ready();
		_attackShadow = GetNodeOrNull<Sprite2D>("%Visuals/AttackShadow");
		_skillShadow = GetNodeOrNull<Sprite2D>("%Visuals/SkillShadow");
		_powerShadow = GetNodeOrNull<Sprite2D>("%Visuals/PowerShadow");
		_attackRing = GetNodeOrNull<WildfireParticles>("%Visuals/AttackRing");
		_skillRing = GetNodeOrNull<WildfireParticles>("%Visuals/SkillRing");
		_powerRing = GetNodeOrNull<WildfireParticles>("%Visuals/PowerRing");
		_exertRing = GetNodeOrNull<WildfireParticles>("%Visuals/ExertRing");
		_igniteEffect = GetNodeOrNull<WildfireParticles>("%Visuals/IgniteEffect");
		_frontFire = GetNodeOrNull<GpuParticles2D>("%Visuals/FireParticlesFront");
		_backFire = GetNodeOrNull<GpuParticles2D>("%Visuals/FireParticlesBack");
		_firepower = 0;

		if (!FirepowerController.FirepowerNodes.Contains(this))
		{
			FirepowerController.FirepowerNodes.Add(this);
		}

		UpdateFireParticles();
		CheckFlowShadows();
	}

	public void CheckFlowShadows()
	{
		var parentNCreature = GetParentOrNull<NCreature>();
		if (parentNCreature == null || _attackShadow == null || _attackRing == null || _skillShadow == null || _skillRing == null || _powerShadow == null || _powerRing == null)
			return;
		Creature creature = parentNCreature.Entity;
		if (_attackShadow.Visible != creature.HasPower<AttackFlowPower>())
		{
			_attackShadow.Visible = creature.HasPower<AttackFlowPower>();
			_attackRing.Restart();
		}
		if (_skillShadow.Visible != creature.HasPower<SkillFlowPower>())
		{
			_skillShadow.Visible = creature.HasPower<SkillFlowPower>();
			_skillRing.Restart();
		}
		if (_powerShadow.Visible != creature.HasPower<PowerFlowPower>())
		{
			_powerShadow.Visible = creature.HasPower<PowerFlowPower>();
			_powerRing.Restart();
		}
	}

	public void UpdateFireParticles()
	{
		var parentNCreature = GetParentOrNull<NCreature>();
		if (parentNCreature == null || parentNCreature.Entity.Player == null)
			return;
		Player player = parentNCreature.Entity.Player;
		if (player.PlayerCombatState == null || _frontFire == null || _backFire == null)
			return;
		int firepower = FirepowerController.Firepower.Get(player.PlayerCombatState);
		if (firepower > _firepower && _igniteEffect != null)
			_igniteEffect.Restart();
		if (firepower < _firepower && _exertRing != null)
			_exertRing.Restart();
		_firepower = firepower;
		int fire = Math.Min(50, firepower);
		if (fire < 1)
		{
			_frontFire.AmountRatio = 0;
			_backFire.AmountRatio = 0;
		}

		if (fire > 0)
		{
			float ratio = fire * 0.02f;
			_frontFire.AmountRatio = ratio;
			_backFire.AmountRatio = ratio;
		}
	}

	public void AfterFirepowerUpdate()
	{
		UpdateFireParticles();
	}
}
