namespace Tool_Menagement.Models
{
    public partial class Zlecenie_TT
    {
        public int IdPozycji { get; set; }
        public int IdZlecenia { get; set; }

        public int IdNarzedzia { get; set; }

        public bool Aktywne { get; set; }

        public virtual Zlecenie IdZlecenieNavigation { get; set; } = null!;
    }
}
