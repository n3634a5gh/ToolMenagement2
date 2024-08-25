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
        await SetViewBags();
        var model = new TechnologiumViewModel
        {
            Opisy = await _context.Kategoria.Select(k => k.Opis).Distinct().ToListAsync(),
            NarzedziaTechnologia = new List<NarzedziaTechnologiumViewModel>()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] TechnologiumViewModel model)
    {
        if (/*ModelState.IsValid*/true)
        {
            Validators validators = new Validators();
            model.OpisTechnologii = validators.Validate_Name(model.OpisTechnologii);

            var istniejeTechnologia = await _context.Technologia
            .AnyAsync(t => t.Opis == model.OpisTechnologii);

            if (istniejeTechnologia)
            {
                var dataUtworzenia = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                model.OpisTechnologii = $"{model.OpisTechnologii} {dataUtworzenia}";
            }
            if (!string.IsNullOrEmpty(model.OpisTechnologii))
            {
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
                        CzasPracy = (int)narzedziaTech.CzasPracy
                    });
                }

                _context.Technologia.Add(technologie);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage1"] = "Nazwa technologii :długość min 5 znaków";
                return RedirectToAction(nameof(Create));
            }
        }
        /*else
        {
            TempData["ErrorMessage1"] = "Nieprawidłowa wartość czasu.";
            return RedirectToAction(nameof(Create));
        }*/
    }

    [HttpGet]
    public async Task<IActionResult> GetPrzeznaczenia(string opis)
    {
        int index = _context.Kategoria
            .Where(k => k.Opis == opis)
            .Select(k => k.IdKategorii)
            .FirstOrDefault();

        var przeznaczenia = await _context.KategoriaDetails
            .Where(k => k.IdKategorii == index)
            .Select(k => k.Przeznaczenie)
            .Distinct()
            .ToListAsync();

        bool load_other_view = _context.Kategoria
            .Where(k => k.Opis == opis)
            .Select(k => k.ToolPolicy == 0)
            .FirstOrDefault();

        return Json(new { Przeznaczenia = przeznaczenia, LoadOtherView = load_other_view });
    }

    [HttpGet]
    public async Task<IActionResult> GetMaterialyWykonania(string opis, string przeznaczenie)
    {
        int index = _context.Kategoria
            .Where(k => k.Opis == opis)
            .Select(k => k.IdKategorii)
            .FirstOrDefault();

        var materialy = await _context.KategoriaDetails
            .Where(k => k.IdKategorii == index && k.Przeznaczenie == przeznaczenie)
            .Select(k => k.MaterialWykonania)
            .Distinct()
            .ToListAsync();
        return Json(materialy);
    }

    [HttpGet]
    public async Task<IActionResult> GetSrednice(string opis, string przeznaczenie, string? materialWykonania)
    {
        var select_category = _context.Kategoria
            .Where(k => k.Opis == opis)
            .FirstOrDefault();

        int index_for_destiny = select_category.IdKategorii;
        int policy_t = select_category.ToolPolicy;
        int index_for_tool = 0;

        var kategoria_przeznaczenie = await _context.KategoriaDetails
            .Where(k => k.IdKategorii == index_for_destiny)
            .Where(k => k.Przeznaczenie == przeznaczenie)
            .ToListAsync();

        if (policy_t == 0)
        {
            foreach (var item in kategoria_przeznaczenie)
            {
                index_for_tool = item.IdKategorii;
                break;
            }
        }
        else
        {
            var kategoria = _context.KategoriaDetails
                .Where(k => k.IdKategorii == index_for_destiny)
                .Where(k => k.Przeznaczenie == przeznaczenie)
                .Where(k => k.MaterialWykonania == materialWykonania)
                .FirstOrDefault();

            index_for_tool = kategoria.IdKategorii;
        }

        if (index_for_tool == null)
        {
            return Json(new List<double>());
        }

        var srednice = await _context.Narzedzies
            .Where(n => n.IdKategorii == index_for_tool)
            .Select(n => n.Srednica)
            .Distinct()
            .ToListAsync();

        return Json(srednice);
    }

    [HttpGet]
    public async Task<IActionResult> GetIdNarzedzia(string opis, string przeznaczenie, string? materialWykonania, double? srednica)
    {
        int index = _context.Kategoria
            .Where(k => k.Opis == opis)
            .Select(k => k.IdKategorii)
            .FirstOrDefault();

        var kategoria = await _context.KategoriaDetails
            .FirstOrDefaultAsync(k =>
                k.IdKategorii == index &&
                k.Przeznaczenie == przeznaczenie &&
                (k.MaterialWykonania == materialWykonania || (materialWykonania == null && k.MaterialWykonania == null)));

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

            int created_order = _context.Zlecenies.OrderBy(y => y.IdZlecenia).Select(x => x.IdZlecenia).LastOrDefault();

            availableTools.Zlecenie_ID_Magazyn_Tools(_context, created_order);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["ErrorMessage"] = "Nie można utworzyć zlecenia. Brak koniecznych narzędzi.";
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
            var disableOrderTools = new Zlecenie_Available_Tools();
            disableOrderTools.Close_Zlecenie_ID_Magazyn(_context, zlecenieId);
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task SetViewBags()
    {
        var kategorie = await _context.Kategoria.ToListAsync();
        var kategorie_details = await _context.KategoriaDetails.ToListAsync();
        ViewBag.Opisy = kategorie.Select(k => k.Opis).Distinct().ToList();
        ViewBag.Przeznaczenia = kategorie_details.Select(k => k.Przeznaczenie).Distinct().ToList();
        ViewBag.MaterialyWykonania = kategorie_details.Select(k => k.MaterialWykonania).Distinct().ToList();
    }
}
