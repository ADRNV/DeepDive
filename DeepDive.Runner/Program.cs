using BenchmarkDotNet.Running;

BenchmarkRunner
   .Run<LocalClient.BenchhmarkRunner>(null, [ "100" ]);

Console.Read();
