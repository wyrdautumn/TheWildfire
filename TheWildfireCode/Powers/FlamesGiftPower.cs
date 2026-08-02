using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using TheWildfire.TheWildfireCode.Cards.Common;
using TheWildfire.TheWildfireCode.Extensions;

namespace TheWildfire.TheWildfireCode.Powers;

public class FlamesGiftPower : CustomTemporaryPowerModelWrapper<FlamesGift, ConstitutionPower>
{
        protected override bool UntilEndOfOtherSideTurn => true;
        public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
        public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ConstitutionPower>()];
        public override LocString Title => new LocString("powers", "THEWILDFIRE-CONSTITUTION_UP_POWER.title");
        public override LocString Description => new LocString("powers", "THEWILDFIRE-CONSTITUTION_UP_POWER.description");
        protected override string SmartDescriptionLocKey => "THEWILDFIRE-CONSTITUTION_UP_POWER.smartDescription";
}