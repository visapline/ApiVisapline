using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPruebaVive.Dto
{
    public class FacturaDTO
    {
        public int IdFactura { get; set; }  // factura.idfactura
        public int FacturaVenta { get; set; }  // facturaventa
        public string Referencia { get; set; }  // CONCAT(terceros.idterceros, ABS(factura.idcont_consecutivo)) AS referencia
        public DateTime FechaEmision { get; set; }  // fechaemision AS FechaEmision
        [Column("codigo")]
        public int CodigoResol { get; set; }  // resol_factura.codigo
        public DateTime Corte { get; set; }  // fechacorte AS corte
        public double ValorFac { get; set; }  // valorfac
        public double IvaFac { get; set; }  // ivafac
        public string NumeroCentro { get; set; }  // centrocosto.numerocentro
        public string TipoDoc { get; set; }  // tipodoc
        public bool Razon { get; set; }  // (CASE WHEN tipodoc = 'CEDULA' THEN false ELSE true END) AS razon
        public string Nombre { get; set; }  // terceros.nombre
        public string Apellido { get; set; }  // terceros.apellido
        public string Identificacion { get; set; }  // identificacion
        public bool Iva { get; set; }  // (CASE WHEN estrato::INTEGER < 3 THEN false ELSE true END) AS iva
        public string Direccion { get; set; }  // terceros.direccion
        public string CodePais { get; set; }  // pais.codigo AS codepais
        public string CodeEstado { get; set; }  // departamento.codigo AS codeestado
        public string CodeCiudad { get; set; }  // municipio.codigo AS codeciudad
        public string Correo { get; set; }  // correo
        public string Telefono { get; set; }  // (SELECT telefono FROM telefono WHERE telefono.terceros_idterceros = idterceros LIMIT 1) AS telefono
        public string PrefijoContable { get; set; }  // resol_factura.prefijocontable
        public string Paiment { get; set; }  // resol_factura.paiment
        public int IdCodigo { get; set; }  // factura_syn_siigo.idcodigo
        public string Observacion { get; set; }  // factura_syn_siigo.observacion
        public string Modalidad { get; set; }  // resol_factura.modalidad
        public int IdResolucion { get; set; }  // resol_factura.idresolucion
    }


}
