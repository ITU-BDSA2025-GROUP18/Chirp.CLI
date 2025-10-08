using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
        Cheeps = new List<CheepDTO>();
    }

    public async Task<ActionResult> OnGet([FromQuery] int page = 1)
    {
        Cheeps = await _repository.GetCheepsAsync(page);
        return Page();
    }
}
