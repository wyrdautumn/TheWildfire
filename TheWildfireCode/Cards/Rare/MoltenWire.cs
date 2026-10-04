using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Powers;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class MoltenWire() : TheWildfireCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public const string _increaseKey = "Increase";
    public const int _baseStrength = 3;
    public int _currentStrength = 3;
    public int _increasedStrength;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move), new PowerVar<MoltenWirePower>(3), new IntVar("Increase", 1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<StrengthPower>()];

    
    [SavedProperty]
    public int CurrentStrength
    {
        get => _currentStrength;
        set
        {
            AssertMutable();
            _currentStrength = value;
            DynamicVars["MoltenWirePower"].BaseValue = _currentStrength;
        }
    }
    
    [SavedProperty]
    public int IncreasedStrength
    {
        get => _increasedStrength;
        set
        {
            AssertMutable();
            _increasedStrength = value;
        }
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CommonActions.CardAttack(this, play,vfx:"vfx/vfx_attack_slash").Execute(choiceContext);
        if (play.Target != null)
            await CommonActions.Apply<MoltenWirePower>(choiceContext, play.Target, this);
        int intValue = DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        if (DeckVersion is not MoltenWire deckVersion)
            return;
        deckVersion.BuffFromPlay(intValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Increase"].UpgradeValueBy(1);
    }
    
    protected override void AfterDowngraded() => UpdateStrength();

    public void BuffFromPlay(int extraStrength)
    {
        IncreasedStrength += extraStrength;
        UpdateStrength();
    }

    public void UpdateStrength() => CurrentStrength = 3 + IncreasedStrength;
}