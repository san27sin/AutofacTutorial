using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateEngine.Docx;

namespace AutofacTutorial.Services.Impl
{
    public class ProductReportWord : IProdactService
    {
        #region Private Field
        private const string _fieldCatalogName = "CatalogName";
        private const string _fieldCreationDate = "CreationDate";
        private const string _fieldCreationAuther = "CreationAuther";

        private string _fieldCatalogClientName = "Name";
        private string _fieldCatalogSurname = "Surname";
        private string _fieldCatalogEmail = "Email";
        private string _fieldCatalogAddress = "Address";
        private string _fieldCatalogPhone = "Phone";
        private string _fieldCatalogAge = "Age";

        private string _fieldOrderId = "OrderId";
        private string _fieldOrderName = "OrderName";
        private string _fieldOrderDescription = "OrderDescription";
        private string _fieldOrderPrice = "OrderPrice";
        private string _fieldTotalPrice = "TotalPrice";

        private string _fieldOrderRow = "OrderRow";

        private readonly FileInfo _templateFile;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportTemplateFile">файл шаблона</param>
        public ProductReportWord(string reportTemplateFile)
        {
            _templateFile = new FileInfo(reportTemplateFile);//класс который содержит ссылку на файл
        }
        #endregion

        #region Public Properties
        public string CatalogName { get; set; }
        public DateTime CreationDate { get; set; }
        public string CatalogAuther { get; set; }
        public string CatalogClientName { get; set; }
        public string CatalogSurname { get; set; }
        public string CatalogEmail { get; set; }
        public string CatalogAddress { get; set; }
        public string CatalogPhone { get; set; }
        public int CatalogAge { get; set; }
        public IEnumerable<(int id, string name, string description, decimal price)> Orders { get; set; }        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportFilePath">Файл отчета</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public FileInfo Create(string reportFilePath)
        {
            if (File.Exists(reportFilePath))
                File.Delete(reportFilePath);

            var reportFile = new FileInfo(reportFilePath);
            reportFile.Delete();
            _templateFile.CopyTo(reportFile.FullName);

            var rows = Orders.Select(order => new TableRowContent(new List<FieldContent> {
            new FieldContent(_fieldOrderId, order.id.ToString()),
            new FieldContent(_fieldOrderName, order.name),
            new FieldContent(_fieldOrderDescription, order.description),
            new FieldContent(_fieldOrderPrice, order.price.ToString())
            })).ToArray();

            var content = new Content(
                new FieldContent(_fieldCreationAuther, CatalogAuther),
                new FieldContent(_fieldCreationDate, CreationDate.ToString("dd.MM.yyyy HH.mm.ss")),
                new FieldContent(_fieldCatalogName, CatalogName),
                new FieldContent(_fieldCatalogClientName, CatalogClientName),
                new FieldContent(_fieldCatalogSurname, CatalogSurname),
                new FieldContent(_fieldCatalogEmail, CatalogEmail),
                new FieldContent(_fieldCatalogAddress, CatalogAddress),
                new FieldContent(_fieldCatalogPhone, CatalogPhone),
                new FieldContent(_fieldCatalogAge, CatalogAge.ToString()),
                TableContent.Create(_fieldOrderRow, rows),
                new FieldContent(_fieldTotalPrice, Orders.Sum(order=>order.price).ToString()));

            var templateProcessor = new TemplateProcessor(reportFile.FullName)
                .SetRemoveContentControls(true);

            templateProcessor.FillContent(content);
            templateProcessor.SaveChanges();    
            reportFile.Refresh();
            return reportFile;
        }
    }
}
