using System.ComponentModel.DataAnnotations;

namespace Tool_Menagement.Models
{
    public class TechnologiumViewModel
    {
        [Display(Name = " Opis Technologii")]
        public string OpisTechnologii { get; set; }

        [Required]
        public string Opis { get; set; }
        public string Przeznaczenie { get; set; }

        [Display(Name = "Materiał Wykonania")]
        public string MaterialWykonania { get; set; }

        [Display(Name = "Średnica")]
        public double Srednica { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Czas Pracy")]
        [Range(0, int.MaxValue, ErrorMessage = "Czas Pracy=INT, value>0")]
        public int CzasPracy { get; set; }

        public List<string> Opisy { get; set; }
        public List<string> Przeznaczenia { get; set; }
        public List<string> MaterialyWykonania { get; set; }
        public List<double> Srednice { get; set; }

        public List<NarzedziaTechnologiumViewModel> NarzedziaTechnologia { get; set; } = new List<NarzedziaTechnologiumViewModel>();
    }

    public class NarzedziaTechnologiumViewModel
    {
        public int IdNarzedzia { get; set; }

        [Required(ErrorMessage = "Opis technologii jest wymagany")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Opis technologii musi mieć minimum 5 znaków i zaczynać się od dużej litery")]
        [RegularExpression(@"^[A-Z].*", ErrorMessage = "Opis technologii musi zaczynać się od dużej litery")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Czas Pracy")]
        [Range(0, int.MaxValue, ErrorMessage = "Czas Pracy=INT, value>0")]
        public int CzasPracy { get; set; }
    }

}