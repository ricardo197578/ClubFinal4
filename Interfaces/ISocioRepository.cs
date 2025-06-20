// Declaraci�n del espacio de nombres System, que contiene tipos fundamentales como Int32, String, etc.
using System;

// Importa el espacio de nombres System.Collections.Generic que contiene interfaces y clases para colecciones gen�ricas
// como List<T>, Dictionary<TKey,TValue>, etc.
using System.Collections.Generic;

// Importa el espacio de nombres ClubDeportivo.Models que contiene las clases de modelo (como Socio)
using ClubDeportivo.Models;

// Declara el espacio de nombres ClubDeportivo.Interfaces que contendr� las interfaces del sistema
namespace ClubDeportivo.Interfaces
{
    // Declaraci�n de la interfaz ISocioRepository que define las operaciones CRUD para la entidad Socio
    public interface ISocioRepository
    {
        // M�todo para agregar un nuevo socio al repositorio
        // Par�metro: socio - objeto de tipo Socio a ser agregado
        // Retorno: void (no devuelve valor)
        void Agregar(Socio socio);

        // M�todo para obtener todos los socios del repositorio
        // Retorno: List<Socio> - lista completa de socios registrados
        List<Socio> ObtenerTodos();

        // M�todo para obtener un socio espec�fico por su ID
        // Par�metro: id - identificador �nico del socio
        // Retorno: Socio - objeto Socio encontrado o null si no existe
        Socio ObtenerPorId(int id);

        // M�todo para obtener un socio por su n�mero de DNI
        // Par�metro: Dni - n�mero de documento del socio (cadena de texto)
        // Retorno: Socio - objeto Socio encontrado o null si no existe
        Socio ObtenerPorDni(string Dni);

        // M�todo para eliminar un socio del repositorio por su ID
        // Par�metro: id - identificador �nico del socio a eliminar
        // Retorno: void (no devuelve valor)
        // Nota: El comentario "por las dudas" sugiere que este m�todo fue agregado como precauci�n
        void Eliminar(int id);
    }
}