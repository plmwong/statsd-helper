using StatsdClient;

namespace StatsDHelper
{
    public interface IStatsDHelper
    {
        void LogCount(string name, int count = 1, params string[] tags);
        void LogGauge(string name, int value, params string[] tags);
        void LogTiming(string name, long milliseconds, params string[] tags);
        void LogSet(string name, int value, params string[] tags);
        IStatsd StatsdClient { get; }
    }
}