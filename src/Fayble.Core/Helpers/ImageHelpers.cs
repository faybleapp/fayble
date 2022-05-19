using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Fayble.Core.Helpers;

public static class ImageHelpers
{
    public static void SaveImage(Image image, string imagePath, int height)
    {
        var path = Path.Combine(Path.GetDirectoryName(imagePath)!,
            $"{Path.GetFileNameWithoutExtension(imagePath)}-{height}.jpg");
        
        image.Mutate(i => i.Resize(new ResizeOptions
        {
            Size = new Size(0, height),
            Mode = ResizeMode.Max
        }));

        image.SaveAsJpeg(path);
    }
}