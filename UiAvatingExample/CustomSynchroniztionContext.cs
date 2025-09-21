using System.Collections.Concurrent;

public class SingleThreadSynchronizationContext : SynchronizationContext, IDisposable
{
    private readonly BlockingCollection<(SendOrPostCallback, object?)> _queue = new();
    private readonly Thread _thread;
    private bool _disposed;

    public SingleThreadSynchronizationContext(string threadName = "SingleThread")
    {
        _thread = new Thread(RunOnThread)
        {
            Name = threadName,
            IsBackground = true
        };

        _thread.Start();
    }

    private void RunOnThread()
    {
        // Устанавливаем себя как текущий контекст

        var previusCtx = SynchronizationContext.Current;

        SetSynchronizationContext(this);


        while (!_disposed)
        {
            try
            {
                var (callback, state) = _queue.Take();
                callback(state); // Выполняем на этом потоке
            }
            catch (OperationCanceledException ex)
            {
                break;
            }
            finally
            {
                SetSynchronizationContext(previusCtx);
            }
        }
    }

    public override void Post(SendOrPostCallback d, object? state)
    {
        if (_disposed) return;
        _queue.Add((d, state)); // Добавляем в очередь
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        if (_disposed) return;

        var tcs = new TaskCompletionSource(state);
        Post(_ =>
        {
            try
            {
                d(state);
                tcs.SetResult();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }, null);

        tcs.Task.Wait(); // Блокируем пока не выполнится
    }

    public void Dispose()
    {
        _disposed = true;
        _queue.CompleteAdding();
    }
}