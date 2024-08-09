using System.ComponentModel.DataAnnotations;

namespace Tool_Menagement.Models
{
    public class KategoriaViewModel
    {
        [Required]
        public string Opis { get; set; }
        [Required]
        [Display(Name = "Polityka narzędziowa")]
        public int ToolPolicy { get; set; }
        [Required]
        public string? Przeznaczenie { get; set; }
        [Display(Name = "Materiał wykonania")]
        public string? MaterialWykonania { get; set; }
    }
}
