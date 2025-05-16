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

        public List<Dispositivo_olt> ObtenerPotenciasRXZte(string query, string oltName, List<Dispositivo_olt> listado)
        {
            string[] registros = query.Split('\n');

            foreach (var linea in registros)
            {
                if (linea == "------------------------------------\r" || linea.Length < 27)
                    continue;

                if (linea.Contains("gpon-onu") || linea.Contains("gpon_onu"))
                {
                    string[] partes = Regex.Split(linea.TrimEnd('\r'), @"\s+");
                    if (partes.Length < 2)
                        continue;

                    string onuIndex = partes[0]; // ej: gpon-onu_1/2:1
                    string rx = partes[1].Replace("(dbm)", "");

                    string[] indexSplit = onuIndex.Split(':');
                    string[] location = indexSplit[0].Split('/');

                    if (location.Length < 3)
                        continue;

                    var tarjeta = location[1];
                    var puerto = location[2];
                    var onu = indexSplit[1];

                    var existente = listado.FirstOrDefault(d =>
                        d.Tarjeta == tarjeta &&
                        d.Puerto == puerto &&
                        d.Onu == onu
                    );

                    if (existente != null)
                    {
                        existente.RX = rx;
                    }
                }
            }

            return listado;
        }

        public List<Dispositivo_olt> ObtenerPotenciasTXZte(string query, string oltName, List<Dispositivo_olt> listado)
        {
            string[] registros = query.Split('\n');

            foreach (var linea in registros)
            {
                if (linea == "------------------------------------\r" || linea.Length < 27)
                    continue;

                if (linea.Contains("gpon-onu") || linea.Contains("gpon_onu"))
                {
                    string[] partes = Regex.Split(linea.TrimEnd('\r'), @"\s+");
                    if (partes.Length < 2)
                        continue;

                    string onuIndex = partes[0]; // ej: gpon-onu_1/2:1
                    string tx = partes[1].Replace("(dbm)", "");

                    string[] indexSplit = onuIndex.Split(':');
                    string[] location = indexSplit[0].Split('/');

                    if (location.Length < 3)
                        continue;

                    var tarjeta = location[1];
                    var puerto = location[2];
                    var onu = indexSplit[1];

                    var existente = listado.FirstOrDefault(d =>
                        d.Tarjeta == tarjeta &&
                        d.Puerto == puerto &&
                        d.Onu == onu
                    );

                    if (existente != null)
                    {
                        existente.TX = tx;
                    }
                }
            }

            return listado;
        }


    }
}
