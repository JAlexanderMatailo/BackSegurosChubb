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
                var existe = _context.Personas.Where(x => x.IdAsegurados == persona.IdAsegurados).Any();
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





        
    }
}
