namespace Tool_Menagement.Models
{
    public class KategoriaDetail
    {
        public int IdDetail { get; set; }

        public int IdKategorii { get; set; }

        public string? Przeznaczenie { get; set; }

        public string? MaterialWykonania { get; set; }

        public virtual Kategorium IdKategoriiNavigation { get; set; } = null!;
    }
}
