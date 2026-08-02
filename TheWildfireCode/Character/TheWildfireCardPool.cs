using BaseLib.Abstracts;
using TheWildfire.TheWildfireCode.Extensions;
using Godot;

namespace TheWildfire.TheWildfireCode.Character;

public class TheWildfireCardPool : CustomCardPoolModel
{
    public override string Title => TheWildfire.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/desha_big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/desha_text_energy.png".ImagePath();
    
    public override Color EnergyOutlineColor => new Color("562F53");


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => .877f; //Hue; changes the color.
    public override float S => .655f; //Saturation
    public override float V => .808f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load TheWildfire/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("5d2461");

    public override bool IsColorless => false;
}