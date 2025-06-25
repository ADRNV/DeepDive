using BenchmarkDotNet.Running;

BenchmarkRunner
   .Run<TPLExamples.BenchmarkRunner>();

//new EnumerableFileReading()
//    .FileAsEnumerable();