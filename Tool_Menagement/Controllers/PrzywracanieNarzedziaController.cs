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
                TempData["ErrorMessage"] = "Narzędzie o podanym numerze nie istnieje.";
                return RedirectToAction(nameof(Przywracanie));
            }

            if (magazyn.Wycofany)
            {
                TempData["ErrorMessage"] = "Narzędzie jest wycofane z użycia.";
                return RedirectToAction(nameof(Przywracanie));
            }
            if (magazyn.Regeneracja==false)
            {
                TempData["ErrorMessage"] = "Nie można przywrócić sprawnego narzędzia.";
                return RedirectToAction(nameof(Przywracanie));
            }

            var kategoria = await _narzedziaRepository.ZnajdzKategorieDlaNarzedziaAsync(magazyn.IdNarzedzia);

            if (kategoria != null)
            {
                if (kategoria.ToolPolicy == 3)
                {
                    await _narzedziaRepository.PrzywrocNarzedzieAsync(magazyn, kategoria.ToolPolicy);
                    TempData["SuccessMessage"] = "Narzędzie zostało przywrócone.";
                }
                else if (kategoria.ToolPolicy == 2)
                {
                    await _narzedziaRepository.PrzywrocNarzedzieAsync(magazyn, kategoria.ToolPolicy);
                    TempData["SuccessMessage"] = "Narzędzie zostało przywrócone z ograniczeniem trwałości.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Błąd danych SQL.";
                }
            }
            else
            {

            }

            return RedirectToAction(nameof(Przywracanie));
        }

        return View(model);
    }
}
