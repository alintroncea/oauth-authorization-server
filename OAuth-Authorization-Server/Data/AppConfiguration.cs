using System.Reflection;
using System.Configuration;

namespace OAuth_Authorization_Server.Data
{
    public class AppConfiguration
    {
        public readonly string _connectionString = string.Empty;
        public AppConfiguration()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string assemblyPath = Uri.UnescapeDataString(uri.Path);
            Configuration cfg = System.Configuration.ConfigurationManager.OpenExeConfiguration(assemblyPath);
            _connectionString = cfg.AppSettings.Settings["AuthorizationDatabase"].Value;
        }
        public string ConnectionString
        {
            get => _connectionString;
        }
    }
}
