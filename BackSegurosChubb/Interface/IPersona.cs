using BackSegurosChubb.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BackSegurosChubb.Interface
{
    public interface IPersona
    {
        #region Persona
        bool SetPersona(PersonaVM persona);
        List<PersonaVM> GetAllPersona();
        PersonaVM GetPersonaByCedula(string cedula);
        bool UpdatePersona(PersonaVM persona);
        bool DeletePersona(int idPersona);
        #endregion

        #region Excel
        List<PersonaVM> setArchivoExcel(ExcelVM ArchivoExcel);
        #endregion



    }
}
