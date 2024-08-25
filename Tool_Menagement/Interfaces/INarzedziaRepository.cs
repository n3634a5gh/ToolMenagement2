using System.Threading.Tasks;
using Tool_Menagement.Models;

namespace Tool_Menagement.Interfaces
{
    public interface INarzedziaRepository
    {
        Task<Magazyn?> ZnajdzPozycjeMagazynowaAsync(int pozycjaMagazynowa);
        Task<Kategorium?> ZnajdzKategorieDlaNarzedziaAsync(int idNarzedzia);
        Task PrzywrocNarzedzieAsync(Magazyn magazyn);
    }
}
