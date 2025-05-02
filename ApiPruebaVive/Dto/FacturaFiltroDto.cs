namespace ApiPruebaVive.Dto
{
    public class FacturaFiltroDto
    {
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public int TipoTercero { get; set; }
        public bool Pagado { get; set; }
    }
}
