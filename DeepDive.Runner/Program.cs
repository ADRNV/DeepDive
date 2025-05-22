using BenchmarkDotNet.Running;
using EnumerableInterfaces;
using StreamsAndAsync;


var example = new StreamsAndAsyncExample();

var dir = "C:\\tmp";

var tasks = new List<Task>();

Enumerable.Range(0, 30_000)
    .ToList()
    .ForEach(async (i) => {
        var task = example.WriteSampleAsync(StreamsAndAsyncExample.GenerateContent(100), dir + @$"\{i}.txt");
        tasks.Add(task);
    });

Task.WaitAll(tasks.ToArray());

BenchmarkRunner
   .Run<StreamsAndAsyncExample>();


//new EnumerableFileReading()
//    .FileAsEnumerable();