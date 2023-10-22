using BackSegurosChubb.ViewModel;

namespace BackSegurosChubb.Interface
{
    public interface ISeguro
    {
        #region Seguros
        bool SetSeguros(SeguroVM seguro);
        List<SeguroVM> GetAllSeguro();
        SeguroVM GetSeguroById(int id);
        SeguroVM GetSeguroByCode(string codigo);
        bool UpdateSeguro(SeguroVM seguroVM);
        bool DeleteSeguro(int id);
        #endregion

        #region Polizas
        bool SetPoliza(SetPolizas setPolizas);
        List<PolizaVM> GetAllPolizas(string cedula, string Codigo);
        #endregion
    }
}
