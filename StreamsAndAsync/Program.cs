//using StreamsAndAsync;
//using System.Diagnostics;

//var ex = new StreamsAndAsyncExample();

//var text = StreamsAndAsyncExample.GenerateContent(50_000);
//var sw = new Stopwatch();
//sw.Start();

////Console.WriteLine(await Task.WhenAny(ex.CompleteFirst(), ex.ReadTextAsync(Environment.CurrentDirectory + "txt.txt"), ex.WriteSampleAsync(text)));
//await ex.WriteSampleAsync(text).ConfigureAwait(false);
//await ex.ReadTextAsync(Environment.CurrentDirectory + "txt.txt");
//await ex.WriteSampleAsync("1234567890").ConfigureAwait(false);
//await ex.ReadTextAsync(Environment.CurrentDirectory + "txt.txt");
//Console.WriteLine("\n AFTER WRITE");
//await ex.ReadTextAsync(Environment.CurrentDirectory + "txt.txt").ConfigureAwait(false);
//await ex.ReadTextAsync(Environment.CurrentDirectory + "txt.txt").ConfigureAwait(false);
//sw.Stop();
//Console.WriteLine($"\n\n\n TOTAL ELAPSED {sw.ElapsedMilliseconds}ms");


Console.ReadLine();
