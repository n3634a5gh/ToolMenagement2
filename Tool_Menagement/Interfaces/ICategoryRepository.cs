using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Models;

namespace Tool_Menagement.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Kategorium> GetByOpisAsync(string opis);
        Task AddKategoriumAsync(Kategorium kategorium);
        Task AddKategoriaDetailAsync(KategoriaDetail kategoriaDetail);

        Task<List<Kategorium>> GetAllWithDetailsAsync();
    }
}