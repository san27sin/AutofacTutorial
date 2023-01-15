using AutofacTutorial.Interface;
using AutofacTutorial.Services;
using AutofacTutorial.Services.Impl;
using ClientsDb;
using Microsoft.AspNetCore.Mvc;

namespace AutofacTutorial.Controllers
{
    public class ReportController : Controller
    {
        private IReport _report;
        private ICrudClient _db;

        public ReportController(IReport report, ICrudClient db)
        {
            _report = report;
            _db = db;
        }

        public IActionResult Create(int id)
        {
            if(_report.CreateReport(id))
            {
                var clients = _db.GetAll();
                return Redirect("~/Client/Index");
            }
            return NotFound();
        }
    }
}
