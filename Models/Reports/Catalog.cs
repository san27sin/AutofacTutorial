using AutofacTutorial.Services;
using ClientsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacTutorial.Models.Reports
{
    public class Catalog
    {
        public string ClientName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int Age { get; set; }
        public string Name { get; set; } = null!;

        public string Auther { get; set; } = null!;
        public DateTime CreationDate { get; set; }

        public IEnumerable<Order> Orders { get; set; } 
    }
}
