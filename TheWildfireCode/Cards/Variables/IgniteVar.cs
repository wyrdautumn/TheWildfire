using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWildfire.TheWildfireCode.Cards.Variables;

public class IgniteVar : DynamicVar
{
    public const string Key = "Ignite";

    public IgniteVar(decimal igniteCount) : base(Key, igniteCount)
    {
        this.WithTooltip();
    }
}