using ClientsDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AutofacTutorial
{
    internal class Program
    {
        private static IHost _host;

        public static IHost Hosting => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(options =>
                options.AddJsonFile("appsettings.json"))
                .ConfigureAppConfiguration(options =>
                options
                .AddJsonFile("appsettings.json")
                //.AddXmlFile("appsettings.xml", true)
                //.AddIniFile("appsettings.ini", true)//булевая переменная указывает на то что файлов может и не быть
                .AddEnvironmentVariables()
                .AddCommandLine(args))
                .ConfigureLogging(options =>
                options.ClearProviders()//сконфигурировать логирование с нуля 
                .AddConsole()
                .AddDebug())
                .ConfigureServices(ConfigureServices);
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            #region Configure EF DBContext Service

            services.AddDbContext<ClientContext>(optionts =>
            {
                optionts.UseSqlServer(host.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });

            #endregion
        }

        public static IServiceProvider Services => Hosting.Services;

        public static async Task Main(string[] args)
        {          
            var host = Hosting;
            host.Start();
            var serviceScope = Services.CreateScope();
            var service = serviceScope.ServiceProvider;
            var clientDB = service.GetRequiredService<ClientContext>();

            var task1 = Calculate(10);
            var task2 = Calculate(20);
            var task3 = Calculate(30);

            await Task.WhenAll(task1, task2, task3);
            //await Task.WhenAny(task1,task2, task3); 
            Console.WriteLine($"the end");
            //Console.WriteLine($"{task1.Result} ; {task2.Result} ; {task3.Result}");
            
        }

        public static async Task<int> Calculate(int n)
        {
            await Task.Delay(2000);
            return n * n;
        }
    }
}