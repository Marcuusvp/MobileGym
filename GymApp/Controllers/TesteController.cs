using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TesteController : ControllerBase
{
    private readonly ILogger<TesteController> _logger;

    public TesteController(ILogger<TesteController> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// Gera logs nos níveis Information, Warning e Error.
    /// </summary>
    [HttpGet("logs")]
    public IActionResult GenerateLogs()
    {
        _logger.LogInformation("🏋️‍♂️ Test log (Information) em {Timestamp}", DateTime.UtcNow);
        _logger.LogWarning("⚠️ Test log (Warning) no treino id={TreinoId}", Guid.NewGuid());
        _logger.LogError("❌ Test log (Error) simulando falha em {Endpoint}", "/api/test/logs");
        return Ok(new { Message = "Logs gerados com sucesso" });
    }
    /// <summary>
    /// Lança uma exceção para testar logging de erros e traces de falha.
    /// </summary>
    [HttpGet("error")]
    public IActionResult GenerateError()
    {
        throw new InvalidOperationException("💥 Exceção de teste lançada em /api/test/error");
    }
}
