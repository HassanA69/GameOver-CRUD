namespace GameOver.Services;

public class GamesService : IGamesService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _imagesPath;

    public GamesService(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _imagesPath = $"{_environment.WebRootPath}{FileSettings.ImagesPath}";
    }

    public IEnumerable<Game> GetAll()
    {
        return _context.Games
            .Include(e => e.Category)
            .Include(d => d.Devices)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .ToList();
    }

    public async Task Create(CreateGameFormViewModel model)
    {
        var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
        var path = Path.Combine(_imagesPath, coverName);
        using var stream = File.Create(path);
        await model.Cover.CopyToAsync(stream);

        Game game = new()
        {
            Name = model.Name,
            Description = model.Description,
            CategoryId = model.CategoryId,
            Cover = coverName,
            Devices = model.SelectedDevices.Select(x => new GameDevice { DeviceId = x }).ToList(),
        };
        _context.Games.Add(game);
        _context.SaveChanges();
    }
}