using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fayble.Core.Helpers
{
    public static class PathHelpers
    {
        public static string GetRelativePath(string path, string libraryPath)
        {
            return path.Replace(libraryPath, string.Empty, StringComparison.InvariantCultureIgnoreCase).TrimStart('\\');
        }
    }
}
