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
        #endregion
    }
}
