using Tool_Menagement.Models;

namespace Tool_Menagement.Interfaces
{
    public interface ITechnologieRepository
    {
        Task<IEnumerable<Technologium>> GetAllTechnologieAsync();
        Task<Zlecenie> GetAktywneZlecenieByTechnologiaIdAsync(int technologiaId);
        Task<Zlecenie> GetZlecenieByIdAsync(int zlecenieId);
        Task UpdateZlecenieAsync(Zlecenie zlecenie);
    }
}
