using System.Text;

namespace StreamsAndAsync
{
    public class StreamsAndAsyncExample
    {
        //public Task InvokeWithAllAwait()
        //{

        //}

        //public Task InvokeWithAnyAwait()
        //{

        //}

        public async Task<int> CompleteFirst()
        {
            await Task.Delay(5000);
            return await Task.FromResult(1);
        }
        public async Task WriteSampleAsync(string text,string path = null)
        {
           if(path == null)
           {
             path = Environment.CurrentDirectory + "txt.txt";
           }

            if (File.Exists(path)) File.Delete(path);

            using var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1);

            var textBytes = Encoding.UTF8.GetBytes(text);

            await fs.WriteAsync(textBytes, 0, textBytes.Length).ConfigureAwait(false);
        }
        public async Task<string> ReadTextAsync(string filePath)
        {
            using var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true);

            var sb = new StringBuilder();

            byte[] buffer = new byte[0x1000];
            int numRead;
            while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                sb.Append(text);
            }
            await Task.Delay(3000);
            return sb.ToString();
        }

        public static string GenerateContent(int length = 10)
        {
           return Enumerable.Range(1, length).AsParallel()
               .Select(i => Guid.NewGuid().ToString())
               .Aggregate((a, b) => $"{a}\n{b}");
        }
    }
}
