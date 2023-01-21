using AutofacTutorial.Extentions;
using AutofacTutorial.Models.Reports;
using ClientsDb;
using ClientsDb.Entities;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AutofacTutorial.Services.Impl
{
    public class Report:IReport
    {
        private ClientContext _db;

        public Report(ClientContext db)
        {
            _db = db;
            

        }   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportGenerator">Объект - генератор отчета</param>
        /// <param name="catalog">Объект с данным</param>
        /// <param name="reportFileName">Наименование файла - отчета</param>
        public bool CreateReport(int id)
        {
            var cl = _db.Clients.FirstOrDefault(client => client.Id == id);//выдает ошибку

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
                Orders = from ord in _db.Orders
                         where ord.ClientId == id
                         select ord
            };
            //string templateFile = "Templates/DefaultTemplate.docx";

            IProdactService report = new ProductReportWord();

            report.CatalogName = catalog.Name;
            report.CatalogClientName = catalog.ClientName;
            report.CatalogSurname = catalog.Surname;
            report.CatalogAuther = catalog.Auther;
            report.CreationDate = catalog.CreationDate;
            report.CatalogPhone = catalog.Phone;
            report.CatalogAddress = catalog.Address;
            report.CatalogAge = catalog.Age;
            report.CatalogEmail = catalog.Email;
            report.Orders = catalog.Orders.Select(order => (order.Id, order.Name, order.Description, order.Price)).ToList();

            var reportfileInfo = report.Create(Environment.CurrentDirectory+@"\Report.docx");
            if(reportfileInfo != null)
            {
                reportfileInfo.Execute();
                return true;
            }
            return false;          
        }
    }
}
