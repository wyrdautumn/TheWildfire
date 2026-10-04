using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;
using TheWildfire.TheWildfireCode.Cards.Common;
using TheWildfire.TheWildfireCode.Extensions;

namespace TheWildfire.TheWildfireCode.Powers;

public class ScatterEmbersPower : CustomTemporaryPowerModelWrapper<ScatterEmbers, StrengthPower>
{
        public override PowerType Type => PowerType.Debuff;
        protected override bool InvertInternalPowerAmount => true;
        public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
        public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}