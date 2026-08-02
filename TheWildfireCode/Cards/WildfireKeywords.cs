using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheWildfire.TheWildfireCode.Cards;

public class WildfireKeywords
{
    [CustomEnum] public static CardKeyword Scorch;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)] public static CardKeyword Afterburn;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)] public static CardKeyword Discipline;
    [CustomEnum] public static StaticHoverTip Firepower;
    [CustomEnum] public static StaticHoverTip Ignite;
    [CustomEnum] public static StaticHoverTip IgniteStatic;
    [CustomEnum] public static StaticHoverTip Exert;
    [CustomEnum] public static StaticHoverTip ExertStatic;
    [CustomEnum] public static StaticHoverTip FlareStatic;
    [CustomEnum] public static CardTag FlareTag;
    [CustomEnum] public static StaticHoverTip Overheat;
    [CustomEnum] public static StaticHoverTip Rekindle;
}