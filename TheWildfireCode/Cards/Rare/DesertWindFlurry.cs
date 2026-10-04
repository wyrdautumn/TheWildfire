using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards.Rare;

public class DesertWindFlurry() : TheWildfireCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move),
        new CalculationBaseVar(6),
        new CalculationExtraVar(1),
        new CalculatedVar("hits").WithMultiplier(Calc)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(WildfireKeywords.Firepower)];
    
    private static decimal Calc(CardModel card, Creature? arg2)
    {
        if (card.Owner.PlayerCombatState == null)
            return 0;
        int val = FirepowerController.Firepower.Get(card.Owner.PlayerCombatState);
        if (val > card.DynamicVars.CalculationBase.IntValue)
            return -card.DynamicVars.CalculationBase.IntValue;
        return -val;
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        if (CombatState == null)
            return;
        int num1 = (int) ((CalculatedVar)DynamicVars["hits"]).Calculate(null);
        Color color = new Color("a85bdd80");
        double num2 = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3;
        NCombatRoom? instance1 = NCombatRoom.Instance;
        if (instance1 != null)
            instance1.CombatVfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(color, 0.8 + Mathf.Min(8, num1) * num2));
        SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_whirlwind");
        NRun? instance2 = NRun.Instance;
        if (instance2 != null)
            instance2.GlobalUi.AddChildSafely(NSmokyVignetteVfx.Create(color, color)); 
        await CommonActions.CardAttack(this, play, num1, vfx:"vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}