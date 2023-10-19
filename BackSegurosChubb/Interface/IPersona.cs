using BackSegurosChubb.ViewModel;

namespace BackSegurosChubb.Interface
{
    public interface IPersona
    {
        bool SetPersona(PersonaVM persona);
        List<PersonaVM> GetAllPersona();
    }
}
