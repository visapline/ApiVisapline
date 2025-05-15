using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using ApiPruebaVive.Dto;

namespace ApiPruebaVive.Services.Parsers
{
  
    public class Zte600Parser
    {
        public List<Dispositivo_olt> ObtenerDispositivos(string query, string oltName)
        {
            var dispositivos = new List<Dispositivo_olt>();
            string[] registros = query.Split('\n');

            foreach (var linea in registros)
            {
                if (linea == "--------------------------------------------------------------\r" || linea.Length < 50)
                    continue;

                if (linea.Contains("GPON"))
                {
                    string[] partes = Regex.Split(linea.TrimEnd('\r'), @"\s+");
                    var dispositivo = new Dispositivo_olt
                    {
                        Onu = partes[0].Split(':')[1],
                        Puerto = partes[0].Split(':')[0].Split('/')[2],
                        Tarjeta = partes[0].Split(':')[0].Split('/')[1],
                        OnuIndex = partes[0],
                        AdminState = partes[1],
                        OmccState = partes[2],
                        PhaseState = partes[3],
                        Channel = partes[4],
                        OltName = oltName
                    };

                    dispositivos.Add(dispositivo);
                }
            }

            return dispositivos;
        }




    }
}
