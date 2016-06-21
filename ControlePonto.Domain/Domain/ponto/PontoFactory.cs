using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public class PontoFactory
    {
        private IPontoDiaRepository repository;

        public PontoFactory(IPontoDiaRepository repository)
        {
            this.repository = repository;
        }

        public PontoDia criarPonto(IDataHoraStrategy dataHoraStrategy, SessaoLogin sessaoLogin)
        {
            DateTime dt = dataHoraStrategy.getDataHoraAtual();
            if (repository.existePontoDia(sessaoLogin.UsuarioLogado as Funcionario, dt))
                throw new PontoDiaJaExisteException(dt);

            return new PontoDia(dt.Date, dt.TimeOfDay, sessaoLogin.UsuarioLogado as Funcionario);
        }

        public PontoDia criarDiaFolga(Funcionario funcionario, DateTime data)
        {
            if (repository.existePontoDia(funcionario, data))
                throw new PontoDiaJaExisteException(data);

            return PontoDia.criarComoDiaFolga(data, funcionario);
        }
    }
}
