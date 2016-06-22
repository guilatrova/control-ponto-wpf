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
        private PontoService pontoService;

        private List<DiaFolgaDTO> diasAlterados;

        public ControleFolgaViewModel(IUsuarioRepositorio usuarioRep, RelatorioService relatorioService)
        {
            this.usuarioRepository = usuarioRep;
            this.relatorioService = relatorioService;

            var today = DateTime.Today;
            this.PeriodoInicio = new DateTime(today.Year, today.Month, 1);
            this.PeriodoFim = PeriodoInicio.AddMonths(1).AddDays(-1);

            this.Funcionarios = usuarioRepository.findFuncionarios().OrderBy(x => x.Nome).ToList();
            this.FuncionarioEscolhido = Funcionarios[0];

            diasAlterados = new List<DiaFolgaDTO>();

            ExibirCommand = new RelayCommand(exibir);
            SalvarCommand = new RelayCommand(salvar, podeSalvar);
            FecharCommand = new RelayCommand(() => requestView(CLOSE));
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

        private List<DiaFolgaDTO> DiasPeriodo { get; set; }

        private List<DiaFolgaDTO> _diasPeriodoFiltro;
        public List<DiaFolgaDTO> DiasPeriodoFiltro
        {
            get { return _diasPeriodoFiltro; }
            private set { SetField(ref _diasPeriodoFiltro, value); }
        }

        public ICommand ExibirCommand { get; private set; }

        public ICommand SalvarCommand { get; private set; }

        public ICommand FecharCommand { get; private set; }

        #endregion

        private void exibir()
        {
            DiasPeriodo = relatorioService
                .gerarCalendario(FuncionarioEscolhido, PeriodoInicio, PeriodoFim).Dias
                .Select(x => new DiaFolgaDTO(x))
                .ToList();
                        
            DiasPeriodo.ForEach(x => x.PropertyChanged += DiaFolgaChanged);
            aplicarFiltro(ExibirSomenteFolgas);
            RaisePropertyChanged("DiasPeriodoFiltro");
        }

        private void DiaFolgaChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            diasAlterados.Add(sender as DiaFolgaDTO);
        }

        private void aplicarFiltro(bool somenteFolga)
        {
            if (somenteFolga)
                DiasPeriodoFiltro = DiasPeriodo.Where(x => x.IsDiaFolga).ToList();
            else
                DiasPeriodoFiltro = DiasPeriodo;
        }

        private void salvar()
        {
            foreach (DiaFolgaDTO folga in diasAlterados)
            {
                pontoService.darFolgaPara(FuncionarioEscolhido, folga.Data, folga.Descricao);
            }
        }

        private bool podeSalvar()
        {
            return (diasAlterados.Count > 0) ;
        }

        protected override string validar(string propertyName)
        {
            return null;
        }
    }
}
