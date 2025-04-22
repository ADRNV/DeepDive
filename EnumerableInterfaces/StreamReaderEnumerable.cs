using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumerableInterfaces
{
    public class StreamReaderAsyncEnumerable : IEnumerable<string>, IAsyncEnumerable<string>
    {
        private string _filePath;
        public StreamReaderAsyncEnumerable(string filePath)
        {
            _filePath = filePath;
        }

        // Must implement GetEnumerator, which returns a new StreamReaderEnumerator.
        public IEnumerator<string> GetEnumerator()
        {
            return new StreamReaderEnumerator(_filePath);
        }

        // Must also implement IEnumerable.GetEnumerator, but implement as a private method.
        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }

        public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new StreamReaderAsyncEnumerator(_filePath);
        }
    }

}
