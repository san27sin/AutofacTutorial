using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial.Services
{
    public interface IProdactService
    {
        string CatalogAuther { get; set; }
        string CatalogName { get; set; }
        DateTime CreationDate { get; set; }

        #region Client
        string CatalogClientName { get; set; }
        string CatalogSurname { get; set; }
        string CatalogEmail { get; set; }
        string CatalogAddress { get; set; }
        string CatalogPhone { get; set; }
        int CatalogAge { get; set; }
        #endregion

        IEnumerable<(int id, string name, string description, decimal price)> Orders { get; set; }
        FileInfo Create(string reportTemplateFile);
    }
}
