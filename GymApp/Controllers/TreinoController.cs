using GymApp.Data;
using GymApp.Dto;
using GymApp.Servico;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreinoController : ControllerBase
{
    private readonly CreateTreinoHandler _createHandler;
    private readonly ILogger<TesteController> _logger;

    public TreinoController(ILogger<TesteController> logger, GymAppDbContext context, CreateTreinoHandler createHandler)
    {
        _logger = logger;
        _createHandler = createHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CreateTreinoRequest request)
    {
        try
        {
            var treino = await _createHandler.CriarAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = treino.Id }, treino);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar treino");
            return StatusCode(500, "Erro interno ao criar treino");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var treino = await _createHandler.BuscarTreinoPorId(id);

            if (treino == null) return NotFound();

            var dto = new GetTreinoResponse
            {
                Id = treino.Id,
                Nome = treino.Nome,
                Exercicios = treino.Exercicios.Select(et => new ExercicioDoTreinoDto
                {
                    ExercicioId = et.ExercicioId,
                    Series = et.Series,
                    Repeticoes = et.Repeticoes,
                    Carga = et.Carga
                }).ToList()
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar treino");
            return StatusCode(500, "Erro interno ao buscar treino");
        }
    }
}
