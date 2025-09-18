namespace GameOver.Controllers;

public class GamesController : Controller
{
    private readonly ICategoriesService _categoriesService;
    private readonly IDevicesService _devicesService;
    private readonly IGamesService _gameService;
    public GamesController(ApplicationDbContext context, ICategoriesService categoriesService, IDevicesService devicesService, IGamesService gameService)
    {
        _categoriesService = categoriesService;
        _devicesService = devicesService;
        _gameService = gameService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        CreateGameFormViewModel viewModel = new()
        {
            Categories = _categoriesService.GetSelectList(),
            Devices = _devicesService.GetSelectList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGameFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _categoriesService.GetSelectList();
            model.Devices =_devicesService.GetSelectList();
            return View(model);
        }

        await _gameService.Create(model);
        
        return RedirectToAction(nameof(Index));
    }
}