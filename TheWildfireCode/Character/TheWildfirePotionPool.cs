using BaseLib.Abstracts;
using TheWildfire.TheWildfireCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using TheWildfire.TheWildfireCode.Epochs;

namespace TheWildfire.TheWildfireCode.Character;

public class TheWildfirePotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheWildfire.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    
    protected override IEnumerable<PotionModel> GenerateAllPotions()
    {
        return Wildfire6Epoch.Potions;
    }

    public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
    {
        if (!unlockState.IsEpochRevealed<Wildfire6Epoch>())
        {
            return Array.Empty<PotionModel>();
        }

        return GenerateAllPotions();
    }
}