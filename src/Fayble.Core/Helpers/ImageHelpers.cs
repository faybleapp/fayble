using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Fayble.Core.Helpers;

public static class ImageHelpers
{
    public static void ResizeImage(string imagePath, int height)
    {
        var path = Path.Combine(Path.GetDirectoryName(imagePath)!,
            $"{Path.GetFileNameWithoutExtension(imagePath)}-{height}.jpg");

        using var image = Image.Load(imagePath);
        image.Mutate(i => i.Resize(new ResizeOptions
        {
            Size = new Size(0, height),
            Mode = ResizeMode.Max
        }));

        image.Save(path);
    }
}