using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheWildfire.TheWildfireCode.Cards.Common;
using TheWildfire.TheWildfireCode.Cards.Rare;
using TheWildfire.TheWildfireCode.Cards.Uncommon;

namespace TheWildfire.TheWildfireCode.Epochs;

public class Wildfire3Epoch : EpochModel
{
    public override string Id => "THEWILDFIRE-WILDFIRE3_EPOCH";
    public override EpochEra Era => EpochEra.Invitation3;
    public override int EraPosition => 4;
    public override string? StoryId => "Wildfire";
    
    public static List<CardModel> Cards =>
    [
        ModelDb.Card<FierySerpent>(),
        ModelDb.Card<FlintStrike>(),
        ModelDb.Card<GoAllOut>()
    ];
    
    public override string UnlockText => CreateCardUnlockText(Cards);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCardUnlock(Cards);
    }
}