namespace LocalClient.Models
{
    public class RunReport(int connectionsCount, int failuresCount, long elapsedMs)
    {
        public int ConnectionsCount { get => connectionsCount; }

        public int FailuresCount { get => failuresCount; }

        public long ElapsedMs { get => elapsedMs; }
    }
}
