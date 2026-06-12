using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.Interfaces.Repositories;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Infrastructure.Data;

namespace SmartLibrary.Infrastructure.Repositories;

public class UsuarioRepository(SmartLibraryDbContext context) : BaseRepository<Usuario>(context), IUsuarioRepository
{
    public async Task<Usuario?> GetByEmailAsync(string email)
        => await Context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<Aluno?> GetAlunoByUsuarioIdAsync(int usuarioId)
        => await Context.Alunos
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);

    public async Task<Aluno?> GetAlunoByIdAsync(int alunoId)
        => await Context.Alunos
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == alunoId);

    public async Task<Aluno?> GetAlunoByEmailAsync(string email)
        => await Context.Alunos
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Usuario.Email == email && a.Usuario.Ativo);
}
