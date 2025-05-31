namespace ApiPruebaVive.Services
{

        public interface IOltStatusService
        {
            Task<List<PuertoDto>> ConsultarPuertosDeTarjeta(string numeroTarjeta);
        }

        public class PuertoDto
        {
            public string Puerto { get; set; }
            public string Estado { get; set; }
            public string Color { get; set; }
        }
    }

