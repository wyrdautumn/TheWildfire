using BaseLib.Config;
using Godot;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using TheWildfire.TheWildfireCode.Epochs;

namespace TheWildfire.TheWildfireCode.Character;

public class TheWildfireConfig : SimpleModConfig
{
    public override void SetupConfigUI(Control optionContainer)
    {
        GenerateOptionsForAllProperties(optionContainer);
        SetupFocusNeighbors(optionContainer);
    }
    
    [ConfigButton("AllStuffUnlocked")]
    public static void UnlockAllStuff()
    {
        List<string> epochs =
        [
            EpochModel.GetId<Wildfire1Epoch>(),
            EpochModel.GetId<Wildfire2Epoch>(),
            EpochModel.GetId<Wildfire3Epoch>(),
            EpochModel.GetId<Wildfire4Epoch>(),
            EpochModel.GetId<Wildfire5Epoch>(),
            EpochModel.GetId<Wildfire6Epoch>(),
            EpochModel.GetId<Wildfire7Epoch>()
        ];
        foreach (string epochId in epochs.Except(SaveManager.Instance.Progress.Epochs.Where(e => e.State == EpochState.Revealed).Select( e => e.Id)))
            SaveManager.Instance.ObtainEpochOverride(epochId, EpochState.Revealed);
    }
}