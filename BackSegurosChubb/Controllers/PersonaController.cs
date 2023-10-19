using BackSegurosChubb.Interface;
using BackSegurosChubb.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BackSegurosChubb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : Controller
    {
        private readonly IPersona _persona;

        public PersonaController(IPersona persona)
        {
            _persona = persona;
        }
        [HttpPost("SetPersona")]
        public IActionResult SetPersona(PersonaVM persona)
        {
            var result = _persona.SetPersona(persona);
            return new JsonResult(result);
        }

        [HttpGet("GetAllPersona")]
        public IActionResult GetAllPersona()
        {
            var result = _persona.GetAllPersona();
            return new JsonResult(result);
        }

    }
}
