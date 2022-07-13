namespace ApiConsumer
{
    public sealed class ServicesConfig
    {
        public class ServiceInfo
        {
            public string BaseUrl { get; set; }
            public string apiKey { get; set; }
        }

        public ServiceInfo CompanyAService { get; set; }
        public ServiceInfo CompanyBService { get; set; }
        public ServiceInfo CompanyCService { get; set; }
    }
}
