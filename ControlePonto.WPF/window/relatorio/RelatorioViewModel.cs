using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ControlePonto.WPF.window.relatorio
{
    public class RelatorioViewModel : ViewModelBase
    {
        private IUsuarioRepositorio usuarioRepository;
        private RelatorioService relatorioService;

        public RelatorioViewModel(IUsuarioRepositorio usuarioRepository, RelatorioService relatorioService)
        {
            this.usuarioRepository = usuarioRepository;
            this.relatorioService = relatorioService;

            this.Funcionarios = usuarioRepository.findFuncionarios();
            this.FuncionarioEscolhido = Funcionarios.FirstOrDefault();

            var today = DateTime.Today;
            this.PeriodoInicio = new DateTime(today.Year, today.Month, 1);
            this.PeriodoFim = PeriodoInicio.AddMonths(1).AddDays(-1);

            this.ExibirCommand = new RelayCommand(exibir);
        }

        #region Propriedades

        public List<Funcionario> Funcionarios { get; private set; }

        public Funcionario FuncionarioEscolhido { get; set; }

        public DateTime PeriodoInicio { get; private set; }

        public DateTime PeriodoFim { get; private set; }

        public ICommand ExibirCommand { get; private set; }

        private List<DiaRelatorioDTO> _dias;
        public List<DiaRelatorioDTO> Dias
        {
            get { return _dias; }
            set 
            { 
                SetField(ref _dias, value);
            }
        }
        

        #endregion
        
        private void exibir()
        {
            Dias = relatorioService
                .gerarRelatorio(FuncionarioEscolhido, PeriodoInicio, PeriodoFim).Dias
                .Select(x => new DiaRelatorioDTO(x))
                .ToList();
        }

        protected override string validar(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
