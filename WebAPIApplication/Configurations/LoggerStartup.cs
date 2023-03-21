using Serilog;

namespace WebAPIApplication.Configurations
{
    public static class LoggerStartup
    {
        public static ILoggingBuilder AddApplicationLogging(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            return loggingBuilder.AddSerilog(Log.Logger);
        }

        public static IApplicationBuilder UseApplicationLogging(this IApplicationBuilder app)
        {
            return app.UseSerilogRequestLogging();
        }
    }
}
