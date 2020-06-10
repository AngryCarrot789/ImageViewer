using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    public static class FileHelpers
    {
        public static bool IsFile(this string path)
        {
            return !string.IsNullOrEmpty(path) && File.Exists(path);
        }

        public static bool IsDirectory(this string path)
        {
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }

        public static bool IsDrive(this string path)
        {
            return !string.IsNullOrEmpty(path) && path.Length < 3;
        }

        public static string GetParentFolder(this string path)
        {
            return new DirectoryInfo(path).Parent.FullName;
        }

        public static string FormatFileNumberForSort(string inVal)
        {
            if (int.TryParse(Path.GetFileName(inVal), out int o))
                return string.Format("{0:0000000000}", o);
            else
                return inVal;
        }
    }
}
