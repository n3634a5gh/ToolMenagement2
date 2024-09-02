using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tool_Menagement.Models
{
    public class RejestracjaViewModel
    {
        [Required]
        [Display(Name = "IdZlecenia")]
        public int IdZlecenia { get; set; }

        [Required]
        [Display(Name = "Ilość sztuk")]
        public int? Sztuk { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Pole o długości co najmniej 5 znaków.")]
        [Display(Name = "Użytkownik")]
        public string? Wykonal { get; set; }

        [Display(Name = "Uszkodzenia narzędzi")]
        public bool IsToolDamaged { get; set; }

        [Display(Name = "Id Narzędzia")]
        public int? ToolId { get; set; }

        [Display(Name = "Typ uszkodzenia")]
        public string? DamageType { get; set; }

        public List<NarzedzieUszkodzoneViewModel> Narzedzia { get; set; } = new List<NarzedzieUszkodzoneViewModel>();
    }

    public class NarzedzieUszkodzoneViewModel
    {
        public int ToolId { get; set; }
        public string DamageType { get; set; }
    }
}
