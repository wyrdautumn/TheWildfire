using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWildfire.TheWildfireCode.Cards.Variables;

public class AfterburnVar : DynamicVar
{
    public const string Key = "Afterburn";

    public AfterburnVar(decimal afterburnCount) : base(Key, afterburnCount)
    {
        this.WithTooltip();
    }
}