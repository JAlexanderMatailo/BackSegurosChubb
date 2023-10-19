namespace BackSegurosChubb.ViewModel
{
    public class PolizaVM
    {
        public int IdPoliza { get; set; }
        public int IdSeguros { get; set; }
        public string DescricionSeguro { get; set; }
        public string CodigoSeguro { get; set; }
        public decimal ValorAsegurado { get; set; }
        public decimal Prima { get; set; }
        public string Estado { get; set; }
        public int IdAsegurados { get; set; }
        public string cedulaPersona { get; set; }
        public string NombrePersona { get; set; }
    }
}
