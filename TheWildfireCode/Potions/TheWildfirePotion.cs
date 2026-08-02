using BaseLib.Abstracts;
using BaseLib.Utils;
using TheWildfire.TheWildfireCode.Character;

namespace TheWildfire.TheWildfireCode.Potions;

[Pool(typeof(TheWildfirePotionPool))]
public abstract class TheWildfirePotion : CustomPotionModel;