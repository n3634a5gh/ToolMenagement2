using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tool_Menagement.Helpers;
using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;


namespace Tool_Menagement.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ToolsBaseContext _context;
        private readonly ICategoryRepository _kategoriaRepository;

        public CategoryController(ICategoryRepository kategoriaRepository, ToolsBaseContext context)
        {
            _context = context;
            bool zxv = false;
            /*Return_Categories return_Categories = new Return_Categories();
            return_Categories.fillTable(_context);*/
            //ConfigTools configTools = new ConfigTools();
            //configTools.fillTable(_context);
            /*Zlecenie_Available_Tools available_Tools=new Zlecenie_Available_Tools();
            zxv = available_Tools.Can_create(_context, 8);*/
            _kategoriaRepository = kategoriaRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.ToolPolicyOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "Nie dotyczy" },
            new SelectListItem { Value = "1", Text = "Nie regenerowalne" },
            new SelectListItem { Value = "2", Text = "Regeneracja stratna" },
            new SelectListItem { Value = "3", Text = "Regeneracja bezstratna" }
        };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Opis,ToolPolicy,Przeznaczenie,MaterialWykonania")] KategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingKategorium = await _kategoriaRepository.GetByOpisAsync(model.Opis);
                if (existingKategorium == null)
                {
                    var newKategorium = new Kategorium
                    {
                        Opis = model.Opis,
                        ToolPolicy = model.ToolPolicy
                    };
                    await _kategoriaRepository.AddKategoriumAsync(newKategorium);
                    var newKategoriaDetail = new KategoriaDetail
                    {
                        IdKategorii = newKategorium.IdKategorii,
                        Przeznaczenie = model.Przeznaczenie,
                        MaterialWykonania = model.MaterialWykonania
                    };
                    await _kategoriaRepository.AddKategoriaDetailAsync(newKategoriaDetail);
                }
                else
                {
                    var newKategoriaDetail = new KategoriaDetail
                    {
                        IdKategorii = existingKategorium.IdKategorii,
                        Przeznaczenie = model.Przeznaczenie,
                        MaterialWykonania = model.MaterialWykonania
                    };
                    await _kategoriaRepository.AddKategoriaDetailAsync(newKategoriaDetail);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.ToolPolicyOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "Nie dotyczy" },
            new SelectListItem { Value = "1", Text = "Nie regenerowalne" },
            new SelectListItem { Value = "2", Text = "Regeneracja stratna" },
            new SelectListItem { Value = "3", Text = "Regeneracja bezstratna" }
        };

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var kategorie = await _kategoriaRepository.GetAllWithDetailsAsync();
            return View(kategorie);
        }
    }
}
