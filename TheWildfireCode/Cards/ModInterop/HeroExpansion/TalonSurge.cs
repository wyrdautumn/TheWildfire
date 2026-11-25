using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.HeroExpansion;

[Pool(typeof(TokenCardPool))]
public class TalonSurge() : TheWildfireHeroExpansionCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(1, ValueProp.Move), new HitCountVar(3), new CardsVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddHeroExpansion()];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, DynamicVars.HitCount.IntValue, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        await CommonActions.Draw(this, choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.HitCount.UpgradeValueBy(1);
    }
    
    public static async Task<IEnumerable<TalonSurge>> CreateInHand(
        Player owner,
        int amount,
        ICombatState combatState,
        Player? creator = null)
    {
        IEnumerable<TalonSurge> surges = Create(owner, amount, combatState).ToList();
        await CardPileCmd.AddGeneratedCardsToCombat(surges, PileType.Hand, creator ?? owner);
        return surges;
    }
    
    public static IEnumerable<TalonSurge> Create(Player owner, int amount, ICombatState combatState)
    {
        List<TalonSurge> surgeList = new List<TalonSurge>();
        for (int index = 0; index < amount; ++index)
            surgeList.Add(combatState.CreateCard<TalonSurge>(owner));
        return surgeList;
    }
}