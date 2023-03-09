using dotenv.net;
using dotenv.net.Utilities;

namespace WeRaven.Tools;

public static class EnvTool
{
    public static readonly string JwtKey  = EnvReader.GetStringValue("SECRET");
    public static readonly SmtpConfiguration Smtp  = new();
    public static readonly string DbConnection  = EnvReader.GetStringValue("DATABASE_URL");
    public static readonly string MongoConnection = EnvReader.GetStringValue("MONGO_URL");
    public static readonly string CorsName = "WERAVEN";
    
    public static void Configure()
    {
        var appRoot = EnvTool.IsDebug() ? Directory.GetCurrentDirectory() : AppDomain.CurrentDomain.BaseDirectory;
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { Path.Combine(appRoot, ".env") }));
    }
    public static bool IsDebug()
    {
        #if DEBUG
            return true;
        #else
            return false;
        #endif
    }

    public class SmtpConfiguration
    {
        public string Host { get; set; } = EnvReader.GetStringValue("MAIL_HOST");
        public int Port { get; set; } = EnvReader.GetIntValue("MAIL_PORT");
        public string UserName { get; set; } = EnvReader.GetStringValue("MAIL_USER");
        public string Password { get; set; } = EnvReader.GetStringValue("MAIL_PASS");
    }
}