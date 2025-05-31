using System.Text.RegularExpressions;
using System.Text;

namespace ApiPruebaVive.Services
{
    public class OltStatusService : IOltStatusService
    {
        private readonly TelnetConnectionManager _telnetManager;

        public OltStatusService(TelnetConnectionManager telnetManager)
        {
            _telnetManager = telnetManager;
        }

        public async Task<List<PuertoDto>> ConsultarPuertosDeTarjeta(string numeroTarjeta)
        {
            var telnet = _telnetManager.GetConnection("ZTEC600");
            telnet.WriteLine("terminal length 0");
            telnet.ReadUntilPrompt();

            var comandos = new StringBuilder();
            for (int puerto = 1; puerto <= 16; puerto++)
                comandos.AppendLine($"show interface gpon_olt-1/{numeroTarjeta}/{puerto}");

            telnet.WriteLine(comandos.ToString());
            string outputTotal = telnet.ReadUntilPrompt("ZXAN#", 8000);

            var lista = new List<PuertoDto>();

            for (int puerto = 1; puerto <= 16; puerto++)
            {
                string siguiente = puerto < 16
                    ? $"gpon_olt-1/{numeroTarjeta}/{puerto + 1}"
                    : "ZXAN#";

                string patron = $"gpon_olt-1/{numeroTarjeta}/{puerto}.*?(?={siguiente})";
                var match = Regex.Match(outputTotal, patron, RegexOptions.Singleline);
                string resultado = match.Success ? match.Value : "";

                string estado = ParseEstadoPuerto(resultado);

                lista.Add(new PuertoDto
                {
                    Puerto = puerto.ToString(),
                    Estado = estado,
                    Color = GetColorForEstado(estado)
                });
            }

            return lista;
        }

        private string ParseEstadoPuerto(string salida)
        {
            var lines = salida.Split('\n');
            var lineaEstado = lines.FirstOrDefault(l => l.Contains("is activate") || l.Contains("is deactive"));

            if (lineaEstado == null) return "Desconocido";

            bool activo = lineaEstado.Contains("activate");
            bool linkUp = lineaEstado.Contains("line protocol is up");

            if (activo && linkUp) return "Activo - Link Up";
            if (activo && !linkUp) return "Activo - Link Down";
            if (!activo) return "Inactivo";

            return "Desconocido";
        }

        private string GetColorForEstado(string estado)
        {
            if (estado.Contains("Link Up")) return "green";
            if (estado.Contains("Link Down") || estado == "Inactivo") return "red";
            return "gray";
        }
    }
}
