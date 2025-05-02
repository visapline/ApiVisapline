

using ApiPruebaVive.Models;

namespace ApiPruebaVive.Entidades
{
    public class JwtValidationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Usuario Result { get; set; }
    }
}
