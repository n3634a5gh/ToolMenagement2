namespace Tool_Menagement.Models
{
    public class TechnologiumViewModel
    {
        public string OpisTechnologii { get; set; }

        public string Opis { get; set; }
        public string Przeznaczenie { get; set; }
        public string MaterialWykonania { get; set; }
        public double Srednica { get; set; }
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
        public string Nazwa { get; set; }
        public int CzasPracy { get; set; }
    }

}