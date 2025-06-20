// En Interfaces/IUsuarioRepository.cs

using System;
using ClubDeportivo.Models;
namespace ClubDeportivo.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario ObtenerPorNombreUsuario(string nombreUsuario);
        void Agregar(Usuario usuario);
    }
}