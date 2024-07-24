using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Helpers;
using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;

public class TechnologieController : Controller
{
    private readonly ToolsBaseContext _context;
    private readonly ITechnologieRepository _technologieRepository;

    public TechnologieController(ToolsBaseContext context, ITechnologieRepository technologieRepository)
    {
        _context = context;
        _technologieRepository = technologieRepository;

    }

    public async Task<IActionResult> Create()
    {
        var model = new TechnologiumViewModel
        {
            Opisy = await _context.Kategoria.Select(k => k.Opis).Distinct().ToListAsync()
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TechnologiumViewModel model)
    {
        //if (ModelState.IsValid)
        //{
            var technologie = new Technologium
            {
                Opis = model.OpisTechnologii,
                DataUtworzenia = DateTime.Now
            };

            foreach (var narzedziaTech in model.NarzedziaTechnologia)
            {
                technologie.NarzedziaTechnologia.Add(new NarzedziaTechnologium
                {
                    IdNarzedzia = narzedziaTech.IdNarzedzia,
                    CzasPracy = narzedziaTech.CzasPracy
                });
            }

            _context.Technologia.Add(technologie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        //}

        /*model.Opisy = await _context.Kategoria.Select(k => k.Opis).Distinct().ToListAsync();
        return View(model);*/
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

    [HttpGet]
    public async Task<IActionResult> GetSrednice(string opis, string przeznaczenie, string materialWykonania)
    {
        var kategoria = await _context.Kategoria.FirstOrDefaultAsync(k =>
            k.Opis == opis && k.Przeznaczenie == przeznaczenie && k.MaterialWykonania == materialWykonania);

        if (kategoria == null)
        {
            return Json(new List<double>());
        }

        var srednice = await _context.Narzedzies
            .Where(n => n.IdKategorii == kategoria.IdKategorii)
            .Select(n => n.Srednica)
            .Distinct()
            .ToListAsync();
        return Json(srednice);
    }

    [HttpGet]
    public async Task<IActionResult> GetIdNarzedzia(string opis, string przeznaczenie, string materialWykonania, double srednica)
    {
        var kategoria = await _context.Kategoria.FirstOrDefaultAsync(k =>
            k.Opis == opis && k.Przeznaczenie == przeznaczenie && k.MaterialWykonania == materialWykonania);

        if (kategoria == null)
        {
            return Json(new { idNarzedzia = (int?)null });
        }

        var narzedzie = await _context.Narzedzies
            .FirstOrDefaultAsync(n => n.IdKategorii == kategoria.IdKategorii && n.Srednica == srednica);

        if (narzedzie == null)
        {
            return Json(new { idNarzedzia = (int?)null });
        }

        return Json(new { idNarzedzia = narzedzie.IdNarzedzia, nazwa = narzedzie.Nazwa });
    }

    public async Task<IActionResult> Index()
    {
        var technologie = await _technologieRepository.GetAllTechnologieAsync();
        var technologieViewModel = new List<TechnologiaViewModel>();
        foreach (var technologia in technologie)
        {
            var aktywneZlecenie = await _technologieRepository.GetAktywneZlecenieByTechnologiaIdAsync(technologia.IdTechnologi);
            technologieViewModel.Add(new TechnologiaViewModel
            {
                Technologia = technologia,
                AktywneZlecenie = aktywneZlecenie
            });
        }

        return View(technologieViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CreateZlecenie(int technologiaId)
    {
        var availableTools = new Zlecenie_Available_Tools();
        bool canCreate = availableTools.Can_create(_context, technologiaId);

        if (canCreate)
        {
            var noweZlecenie = new Zlecenie
            {
                IdTechnologi = technologiaId,
                Aktywne = true
            };

            _context.Zlecenies.Add(noweZlecenie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = "Nie można utworzyć zlecenia. Brak dostępnych narzędzi.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ClseZleocenie(int zlecenieId)
    {
        var zlecenie = await _technologieRepository.GetZlecenieByIdAsync(zlecenieId);
        if (zlecenie != null)
        {
            zlecenie.Aktywne = false;
            await _technologieRepository.UpdateZlecenieAsync(zlecenie);
        }

        return RedirectToAction(nameof(Index));
    }

}
