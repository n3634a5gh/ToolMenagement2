using Microsoft.AspNetCore.Mvc;
using Tool_Menagement.Helpers;
using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;

namespace Tool_Menagement.Controllers
{
    public class RejestracjaController : Controller
    {
        private readonly ToolsBaseContext _context;
        private readonly IRejestracjaRepository _repository;

        public RejestracjaController(ToolsBaseContext context, IRejestracjaRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new RejestracjaViewModel
            {
                Narzedzia = new List<NarzedzieUszkodzoneViewModel>()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RejestracjaViewModel model)
        {
            if (model.Narzedzia == null)
            {
                model.Narzedzia = new List<NarzedzieUszkodzoneViewModel>();
            }

            if (!model.IsToolDamaged)
            {
                model.Narzedzia.Clear();
            }

            if (ModelState.IsValid)
            {
                if (_repository.IsOrderValid(model.IdZlecenia))
                {
                    var rejestracja = new Rejestracja
                    {
                        IdZlecenia = model.IdZlecenia,
                        Sztuk = model.Sztuk,
                        Wykonal = model.Wykonal,
                        DataWykonania = DateOnly.FromDateTime(DateTime.Now)
                    };
                    _repository.AddRejestracja(rejestracja);

                    var narzedzia_TT = _context.OrderTTs
                        .Where(x => x.OrderId == model.IdZlecenia)
                        .Where(x => x.Active == true)
                        .ToArray();

                    if (narzedzia_TT != null)
                    {
                        foreach (var item in narzedzia_TT)
                        {
                            var seleced_tool_position = _context.Magazyns
                                .Where(x => x.PozycjaMagazynowa == item.ToolId)
                                .FirstOrDefault();

                            int technologyid = _context.Zlecenies
                                .Where(x => x.IdZlecenia == model.IdZlecenia)
                                .Select(x => x.IdTechnologi)
                                .FirstOrDefault();

                            var secected_tool = _context.NarzedziaTechnologia
                                .Where(x => x.IdTechnologi == technologyid)
                                .Where(x => x.IdNarzedzia == seleced_tool_position.IdNarzedzia)
                                .FirstOrDefault();

                            seleced_tool_position.Uzycie = seleced_tool_position.Uzycie + (int)(secected_tool.CzasPracy * model.Sztuk);
                            _context.Magazyns.Update(seleced_tool_position);
                            _context.SaveChanges();
                        }
                    }
                    ToolCheck newcheck = new ToolCheck();
                    List<int> updatedPositions = newcheck.UpdateToolStatus(_context, model.IdZlecenia);
                    if (updatedPositions.Any())
                    {
                        TempData["WarningMessage"] = "Zlecenie zakończone: przekroczony czas użycia narzędzia.";
                    }

                    int count_closed_orders = newcheck.CloseSharedToolsOrders(updatedPositions, _context);

                    if (count_closed_orders > 1)
                    {
                        TempData["WarningMessage"] = "Zakończonych zleceń:" + count_closed_orders + ". Przekroczony czas użycia narzędzia.";
                    }

                    if (model.IsToolDamaged)
                    {
                        var checkTool = new ToolCheck();
                        checkTool.Close_order_on_production(model.Narzedzia, _context, model.IdZlecenia);
                        TempData["ErrorMessage"] = "Zlecenie zakończono.";
                    }

                    TempData["SuccessMessage"] = "Rejestracja zapisana pomyślnie.";
                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["ErrorMessage"] = "Nieprawidłowy numer zlecenia/zlecenie nieaktywne.";
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CheckToolExists(int toolId, int idZlecenia)
        {
            var toolExists = _context.OrderTTs
                .Where(n => n.Active == true)
                .Where(n => n.OrderId == idZlecenia)
                .Any(n => n.ToolId == toolId);

            return Json(new { exists = toolExists });
        }

        [HttpGet]
        public IActionResult CanRegenerateTool(int toolId)
        {
            ToolCheck toolCheck = new ToolCheck();
            bool canRegenerate = toolCheck.CanRegenerate(toolId, _context);
            return Json(new { canRegenerate = canRegenerate });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTool(RejestracjaViewModel model)
        {
            if (model.Narzedzia == null)
            {
                model.Narzedzia = new List<NarzedzieUszkodzoneViewModel>();
            }

            if (!model.IsToolDamaged)
            {
                model.Narzedzia.Clear();
            }
            else if (model.ToolId.HasValue && !string.IsNullOrEmpty(model.DamageType))
            {
                var open_orders_TT = _context.OrderTTs
                    .Where(n => n.Active == true)
                    .Where(n => n.OrderId == model.IdZlecenia)
                    .ToArray();

                var toolExists = open_orders_TT.Any(n => n.ToolId == model.ToolId);
                var check_regeneration = new ToolCheck();

                if (toolExists)
                {
                    if(check_regeneration.CanRegenerate(model.ToolId.Value,_context))
                    {
                        model.Narzedzia.Add(new NarzedzieUszkodzoneViewModel
                        {
                            ToolId = model.ToolId.Value,
                            DamageType = model.DamageType
                        });

                        TempData["SuccessMessage"] = "Narzędzie zostało dodane do listy.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Narzędzie nie podlega regeneracji.";
                    }
                    
                }
                else
                {
                    TempData["ErrorMessage"] = "Nie znaleziono narzędzia o podanym Id w aktywnych zleceniach.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Id Narzędzia i Typ uszkodzenia są wymagane.";
            }

            if (model.IsToolDamaged)
            {
                var checkTool=new ToolCheck();
                checkTool.Close_order_on_production(model.Narzedzia, _context, model.IdZlecenia);
                TempData["ErrorMessage"] = "Zlecenie zakończono.";
            }

                return View("Create", model);
        }

    }
}
