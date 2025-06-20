// Declaración del espacio de nombres System, que contiene tipos fundamentales como Int32, String, etc.
using System;

// Importa el espacio de nombres System.Collections.Generic que contiene interfaces y clases para colecciones genéricas
// como List<T>, Dictionary<TKey,TValue>, etc.
using System.Collections.Generic;

// Importa el espacio de nombres ClubDeportivo.Models que contiene las clases de modelo (como Socio)
using ClubDeportivo.Models;

// Declara el espacio de nombres ClubDeportivo.Interfaces que contendrá las interfaces del sistema
namespace ClubDeportivo.Interfaces
{
    // Declaración de la interfaz ISocioRepository que define las operaciones CRUD para la entidad Socio
    public interface ISocioRepository
    {
        // Método para agregar un nuevo socio al repositorio
        // Parámetro: socio - objeto de tipo Socio a ser agregado
        // Retorno: void (no devuelve valor)
        void Agregar(Socio socio);

        // Método para obtener todos los socios del repositorio
        // Retorno: List<Socio> - lista completa de socios registrados
        List<Socio> ObtenerTodos();

        // Método para obtener un socio específico por su ID
        // Parámetro: id - identificador único del socio
        // Retorno: Socio - objeto Socio encontrado o null si no existe
        Socio ObtenerPorId(int id);

        // Método para obtener un socio por su número de DNI
        // Parámetro: Dni - número de documento del socio (cadena de texto)
        // Retorno: Socio - objeto Socio encontrado o null si no existe
        Socio ObtenerPorDni(string Dni);

        // Método para eliminar un socio del repositorio por su ID
        // Parámetro: id - identificador único del socio a eliminar
        // Retorno: void (no devuelve valor)
        // Nota: El comentario "por las dudas" sugiere que este método fue agregado como precaución
        void Eliminar(int id);
    }
}