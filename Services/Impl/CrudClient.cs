using AutofacTutorial.Interface;
using ClientsDb;
using ClientsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial.Services.Impl
{
    public class CrudClient : ICrudClient, IDisposable
    {
        private ClientContext _db;

        public CrudClient(ClientContext db) { _db = db; }

        public bool Create(Client entity)
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

            var client = _db.Clients.FirstOrDefault(client => client.Id == id);
            if (client != null)
            {
                _db.Remove(client);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public bool Update(Client entity)
        {
            if (entity == null)
                return false;

            var clientUp = _db.Clients.FirstOrDefault(client => client.Id == entity.Id);

            if(clientUp != null)
            {
                clientUp.Name = entity.Name;
                clientUp.Surname= entity.Surname;
                clientUp.Email = entity.Email;
                clientUp.Address = entity.Address;
                clientUp.Phone= entity.Phone;
                clientUp.Age= entity.Age;
                _db.Update(entity);
                _db.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public Client Get(int id)
        {
            if (_db.Clients.Any(c => c.Id == id))
                return _db.Clients.FirstOrDefault(client => client.Id == id);
            else
                return null;
        }

        public ICollection<Client> GetAll()
        {
            return _db.Clients.ToList();
        }
    }
}
