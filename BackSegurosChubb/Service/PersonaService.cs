using BackSegurosChubb.Interface;
using BackSegurosChubb.Models;
using BackSegurosChubb.ViewModel;
using Microsoft.AspNetCore.Mvc;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using EFCore.BulkExtensions;

namespace BackSegurosChubb.Service
{
    public class PersonaService : IPersona
    {
        PruebaSegurosChubbContext _context;
        public PersonaService(PruebaSegurosChubbContext context) { 
            _context = context;
        }

        public bool EsCedulaValido(string Cedula)
        {
            return Cedula.Length == 10;
        }
        public bool EsTelefonoValido(string Telefono)
        {
            return Telefono.Length == 10;
        }
        #region Persona
        public bool SetPersona(PersonaVM persona)
        {
            if (EsCedulaValido(persona.Cedula) && EsTelefonoValido(persona.Telefono))
            {
                var existe = _context.Personas.Where(x => x.Cedula == persona.Cedula).Any();
                bool registrado = false;
                if (!existe)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Persona personas = new Persona
                            {
                                Cedula = persona.Cedula,
                                NombreCliente = persona.NombreCliente,
                                Telefono = persona.Telefono,
                                Edad = persona.Edad,
                                Estado = "A"
                            };
                            _context.Personas.Add(personas);
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
                return registrado;
            }
            else
            {
                return false;
            }
        }
        public List<PersonaVM> GetAllPersona()
        {
            List<PersonaVM> listPersonas = new List<PersonaVM>();
            var persona = _context.Personas.Where(x => x.Estado == "A").ToList();
            foreach (var personas in persona)
            {
                try
                {
                    PersonaVM registro = new PersonaVM
                    {
                        IdAsegurados = personas.IdAsegurados,
                        Cedula = personas.Cedula,
                        NombreCliente = personas.NombreCliente,
                        Telefono = personas.Telefono,
                        Edad = personas.Edad
                    };
                    listPersonas.Add(registro);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consultar clientes: " + ex.Message);
                }
            }
            return listPersonas;
        }

        public PersonaVM GetPersonaByCedula(string cedula)
        {
            PersonaVM persona = null;
            var personaId = _context.Personas.Where(x => x.Cedula == cedula && x.Estado == "A").FirstOrDefault();
            using (var context = _context.Database.BeginTransaction())
            {
                if (personaId != null)
                {
                    persona = new PersonaVM
                    {
                        IdAsegurados = personaId.IdAsegurados,
                        Cedula = personaId.Cedula,
                        NombreCliente = personaId.NombreCliente,
                        Telefono = personaId.Telefono,
                        Edad = personaId.Edad
                    };
                }
            }
            return persona;
        }

        public bool UpdatePersona(PersonaVM persona)
        {
            if (EsCedulaValido(persona.Cedula))
            {
                var personaExiste = _context.Personas.Where(x => x.IdAsegurados == persona.IdAsegurados).FirstOrDefault();
                bool registro = false;
                if (personaExiste != null)
                {
                    using (var context = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            personaExiste.Cedula = persona.Cedula;
                            personaExiste.NombreCliente = persona.NombreCliente;
                            personaExiste.Telefono = persona.Telefono;
                            personaExiste.Edad = persona.Edad;
                            personaExiste.Estado = "A";

                            _context.SaveChanges();
                            context.Commit();
                            registro = true;

                        }
                        catch (Exception )
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

        public bool DeletePersona(int idPersona)
        {
            var personaExiste = _context.Personas.FirstOrDefault(x => x.IdAsegurados == idPersona);
            bool eliminado = false;
            using (var context = _context.Database.BeginTransaction())
            {
                try
                {
                    if (personaExiste != null)
                    {
                        personaExiste.Estado = "I";

                        _context.SaveChanges();
                        context.Commit();
                        eliminado = true;
                    }
                }
                catch (Exception )
                {
                    context.Rollback();
                    eliminado = false;
                }
            }
            return eliminado;
        }

        #endregion

        #region Excel

        public List<PersonaVM> setArchivoExcel(ExcelVM ArchivoExcel)
        {
            try
            {
                byte[] excelBytes = Convert.FromBase64String(ArchivoExcel.File);

                using (MemoryStream ms = new MemoryStream(excelBytes))
                {
                    IWorkbook MiExcel = new XSSFWorkbook(ms);

                    if (MiExcel != null)
                    {
                        ISheet HojaExcel = MiExcel.GetSheetAt(0);
                        if (HojaExcel != null)
                        {
                            int cantidadFilas = HojaExcel.LastRowNum;
                            List<PersonaVM> lista = new List<PersonaVM>();

                            for (int i = 1; i <= cantidadFilas; i++)
                            {
                                IRow fila = HojaExcel.GetRow(i);

                                if (fila != null)
                                {
                                    lista.Add(new PersonaVM
                                    {
                                        Cedula = fila.GetCell(0)?.ToString() ?? "",
                                        NombreCliente = fila.GetCell(1)?.ToString() ?? "",
                                        Telefono = fila.GetCell(2)?.ToString() ?? "",
                                        Edad = int.TryParse(fila.GetCell(3)?.ToString(), out int edad) ? edad : 0
                                    });
                                    foreach (var persona in lista)
                                    {
                                        SetPersona(persona);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Fila nula en el archivo Excel.");
                                }
                            }

                            return lista;

                        }
                        else
                        {
                            Console.WriteLine("No se encontraron hojas en el archivo Excel.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("El objeto MiExcel no se ha inicializado correctamente.");
                    }
                }

                return null; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al procesar el archivo Excel: " + ex.Message);
                return null;
            }
        }

        #endregion
    }





}

