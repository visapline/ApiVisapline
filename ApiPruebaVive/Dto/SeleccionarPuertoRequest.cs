namespace ApiPruebaVive.Dto
{
    public class SeleccionarPuertoRequest
    {
        public string Puerto { get; set; }
        public string Tarjeta { get; set; }
        public string TxtOlt { get; set; }  // Este es el nombre de la OLT
    }
}
