using Microsoft.AspNetCore.Mvc;
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

                    var narzedzia=_context.OrderTTs
                        .Where (x => x.OrderId==model.IdZlecenia)
                        .Where (x => x.Active==true)
                        .Select (x => x.ToolId)
                        .ToArray();

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
