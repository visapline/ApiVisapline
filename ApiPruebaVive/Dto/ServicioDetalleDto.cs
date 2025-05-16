namespace ApiPruebaVive.Dto
{
   public class ServicioDetalleDto
{
    public int IdServicio { get; set; }
    public int? InventarioId { get; set; }
    public int? RouterboardId { get; set; }
    public string? DireccionIp { get; set; }
    public int IndicePuerto { get; set; }
    public int PuertoId { get; set; } // corregido
    public int InventarioId2 { get; set; }
    public int BarrioId { get; set; }
    public string Barrio { get; set; }
    public string Direccion { get; set; }
    public string TipoDireccion { get; set; }
    public string Serial { get; set; }
    public string EstadoInventario { get; set; }
    public int NumeroPuerto { get; set; }
    public string ReferenciaTarjeta { get; set; }

    public int Codigo { get; set; }
    public string Identificacion { get; set; }
    public string Usuario { get; set; }
    public string TipoTercero { get; set; }
    public int IdContrato { get; set; } // corregido
    public string EstadoContrato { get; set; }
    public string EstadoServicio { get; set; }
    public string? UsuarioServicio { get; set; }
}


}
