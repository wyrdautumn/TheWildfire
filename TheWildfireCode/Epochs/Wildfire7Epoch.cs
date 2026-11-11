using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheWildfire.TheWildfireCode.Cards.Common;
using TheWildfire.TheWildfireCode.Cards.Rare;
using TheWildfire.TheWildfireCode.Cards.Uncommon;

namespace TheWildfire.TheWildfireCode.Epochs;

public class Wildfire7Epoch : EpochModel
{
    public override string Id => "THEWILDFIRE-WILDFIRE7_EPOCH";
    public override EpochEra Era => EpochEra.Invitation7;
    public override int EraPosition => 3;
    public override string? StoryId => "Wildfire";
    
    public static List<CardModel> Cards =>
    [
        ModelDb.Card<LivingBonfire>(),
        ModelDb.Card<CraneDance>(),
        ModelDb.Card<MasterOfFlow>()
    ];
    
    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}