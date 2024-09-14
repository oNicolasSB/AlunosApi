using AlunosApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Context;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>().HasData(
            new Aluno { Id = 1, Nome = "João da Silva", Email = "j@j.com", Idade = 20 },
            new Aluno { Id = 2, Nome = "Maria da Silva", Email = "m@m.com", Idade = 22 }
        );
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Aluno> Alunos { get; set; }
}
