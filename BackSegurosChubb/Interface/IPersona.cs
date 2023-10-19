using BackSegurosChubb.ViewModel;

namespace BackSegurosChubb.Interface
{
    public interface IPersona
    {
        bool SetPersona(PersonaVM persona);
        List<PersonaVM> GetAllPersona();
        PersonaVM GetPersonaByCedula(string cedula);
        bool UpdatePersona(PersonaVM persona);
        bool DeletePersona(int idPersona);
    }
}
