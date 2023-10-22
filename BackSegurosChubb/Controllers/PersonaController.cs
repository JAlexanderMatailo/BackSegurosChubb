using BackSegurosChubb.Interface;
using BackSegurosChubb.Service;
using BackSegurosChubb.ViewModel;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

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

        #region Persona
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
        [HttpGet("GetPersonaByCedula")]
        public IActionResult GetPersonaByCedula(string cedula)
        {
            var result = _persona.GetPersonaByCedula(cedula);
            return new JsonResult(result);
        }

        [HttpPost("UpdatePersona")]
        public IActionResult UpdatePersona(PersonaVM persona)
        {
            var result = _persona.UpdatePersona(persona);
            return new JsonResult(result);
        }
        [HttpPost("DeletePersona")]
        public IActionResult DeletePersona(int id)
        {
            var result = _persona.DeletePersona(id);
            return new JsonResult(result);
        }
        #endregion

        #region Excel

        [HttpPost("SetExcel")]
        public IActionResult SetExcel( ExcelVM ArchivoExcel)
        {
            var result = _persona.setArchivoExcel(ArchivoExcel);
            return new JsonResult(result);
        }
        #endregion
    }

}

