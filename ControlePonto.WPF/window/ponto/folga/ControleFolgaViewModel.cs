using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.window.ponto.folga
{
    public class ControleFolgaViewModel : ViewModelBase
    {
        private IUsuarioRepositorio usuarioRepository;
        private IPontoDiaRepository pontoRepository;

        public ControleFolgaViewModel(IUsuarioRepositorio usuarioRep, IPontoDiaRepository pontoRep)
        {
            this.usuarioRepository = usuarioRep;
            this.pontoRepository = pontoRep;
        }

        #region Propriedades

        public List<Funcionario> Funcionarios{ get; private set; }

        private DateTime _periodoInicio;
        public DateTime PeriodoInicio
        {
            get { return _periodoInicio; }
            set 
            {
                SetField(ref _periodoInicio, value); 
            }
        }

        private DateTime _periodoFim;
        public DateTime PeriodoFim
        {
            get { return _periodoFim; }
            set
            {
                SetField(ref _periodoFim, value);
            }
        }

        public ICommand ExibirCommand { get; private set; }

        public List<object> CalendarioFolgas { get; private set; }

        #endregion

        protected override string validar(string propertyName)
        {
            return null;
        }
    }
}
