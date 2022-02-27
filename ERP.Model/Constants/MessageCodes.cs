namespace ERP.Domain.Constants
{
    public struct MessageCodes
    {
        public static readonly string UnknownException = "Ha ocurrido un error inesperado.";
        public static readonly string SecurityException = "Acceso no autorizado.";
        public static readonly string ArgumentException = "Error en parámetros recibidos.";
        public static readonly string ArgumentNullException = "Error en parámetros recibidos.";
        public static readonly string ADProfileNotFoundException = "El Correo del Usuario no tiene Perfil en el Active Directory";
        public static readonly string IncorrectUserOrPassword = "Usuario o contraseña incorrecta";
        public static readonly string UriFormatException = "Formato de ruta inválido.";
        public static readonly string HttpRequestException = "Error de comunicación.";
        public static readonly string EntityValidationException = "Error de Validación en la Entidad.";
        public static readonly string AuthorizationValidationException = "Error de Validació de Authorización.";
        public static readonly string EmptyCollections = "No se encontraron registros para mostrar o procesar";
        public static readonly string ErrorCreating = "Se ha producido un error en la creación del registro";
        public static readonly string ErrorUpdating = "Se ha producido un error en la actualización del registro";
        public static readonly string ErrorDeleting = "Se ha producido un error al eliminar el registro!";
        public static readonly string RecordExist = "Ya existe un registro con estos Datos";
        public static readonly string UserPasswordExist = "Ya existe un registro con este Usuario y Contraseña";
        public static readonly string AuthorizationException = "El usuario no tiene acceso al sistema";
        public static readonly string AuthorizationExceptionRol = "El usuario no tiene rol de acceso al sistema";
        public static readonly string TokenAuthorizationException = "El usuario no ha iniciado sesión o hay errores en el token";
        public static readonly string CompanyNotSpecified = "Debe especificar una compañia";
        public static readonly string CompanyNotFound = "Compañia no encontrada";
        public static readonly string LocationNotSpecified = "Debe especificar una localidad";
        public static readonly string LocationNotFoundException = "Localidad no encontrada";
        public static readonly string SubLocationNotFoundException = "Sub-Localidad no encontrada";
        public static readonly string InactiveUserException = "Su usuario está inactivo";

        public static string NotFound(object property)
        {
            return $"{property} no encontrardo!";
        }

        public static string NotFound()
        {
            return $"Registro no encontrardo!";
        }

        public static string AddedSuccessfully()
        {
            return "Registro(s) ha sido agregado(s)!";
        }

        public static string AddedSuccessfully(object property, string keyValue)
        {
            return $"{property}:{keyValue} ha sido agregado!";
        }

        public static string UpdatedSuccessfully(object property)
        {
            return $"El registro {property} ha sido modificado correctamente!";
        }

        public static string UpdatedSuccessfully()
        {
            return "Registro(s) ha sido actualizado(s)!";
        }

        public static string DeletedSuccessfully(object property)
        {
            return $"El registro {property} ha sido eliminado!";
        }

        public static string InactivatedSuccessfully(object property)
        {
            return $"El registro {property} ha sido inactivado correctamente!";
        }
    }
}