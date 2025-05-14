using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskVsValueTask
{
    public static class Utils
    {
        public static IEnumerable<Guid> GetGuids(int count = 10_000) => Enumerable.Range(0, count).Select(i => Guid.NewGuid()).AsEnumerable();

        public static void WriteSampleAsync(string text, string path = null)
        {
            if (path == null)
            {
                path = Environment.CurrentDirectory + "txt.txt";
            }

            if (File.Exists(path)) File.Delete(path);

            using var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1);

            var textBytes = Encoding.UTF8.GetBytes(text);

            fs.Write(textBytes, 0, textBytes.Length);
        }
    }
}
