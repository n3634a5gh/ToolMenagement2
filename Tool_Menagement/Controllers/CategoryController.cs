    using Microsoft.AspNetCore.Mvc;
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
            //Return_Categories return_Categories = new Return_Categories();
            //return_Categories.fillTable(_context);
            //ConfigTools configTools = new ConfigTools();
            //configTools.fillTable(_context);
            /*Zlecenie_Available_Tools available_Tools=new Zlecenie_Available_Tools();
            zxv = available_Tools.Can_create(_context, 8);*/
            _kategoriaRepository = kategoriaRepository;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kategorium kategoria)
        {
            if (ModelState.IsValid)
            {
                await _kategoriaRepository.AddCategoryAsync(kategoria);
                return RedirectToAction("Index");
            }
            return View(kategoria);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kategorie = await _kategoriaRepository.GetCategoryAsync();
            return View(kategorie);
        }
    }
}
