using AlunosApi.Context;
using AlunosApi.Interfaces;
using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Services;

public class AlunoService : IAlunoService
{
    private readonly ApplicationDbContext _context;

    public AlunoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAluno(Aluno aluno)
    {
        _context.Alunos.Add(aluno);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAluno(Aluno aluno)
    {
        _context.Alunos.Remove(aluno);
        await _context.SaveChangesAsync();
    }

    public async Task<Aluno?> GetAluno(int id)
    {
        try
        {
            Aluno? aluno = await _context.Alunos.FindAsync(id) ?? throw new Exception("Aluno não encontrado");
            return aluno;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<Aluno>> GetAlunos()
    {
        try
        {

            return await _context.Alunos.ToListAsync();
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<Aluno>> GetAlunosByNome(string nome)
    {
        IEnumerable<Aluno> alunos;
        if (!string.IsNullOrWhiteSpace(nome))
        {
            alunos = await _context.Alunos.Where(aluno => aluno.Nome.Contains(nome)).ToListAsync();
        }
        else
        {
            alunos = await _context.Alunos.ToListAsync();
        }
        return alunos;
    }

    public async Task UpdateAluno(Aluno aluno)
    {
        _context.Entry(aluno).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
