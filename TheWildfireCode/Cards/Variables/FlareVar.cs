using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWildfire.TheWildfireCode.Cards.Variables;

public class FlareVar : DynamicVar
{
    public const string Key = "Flare";

    public FlareVar(decimal Flare) : base(Key, Flare)
    {
        this.WithTooltip();
    }
}