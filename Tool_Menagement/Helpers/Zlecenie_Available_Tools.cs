using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Models;

namespace Tool_Menagement.Helpers
{
    public class Zlecenie_Available_Tools
    {
        public bool Can_create(ToolsBaseContext _context, int technologyID)
        {
            //dostępne narz w magazynie
            var available_tools = _context.NarzedziaTechnologia
            .Where(technologium => technologium.IdTechnologi == technologyID)
            .Join(
                _context.Narzedzies,
                technologium => technologium.IdNarzedzia,
                tool => tool.IdNarzedzia,
                (technologium, tool) => tool
            )
            .Join(
                _context.Magazyns,
                narzedzie => narzedzie.IdNarzedzia,
                magazyn => magazyn.IdNarzedzia,
                ((narzedzie, magazyn) => magazyn)
                )
            .Where(magazyn => magazyn.Wycofany == false)
            .Where(magazyn => magazyn.Regeneracja == false)
            .Where(magazyn => magazyn.Uzycie < magazyn.Trwalosc)
            .ToArray();

            //narzędzia z technologii
            var technology_tools = _context.NarzedziaTechnologia
            .Where(technologium => technologium.IdTechnologi == technologyID)
            .ToArray();

            int number_of_available = 0;


            // czy każde narzędzie wymienione w technologi można "pobrać " z magazynu

            foreach (var item2 in technology_tools)
            {
                if (available_tools.Any(item => item2.IdNarzedzia == item.IdNarzedzia))
                {
                    number_of_available++;
                }
            }

            if (number_of_available >= technology_tools.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Zlecenie_ID_Magazyn_Tools(ToolsBaseContext _context, int orderID)
        {
            //id technologi na podstawie orderid
            int technologyID = _context.Zlecenies
                .Where(n => n.IdZlecenia == orderID)
                .Select(k => k.IdTechnologi)
                .FirstOrDefault();

            //pobranie listy wszystkich odpowiadających pozycji magazynowych
            var available_tools = _context.NarzedziaTechnologia
            .Where(technologium => technologium.IdTechnologi == technologyID)
            .Join(
                _context.Narzedzies,
                technologium => technologium.IdNarzedzia,
                tool => tool.IdNarzedzia,
                (technologium, tool) => tool
            )
            .Join(
                _context.Magazyns,
                narzedzie => narzedzie.IdNarzedzia,
                magazyn => magazyn.IdNarzedzia,
                ((narzedzie, magazyn) => magazyn)
                )
            .Where(magazyn => magazyn.Wycofany == false)
            .Where(magazyn => magazyn.Regeneracja == false)
            .Where(magazyn => magazyn.Uzycie < magazyn.Trwalosc)
            .GroupBy(magazyn => magazyn.IdNarzedzia)
            .Select(group => group.OrderBy(m => m.PozycjaMagazynowa).FirstOrDefault())
            .ToArray();

            foreach (var item in available_tools)
            {

                var new_zlecenieTT_position = new OrderTT
                {
                    OrderId = orderID,
                    ToolId = item.PozycjaMagazynowa,
                    Active = true
                };

                _context.OrderTTs.Add(new_zlecenieTT_position);
                _context.SaveChanges();
            }
        }

        public void Close_Zlecenie_ID_Magazyn(ToolsBaseContext _context, int orderID)
        {
            var zleceniaTT = _context.OrderTTs
                .Where(z => z.OrderId == orderID)
                .ToList();

            foreach (var zlecenie in zleceniaTT)
            {
                zlecenie.Active = false;
            }

            _context.SaveChanges();
        }
    }
}
