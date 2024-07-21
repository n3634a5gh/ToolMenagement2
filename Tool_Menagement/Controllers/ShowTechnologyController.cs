using Microsoft.AspNetCore.Mvc;
using Tool_Menagement.Interfaces;

namespace Tool_Menagement.Controllers
{
    public class ShowTechnologyController : Controller
    {
        private readonly ITechnologieRepository _technologyRepository;

        public ShowTechnologyController(ITechnologieRepository technologyRepository)
        {
            _technologyRepository = technologyRepository;
        }

        public async Task<IActionResult> Index()
        {
            var technologie = await _technologyRepository.GetAllTechnologieAsync();
            return View(technologie);
        }
    }
}
