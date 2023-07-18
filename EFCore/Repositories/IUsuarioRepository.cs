﻿using EFCore.Models;

namespace EFCore.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAll();
        Task<Usuario> GetById(int id);

        Task<Usuario> Create(Usuario usuario);

        Task Update(Usuario usuario);
    
        Task Delete(int id);

        Task<int> Check(UsuarioAuthForm login);

        int GetUserByCPF(string cpf);

        int GetUserByEmail(string email);

        int GetUserByTel(string telefone);
    }
}
