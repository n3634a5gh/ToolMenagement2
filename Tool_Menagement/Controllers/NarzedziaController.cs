using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tool_Menagement.Models;
using Tool_Menagement.Helpers;

    public class NarzedziaController : Controller
    {
        private readonly ToolsBaseContext _context;
        private bool _activewarning = false;
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
            await SetViewBags();
            if (_activewarning)
            {
                TempData["ErrorMessage1"] = "Średnica musi być typu double";
                _activewarning = false;
            }
            return View();
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] NarzedzieViewModel model)
    {
        int _categoryId = 0;
        bool type_other = false;
        string generatedName = "";
        double? diameter = 0;

        if (_activewarning)
        {
            TempData["ErrorMessage1"] = "Średnica musi być typu double";
            _activewarning = false;
        }

        Validators validators = new Validators();
        if (validators.Validate_Double(Convert.ToString(model.Srednica)))
        {
            if (ModelState.IsValid)
            {
                var if_kategoria = _context.Kategoria
                    .Where(k => k.Opis == model.Opis)
                    .FirstOrDefault();

                if (if_kategoria.ToolPolicy == 0)
                {
                    type_other = true;
                }

                if (if_kategoria != null)
                {
                    var kategoria = _context.KategoriaDetails
                        .Where(k => k.IdKategorii == if_kategoria.IdKategorii)
                        .Where(k => k.Przeznaczenie == model.Przeznaczenie)
                        .Where(k => k.MaterialWykonania == model.MaterialWykonania)
                        .FirstOrDefault();

                    _categoryId = kategoria.IdKategorii;

                    if (kategoria == null)
                    {
                        ModelState.AddModelError("", "Kategoria nie istnieje.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kategoria nie istnieje.");
                    return View(model);
                }

                if (type_other)
                {
                    ModelState.Remove("MaterialWykonania");
                    ModelState.Remove("Srednica");
                    ModelState.Remove("Trwalosc");

                    model.MaterialWykonania = null;
                    model.Srednica = 0;
                    model.Trwalosc = 0;
                }

                var tool_exist = await _context.Narzedzies
                    .FirstOrDefaultAsync(n => n.IdKategorii == _categoryId && n.Srednica == model.Srednica);

                int idNarzedzia;

                if (tool_exist == null)
                {
                    if (type_other == false)
                    {
                        generatedName = Create_name.Tool_Name(model.Opis, model.Srednica, model.MaterialWykonania, model.Przeznaczenie);
                    }
                    else
                    {
                        generatedName = Create_name.Tool_Name(model.Przeznaczenie);
                    }

                    if (type_other)
                    {
                        diameter = null;
                    }
                    else
                    {
                        diameter = model.Srednica;
                    }

                    var newTool = new Narzedzie
                    {
                        IdKategorii = _categoryId,
                        Nazwa = generatedName,
                        Srednica = diameter
                    };

                    _context.Narzedzies.Add(newTool);
                    await _context.SaveChangesAsync();

                    idNarzedzia = newTool.IdNarzedzia;
                }
                else
                {
                    idNarzedzia = tool_exist.IdNarzedzia;
                }

                if (type_other)
                {
                    model.Trwalosc = 0;
                }

                for (int i = 0; i < model.Ilosc; i++)
                {
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
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                _activewarning = true;
                await SetViewBags();
                return View(model);
            }
        }
        else
        {
            _activewarning = true;
            TempData["ErrorMessage1"] = "Średnica musi być typu double";
            _activewarning = true;
            await SetViewBags();
            return RedirectToAction(nameof(Create));
        }
    }


    private async Task SetViewBags()
    {
        var kategorie = await _context.Kategoria.ToListAsync();
        var kategorie_details=await _context.KategoriaDetails.ToListAsync();
        ViewBag.Opisy = kategorie.Select(k => k.Opis).Distinct().ToList();
        ViewBag.Przeznaczenia = kategorie_details.Select(k => k.Przeznaczenie).Distinct().ToList();
        ViewBag.MaterialyWykonania = kategorie_details.Select(k => k.MaterialWykonania).Distinct().ToList();
    }

}
