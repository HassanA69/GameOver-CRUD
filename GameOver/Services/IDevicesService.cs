namespace GameOver.Services;

public interface IDevicesService
{
    IEnumerable<SelectListItem> GetSelectList();

}
