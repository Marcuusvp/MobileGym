using GymApp.Dto;
using GymApp.Servico;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercicioController : ControllerBase
{
    private readonly ILogger<TesteController> _logger;
    private readonly CreateExercicioHandler _createHandler;

    public ExercicioController(ILogger<TesteController> logger, CreateExercicioHandler createHandler)
    {
        _logger = logger;
        _createHandler = createHandler;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Criar([FromForm] CreateExercicioRequest request)
    {
        try
        {
            var exercicio = await _createHandler.CriarAsync(request);
            return CreatedAtAction(nameof(Criar), new { id = exercicio.Id }, exercicio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar exercicio");
            return StatusCode(500, "Erro interno ao criar exercicio");
        }
    }
}
