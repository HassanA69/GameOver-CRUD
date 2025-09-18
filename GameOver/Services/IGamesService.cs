namespace GameOver.Services;

public interface IGamesService
{
 IEnumerable<Game> GetAll();
 Task Create(CreateGameFormViewModel model);
}