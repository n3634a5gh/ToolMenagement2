using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;

namespace Tool_Menagement.Repositories
{
    public class NarzedziaRepository : INarzedziaRepository
    {
        private readonly ToolsBaseContext _context;

        public NarzedziaRepository(ToolsBaseContext context)
        {
            _context = context;
        }

        public async Task<Magazyn?> ZnajdzPozycjeMagazynowaAsync(int pozycjaMagazynowa)
        {
            return await _context.Magazyns.FirstOrDefaultAsync(m => m.PozycjaMagazynowa == pozycjaMagazynowa);
        }

        public async Task<Kategorium?> ZnajdzKategorieDlaNarzedziaAsync(int idNarzedzia)
        {
            var narzedzie = await _context.Narzedzies
                .Include(n => n.IdKategoriiNavigation)
                .FirstOrDefaultAsync(n => n.IdNarzedzia == idNarzedzia);

            return narzedzie?.IdKategoriiNavigation;
        }

        public async Task PrzywrocNarzedzieAsync(Magazyn magazyn)
        {
            magazyn.Uzycie = 0;
            magazyn.CyklRegeneracji += 1;
            magazyn.Regeneracja = false;

            _context.Magazyns.Update(magazyn);
            await _context.SaveChangesAsync();
        }
    }
}
