using Tool_Menagement.Models;

namespace Tool_Menagement.Interfaces
{
    public interface ITechnologieRepository
    {
        Task<IEnumerable<Technologium>> GetAllTechnologieAsync();
    }
}
