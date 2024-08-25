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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RejestracjaViewModel model)
        {
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

                            int technologyid=_context.Zlecenies
                                .Where(x=>x.IdZlecenia==model.IdZlecenia)
                                .Select(x=>x.IdTechnologi)
                                .FirstOrDefault();

                            var secected_tool=_context.NarzedziaTechnologia
                                .Where(x=>x.IdTechnologi==technologyid)
                                .Where (x=>x.IdNarzedzia==seleced_tool_position.IdNarzedzia)
                                .FirstOrDefault();

                            seleced_tool_position.Uzycie = seleced_tool_position.Uzycie+(int)(secected_tool.CzasPracy * model.Sztuk);
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

                    if(count_closed_orders>1)
                    {
                        TempData["WarningMessage"] = "Zakończonych zleceń:" + count_closed_orders+". Przekroczony czas użycia narzędzia.";
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
    }
}
