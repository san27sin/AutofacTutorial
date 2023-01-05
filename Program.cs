using Autofac;
using ClientsDb;
using ClientsDb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutofacTutorial.Interface;

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
                .AddXmlFile("appsettings.xml", true)
                .AddIniFile("appsettings.ini", true)//булевая переменная указывает на то что файлов может и не быть
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
        }

        public static void FillOrder(ClientContext clientDB)
        {
            Console.WriteLine(
                "c - create: c [name] [description] [client_id]\n" +
                "r - remove: r [id]\n" +
                "up - update: up [id] [name] [description] [client_id]\n" +
                "get - get: get [id]\n" + 
                "gAll - get all");

            string answer = Console.ReadLine();

            if (string.IsNullOrEmpty(answer))
                return;

            var words = answer.Split(' ');

            using (var consoleManager = new CrudOrder(clientDB))
            {
                switch (words[0])
                {
                    case "c":
                        if (words.Length != 4)
                        {
                            Console.WriteLine("Недостаточно кол-во данных");
                            break;
                        }

                        if (consoleManager.Create(new Order()
                        {
                            Name = words[1],
                            Description = words[2],
                            ClientId = int.TryParse(words[3], out int result_c) ? result_c : 0
                        }))
                            Console.WriteLine("Операция успешно выполнена!");
                        else
                            Console.WriteLine("Операция не выполнена!");
                        break;
                    case "r":
                        if (words.Length != 2 || !int.TryParse(words[1], out int result_r))
                            break;
                        if (consoleManager.Delete(result_r))
                            Console.WriteLine("Операция успешно выполнена!");
                        else
                            Console.WriteLine("Операция не выполнена!");
                        break;
                    case "up":
                        if (words.Length != 4)
                        {
                            Console.WriteLine("Недостаточно кол-во данных");
                            break;
                        }

                        if (consoleManager.Update(new Order()
                        {
                            Name = words[1],
                            Description = words[2],
                            ClientId = int.TryParse(words[4], out int result_up) ? result_up : 0
                        }))
                            Console.WriteLine("Обновление прошло успешно!");
                        else
                            Console.WriteLine("Не удалось обновить данные!");
                        break;
                    case "get":
                        if (words.Length != 2 || !int.TryParse(words[1], out int result_g))
                            break;
                        var order = consoleManager.Get(result_g);
                        if (order != null)
                        {
                            Console.WriteLine($"{order.Name} {order.Description} {order.ClientId}");
                        }
                        else
                            Console.WriteLine("Пользователь не найден!");
                        break;
                    case "gAll":
                        if (words.Length != 1)
                            break;
                        var orders = consoleManager.GetAll();
                        if (orders == null)
                            Console.WriteLine("Нет данных в бд");
                        else
                        {
                            foreach (var or in orders)
                            {
                                Console.WriteLine($"{or.Id} {or.Name} {or.Description} {or.ClientId}");                                
                            }                                
                        }
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
                "c - create: c [name] [surname] [email] [age]\n" +
                "r - remove: r [id]\n" +
                "up - update: up [id] [name] [surname] [email] [age]\n" +
                "get - get: get [id]\n" +
                "gAll - get all");

            string answer = Console.ReadLine();

            if (string.IsNullOrEmpty(answer))
                return;

            var words = answer.Split(' ');

            using (var consoleManager = new CrudClient(clientDB))
            {
                switch (words[0])
                {
                    case "c":
                        if (words.Length != 5)
                        {
                            Console.WriteLine("Недостаточно кол-во данных");
                            break;
                        }

                        if (consoleManager.Create(new Client()
                        {
                            Name = words[1],
                            Surname = words[2],
                            Email = words[3],
                            Age = int.TryParse(words[4], out int result_c) ? result_c : 18
                        }))
                            Console.WriteLine("Операция успешно выполнена!");
                        else
                            Console.WriteLine("Операция не выполнена!");
                        break;
                    case "r":
                        if (words.Length != 2 || !int.TryParse(words[1], out int result_r))
                            break;
                        if (consoleManager.Delete(result_r))
                            Console.WriteLine("Операция успешно выполнена!");
                        else
                            Console.WriteLine("Операция не выполнена!");
                        break;
                    case "up":
                        if (words.Length != 5)
                        {
                            Console.WriteLine("Недостаточно кол-во данных");
                            break;
                        }

                        if (consoleManager.Update(new Client()
                        {
                            Name = words[1],
                            Surname = words[2],
                            Email = words[3],
                            Age = int.TryParse(words[4], out int result_up) ? result_up : 18
                        }))
                            Console.WriteLine("Обновление прошло успешно!");
                        else
                            Console.WriteLine("Не удалось обновить данные!");
                        break;
                    case "get":
                        if (words.Length != 2 || !int.TryParse(words[1], out int result_g))
                            break;
                        var client = consoleManager.Get(result_g);
                        if (client != null)
                        {
                            Console.WriteLine($"{client.Name} {client.Surname} {client.Email} {client.Age}");
                            var orders = clientDB.Orders.Where(c => c.ClientId == client.Id).ToList();
                            if(orders.Count > 0)
                            {
                                foreach(var order in orders) 
                                {
                                    Console.WriteLine($"         {order.Name} {order.Description}");
                                }
                            }
                        }
                        else
                            Console.WriteLine("Пользователь не найден!");
                        break;
                    case "gAll":
                        if (words.Length != 1)
                            break;
                        var clients = consoleManager.GetAll();
                        if (clients == null)
                            Console.WriteLine("Нет данных в бд");
                        else
                        {
                            foreach (var cl in clients)
                                Console.WriteLine($"{cl.Id} {cl.Name} {cl.Surname} {cl.Email} {cl.Age}");
                        }
                        break;
                    default:
                        Console.WriteLine("unknown command!");
                        break;
                }
            }
        }
    }
}