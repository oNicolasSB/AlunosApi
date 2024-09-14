using AlunosApi.Interfaces;
using AlunosApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlunosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AlunosController : ControllerBase
{

    private readonly IAlunoService _alunoService;

    public AlunosController(IAlunoService alunoService)
    {
        _alunoService = alunoService;
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
    {
        try
        {
            return Ok(await _alunoService.GetAlunos());
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar alunos");
        }
    }

    [HttpGet("AlunoPorNome")]
    public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByNome([FromQuery] string nome)
    {
        try
        {
            var alunos = await _alunoService.GetAlunosByNome(nome);
            if (alunos == null || alunos.Count() == 0)
                return NotFound($"Não existem alunos com o critério {nome}.");
            return Ok(alunos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar alunos");
        }
    }
    [HttpGet("{id:int}", Name = "GetAluno")]
    public async Task<ActionResult<Aluno>> GetAluno(int id)
    {
        try
        {
            Aluno? aluno = await _alunoService.GetAluno(id);
            if (aluno == null)
                return NotFound($"Aluno com o id {id} não encontrado.");
            return Ok(aluno);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult> Create(Aluno aluno)
    {
        try
        {
            await _alunoService.CreateAluno(aluno);
            return CreatedAtRoute("GetAluno", new { id = aluno.Id }, aluno);
        }
        catch
        {
            return BadRequest("Requisição inválida.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] Aluno aluno)
    {
        try
        {
            if (id != aluno.Id)
                return BadRequest("Dados inconsistentes.");
            await _alunoService.UpdateAluno(aluno);
            return Ok($"Aluno id {id} atualizado com sucesso.");
        }
        catch
        {
            return BadRequest("Requisição inválida.");
        }
    }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            Aluno? aluno = await _alunoService.GetAluno(id);
            if (aluno == null)
                return NotFound($"Aluno com o id {id} não encontrado.");
            await _alunoService.DeleteAluno(aluno);
            return Ok($"Aluno id {id} excluído com sucesso.");
        }
        catch
        {
            return BadRequest("Requisição inválida.");
        }
    }
}
