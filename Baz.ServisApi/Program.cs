using Gelf.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Baz.KisiServisApi
{
    /// <summary>
    /// API'ýn çalýþmasý için gereken Main() methodunu barýndýran class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// API'ý ayaða kaldýran method.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Host oluþturan method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((context, builder) => builder.AddGelf(options =>
                {
                    options.LogSource = context.HostingEnvironment.ApplicationName;
                    options.AdditionalFields["machine_name"] = Environment.MachineName;
                    options.AdditionalFields["app_version"] = Assembly.GetEntryAssembly()
                        ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                }));
    }
}