using BackSegurosChubb.Interface;
using BackSegurosChubb.Models;
using BackSegurosChubb.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BackSegurosChubb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguroController : Controller
    {
        private readonly ISeguro _seguro;
        public SeguroController(ISeguro seguro)
        {
            _seguro = seguro;
        }
        #region Seguro
        [HttpPost("SetSeguro")]
        public IActionResult SetSeguro(SeguroVM seguro) { 
            var result = _seguro.SetSeguros(seguro);
            return new JsonResult(result);
        }

        [HttpPost("UpdateSeguro")]
        public IActionResult UpdateSeguro(SeguroVM seguro)
        {
            var result = _seguro.UpdateSeguro(seguro);
            return new JsonResult(result);
        }

        [HttpGet("GetAllSeguro")]
        public IActionResult GetAllSeguro() { 
            var result = _seguro.GetAllSeguro();
            return new JsonResult(result);
        }

        [HttpGet("GetSeguroById")]
        public IActionResult GetSeguroById(int id)
        {
            var result = _seguro.GetSeguroById(id);
            return new JsonResult(result);
        }

        [HttpGet("GatSeguroByCode")]
        public IActionResult GetSeguroByCode(string codigo)
        {
            var result = _seguro.GetSeguroByCode(codigo);
            return new JsonResult(result);
        }

        [HttpPost("DeleteSeguro")]
        public IActionResult DeleteSeguro(int id)
        {
            var result = _seguro.DeleteSeguro(id);
            return new JsonResult(result);
        }

        #endregion

        #region Polizas
        [HttpPost("SetPoliza")]
        public IActionResult SetPoliza(SetPolizas setPolizas)
        {
            //var result = _seguro.SetPoliza(setPolizas.idAsegurados, 0);
            var result = _seguro.SetPoliza(setPolizas);
            return new JsonResult(result);
        }

        [HttpPost("GetAllPolizas")]
        public IActionResult GetAllPolizas(string? cedula, string? Codigo)
        {
            if (cedula == null)
            {
                cedula = string.Empty;
                var result = _seguro.GetAllPolizas(cedula, Codigo);
                return new JsonResult(result);
            }

            return new JsonResult("");
        }
        #endregion
    }
}
