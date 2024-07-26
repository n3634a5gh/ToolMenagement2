using System.ComponentModel.DataAnnotations;

namespace Tool_Menagement.Models
{
    public class TechnologiumViewModel
    {
        public string OpisTechnologii { get; set; }

        public string Opis { get; set; }
        public string Przeznaczenie { get; set; }
        public string MaterialWykonania { get; set; }
        public double Srednica { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Czas pracy musi być liczbą większą lub równą 0")]
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
        public int? CzasPracy { get; set; }
    }

}