using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tool_Menagement.Models;
using Tool_Menagement.Helpers;

namespace Tool_Menagement.Controllers
{
    public class NarzedziaController : Controller
    {
        private readonly ToolsBaseContext _context;

        public NarzedziaController(ToolsBaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var narzedzia = await _context.Narzedzies
                .Include(n => n.Magazyns)
                .ToListAsync();

            return View(narzedzia);
        }

        public async Task<IActionResult> Create()
        {
            var kategorie = await _context.Kategoria.ToListAsync();
            ViewBag.Opisy = kategorie.Select(k => k.Opis).Distinct().ToList();
            ViewBag.Przeznaczenia = kategorie.Select(k => k.Przeznaczenie).Distinct().ToList();
            ViewBag.MaterialyWykonania = kategorie.Select(k => k.MaterialWykonania).Distinct().ToList();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPrzeznaczenia(string opis)
        {
            var przeznaczenia = await _context.Kategoria
                .Where(k => k.Opis == opis)
                .Select(k => k.Przeznaczenie)
                .Distinct()
                .ToListAsync();
            return Json(przeznaczenia);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterialyWykonania(string opis, string przeznaczenie)
        {
            var materialy = await _context.Kategoria
                .Where(k => k.Opis == opis && k.Przeznaczenie == przeznaczenie)
                .Select(k => k.MaterialWykonania)
                .Distinct()
                .ToListAsync();
            return Json(materialy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NarzedzieViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kategoria = await _context.Kategoria.FirstOrDefaultAsync(k =>
                    k.Opis == model.Opis &&
                    k.Przeznaczenie == model.Przeznaczenie &&
                    k.MaterialWykonania == model.MaterialWykonania);

                if (kategoria == null)
                {
                    ModelState.AddModelError("", "Kategoria nie istnieje.");
                    return View(model);
                }

                var tool_exist = await _context.Narzedzies
                    .FirstOrDefaultAsync(n => n.IdKategorii == kategoria.IdKategorii && n.Srednica == model.Srednica);

                int idNarzedzia;

                if (tool_exist == null)
                {
                    string generatedName = Create_name.Tool_Name(model.Opis, model.Srednica, model.MaterialWykonania, model.Przeznaczenie);

                    var newTool = new Narzedzie
                    {
                        IdKategorii = kategoria.IdKategorii,
                        Nazwa = generatedName,
                        Srednica = model.Srednica
                    };

                    _context.Narzedzies.Add(newTool);
                    await _context.SaveChangesAsync();

                    idNarzedzia = newTool.IdNarzedzia;
                }
                else
                {
                    idNarzedzia = tool_exist.IdNarzedzia;
                }

                var magazyn = new Magazyn
                {
                    IdNarzedzia = idNarzedzia,
                    Trwalosc = model.Trwalosc,
                    Uzycie = 0,
                    CyklRegeneracji = 0,
                    Wycofany = false,
                    Regeneracja = false
                };

                _context.Magazyns.Add(magazyn);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        //return View(model);
        // }

    }
}
