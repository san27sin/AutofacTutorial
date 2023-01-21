using AutofacTutorial.Interface;
using AutofacTutorial.ViewModels;
using AutoMapper;
using ClientsDb.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AutofacTutorial.Controllers
{
    public class OrderController : Controller
    {
        private ICrudOrder _crudOrder;
        private IMapper _mapper;

        public OrderController(ICrudOrder crudOrder, IMapper mapper)
        {
            _crudOrder = crudOrder;
            _mapper = mapper;
        }

        public IActionResult Index(int id)
        {
            var ordersDb = _crudOrder.GetAll().Where(order => order.ClientId == id);
            var ordersViewModel = new ClientsOrdersViewModel(id, _mapper.Map<List<OrderViewModel>>(ordersDb));
            return View("Index",ordersViewModel);
        }
        
        public IActionResult Create(int id)
        {
            return View("Create", new OrderViewModel() { ClientId = id});
        }

        [HttpPost]
        public IActionResult Create(OrderViewModel orderViewModel)
        {
            orderViewModel.Id = 0;
            var order = _mapper.Map<Order>(orderViewModel);
            if(_crudOrder.Create(order))
            {
                return Index(order.ClientId);
            }

            return NotFound();
        }

        public IActionResult Edit(int id)
        {
           var order = _mapper.Map<OrderViewModel>(_crudOrder.Get(id));
            if (order != null)
                return View("Edit", order);

            return NotFound();
        }

        public IActionResult Delete(int id) 
        {
            var clientId = _crudOrder.Get(id).ClientId;
            if (_crudOrder.Delete(id))
                return Index(clientId);

            return NotFound();
        }

        public IActionResult AskToDelete(int id)
        {
            var order = _mapper.Map<OrderViewModel>(_crudOrder.Get(id));

            if (order != null)
                return View("Delete", order);

            return NotFound();
        }

        public IActionResult Update(OrderViewModel orderViewModel)
        {
            var order = _mapper.Map<Order>(orderViewModel);
            if(_crudOrder.Update(order))
                return Index(order.ClientId);
                
            return NotFound();
        }
                
        public IActionResult Details(int id)
        {
            var order = _mapper.Map<OrderViewModel>(_crudOrder.Get(id));
            return View("Details", order);
        }
    }
}
