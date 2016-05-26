using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario;
using ControlePonto.WPF.framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ControlePonto.WPF.window.usuario
{
    public class LoginViewModel : ViewModelBase
    {
        private ILoginService loginService;
        private IUsuarioRepositorio usuarioRepositorio;

        private ICommand _verificarExisteUsuarioJaCadastradoCommand;
        private ICommand _logarCommand;

        public const int VIEW_CADASTRAR_USUARIO = 1;
        public const int VIEW_CRIAR_SENHA = 2;

        public LoginViewModel(ILoginService loginService, IUsuarioRepositorio usuarioRepositorio)
        {
            this.loginService = loginService;
            this.usuarioRepositorio = usuarioRepositorio;

            _verificarExisteUsuarioJaCadastradoCommand = new RelayCommand(verificarExisteUsuarioJaCadastrado);
            _logarCommand = new RelayParameterCommand<System.Windows.Controls.PasswordBox>(logar, base.isModelValid);
        }

        #region Propriedades

        private string _login;
        public string Login
        {
            get { return _login; }
            set { SetField(ref _login, value); }
        }

        public ICommand VerificarExisteUsuarioJaCadastradoCommand { get { return _verificarExisteUsuarioJaCadastradoCommand; } }

        public ICommand LogarCommand { get { return _logarCommand; } }
        
        #endregion
        
        private void verificarExisteUsuarioJaCadastrado()
        {
            if (!alguemJaCadastrou())
            {
                requestView(VIEW_CADASTRAR_USUARIO);
            }
        }

        private void logar(System.Windows.Controls.PasswordBox pbox)
        {
            try
            {
                string senha = pbox.Password;
                Usuario usuario = loginService.Logar(Login, senha);
                if (precisaCriarSenha(usuario))
                {
                    requestView(VIEW_CRIAR_SENHA);
                }
            }
            catch (LoginInvalidoException)
            {
                showMessageBox("Não foi possível efetuar login", "Login inválido", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        protected override string validar(string propertyName)
        {
            switch (propertyName)
            {
                case "Login":
                    if (string.IsNullOrWhiteSpace(Login))
                        return "O login é obrigatório";
                    break;
            }
            return null;
        }

        private bool alguemJaCadastrou()
        {
            return (usuarioRepositorio.getCount() > 0);            
        }

        private bool precisaCriarSenha(Usuario usuario)
        {
            return (string.IsNullOrEmpty(usuario.Senha));
        }
    }
}
