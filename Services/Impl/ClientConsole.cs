
using ClientsDb;
using ClientsDb.Entities;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial.Services.Impl
{
    public class ClientConsole : IConsole
    {
        private CrudClient _client;
        private ClientContext _clientDB;

        public ClientConsole(CrudClient client, ClientContext clientDB)
        {
            _client = client;
            _clientDB = clientDB;
        }

        public void Create()
        {
            Console.Write("name: ");
            string? name = Console.ReadLine();
            Console.Write("surname: ");
            string? surname = Console.ReadLine();
            Console.Write("email: ");
            string? email = Console.ReadLine();
            Console.Write("address: ");
            string? address = Console.ReadLine();
            Console.Write("phone: ");
            string? phone = Console.ReadLine();
            Console.Write("age: ");
            string? age = Console.ReadLine();


            if (_client.Create(new Client()
            {
                Name = name,
                Surname = surname,
                Email = email,
                Address = address,
                Phone = phone,
                Age = int.TryParse(age, out int result_c) ? result_c : 18
            }))
                Console.WriteLine("Операция успешно выполнена!");
            else
                Console.WriteLine("Операция не выполнена!");
        }

        public void Get()
        {
            int id = Convert.ToInt32(Console.ReadLine());
            var client = _client.Get(id);
            if (client != null)
            {
                Console.WriteLine($"{client.Name} {client.Surname} {client.Email} {client.Age}");
                var orders = _clientDB.Orders.Where(c => c.ClientId == client.Id).ToList();
                if (orders.Count > 0)
                {
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"         {order.Name} {order.Description}");
                    }
                }
            }
            else
                Console.WriteLine("Пользователь не найден!");
        }

        public void GetAll()
        {
            var clients = _client.GetAll();
            if (clients == null)
                Console.WriteLine("Нет данных в бд");
            else
            {
                foreach (var cl in clients)
                    Console.WriteLine($"{cl.Id} {cl.Name} {cl.Surname} {cl.Email} {cl.Age}");
            }
        }

        public void Remove()
        {
            int id = Convert.ToInt32(Console.ReadLine());
            if (_client.Delete(id))
                Console.WriteLine("Операция успешно выполнена!");
            else
                Console.WriteLine("Операция не выполнена!");
        }   

        public void Update()
        {
            Console.Write("id: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("name: ");
            string? name = Console.ReadLine();
            Console.Write("surname: ");
            string? surname = Console.ReadLine();
            Console.Write("email: ");
            string? email = Console.ReadLine();
            Console.Write("address: ");
            string? address = Console.ReadLine();
            Console.Write("phone: ");
            string? phone = Console.ReadLine();
            Console.Write("age: ");
            string? age = Console.ReadLine();

            if (_client.Update(new Client()
            {
                Id = id,
                Name = name,
                Surname = surname,
                Email = email,
                Address = address,
                Phone = phone,
                Age = int.TryParse(age, out int result_c) ? result_c : 18
            }))
                Console.WriteLine("Обновление прошло успешно!");
            else
                Console.WriteLine("Не удалось обновить данные!");
        }
    }
}
