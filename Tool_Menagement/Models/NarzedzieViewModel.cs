using System.ComponentModel.DataAnnotations;

namespace Tool_Menagement.Models
{
    public class NarzedzieViewModel
    {
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [Display(Name = "Przeznaczenie")]
        public string Przeznaczenie { get; set; }

        [Display(Name = "Materiał Wykonania")]
        public string ?MaterialWykonania { get; set; }

        [Display(Name = "Średnica")]
        public double Srednica { get; set; }

        [Display(Name = "Trwałość")]
        [Range(0, int.MaxValue, ErrorMessage = "Trwałość musi być liczbą całkowitą większą niż 0.")]
        public int Trwalosc { get; set; }

        [Display(Name = "Ilość")]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość musi być liczbą całkowitą większą lub równą 1.")]
        public int Ilosc { get; set; } = 1;
    }
}