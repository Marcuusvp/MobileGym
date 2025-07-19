using GymApp.Dto;
using GymApp.Servico.ExercicioHandler;
using GymApp.Servico.ExercicioHAndler;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercicioController : ControllerBase
{
    private readonly ILogger<ExercicioController> _logger;
    private readonly CreateExercicioHandler _createHandler;
    private readonly UpdateExercicioHandler _updateHandler;
    private readonly DeleteExercicioHandler _deleteHandler;
    private readonly ExercicioQueryHandler _queryHandler;


    public ExercicioController(ILogger<ExercicioController> logger, CreateExercicioHandler createHandler, ExercicioQueryHandler queryHandler, DeleteExercicioHandler deleteHandler, UpdateExercicioHandler updateHandler)
    {
        _logger = logger;
        _createHandler = createHandler;
        _queryHandler = queryHandler;
        _deleteHandler = deleteHandler;
        _updateHandler = updateHandler;
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
            throw;
        }
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromForm] UpdateExercicioRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("O ID na URL não corresponde ao ID no corpo da requisição.");
        }

        try
        {
            var exercicioAtualizado = await _updateHandler.AtualizarAsync(id, request);
            if (exercicioAtualizado == null)
            {
                return NotFound($"Exercício com ID {id} não encontrado para atualização.");
            }
            return Ok(exercicioAtualizado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar exercicio com ID {Id}", id);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            var sucesso = await _deleteHandler.DeletarAsync(id);
            if (!sucesso)
            {
                return NotFound($"Exercício com ID {id} não encontrado para deleção.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar exercicio com ID {Id}", id);
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var exercicio = await _queryHandler.GetByIdAsync(id);
            if (exercicio == null)
            {
                return NotFound($"Exercício com ID {id} não encontrado.");
            }
            return Ok(exercicio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter exercicio com ID {Id}", id);
            throw;
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
    {
        try
        {
            var response = await _queryHandler.GetAllPaginatedAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todos os exercícios paginados.");
            throw;
        }
    }
}
