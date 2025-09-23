Console.WriteLine($"{Environment.CurrentManagedThreadId}");
await Task.Delay(500).ConfigureAwait(false);
Console.WriteLine($"{Environment.CurrentManagedThreadId}");