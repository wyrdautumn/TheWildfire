using MegaCrit.Sts2.Core.Entities.Powers;

namespace TheWildfire.TheWildfireCode.Powers;

public class GreaterFortitudePower : TheWildfirePower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}