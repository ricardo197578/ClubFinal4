//CODIGO COMENTADO POR IA
using System;
using System.Collections.Generic;
using ClubDeportivo.Models;

namespace ClubDeportivo.Interfaces
{
    public interface ICuotaRepository
    {
       
        /// Busca un socio activo por su DNI
        Socio BuscarSocioActivoPorDni(string dni);
        
      
        /// Registra el pago de una cuota y actualiza automáticamente la fecha de vencimiento
       
        void RegistrarPagoCuota(int socioId, decimal monto, DateTime fechaPago, MetodoPago metodo);
        
        
        /// Obtiene la fecha de vencimiento actual del socio
      
        DateTime ObtenerFechaVencimientoActual(int socioId);
        
        
        /// Actualiza manualmente la fecha de vencimiento de un socio
     
        void ActualizarFechaVencimiento(int socioId, DateTime nuevaFecha);
        
    
        /// Activa el estado de un socio
        
        void ActivarSocio(int socioId);
        
       
        /// Obtiene las cuotas por vencer hasta la fecha límite especificada
     
        IEnumerable<Cuota> ObtenerCuotasPorVencer(DateTime fechaLimite);
        
       
        /// Obtiene todas las cuotas de un socio específico
       
        IEnumerable<Cuota> ObtenerCuotasPorSocio(int socioId);
        
       
        /// Obtiene el valor actual de la cuota
      
        decimal ObtenerValorActualCuota();
    }
}
