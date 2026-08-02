using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheWildfire.TheWildfireCode.Character;
using TheWildfire.TheWildfireCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheWildfire.TheWildfireCode.Firepower;

namespace TheWildfire.TheWildfireCode.Cards;

[Pool(typeof(TheWildfireCardPool))]
public abstract class TheWildfireCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target), IFirepowerListener
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    public int ResolveExert()
    {
        if (Owner.PlayerCombatState == null)
            return 0;
        int fire = FirepowerController.Firepower.Get(Owner.PlayerCombatState);
        int exert = DynamicVars["Exert"].IntValue;
        if (fire > exert)
        {
            return exert;
        }
        return fire;
    }

    public virtual Task AfterIgnite(PlayerChoiceContext choiceContext, int amount, Player igniter) => Task.CompletedTask;

    public virtual Task AfterScorch(PlayerChoiceContext choiceContext, int amount, Player scorcher) => Task.CompletedTask;

    public virtual Task AfterExert(PlayerChoiceContext choiceContext, int amount, Player exerter)  => Task.CompletedTask;

    public virtual Task AfterFlare(PlayerChoiceContext choiceContext, int amount, Player flarer)  => Task.CompletedTask;

    public virtual Task AfterburnPlayed(PlayerChoiceContext choiceContext, Player afterburner)  => Task.CompletedTask;

    public virtual Task AfterBurnStatusTrigger(PlayerChoiceContext choiceContext, Player burnee)  => Task.CompletedTask;
    public virtual Task AfterOverheatDamage(PlayerChoiceContext choiceContext, int damage, int damageMitigated,
        Player overheater) => Task.CompletedTask;
}