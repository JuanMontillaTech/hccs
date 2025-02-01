using System;
using System.Collections.Generic;
using ERP.Domain.Constants;
namespace ERP.Domain.Constants;
 
public static class RecipeStatusGuids
{
    public enum RecipeStatusType
    {
        Pendiente,    // Estado: Pendiente
        Completado,   // Estado: Completado
        EnEspera      // Estado: En espera
    }

    public static readonly Dictionary<RecipeStatusType, Guid> StatusGuids = new Dictionary<RecipeStatusType, Guid>
    {
        { RecipeStatusType.Pendiente, new Guid("10A8CA88-B198-4241-9F5F-EAD7AE023484") },  // Guid para Pendiente
        { RecipeStatusType.Completado, new Guid("D6A2FCF6-D571-4F4B-A53D-D41A7C8A066F") }, // Guid para Completado
        { RecipeStatusType.EnEspera, new Guid("926142F4-0DA8-448F-ACA8-87BA2778CC78") }    // Guid para En espera
    };

    public static Guid GetGuidForStatus(RecipeStatusType status)
    {
        if (StatusGuids.TryGetValue(status, out Guid guid)) // Corregido: Acceso directo a StatusGuids
        {
            return guid;
        }
        throw new KeyNotFoundException($"No se encontró un Guid para el estado: {status}");
    }
}