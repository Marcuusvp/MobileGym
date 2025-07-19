using GymApp.Data;
using GymApp.Dto;
using GymApp.Servico.TreinoHandler;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreinoController : ControllerBase
{
    private readonly CreateTreinoHandler _createHandler;
    private readonly UpdateTreinoHandler _updateHandler;
    private readonly DeleteTreinoHandler _deleteHandler;
    private readonly TreinoQueryHandler _queryHandler;
    private readonly ILogger<TesteController> _logger;

    public TreinoController(ILogger<TesteController> logger, GymAppDbContext context, CreateTreinoHandler createHandler, UpdateTreinoHandler updateHandler, DeleteTreinoHandler deleteHandler, TreinoQueryHandler queryHandler)
    {
        _logger = logger;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _queryHandler = queryHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CreateTreinoRequest request)
    {
        try
        {
            var treino = await _createHandler.CriarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = treino.Id }, treino);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar treino");
            throw;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
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
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(
        [FromRoute] Guid id,
        [FromBody] UpdateTreinoRequest request)
    {
        try
        {
            var treinoAtualizado = await _updateHandler.AtualizarAsync(id, request);
            if (treinoAtualizado == null)
            {
                return NotFound($"Treino com ID {id} não encontrado para atualização.");
            }
            return Ok(treinoAtualizado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar treino com ID {Id}", id);
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
                return NotFound($"Treino com ID {id} não encontrado para deleção.");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar treino com ID {Id}", id);
            throw;
        }
    }

        // Endpoint para obter treinos de um usuário com paginação
    [HttpGet("user/{userId}")] // Rota para buscar treinos por ID de usuário
    public async Task<IActionResult> GetTreinosDoUsuario(
        [FromRoute] string userId,
        [FromQuery] GetTreinosDoUsuarioRequest request) // Recebe parâmetros de paginação
    {
        try
        {
            request.Usuario = userId;
            var response = await _queryHandler.GetTreinosByUserIdPaginatedAsync(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter treinos para o usuário {UserId}.", userId);
            throw;
        }
    }
}
