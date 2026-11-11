using BaseLib.Abstracts;
using TheWildfire.TheWildfireCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;
using TheWildfire.TheWildfireCode.Epochs;

namespace TheWildfire.TheWildfireCode.Character;

public class TheWildfireRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => TheWildfire.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    
    public override IEnumerable<RelicModel> GetUnlockedRelics(UnlockState unlockState)
    {
        var list = AllRelics.ToList();
        if (!unlockState.IsEpochRevealed<Wildfire1Epoch>())
        {
            list.RemoveAll(r => Wildfire1Epoch.Relics.Any(relic => relic.Id == r.Id));
        }

        if (!unlockState.IsEpochRevealed<Wildfire4Epoch>())
        {
            list.RemoveAll(r => Wildfire4Epoch.Relics.Any(relic => relic.Id == r.Id));
        }
        return list;
    }
}