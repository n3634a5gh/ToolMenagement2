using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Models;
using Tool_Menagement.Interfaces;

namespace Tool_Menagement.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ToolsBaseContext _context;

        public CategoryRepository(ToolsBaseContext context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Kategorium kategoria)
        {
            _context.Kategoria.Add(kategoria);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Kategorium>> GetCategoryAsync()
        {
            return await _context.Kategoria.ToListAsync();
        }
    }
}
