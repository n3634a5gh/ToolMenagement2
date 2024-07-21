using Tool_Menagement.Models;

namespace Tool_Menagement.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddCategoryAsync(Kategorium kategoria);
        Task<IEnumerable<Kategorium>> GetCategoryAsync();
    }
}