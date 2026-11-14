using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheWildfire.TheWildfireCode.Relics;

namespace TheWildfire.TheWildfireCode.Epochs;

public class Wildfire4Epoch : EpochModel
{
    public override string Id => "THEWILDFIRE-WILDFIRE4_EPOCH";
    public override EpochEra Era => EpochEra.Invitation4;
    public override int EraPosition => 5;
    public override string? StoryId => "Wildfire";

    public static List<RelicModel> Relics =>
    [
        ModelDb.Relic<BoxOfTinder>(),
        ModelDb.Relic<ElementalInfuser>(),
        ModelDb.Relic<Matchbook>()
    ];

    public override string UnlockText => CreateRelicUnlockText(Relics);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueRelicUnlock(Relics);
    }
}