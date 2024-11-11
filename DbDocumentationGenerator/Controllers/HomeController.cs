using DatabaseMetadataReporting.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseMetadataReporting.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public IDatabaseService databaseService;
    public HomeController(ILogger<HomeController> logger, IDatabaseService _databaseService)
    {
        _logger = logger;
        databaseService = _databaseService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var model = await databaseService.GetDatabaseMetadata();

            return View(model);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
