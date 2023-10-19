using BackSegurosChubb.Interface;
using BackSegurosChubb.Models;

namespace BackSegurosChubb.Service
{
    public class PersonaService : IPersona
    {
        PruebaSegurosChubbContext _context;
        public PersonaService(PruebaSegurosChubbContext context) { 
            _context = context;
        }


    }
}
