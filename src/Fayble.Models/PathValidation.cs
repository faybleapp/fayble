namespace Fayble.Models;

public class PathValidation
{
    public string Path { get; private set; }

    public PathValidation(string path)
    {
        Path = path;
    }
}