using System.ComponentModel.DataAnnotations;

namespace Tool_Menagement.Models
{
    public class PrzywracanieNarzedziaViewModel
    {
        [Required(ErrorMessage = "Numer wymagany.")]
        public int NumerNarzedzia { get; set; }
    }
}
