using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tool_Menagement.Interfaces;
using Tool_Menagement.Models;

public class PrzywracanieNarzedziaController : Controller
{
    private readonly INarzedziaRepository _narzedziaRepository;

    public PrzywracanieNarzedziaController(INarzedziaRepository narzedziaRepository)
    {
        _narzedziaRepository = narzedziaRepository;
    }

    public IActionResult Przywracanie()
    {
        return View(new PrzywracanieNarzedziaViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Przywracanie(PrzywracanieNarzedziaViewModel model)
    {
        if (ModelState.IsValid)
        {
            var magazyn = await _narzedziaRepository.ZnajdzPozycjeMagazynowaAsync(model.NumerNarzedzia);

            if (magazyn == null)
            {
                TempData["ErrorMessage"] = "Takie narzędzie nie istnieje.";
                return RedirectToAction(nameof(Przywracanie));
            }

            var kategoria = await _narzedziaRepository.ZnajdzKategorieDlaNarzedziaAsync(magazyn.IdNarzedzia);

            if (kategoria != null && kategoria.ToolPolicy == 3)
            {
                await _narzedziaRepository.PrzywrocNarzedzieAsync(magazyn);
                TempData["SuccessMessage"] = "Narzędzie zostało pomyślnie przywrócone.";
            }
            else
            {
                TempData["ErrorMessage"] = "Narzędzie nie może zostać przywrócone.";
            }

            return RedirectToAction(nameof(Przywracanie));
        }

        return View(model);
    }
}
