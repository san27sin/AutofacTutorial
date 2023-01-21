using AutofacTutorial.Interface;
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
    public class CrudOrder : ICrudOrder, IDisposable
    {
        private ClientContext _db;

        public CrudOrder(ClientContext db) { _db = db; }

        public bool Create(Order entity)
        {
            if (entity == null)
                return false;

            _db.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            if (id == default)
                return false;

            var order = _db.Orders.FirstOrDefault(order => order.Id == id);
            if (order != null)
            {
                _db.Remove(order);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Order Get(int id)
        {
            if (_db.Orders.Any(c => c.Id == id))
                return _db.Orders.FirstOrDefault(order => order.Id == id);
            else
                return null;

        }

        public ICollection<Order> GetAll()
        {
            return _db.Orders.ToList();
        }

        public bool Update(Order entity)
        {
            if (entity == null)
                return false;

            var orderUP = _db.Orders.FirstOrDefault(order => order.Id == entity.Id);

            if (orderUP != null)
            {
                orderUP.Name= entity.Name;
                orderUP.Price= entity.Price;
                orderUP.Description = entity.Description;
                _db.Update(orderUP);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
