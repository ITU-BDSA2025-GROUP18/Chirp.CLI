using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepQueryRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public UserTimelineModel(ICheepQueryRepository repository)
    {
        _repository = repository;
        Cheeps = new List<CheepDTO>();
    }

    public async Task<ActionResult> OnGet(string author, [FromQuery] int page = 1)
    {
        Cheeps = await _repository.GetCheepsFromAuthorAsync(author, page);
        return Page();
    }
}
