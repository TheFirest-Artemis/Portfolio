using System.ComponentModel.DataAnnotations;

namespace Rosshina_v0._2.Models
{
    public class CompaniesModel
    {
        public int Id { get; set; }
        public string? Manufacturer { get; set; }

        public string? Trademark { get; set; }

        public string? Type { get; set; }

        public string? TypeRus { get; set; }

        [Display(Name = "Generalized information")]
        public string? GeneralizedInfo { get; set; }
    }
}
