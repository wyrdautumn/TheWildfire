using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Firebolt() : TheWildfireCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AnyEnemy)
{
    public int CurrentDamage = 0;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];
    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
        if (Owner.PlayerCombatState == null)
            return;
        foreach (Firebolt firebolt in Owner.PlayerCombatState.AllCards.OfType<Firebolt>())
        {
            firebolt.DynamicVars.Damage.BaseValue *= 2;
            firebolt.CurrentDamage = firebolt.DynamicVars.Damage.IntValue;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }
    
    protected override void AfterDowngraded()
    {
        if (CurrentDamage > DynamicVars.Damage.IntValue)
            DynamicVars.Damage.BaseValue = CurrentDamage;
    }
    
    public static async Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
    {
        return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
    }
    
    public static async Task<IEnumerable<CardModel>> CreateInHand(
        Player owner,
        int count,
        ICombatState combatState)
    {
        if (count == 0)
            return Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding)
            return Array.Empty<CardModel>();
        List<CardModel> firebolts = new List<CardModel>();
        for (int index = 0; index < count; ++index)
            firebolts.Add(combatState.CreateCard<Firebolt>(owner));
        await CardPileCmd.AddGeneratedCardsToCombat(firebolts, PileType.Hand, owner);
        return firebolts;
    }
}