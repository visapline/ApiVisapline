namespace ApiPruebaVive.Dto
{
    public class Dispositivo_olt
    {
        public string Onu { get; set; }
        public string Puerto { get; set; }
        public string Tarjeta { get; set; }
        public string OnuIndex { get; set; }
        public string AdminState { get; set; }
        public string OmccState { get; set; }
        public string PhaseState { get; set; }
        public string Channel { get; set; }
        public string OltName { get; set; }
        public string RX { get; set; }
        public string TX { get; set; }

        // Propiedades adicionales para cruzar con servicios
        public string UserName { get; set; }          // usuario desde servicios
        public string EstadoDb { get; set; }          // estado desde servicios
        public string EstadocDb { get; set; }         // estadoc desde servicios
        public string SerialDb { get; set; }          // serial desde servicios
        public string IdentificacionDb { get; set; }  // identificacion desde servicios

        // Puedes agregar también propiedades para estados visuales sin HTML
        public string EstadoRXVisual { get; set; }
        public string EstadoPhaseStateVisual { get; set; }
    }
}
