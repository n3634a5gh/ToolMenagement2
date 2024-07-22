using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;

namespace Tool_Menagement.Repositories
{
    public class TechnologieRepository : ITechnologieRepository
    {
        private readonly ToolsBaseContext _context;

        public TechnologieRepository(ToolsBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Technologium>> GetAllTechnologieAsync()
        {
            return await _context.Technologia
                .Include(t => t.NarzedziaTechnologia)
                    .ThenInclude(nt => nt.IdNarzedziaNavigation)
                .ToListAsync();
        }

        public async Task<Zlecenie> GetAktywneZlecenieByTechnologiaIdAsync(int technologiaId)
        {
            return await _context.Zlecenies
                .FirstOrDefaultAsync(z => z.IdTechnologi == technologiaId && z.Aktywne);
        }

        public async Task<Zlecenie> GetZlecenieByIdAsync(int zlecenieId)
        {
            return await _context.Zlecenies.FindAsync(zlecenieId);
        }

        public async Task UpdateZlecenieAsync(Zlecenie zlecenie)
        {
            _context.Zlecenies.Update(zlecenie);
            await _context.SaveChangesAsync();
        }
    }
}
