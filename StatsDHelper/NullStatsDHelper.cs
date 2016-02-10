using StatsdClient;

namespace StatsDHelper
{
    public class NullStatsDHelper : IStatsDHelper
    {
        internal NullStatsDHelper() { }
        public void LogCount(string name, int count = 1, params string[] tags) {}
        public void LogGauge(string name, int value, params string[] tags) {}
        public void LogTiming(string name, long milliseconds, params string[] tags) {}
        public void LogSet(string name, int value, params string[] tags) {}

        public IStatsd StatsdClient
        {
            get { return null; }
        }
    }
}