using BaseLib.Abstracts;
using BaseLib.Extensions;
using TheWildfire.TheWildfireCode.Extensions;
using Godot;

namespace TheWildfire.TheWildfireCode.Powers;

public abstract class TheWildfirePower : CustomPowerModel
{
    //Loads from TheWildfire/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}