namespace ApiPruebaVive.Dto
{
    public class Dispositivo_olt
    {
        public string Onu { get; set; }           // ID de la ONU (ej: "1" en gpon-1/2:1)
        public string Puerto { get; set; }        // Puerto (ej: "2")
        public string Tarjeta { get; set; }       // Tarjeta (ej: "1")
        public string OnuIndex { get; set; }      // Índice completo: gpon-1/2:1
        public string AdminState { get; set; }    // Estado administrativo (ej: "online")
        public string OmccState { get; set; }     // Estado OMCC
        public string PhaseState { get; set; }    // Estado de fase (working/offline)
        public string Channel { get; set; }       // Canal asociado (ej: "0")
        public string OltName { get; set; }       // Nombre de la OLT
    }
}
