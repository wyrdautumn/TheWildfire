using BaseLib.Utils.ModInterop;
using MegaCrit.Sts2.Core.Models;

namespace TheWildfire.TheWildfireCode.Cards.ModInterop;

[ModInterop("AncientsAwakened", "AncientsAwakened.AncientsAwakenedCode.CrossModCompatability.CustomPerfectedCardExtension")]
public static class AncientsAwakenedPerfectedInterop
{
    public static void AddPerfectedCardForCustomCharacters(this CardModel cardModel, CardModel originalCardModel)
    {
    }

    public static CardPoolModel? PerfectedCardPool(CardPoolModel? optionalFallBack)
    {
        return optionalFallBack;
    }
}