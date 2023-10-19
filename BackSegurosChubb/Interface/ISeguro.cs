using BackSegurosChubb.ViewModel;

namespace BackSegurosChubb.Interface
{
    public interface ISeguro
    {
        bool SetSeguros(SeguroVM seguro);
        List<SeguroVM> GetAllSeguro();
        SeguroVM GetSeguroById(int id);
        SeguroVM GetSeguroByCode(string codigo);
    }
}
