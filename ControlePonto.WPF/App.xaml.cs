using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.services.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.WPF.window.jornada;
using ControlePonto.WPF.window.ponto;
using ControlePonto.WPF.window.usuario;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ControlePonto.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //JornadaWindowFactory.criarJornadaWindow().Show();
            //TODO Volta pra login            
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            tratarArgumentos(e.Args);

            var loginWindow = UsuarioWindowFactory.criarLoginWindow();
            var loginResult = loginWindow.ShowDialog();
            if (loginResult.HasValue && loginResult.Value)
            {                
                try
                {
                    var pontoService = PontoServiceFactory.criarPontoService();
                    var ponto = recuperarOuIniciarPonto(pontoService);
                    PontoWindowFactory.criarPontoWindow(ponto, pontoService).ShowDialog();                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Não foi possível completar a operação", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            Current.Shutdown();
        }

        private PontoDia recuperarOuIniciarPonto(PontoService pontoService)
        {
            var ponto = pontoService.recuperarPontoAbertoFuncionario(SessaoLogin.getSessao().UsuarioLogado as Funcionario);
            if (ponto == null)
                return pontoService.iniciarDia();
            return ponto;
        }

        private void tratarArgumentos(string[] args)
        {
            if (args.Length == 1)
            {
                Task.Factory.StartNew(() => aplicarHost(args[0]));
            }
        }

        private void aplicarHost(string host)
        {
            try
            {
                ControlePonto.Infrastructure.nhibernate.NHibernateHelper.Host = host;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não foi possível conectar-se ao banco de dados", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
