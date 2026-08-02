using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using TheWildfire.TheWildfireCode.Cards.Uncommon;
using TheWildfire.TheWildfireCode.Extensions;

namespace TheWildfire.TheWildfireCode.Powers;

public class ConstitutionDownPower : CustomTemporaryPowerModelWrapper<FlameConsuming, ConstitutionPower>
{
        public override PowerType Type => PowerType.Debuff;
        protected override bool InvertInternalPowerAmount => true;
        public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
        public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
        protected override bool UntilEndOfOtherSideTurn => true;
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ConstitutionPower>()];
        public override LocString Title => new LocString("powers", "THEWILDFIRE-CONSTITUTION_DOWN_POWER.title");
        public override LocString Description => new LocString("powers", "THEWILDFIRE-CONSTITUTION_DOWN_POWER.description");
        protected override string SmartDescriptionLocKey => "THEWILDFIRE-CONSTITUTION_DOWN_POWER.smartDescription";
}