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

    public Game? GetById(int id)
    {
        return _context.Games
            .Include(e => e.Category)
            .Include(d => d.Devices)
            .ThenInclude(d => d.Device)
            .AsNoTracking()
            .SingleOrDefault(e => e.Id == id);
    }

    public async Task Create(CreateGameFormViewModel model)
    {
        var coverName = await saveCover(model.Cover);

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

    public async Task<Game?> Update(EditGameFormViewModel model)
    {
        var game = _context.Games
            .Include(g => g.Devices)
            .SingleOrDefault(g => g.Id == model.Id);

        if (game is null)
            return null;


        var hasNewCover = model.Cover != null;
        var oldCover = game.Cover;

        game.Name = model.Name;
        game.Description = model.Description;
        game.CategoryId = model.CategoryId;
        game.Devices.Clear();
        foreach (var deviceId in model.SelectedDevices)
        {
            game.Devices.Add(new GameDevice
            {
                GameId = game.Id,
                DeviceId = deviceId
            });
        }

        if (hasNewCover)
        {
            game.Cover = await saveCover(model.Cover!);
        }


        var effectedRows = await _context.SaveChangesAsync();
        if (effectedRows > 0)
        {
            if (hasNewCover)
            {
                var cover = Path.Combine(_imagesPath, oldCover);
                if (File.Exists(cover))
                    File.Delete(cover);
            }

            return game;
        }
        else
        {
            if (hasNewCover)
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                if (File.Exists(cover))
                    File.Delete(cover);
            }

            return null;
        }
    }

    public bool Delete(int id)
    {
        var isDeleted = false;

        var game = GetById(id);

        if (game is null)
        {
            return isDeleted;
        }

        _context.Games.Remove(game);

        var effectedRows = _context.SaveChanges();
        if (effectedRows > 0)
        {
            isDeleted = true;
            var cover = Path.Combine(_imagesPath, game.Cover);
            File.Delete(cover);
        }


        return isDeleted;
    }

    private async Task<string> saveCover(IFormFile cover)
    {
        var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

        var path = Path.Combine(_imagesPath, coverName);

        using var stream = File.Create(path);
        await cover.CopyToAsync(stream);

        return coverName;
    }

   
}