
namespace EnumerableInterfaces
{
    public class StreamReaderAsyncEnumerator : StreamReaderEnumerator, IAsyncEnumerator<string>
    {
        public StreamReaderAsyncEnumerator(string filePath) : base(filePath)
        {
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.Run(() => _sr.Dispose()));
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            _current = await _sr.ReadLineAsync();

            if (_current == null)
                return false;
            return true;
        }
    }
}
