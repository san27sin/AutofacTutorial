using ClientsDb;
using ClientsDb.Entities;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial.Services.Impl
{
    public class OrderConsole : IConsole
    {
        private CrudOrder _order;
        private ClientContext _db;

        public OrderConsole(CrudOrder crudClient, ClientContext db)
        {
            _order = crudClient;
            _db = db;
        }

        public void Create()
        {
            Console.Write("name: ");
            string? name = Console.ReadLine();
            Console.Write("price: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("description: ");
            string? description = Console.ReadLine();
            Console.Write("client id: ");
            int clientId = Convert.ToInt32(Console.ReadLine());

            if (_order.Create(new Order()
            {
               Name = name,
               Price = price,
               Description = description,
               ClientId = clientId
            }))
                Console.WriteLine("Операция успешно выполнена!");
            else
                Console.WriteLine("Операция не выполнена!");
        }

        public void Get()
        {
            int id = Convert.ToInt32(Console.ReadLine());
            var order = _order.Get(id);
            if (order != null)
                Console.WriteLine($"{order.Name} / {order.Price.ToString()} / {order.Description} / {order.ClientId.ToString()}");
            else
                Console.WriteLine("Заказ не найден!");
        }

        public void GetAll()
        {
            var orders = _order.GetAll();
            if (orders == null)
                Console.WriteLine("Нет данных в бд");
            else
            {
                foreach (var order in orders)
                    Console.WriteLine($"{order.Name} / {order.Price.ToString()} / {order.Description} / {order.ClientId.ToString()}");
            }
        }

        public void Remove()
        {
            int id = Convert.ToInt32(Console.ReadLine());
            if (_order.Delete(id))
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
            Console.Write("price: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("description: ");
            string? description = Console.ReadLine();
            Console.Write("client id: ");
            int clientId = Convert.ToInt32(Console.ReadLine());

            if (_order.Update(new Order()
            {
                Id = id,
                Name = name,
                Price = price,
                Description = description,
                ClientId = clientId
            }))
                Console.WriteLine("Операция успешно выполнена!");
            else
                Console.WriteLine("Операция не выполнена!");
        }
    }
}
