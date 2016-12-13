using System.Linq;

namespace StatsDHelper
{
    internal class DomainNameProvider : IDomainNameProvider
    {
        private readonly IHostPropertiesProvider _hostPropertiesProvider;

        public DomainNameProvider(IHostPropertiesProvider hostPropertiesProvider)
        {
            _hostPropertiesProvider = hostPropertiesProvider;
        }

        public string GetFullyQualifiedDomainName()
        {
            var domainName = _hostPropertiesProvider.GetDomainName();
            var hostName = _hostPropertiesProvider.GetHostName();
            var domainSegment = string.Join(".", domainName.Split('.').Reverse());

            return string.Format("{0}.{1}", domainSegment, hostName);
        }
    }
}