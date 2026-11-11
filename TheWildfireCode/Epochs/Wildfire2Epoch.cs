using MegaCrit.Sts2.Core.Timeline;

namespace TheWildfire.TheWildfireCode.Epochs;

public class Wildfire2Epoch : EpochModel
{
    public override string Id => "THEWILDFIRE-WILDFIRE2_EPOCH";
    public override EpochEra Era => EpochEra.Invitation3;
    public override int EraPosition => 3;
    public override string? StoryId => "Wildfire";
    
    public override EpochModel[] GetTimelineExpansion()
    {
        return new EpochModel[6]
        {
            EpochModel.Get(EpochModel.GetId<Wildfire1Epoch>()),
            EpochModel.Get(EpochModel.GetId<Wildfire3Epoch>()),
            EpochModel.Get(EpochModel.GetId<Wildfire4Epoch>()),
            EpochModel.Get(EpochModel.GetId<Wildfire5Epoch>()),
            EpochModel.Get(EpochModel.GetId<Wildfire6Epoch>()),
            EpochModel.Get(EpochModel.GetId<Wildfire7Epoch>()),
        };
    }
    
    public override void QueueUnlocks()
    {
        EpochModel.QueueTimelineExpansion(this.GetTimelineExpansion());
    }
}