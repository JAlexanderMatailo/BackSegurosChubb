using BackSegurosChubb.Interface;
using BackSegurosChubb.Models;
using BackSegurosChubb.ViewModel;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BackSegurosChubb.Service
{
    public class SeguroService : ISeguro
    {
        PruebaSegurosChubbContext _context;
        public SeguroService(PruebaSegurosChubbContext context) {
        
            _context = context;

        }
        #region Seguro
        public bool EsCodigoValido(string codigo)
        {
            if (Regex.IsMatch(codigo, "^[A-Za-z0-9]{6}$"))
            {
                return true;
            }
            return false;
        }
        public bool SetSeguros(SeguroVM seguro)
        {
            if (EsCodigoValido(seguro.Codigo))
            {
                var existe = _context.Seguros.Where(x => x.Codigo == seguro.Codigo).Any();
                bool registrado = false;
                if (!existe)
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
                        }
                        catch (Exception )
                        {
                            context.Rollback();
                            registrado = false;
                        }

                    }
                }
                return registrado;
            }
            else
            {
                return false;
            }
        }

        public List<SeguroVM> GetAllSeguro()
        {
            List<SeguroVM> listaSeguros = new List<SeguroVM>();
            var seguros = _context.Seguros.Where(x => x.Estado == "A").ToList();
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

        public bool UpdateSeguro(SeguroVM seguro)
        {
            if (EsCodigoValido(seguro.Codigo))
            {
                var seguroExiste = _context.Seguros.Where(x => x.IdSeguros == seguro.IdSeguros).FirstOrDefault();
                bool registro = false;
                if (seguroExiste != null)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            seguroExiste.NombreSeguro = seguro.NombreSeguro;
                            seguroExiste.Codigo = seguro.Codigo;
                            seguroExiste.SumaAsegurada = seguro.SumaAsegurada;
                            seguroExiste.Prima = seguro.Prima;
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
            else
            {
                return false;
            }
        }

        public bool DeleteSeguro(int id)
        {
            var polizaExiste = _context.Polizas.Where(s => s.IdSeguros == id).Any();
            var seguroExiste = _context.Seguros.FirstOrDefault(x => x.IdSeguros == id && x.Estado == "A");
            bool eliminado = false;
            using (var context = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!polizaExiste)
                    {
                        if (seguroExiste != null)
                        {
                            seguroExiste.Estado = "I"; //inactivo

                            _context.SaveChanges();
                            context.Commit();
                            eliminado = true;

                        }
                    }
                }
                catch (Exception)
                {
                    context.Rollback();
                    eliminado = false;
                }
            }
            return eliminado;
        }
        #endregion

        #region Poliza
        public bool SetPoliza(SetPolizas setPolizas)
        {
            bool registrado = false;

            if (setPolizas.seguros != null)
            {
                using (var context = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var seguroID in setPolizas.seguros)
                        {
                            if (int.TryParse(seguroID, out int idSeguro))
                            {
                                Poliza poliza = new Poliza
                                {
                                    IdAsegurados = setPolizas.idAsegurados,
                                    IdSeguros = idSeguro,
                                    Estado = "A"
                                };

                                _context.Polizas.Add(poliza);
                            }
                            else
                            {
                                Console.WriteLine($"No se pudo convertir '{seguroID}' en un número entero.");
                            }
                        }

                        _context.SaveChanges();
                        context.Commit();
                        registrado = true;
                    }
                    catch (Exception)
                    {
                        context.Rollback();
                        registrado = false;
                    }
                }
            }
            else
            {
                Console.WriteLine("La lista de seguros está vacía o nula.");
            }

            return registrado;
        }

        public List<PolizaVM> GetAllPoliza()
        {
            List<PolizaVM> Polizas = new List<PolizaVM>();
            PolizaVM poliza = new PolizaVM();
            using (var context = _context.Database.BeginTransaction())
            {
                var seguros = (
                    from pol in _context.Polizas
                    join per in _context.Personas on pol.IdAsegurados equals per.IdAsegurados
                    join seg in _context.Seguros on pol.IdSeguros equals seg.IdSeguros
                    where (pol.Estado == "A")
                    select new
                    {
                        pol.IdSeguros,
                        pol.IdAsegurados,
                        pol.IdPoliza,
                        per.Cedula,
                        per.NombreCliente,
                        seg.NombreSeguro,
                        seg.Codigo,
                        seg.SumaAsegurada,
                        seg.Prima,
                        pol.Estado

                    }
                ).ToList();
                if (seguros != null)
                {
                    foreach (var seguro in seguros)
                    {
                        poliza = new PolizaVM
                        {
                            IdPoliza = seguro.IdPoliza,
                            IdAsegurados = seguro.IdAsegurados,
                            IdSeguros = seguro.IdSeguros,
                            cedulaPersona = seguro.Cedula,
                            NombrePersona = seguro.NombreCliente,
                            DescricionSeguro = seguro.NombreSeguro,
                            CodigoSeguro = seguro.Codigo,
                            ValorAsegurado = Convert.ToDecimal(seguro.SumaAsegurada),
                            Prima = Convert.ToDecimal(seguro.Prima),
                            Estado = seguro.Estado
                        };
                        Polizas.Add(poliza);
                    }

                }
            }
            return Polizas;
        }

        public List<PolizaVM> GetPolizaByCedula(string cedula)
        {
            List<PolizaVM> polizas = new List<PolizaVM>();
            PolizaVM polizaVM = new PolizaVM();

            using (var context = _context.Database.BeginTransaction())
            {
                var seguros = (
                    from pol in _context.Polizas
                    join per in _context.Personas on pol.IdAsegurados equals per.IdAsegurados
                    join seg in _context.Seguros on pol.IdSeguros equals seg.IdSeguros
                    where (pol.Estado == "A" && per.Cedula == cedula)
                    select new
                    {
                        pol.IdSeguros,
                        pol.IdAsegurados,
                        pol.IdPoliza,
                        per.Cedula,
                        per.NombreCliente,
                        seg.NombreSeguro,
                        seg.Codigo,
                        seg.SumaAsegurada,
                        seg.Prima,
                        pol.Estado
                    }
                    ).ToList();
                if (seguros != null)
                {
                    foreach (var seguro in seguros)
                    {
                        polizaVM = new PolizaVM
                        {
                            IdPoliza = seguro.IdPoliza,
                            IdAsegurados = seguro.IdAsegurados,
                            IdSeguros = seguro.IdSeguros,
                            cedulaPersona = seguro.Cedula,
                            NombrePersona = seguro.NombreCliente,
                            DescricionSeguro = seguro.NombreSeguro,
                            CodigoSeguro = seguro.Codigo,
                            ValorAsegurado = Convert.ToDecimal(seguro.SumaAsegurada),
                            Prima = Convert.ToDecimal(seguro.Prima),
                            Estado = seguro.Estado
                        };
                        polizas.Add(polizaVM);
                    }

                }
            }
            return polizas;
        }

        public List<PolizaVM> GetPolizasByCodigoSeguro(string Codigo)
        {
            List<PolizaVM> polizas = new List<PolizaVM>();
            PolizaVM polizaVM = new PolizaVM();

            using (var context = _context.Database.BeginTransaction())
            {
                var seguros = (
                    from pol in _context.Polizas
                    join per in _context.Personas on pol.IdAsegurados equals per.IdAsegurados
                    join seg in _context.Seguros on pol.IdSeguros equals seg.IdSeguros
                    where (pol.Estado == "A" && seg.Codigo == Codigo)
                    select new
                    {
                        pol.IdSeguros,
                        pol.IdAsegurados,
                        pol.IdPoliza,
                        per.Cedula,
                        per.NombreCliente,
                        seg.NombreSeguro,
                        seg.Codigo,
                        seg.SumaAsegurada,
                        seg.Prima,
                        pol.Estado
                    }
                    ).ToList();
                if (seguros != null)
                {
                    foreach (var seguro in seguros)
                    {
                        polizaVM = new PolizaVM
                        {
                            IdPoliza = seguro.IdPoliza,
                            IdAsegurados = seguro.IdAsegurados,
                            IdSeguros = seguro.IdSeguros,
                            cedulaPersona = seguro.Cedula,
                            NombrePersona = seguro.NombreCliente,
                            DescricionSeguro = seguro.NombreSeguro,
                            CodigoSeguro = seguro.Codigo,
                            ValorAsegurado = Convert.ToDecimal(seguro.SumaAsegurada),
                            Prima = Convert.ToDecimal(seguro.Prima),
                            Estado = seguro.Estado
                        };
                        polizas.Add(polizaVM);
                    }

                }
            }
            return polizas;

        }

        public List<PolizaVM> GetPolizasPorByAmbosFiltros(string cedula, string Codigo)
        {
            List<PolizaVM> polizas = new List<PolizaVM>();
            PolizaVM polizaVM = new PolizaVM();

            using (var context = _context.Database.BeginTransaction())
            {
                var seguros = (
                    from pol in _context.Polizas
                    join per in _context.Personas on pol.IdAsegurados equals per.IdAsegurados
                    join seg in _context.Seguros on pol.IdSeguros equals seg.IdSeguros
                    where (pol.Estado == "A" && per.Cedula == cedula && seg.Codigo == Codigo)
                    select new
                    {
                        pol.IdSeguros,
                        pol.IdAsegurados,
                        pol.IdPoliza,
                        per.Cedula,
                        per.NombreCliente,
                        seg.NombreSeguro,
                        seg.Codigo,
                        seg.SumaAsegurada,
                        seg.Prima,
                        pol.Estado
                    }
                    ).ToList();
                if (seguros != null)
                {
                    foreach (var seguro in seguros)
                    {
                        polizaVM = new PolizaVM
                        {
                            IdPoliza = seguro.IdPoliza,
                            IdAsegurados = seguro.IdAsegurados,
                            IdSeguros = seguro.IdSeguros,
                            cedulaPersona = seguro.Cedula,
                            NombrePersona = seguro.NombreCliente,
                            DescricionSeguro = seguro.NombreSeguro,
                            CodigoSeguro = seguro.Codigo,
                            ValorAsegurado = Convert.ToDecimal(seguro.SumaAsegurada),
                            Prima = Convert.ToDecimal(seguro.Prima),
                            Estado = seguro.Estado
                        };
                        polizas.Add(polizaVM);
                    }
                }
            }
            return polizas;
        }


        public List<PolizaVM> GetAllPolizas(string cedula, string Codigo)
        {
            if (!string.IsNullOrEmpty(cedula) && !string.IsNullOrEmpty(Codigo))
            {
                return GetPolizasPorByAmbosFiltros(cedula, Codigo);

            }
            else if (!string.IsNullOrEmpty(cedula))
            {
                return GetPolizaByCedula(cedula);

            }
            else if (!string.IsNullOrEmpty(Codigo))
            {
                return GetPolizasByCodigoSeguro(Codigo);
            }
            else
            {
                return GetAllPoliza();
            }
        }

        #endregion
    }
}
