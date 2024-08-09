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

        public async Task<Kategorium> GetByOpisAsync(string opis)
        {
            return await _context.Kategoria.FirstOrDefaultAsync(k => k.Opis == opis);
        }

        public async Task AddKategoriumAsync(Kategorium kategorium)
        {
            _context.Kategoria.Add(kategorium);
            await _context.SaveChangesAsync();
        }

        public async Task AddKategoriaDetailAsync(KategoriaDetail kategoriaDetail)
        {
            _context.KategoriaDetails.Add(kategoriaDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Kategorium>> GetAllWithDetailsAsync()
        {
            return await _context.Kategoria
                .Include(k => k.KategoriaDetails)
                .ToListAsync();
        }
    }
}