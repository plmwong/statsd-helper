using System.Configuration;
using System.Diagnostics;
using StatsdClient;


namespace StatsDHelper
{
    public class StatsDHelper : IStatsDHelper
    {
        private readonly IPrefixProvider _prefixProvider;
        private readonly IStatsd _statsdClient;
        private string _prefix;

        private static readonly object Padlock = new object();
        private static IStatsDHelper _instance;

        internal StatsDHelper(IPrefixProvider prefixProvider, IStatsd statsdClient)
        {
            _prefixProvider = prefixProvider;
            _statsdClient = statsdClient;
        }

        public IStatsd StatsdClient
        {
            get{return _statsdClient;}
        }

        public void LogCount(string name, int count = 1, params string[] tags)
        {
            _statsdClient.Add<Statsd.Counting, int>(string.Format("{0}.{1}", GetStandardPrefix, name), count, 1.0f, tags);
        }

        public void LogGauge(string name, int value, params string[] tags)
        {
            _statsdClient.Add<Statsd.Gauge, int>(string.Format("{0}.{1}", GetStandardPrefix, name), value, 1.0f, tags);
        }

        public void LogTiming(string name, long milliseconds, params string[] tags)
        {
            _statsdClient.Add<Statsd.Timing, long>(string.Format("{0}.{1}", GetStandardPrefix, name), milliseconds, 1.0f, tags);
        }

        public void LogSet(string name, int value, params string[] tags)
        {
            _statsdClient.Add<Statsd.Set, int>(string.Format("{0}.{1}", GetStandardPrefix, name), value, 1.0f, tags);
        }

        public string GetStandardPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_prefix))
                {
                    _prefix = _prefixProvider.GetPrefix();
                }
                return _prefix;
            }
        }

        public static IStatsDHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Padlock)
                    {
                        if (_instance == null)
                        {
                            var host = ConfigurationManager.AppSettings["StatsD.Host"];
                            var port = ConfigurationManager.AppSettings["StatsD.Port"];
                            var applicationName = ConfigurationManager.AppSettings["StatsD.ApplicationName"];

                            if (string.IsNullOrEmpty(host)
                                || string.IsNullOrEmpty(port)
                                || string.IsNullOrEmpty(applicationName))
                            {
                                Debug.WriteLine(
                                    "One or more StatsD Client Settings missing. This is designed to fail silently. Ensure an application name, host and port are set or no metrics will be sent. Set Values: Host={0} Port={1}",
                                    host, port);
                                return new NullStatsDHelper();
                            }

                            _instance = new StatsDHelper(new PrefixProvider(new HostPropertiesProvider()), new Statsd(new StatsdUDP(host, int.Parse(port))));
                        }
                    }
                }
                return _instance;
            }
        }
    }
}