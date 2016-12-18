namespace Branslekollen.Core
{
    public interface IConfiguration
    {
        string ApiBaseUrl { get; }
    }

    public class Configuration : IConfiguration
    {
        public string ApiBaseUrl => "http://169.254.80.80:51058/api";
    }
}

