using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AutofacTutorial.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; } = 0;
        [Display(Name = "Название товара")]
        public string Name { get; set; }
        [Display(Name = "Цена")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public int? ClientId { get; set; }
    }
}
