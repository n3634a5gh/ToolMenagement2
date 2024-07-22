using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Tool_Menagement.Models;

namespace Tool_Menagement.Helpers
{
    public class ConfigTools
    {
        public void fillTable(ToolsBaseContext _context)
        {
            var CatId=_context.Kategoria
                .Where(m=>m.Opis== "Wiertło")
                .Where(n=>n.MaterialWykonania=="VHM" || n.MaterialWykonania=="DIA" || 
                n.MaterialWykonania == "PM")
                .Where(k=>k.Przeznaczenie=="Stal" || k.Przeznaczenie== "Nieżelazne")
                .Select(n=>n.IdKategorii)
                .ToArray();
            string opis = "",material="", przeznacz="";
            double sred = 0;
            int idNarzedzia;


            for (int i=0;i<CatId.Length;i++)
            {
                for(int j=1;j<=15;j+=2)
                {
                    opis = _context.Kategoria
                        .Where(n => n.IdKategorii == CatId[i])
                        .Select(n => n.Opis)
                        .FirstOrDefault();
                    material = _context.Kategoria
                        .Where(n => n.IdKategorii == CatId[i])
                        .Select(n => n.MaterialWykonania)
                        .FirstOrDefault();
                    przeznacz = _context.Kategoria
                        .Where(n => n.IdKategorii == CatId[i])
                        .Select(n => n.Przeznaczenie)
                        .FirstOrDefault();
                    string generatedName = "";
                    generatedName = Create_name.Tool_Name(opis, Convert.ToDouble(j) , material, przeznacz);

                    var newTool = new Narzedzie
                    {
                        IdKategorii = CatId[i],
                        Nazwa = generatedName,
                        Srednica = Convert.ToDouble(j),
                    };
                    _context.Narzedzies.Add(newTool);
                    _context.SaveChanges();

                    idNarzedzia = newTool.IdNarzedzia;

                    var magazyn = new Magazyn
                    {
                        IdNarzedzia = idNarzedzia,
                        Trwalosc = 4000,
                        Uzycie = 0,
                        CyklRegeneracji = 0,
                        Wycofany = false,
                        Regeneracja = false
                    };

                    _context.Magazyns.Add(magazyn);
                    _context.SaveChanges();
                }
            }
        }
    }
}
