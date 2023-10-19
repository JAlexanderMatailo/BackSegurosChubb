using BackSegurosChubb.Interface;
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

        #endregion
    }
}
