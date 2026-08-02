using BaseLib.Abstracts;
using TheWildfire.TheWildfireCode.Extensions;
using Godot;

namespace TheWildfire.TheWildfireCode.Character;

public class TheWildfirePotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheWildfire.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}