using System.Security.Cryptography;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using TheWildfire.TheWildfireCode.Extensions;

namespace TheWildfire.TheWildfireCode.Interop;

public class WildfireCookieCursorPatch
{
    private const string CookieCursorModId = "CookieCursor";
    private const string CookieFile = "icon.png";
    private const string StampFile = "icon.thewildfire";
    private static bool _done;

    [HarmonyPatch(typeof(NMainMenu), "_Ready")]
    public static class ExportCookie
    {
        public static void Prefix()
        {
            if (_done) return;
            _done = true;
            try
            {
                Export();
            }
            catch (System.Exception e)
            {
                MainFile.Logger.Warn($"Wildfire CookieCursor cookie export failed: {e.Message}");
            }
        }
    }

    // The stamp holds the hash of the icon this mod last wrote: a matching icon is ours and is
    // replaced when the art changes, any other icon was put there by hand and is left alone
    private static void Export()
    {
        var mod = ModManager.GetLoadedMods().FirstOrDefault(m => m.manifest?.id == CookieCursorModId);
        if (mod == null) return;
        var root = Directory.Exists(mod.path) ? mod.path : Path.GetDirectoryName(mod.path);
        if (root == null) return;
        var dir = $"{root}/Cursors/thewildfire";
        var file = $"{dir}/{CookieFile}";
        var stamp = $"{dir}/{StampFile}";
        var art = "yummy_cookie_wildfire.png".BigRelicImagePath();
        var image = ComposeImage(art) ?? ResourceLoader.Load<Texture2D>(art)?.GetImage();
        if (image == null) return;
        if (image.IsCompressed()) image.Decompress();
        var png = image.SavePngToBuffer();
        var hash = System.Convert.ToHexString(SHA256.HashData(png));
        if (File.Exists(file))
        {
            var written = File.Exists(stamp) ? File.ReadAllText(stamp).Trim() : null;
            if (written == null) return;
            if (written == hash) return;
            if (System.Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(file))) != written) return;
        }
        Directory.CreateDirectory(dir);
        File.WriteAllBytes(file, png);
        File.WriteAllText(stamp, hash);
        MainFile.Logger.Info($"Wrote the Wildfire cookie for CookieCursor to {file}");
    }
    
    public static Image? ComposeImage(string art)
    {
        if (!art.StartsWith(MainFile.ResPath) || !art.EndsWith(".png")) return null;
        var outlinePath = art[..^4] + "_outline" + ".png";
        if (!ResourceLoader.Exists(outlinePath)) return null;
        var artImage = ResourceLoader.Load<Texture2D>(art, null, ResourceLoader.CacheMode.Reuse)?.GetImage();
        var outlineImage = ResourceLoader.Load<Texture2D>(outlinePath, null, ResourceLoader.CacheMode.Reuse)?.GetImage();
        if (artImage == null || outlineImage == null) return null;
        if (artImage.IsCompressed()) artImage.Decompress();
        if (outlineImage.IsCompressed()) outlineImage.Decompress();
        artImage.Convert(Image.Format.Rgba8);
        outlineImage.Convert(Image.Format.Rgba8);
        var band = Image.CreateEmpty(outlineImage.GetWidth(), outlineImage.GetHeight(), false, Image.Format.Rgba8);
        for (var y = 0; y < band.GetHeight(); y++)
        for (var x = 0; x < band.GetWidth(); x++)
            band.SetPixel(x, y, new Color(0, 0, 0, outlineImage.GetPixel(x, y).A * 0.5f));
        band.BlendRect(artImage, new Rect2I(0, 0, artImage.GetWidth(), artImage.GetHeight()), Vector2I.Zero);
        return band;
    }
}