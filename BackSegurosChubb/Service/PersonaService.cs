using BackSegurosChubb.Interface;
using BackSegurosChubb.Models;
using BackSegurosChubb.ViewModel;

namespace BackSegurosChubb.Service
{
    public class PersonaService : IPersona
    {
        PruebaSegurosChubbContext _context;
        public PersonaService(PruebaSegurosChubbContext context) { 
            _context = context;
        }

        public bool EsCedulaValido(string cedula)
        {
            return cedula.Length == 10;
        }

        public bool SetPersona(PersonaVM persona)
        {
            if (EsCedulaValido(persona.Cedula))
            {
                var existe = _context.Personas.Where(x => x.Cedula == persona.Cedula).Any();
                bool registrado = true;
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
                        catch (Exception ex)
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
            //var persona = _context.Personas.ToList();
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
                        catch (Exception ex)
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
                catch (Exception ex)
                {
                    context.Rollback();
                    eliminado = false;
                }
            }
            return eliminado;
        }


    }
}
