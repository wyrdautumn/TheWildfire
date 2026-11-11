using MegaCrit.Sts2.Core.Timeline;

namespace TheWildfire.TheWildfireCode.Epochs;

public class WildfireStory : StoryModel
{
    protected override string Id => "WILDFIRE";
    
    public override EpochModel[] Epochs =>
    [
        EpochModel.Get(EpochModel.GetId<Wildfire1Epoch>()),
        EpochModel.Get(EpochModel.GetId<Wildfire2Epoch>()),
        EpochModel.Get(EpochModel.GetId<Wildfire3Epoch>()),
        EpochModel.Get(EpochModel.GetId<Wildfire4Epoch>()),
        EpochModel.Get(EpochModel.GetId<Wildfire5Epoch>()),
        EpochModel.Get(EpochModel.GetId<Wildfire6Epoch>()),
        EpochModel.Get(EpochModel.GetId<Wildfire7Epoch>()),
    ];
}