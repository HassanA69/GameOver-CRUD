
using GameOver.Attribute;

namespace GameOver.ViewModel;

public class CreateGameFormViewModel   : GameFormViewModel
{
    [AllowedExtensions(FileSettings.AllowedExtensions),
    MaxFileSize(FileSettings.MaxFileSizeInByte)]
    public IFormFile Cover { get; set; } = default!;




}