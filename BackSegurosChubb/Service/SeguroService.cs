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
        #region Seguro
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
                        registrado = false;
                    }
                }
            }
            return registrado;
        }

        public List<SeguroVM> GetAllSeguro()
        {
            List<SeguroVM> listaSeguros = new List<SeguroVM>();
            //var seguros = _context.Seguros.Where(x => x.Estado == "A").ToList();
            var seguros = _context.Seguros.ToList();
            foreach (var seguro in seguros)
            {
                try
                {
                    SeguroVM registro = new SeguroVM
                    {
                        IdSeguros = seguro.IdSeguros,
                        NombreSeguro = seguro.NombreSeguro,
                        Codigo = seguro.Codigo,
                        SumaAsegurada = seguro.SumaAsegurada,
                        Prima = seguro.Prima
                    };
                    listaSeguros.Add(registro);
                }catch (Exception ex)
                {
                    Console.WriteLine("Error al consultar seguros: " + ex.Message);
                }
                
            }
            return listaSeguros;
        }

        public SeguroVM GetSeguroById(int id)
        {
            SeguroVM seguroVM = null;
            var seguroId = _context.Seguros.Where(x => x.IdSeguros == id && x.Estado == "A").FirstOrDefault();
            if (seguroId != null)
            {
                using (var context = _context)
                {
                    try
                    {
                        seguroVM = new SeguroVM
                        {
                            IdSeguros = seguroId.IdSeguros,
                            NombreSeguro = seguroId.NombreSeguro,
                            Codigo = seguroId.Codigo,
                            SumaAsegurada = seguroId.SumaAsegurada,
                            Prima = seguroId.Prima
                        };
                    }catch(Exception ex)
                    {
                        Console.WriteLine("Error al consultar por el id del seguro: " + ex.Message);
                    }
                }
            }

            return seguroVM;
        }

        public SeguroVM GetSeguroByCode(string codigo)
        {
            SeguroVM seguroVM = null;
            var seguroId = _context.Seguros.Where(x => x.Codigo == codigo && x.Estado == "A").FirstOrDefault();
            if (seguroId != null)
            {
                using (var context = _context)
                {
                    try
                    {
                        seguroVM = new SeguroVM
                        {
                            IdSeguros = seguroId.IdSeguros,
                            NombreSeguro = seguroId.NombreSeguro,
                            Codigo = seguroId.Codigo,
                            SumaAsegurada = seguroId.SumaAsegurada,
                            Prima = seguroId.Prima                            
                        };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al consultar por codigo del seguro: " + ex.Message);
                    }
                }
            }

            return seguroVM;
        }

        public bool UpdateSeguro(SeguroVM seguroVM)
        {
            var seguroExiste = _context.Seguros.Where(x => x.IdSeguros == seguroVM.IdSeguros).FirstOrDefault();
            bool registro = false;
            if (seguroExiste != null)
            {
                using(var context = _context.Database.BeginTransaction())
                {
                    try
                    {
                        seguroExiste.NombreSeguro = seguroVM.NombreSeguro;
                        seguroExiste.Codigo = seguroVM.Codigo;
                        seguroExiste.SumaAsegurada = seguroVM.SumaAsegurada;
                        seguroExiste.Prima = seguroVM.Prima;
                        seguroExiste.Estado = "A";

                        _context.SaveChanges();
                        context.Commit();
                        registro = true;
                    }
                    catch (Exception)
                    {
                        context.Rollback();
                        registro = false;
                    }
                }
            }
            return registro;
        }
        #endregion
    }
}
