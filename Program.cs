using Autofac;
using ClientsDb;
using ClientsDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutofacTutorial.Interface;
using AutofacTutorial.Services.Impl;
using AutofacTutorial.Models.Reports;
using AutofacTutorial.Services;
using AutofacTutorial.Extentions;

namespace AutofacTutorial
{
    //Домашняя работа #6 "Знакомство с autofac"
    internal class Program
    {
        private static WebApplication? _app;

        public static WebApplication App // => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        {
            get
            {
                if(_app == null)
                {
                    _app = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
                    if (!_app.Environment.IsDevelopment())
                    {
                        _app.UseExceptionHandler("/Home/Error");
                    }
                    _app.UseStaticFiles();

                    _app.UseRouting();

                    _app.UseAuthorization();

                    _app.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                }
                return _app;
            }
        }

        //добавляем пакеты [1]TemplateEngine.Docx
        public static WebApplicationBuilder CreateHostBuilder(string[] args)
        {
            //меняем host на webApplication

            var webAplicationBuilder = WebApplication.CreateBuilder(args);
            webAplicationBuilder.Host
                .ConfigureHostConfiguration(options =>
                options.AddJsonFile("appsettings.json"))
                .ConfigureAppConfiguration(options =>
                options
                .AddJsonFile("appsettings.json")
                .AddXmlFile("appsettings.xml", true)
                .AddIniFile("appsettings.ini", true)//булевая переменная указывает на то что файла может и не быть
                .AddEnvironmentVariables()
                .AddCommandLine(args))
                .ConfigureLogging(options =>
                options.ClearProviders()//сконфигурировать логирование с нуля 
                .AddConsole()
                .AddDebug())
                .ConfigureServices(ConfigureServices);

            return webAplicationBuilder;
        }

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            #region Register Base Services
            services.AddControllersWithViews();
            #endregion

            #region Configure EF DBContext Service
            services.AddDbContext<ClientContext>(optionts =>
            {
                optionts.UseSqlServer(host.Configuration["Settings:DatabaseOptions:ConnectionString"]);
            });
            #endregion
        }

        public static IServiceProvider Services => App.Services;

        public static async Task Main(string[] args)
        {
            var host = App;
            host.Run();
            var serviceScope = Services.CreateScope();
            var service = serviceScope.ServiceProvider;
            var clientDB = service.GetRequiredService<ClientContext>();
            /*
            string? reply = "no";
            do
            {
                Console.WriteLine("client/order");
                string answer = Console.ReadLine();
                if (answer == "client")
                    FillClient(clientDB);
                if (answer == "order")
                    FillOrder(clientDB);
                Console.WriteLine("Продолжить работу? (yes/no)");
                reply = Console.ReadLine();
            } while (reply == "yes");

            Console.WriteLine("Сформировать отчет о покупателe и его заказах?");
            var answerReport = Console.ReadLine();
            if(answerReport == "no")
            {
                Console.WriteLine("Досвидание!");
                return;
            }

            Console.Write("Id покупателя: ");
            int id_client = Convert.ToInt32(Console.ReadLine());

            var cl = clientDB.Clients.FirstOrDefault(client => client.Id == id_client);//выдает ошибку

            ///репорт            
            var catalog = new Catalog()
            {
                Name = "Список товаров",
                Auther = "Продавец: Александр",
                CreationDate = DateTime.Now,
                ClientName = cl.Name,
                Surname = cl.Surname,
                Email = cl.Email,
                Address = cl.Address,
                Phone = cl.Phone,
                Age = cl.Age,
                Orders = from ord in clientDB.Orders
                         where ord.ClientId == id_client
                         select ord
            };

            string templateFile = "Templates/DefaultTemplate.docx";

            IProdactService report = new ProductReportWord(templateFile);

            CreateReport(report, catalog, "Report.docx");
                        
            Console.ReadKey(true);
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportGenerator">Объект - генератор отчета</param>
        /// <param name="catalog">Объект с данным</param>
        /// <param name="reportFileName">Наименование файла - отчета</param>
        private static void CreateReport(IProdactService reportGenerator, Catalog catalog, string reportFileName)
        {
            reportGenerator.CatalogName = catalog.Name;
            reportGenerator.CatalogClientName= catalog.ClientName;
            reportGenerator.CatalogSurname = catalog.Surname;
            reportGenerator.CatalogAuther = catalog.Auther;
            reportGenerator.CreationDate = catalog.CreationDate;
            reportGenerator.CatalogPhone = catalog.Phone;
            reportGenerator.CatalogAddress = catalog.Address;
            reportGenerator.CatalogAge = catalog.Age;
            reportGenerator.CatalogEmail = catalog.Email;
            reportGenerator.Orders = catalog.Orders.Select(order => (order.Id, order.Name, order.Description, order.Price)).ToList();

            var reportfileInfo = reportGenerator.Create(reportFileName);
            reportfileInfo.Execute();
        }

        public static void FillOrder(ClientContext clientDB)
        {
            Console.WriteLine(
                "create\t" +
                "remove\t" +
                "update\t" +
                "get\t" +
                "get all\t");

            string answer = Console.ReadLine();

            if (string.IsNullOrEmpty(answer))
                return;

            using (var consoleManager = new CrudOrder(clientDB))
            {
                var consoleOrder = new OrderConsole(consoleManager, clientDB);
                switch (answer)
                {
                    case "create":
                        consoleOrder.Create();
                        break;
                    case "remove":
                        consoleOrder.Remove();
                        break;
                    case "update":
                        consoleOrder.Update();
                        break;
                    case "get":
                        consoleOrder.Get();
                        break;
                    case "get all":
                        consoleOrder.GetAll();
                        break;
                    default:
                        Console.WriteLine("unknown command!");
                        break;
                }

            }

        }

        public static void FillClient(ClientContext clientDB)
        {
            Console.WriteLine(
                "create\t" +
                "remove\t" +
                "update\t" +
                "get\t" +
                "get all\t");

            string answer = Console.ReadLine();

            if (string.IsNullOrEmpty(answer))
                return;

            using (var crud = new CrudClient(clientDB))
            {
                var consoleClient = new ClientConsole(crud, clientDB);

                switch (answer)
                {
                    case "create":
                        consoleClient.Create();
                        break;
                    case "remove":
                        consoleClient.Remove();
                        break;
                    case "update":
                        consoleClient.Update();
                        break;
                    case "get":
                        consoleClient.Get();
                        break;
                    case "get all":
                        consoleClient.GetAll();
                        break;
                    default:
                        Console.WriteLine("unknown command!");
                        break;
                }
            }
        }
    }
}