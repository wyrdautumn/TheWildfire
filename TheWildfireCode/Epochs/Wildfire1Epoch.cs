using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Epochs;

public class Wildfire1Epoch : EpochModel
{
    public override string Id => "THEWILDFIRE-WILDFIRE1_EPOCH";
    public override EpochEra Era => EpochEra.Invitation1;
    public override int EraPosition => 5;
    public override string? StoryId => "Wildfire";

    public static List<RelicModel> Relics =>
    [
        ModelDb.Relic<AshGown>(),
        ModelDb.Relic<SparkGuards>(),
        ModelDb.Relic<MartialArtistsBelt>()
    ];

    public override string UnlockText => CreateRelicUnlockText(Relics);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueRelicUnlock(Relics);
    }
}