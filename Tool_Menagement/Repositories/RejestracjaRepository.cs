using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;

namespace Tool_Menagement.Repositories
{
    public class RejestracjaRepository : IRejestracjaRepository
    {
        private readonly ToolsBaseContext _context;

        public RejestracjaRepository(ToolsBaseContext context)
        {
            _context = context;
        }

        public bool IsOrderValid(int orderId)
        {
            return _context.Zlecenies.Any(z => z.IdZlecenia == orderId && z.Aktywne);
        }

        public void AddRejestracja(Rejestracja rejestracja)
        {
            _context.Rejestracjas.Add(rejestracja);
            _context.SaveChanges();
        }
    }
}
