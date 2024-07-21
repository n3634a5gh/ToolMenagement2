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
    }
}
