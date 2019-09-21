using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fluent.Architecture.Core.Doc
{
    public class Program
    {
        public static string IP { get; set; }
        public static int Porta { get; set; }

        public static void Main(string[] args)
        {
            foreach (var a in args) { Console.WriteLine(a); }
            if (args.Length == 2) { IP = args[0]; Porta = int.Parse(args[1]); }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                if (!string.IsNullOrEmpty(IP) && Porta > 0)
                {
                    webBuilder.ConfigureKestrel((context, serverOptions) =>
                    {
                        serverOptions.Listen(IPAddress.Parse(IP), Porta);
                    });
                }
            });
        }
    }
}
