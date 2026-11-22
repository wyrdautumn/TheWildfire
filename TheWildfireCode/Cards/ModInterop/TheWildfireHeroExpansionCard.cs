using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop;

public abstract class TheWildfireHeroExpansionCard : TheWildfireCard
{
    protected TheWildfireHeroExpansionCard(int cost, CardType type, CardRarity rarity, TargetType target, bool library = true, bool auto = false) : base(cost, type, rarity, target, library, auto)
    {
        var mysteriousFlashlightType =
            AccessTools.TypeByName("TheHeroExpansion.TheHeroExpansionCode.Extensions.CustomMysteriousFlashlightExtension");
        if (mysteriousFlashlightType != null)
            CustomContentDictionary.AddModel(GetType());
    }
}