using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheWildfire.TheWildfireCode.Cards.Variables;

public class ExertVar : DynamicVar
{
    public const string Key = "Exert";

    public ExertVar(decimal exertCount) : base(Key, exertCount)
    {
        this.WithTooltip();
    }
}