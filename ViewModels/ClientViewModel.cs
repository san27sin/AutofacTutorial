using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AutofacTutorial.ViewModels
{
    public class ClientViewModel : IValidatableObject
    {

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }


        [Required(ErrorMessage = "Имя является обязательным!")]
        [Display(Name = "Имя")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Максимальная длина строки 20 символов, минимальная длина строки 2 символа. ")]
        [RegularExpression(@"([А-Я][а-я\-0-9]+)|([A-Z][a-z]+)", ErrorMessage = "Строка имела неверный формат.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Фамилия является обязательным!")]
        [Display(Name = "Фамилия")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Максимальная длина строки 20 символов, минимальная длина строки 2 символа. ")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Электронная почта является обязательным!")]
        [Display(Name = "Электронная почта")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Remote(action: "CheckEmail", controller: "Client", ErrorMessage = "Email уже используется")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Адрес доставки является обязательным!")]
        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Номер телефона является обязательным!")]
        [Display(Name = "Номер телефона")]
        [Phone]
        [Remote(action: "CheckPhone", controller: "Client", ErrorMessage = "Такой номер телефона уже используется")]
        public string Phone { get; set; }


        [Display(Name="Возраст")]
        [Range(17, 110, ErrorMessage = "Недопустимый возраст")]
        public int Age { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == "Иван" && Surname == "Иванов" && Age == 18)
                yield return new ValidationResult("Иван Иванов которому 18 лет - слишком обыденно!", new[] { nameof(Name), nameof(Surname), nameof(Age)});

            yield return ValidationResult.Success;
        }
    }
}
