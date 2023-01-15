using AutofacTutorial.Interface;
using ClientsDb.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AutofacTutorial.Controllers
{
    public class ClientController : Controller
    {
        private readonly ICrudClient _crudClient;

        public ClientController(ICrudClient crudClient)
        {
            _crudClient = crudClient;
        }

        public IActionResult Index()
        {
            var clients = _crudClient.GetAll();

            return View(clients);
        }

        public IActionResult Details(int id)
        {
            var client = _crudClient.Get(id);
            
            if(client is null)
                return NotFound();

            return View("Details",client);
        }

        public IActionResult Create()
        {
            return View("Create", new Client());
        }

        public IActionResult Save(Client client) 
        {
            if( _crudClient.Create(client))
            {
                var clients = _crudClient.GetAll();
                return View("Index", clients);
            }

            return NotFound();   
        }

        public IActionResult Edit(int id)
        {
            var client = _crudClient.Get(id);

            if(client==null)
                return NotFound();

            return View("Edit", client);
        }

        public IActionResult Update(Client client)
        {
            if(_crudClient.Update(client))
            {
                var clients = _crudClient.GetAll();
                return View("Index", clients);
            }

            return NotFound();
        }

        public IActionResult Delete(int id)
        {
            if (_crudClient.Delete(id))
            {
                var clients = _crudClient.GetAll();
                return View("Index", clients);
            }                
            else
                return NotFound();            
        }
    }
}
