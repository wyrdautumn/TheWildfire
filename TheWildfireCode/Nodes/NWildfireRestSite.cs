using BaseLib.Patches.Fixes;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.RestSite;

namespace TheWildfire.TheWildfireCode.Nodes;

[GlobalClass]
public partial class NWildfireRestSite : NRestSiteCharacter
{
	[HarmonyPatch(typeof(NRestSiteCharacter), nameof(NRestSiteCharacter.FlipX))]
	public class DeshaRestSiteFlipXPatch
	{
		[HarmonyPostfix]
		private static void RestPositionPostfix(NRestSiteCharacter __instance)
		{
			Sprite2D? sprite = __instance.GetNodeOrNull<Sprite2D>("DeshaSprite");
			if (sprite == null)
				return;
			Control? control = __instance.GetNodeOrNull<Control>("ControlRoot");
			if (control != null)
			{
				sprite.Scale = sprite.Scale with
				{
					X = -sprite.Scale.X
				};
				sprite.Position = new Vector2(sprite.Position.X - 258, sprite.Position.Y);
				control.Position = new Vector2(control.Position.X + 198, control.Position.Y);
			}
		}
	}
}
