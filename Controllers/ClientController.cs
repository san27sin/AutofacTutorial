using AutofacTutorial.Interface;
using AutofacTutorial.ViewModels;
using AutoMapper;
using ClientsDb.Entities;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;

namespace AutofacTutorial.Controllers
{
    public class ClientController : Controller
    {
        private readonly ICrudClient _crudClient;
        private readonly IMapper _mapper;

        public ClientController(ICrudClient crudClient, IMapper mapper)
        {
            _crudClient = crudClient;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var clients = _mapper.Map<ICollection< ClientViewModel>>(_crudClient.GetAll());

            return View("Index", clients);
        }

        public IActionResult Details(int id)
        {
            var client = _mapper.Map<ClientViewModel>(_crudClient.Get(id));
            
            if(client is null)
                return NotFound();

            return View("Details",client);
        }

        public IActionResult Create()
        {                        
            return View("Create", new ClientViewModel());
        }

        public IActionResult Save(ClientViewModel clientViewModel) 
        {
            if(!ModelState.IsValid)
                return View("Create",clientViewModel);

            var client = _mapper.Map<ClientViewModel, Client>(clientViewModel);

            if (_crudClient.Create(client))
                return Index();

            return NotFound();   
        }

        public IActionResult Edit(int id)
        {
            var client = _mapper.Map<ClientViewModel>(_crudClient.Get(id));

            if (client==null)
                return NotFound();

            return View("Edit", client);
        }

        public IActionResult Update(ClientViewModel client)
        {
            var clientBd = _mapper.Map<Client>(client); 
            if(_crudClient.Update(clientBd))
                return Index();

            return NotFound();
        }

        
        public IActionResult AskToDelete(int id)
        {
            var client = _mapper.Map<ClientViewModel>(_crudClient.Get(id));
            if (client != null)
                return View("Delete", client);

            return NotFound();

                       
        }

        public IActionResult Delete(int id)
        {
            if (_crudClient.Delete(id))
                return Index();

            return NotFound();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email)
        {
            var clientsEmail = _crudClient.GetAll().Select(x => x.Email).ToList();
            if (clientsEmail.Any(e=>e==email))
                return Json(false);
            return Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPhone(string phone)
        {
            var clientsPhone = _crudClient.GetAll().Select(x => x.Phone).ToList();
            if (clientsPhone.Any(e => e == phone))
                return Json(false);
            return Json(true);
        }
    }
}
