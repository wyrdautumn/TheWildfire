using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheWildfire.TheWildfireCode.Cards;
using TheWildfire.TheWildfireCode.Character;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop.HeroExpansion;

[Pool(typeof(EventCardPool))]
public class PathToPerfection : TheWildfireHeroExpansionCard
{
    public PathToPerfection() : base(1,
        CardType.Skill, CardRarity.Event,
        TargetType.Self)
    {
        var mysteriousFlashlightType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension");
        if (mysteriousFlashlightType != null)
        {
            var addMethod = AccessTools.DeclaredMethod(mysteriousFlashlightType, "AddFlashlightCardForCustomCharacter");
            addMethod.Invoke(null, [this, ModelDb.Character<Character.TheWildfire>()]);
        }
    }
    
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheWildfireCardPool>();
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [..AddHeroExpansion(),HoverTipFactory.FromCard<TalonSurge>(IsUpgraded),HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        CardPile? discard = CardPile.Get(PileType.Discard, Owner);
        if (discard == null)
            return;
        List<CardModel> statuses =
            discard.Cards.Where(c => c.Type == CardType.Status).ToList();
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        foreach (CardModel card in statuses)
        {
            CardPileAddResult? nullable = await CardCmd.TransformTo<TalonSurge>(card);
            if (IsUpgraded && nullable.HasValue)
            {
                CardCmd.Upgrade(nullable.Value.cardAdded);
            }
        }
    }

    protected override void OnUpgrade()
    {

    }
}