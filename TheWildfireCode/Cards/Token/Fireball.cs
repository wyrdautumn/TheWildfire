using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Cards.Variables;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Fireball() : TheWildfireCard(0,
    CardType.Attack, CardRarity.Token,
    TargetType.AllEnemies)
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
        foreach (Fireball fireball in Owner.PlayerCombatState.AllCards.OfType<Fireball>())
        {
            fireball.DynamicVars.Damage.BaseValue *= 2;
            fireball.CurrentDamage = fireball.DynamicVars.Damage.IntValue;
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
}