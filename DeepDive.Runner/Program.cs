using BenchmarkDotNet.Running;
using EnumerableInterfaces;

BenchmarkRunner
   .Run<EnumerableFileReading>();


//new EnumerableFileReading()
//    .FileAsEnumerable();