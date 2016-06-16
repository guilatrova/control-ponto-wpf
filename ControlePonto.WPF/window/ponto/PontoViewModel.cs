﻿using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.ponto
{
    public class PontoViewModel : ViewModelBase
    {
        private Funcionario funcionario;
        private PontoDia ponto;
        private PontoService pontoService;
        private ITipoIntervaloRepository tipoIntervaloRepository;

        public const int CLOSE_VIEW = -1;

        private ICommand _encerrarDiaCommand;
        private ICommand _entrarIntervaloCommand;
        private ICommand _sairIntervaloCommand;

        public PontoViewModel(Funcionario funcionario, PontoDia ponto, PontoService pontoService, ITipoIntervaloRepository tipoRepository)
        {
            this.funcionario = funcionario;
            this.ponto = ponto;
            this.pontoService = pontoService;
            this.tipoIntervaloRepository = tipoRepository;            

            _encerrarDiaCommand = new RelayCommand(confirmarEncerrarDia);
            _entrarIntervaloCommand = new RelayParameterEvaluatorCommand<TipoIntervalo>(registrarIntervalo, podeEntrarIntervalo);
            _sairIntervaloCommand = new RelayParameterEvaluatorCommand<TipoIntervalo>(registrarIntervalo, podeSairIntervalo);
        }

        #region Propriedades

        public string Titulo
        {
            get
            {
                return string.Format("Ponto de {0}", funcionario.Nome);
            }
        }

        public string DataHojeHeader
        {
            get
            {
                return DateTime.Today.ToShortDateString();
            }
        }

        public string Entrada
        {
            get
            {
                return string.Format("ENTRADA: {0}", ponto.Inicio);
            }
        }

        private List<TipoIntervalo> _intervalos;

        public List<TipoIntervalo> Intervalos
        {
            get 
            {
                if (_intervalos == null)
                    _intervalos = tipoIntervaloRepository.findAll();
                return _intervalos; 
            }            
        }

        public ICommand EncerrarDiaCommand { get { return _encerrarDiaCommand; } }
        public ICommand EntrarIntervaloCommand { get { return _entrarIntervaloCommand; } }
        public ICommand SairIntervaloCommand { get { return _sairIntervaloCommand; } }
        

        #endregion

        private void registrarIntervalo(TipoIntervalo tipoIntervalo)
        {
            pontoService.registrarIntervalo(tipoIntervalo, ponto);
        }
        
        private bool podeEntrarIntervalo(TipoIntervalo tipoIntervalo)
        {
            return !ponto.intervaloRegistrado(tipoIntervalo);
        }

        private bool podeSairIntervalo(TipoIntervalo tipoIntervalo)
        {
            if (ponto.intervaloRegistrado(tipoIntervalo))
            {
                var intervalo = ponto.getIntervalo(tipoIntervalo);
                return !intervalo.Saida.HasValue; //Se não houver saída, então pode registrar
            }
            return false;
        }

        private void confirmarEncerrarDia()
        {
            showMessageBox(encerrarDia, "Tem certeza que deseja encerrar sua jornada de trabalho hoje?",
                "Encerrar jornada de trabalho",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning,
                System.Windows.MessageBoxResult.No);
        }

        private void encerrarDia(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    pontoService.encerrarDia(ponto);
                    requestView(CLOSE_VIEW);
                }
                catch (Exception e)
                {
                    showMessageBox(e.Message, "Não foi possível encerrar", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        protected override string validar(string propertyName)
        {
            return null;
        }
    }
}
