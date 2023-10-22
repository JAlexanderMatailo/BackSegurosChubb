using BackSegurosChubb.Interface;
using BackSegurosChubb.Models;
using BackSegurosChubb.ViewModel;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.IO;

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
            var result = _seguro.SetPoliza(setPolizas);
            return new JsonResult(result);
        }

        [HttpPost("GetAllPolizas")]
        public IActionResult GetAllPolizas(string? cedula, string? Codigo)
        {
            if (cedula == null)
            {
                cedula = string.Empty;                
            }
            var result = _seguro.GetAllPolizas(cedula, Codigo);
            return new JsonResult(result);

        }

        #endregion

        [HttpGet("GetFormato")]
        public IActionResult GetFormato()
        {
            var result = ConvertExcelToBase64();
            return new JsonResult(result);
        }

        private  string ConvertExcelToBase64()
        {

            string base64 = String.Empty;
            
            string path = Path.GetFullPath("../BackSegurosChubb/Formato/ExcelPrueba.xlsx");

            byte[] docBytes = null;

            using (StreamReader strm = new StreamReader(path))
            {
                Stream s = strm.BaseStream;
                BinaryReader r = new BinaryReader(s);
                docBytes = r.ReadBytes(Convert.ToInt32(r.BaseStream.Length));
                base64 = Convert.ToBase64String(docBytes);
                r.Close();
                s.Close();
                strm.Close();
            }
            return base64;

        }
    }
}
