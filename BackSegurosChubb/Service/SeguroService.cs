using BackSegurosChubb.Interface;
using BackSegurosChubb.Models;
using BackSegurosChubb.ViewModel;

namespace BackSegurosChubb.Service
{
    public class SeguroService : ISeguro
    {
        PruebaSegurosChubbContext _context;
        public SeguroService(PruebaSegurosChubbContext context) {
        
            _context = context;

        }
        
        public bool SetSeguros(SeguroVM seguro)
        {
            var existe = _context.Seguros.Where(x => x.Codigo == seguro.Codigo).Any();
            bool registrado = false;
            if(!existe)
            {
                using (var context = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Seguro seguros = new Seguro
                        {
                            NombreSeguro = seguro.NombreSeguro,
                            Codigo = seguro.Codigo,
                            SumaAsegurada = seguro.SumaAsegurada,
                            Prima = seguro.Prima,
                            Estado = "A"

                        };
                        _context.Seguros.Add(seguros);
                        _context.SaveChanges();

                        context.Commit();
                        registrado = true;
                    }catch (Exception ex)
                    {
                        context.Rollback();
                        Console.WriteLine("Error al registrar: " + ex.ToString());
                        registrado = false;
                    }
                }
            }
            return registrado;
        }
    }
}
