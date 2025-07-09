using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreinoController : ControllerBase
{
    private readonly ILogger<TesteController> _logger;

    public TreinoController(ILogger<TesteController> logger)
    {
        _logger = logger;
    }
}
