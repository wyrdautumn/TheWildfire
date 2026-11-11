using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Timeline;
using TheWildfire.TheWildfireCode.Potions;

namespace TheWildfire.TheWildfireCode.Epochs;

public class Wildfire6Epoch : EpochModel
{
    public override string Id => "THEWILDFIRE-WILDFIRE6_EPOCH";
    public override EpochEra Era => EpochEra.Invitation6;
    public override int EraPosition => 3;
    public override string? StoryId => "Wildfire";

    public static List<PotionModel> Potions =>
    [
        ModelDb.Potion<BurningElixir>(),
        ModelDb.Potion<FlowPotion>(),
        ModelDb.Potion<SoothingTonic>()
    ];

    public override string UnlockText => CreatePotionUnlockText(Potions);

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueuePotionUnlock(Potions);
        NTimelineScreen.Instance.QueueMiscUnlock(new LocString("epochs", this.Id + ".unlock").GetFormattedText() ?? "");

    }
}