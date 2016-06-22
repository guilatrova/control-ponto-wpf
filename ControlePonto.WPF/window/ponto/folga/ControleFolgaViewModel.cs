using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.relatorio;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace ControlePonto.WPF.window.ponto.folga
{
    public class ControleFolgaViewModel : ViewModelBase
    {
        private IUsuarioRepositorio usuarioRepository;
        private RelatorioService relatorioService;

        public ControleFolgaViewModel(IUsuarioRepositorio usuarioRep, RelatorioService relatorioService)
        {
            this.usuarioRepository = usuarioRep;
            this.relatorioService = relatorioService;

            var today = DateTime.Today;
            this.PeriodoInicio = new DateTime(today.Year, today.Month, 1);
            this.PeriodoFim = PeriodoInicio.AddMonths(1).AddDays(-1);

            this.Funcionarios = usuarioRepository.findFuncionarios().OrderBy(x => x.Nome).ToList();
            this.FuncionarioEscolhido = Funcionarios[0];

            ExibirCommand = new RelayCommand(exibir);
        }

        #region Propriedades

        public List<Funcionario> Funcionarios{ get; private set; }
        public Funcionario FuncionarioEscolhido { get; set; }

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
        
        private bool _exibirSomenteFolgas;
        public bool ExibirSomenteFolgas
        {
            get { return _exibirSomenteFolgas; }
            set 
            { 
                if (SetField(ref _exibirSomenteFolgas, value) && DiasPeriodo != null)
                {
                    aplicarFiltro(value);
                }
            }
        }

        public ICommand ExibirCommand { get; private set; }

        private List<DiaFolgaDTO> DiasPeriodo { get; set; }

        private List<DiaFolgaDTO> _diasPeriodoFiltro;
        public List<DiaFolgaDTO> DiasPeriodoFiltro
        {
            get { return _diasPeriodoFiltro; }
            private set { SetField(ref _diasPeriodoFiltro, value); }
        }
        

        #endregion

        private void exibir()
        {
            DiasPeriodo = relatorioService
                .gerarCalendario(FuncionarioEscolhido, PeriodoInicio, PeriodoFim).Dias
                .Select(x => new DiaFolgaDTO(x))
                .ToList();

            aplicarFiltro(ExibirSomenteFolgas);
            RaisePropertyChanged("DiasPeriodoFiltro");
        }

        private void aplicarFiltro(bool somenteFolga)
        {
            if (somenteFolga)
                DiasPeriodoFiltro = DiasPeriodo.Where(x => x.IsDiaFolga).ToList();
            else
                DiasPeriodoFiltro = DiasPeriodo;
        }

        protected override string validar(string propertyName)
        {
            return null;
        }
    }
}
