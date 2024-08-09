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
    }
}
