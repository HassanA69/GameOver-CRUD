using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameOver.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ApplicationDbContext _context;

    public CategoriesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<SelectListItem> GetSelectList()
    {
        return _context.Categories
            .Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Name
            }).OrderBy(x => x.Text)
            .AsNoTracking()
            .ToList();
    }

  
}