using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.services.ponto
{
    public class PontoService
    {
        private PontoFactory pontoFactory;
        private SessaoLogin sessaoLogin;
        private IDataHoraStrategy dataHoraStrategy;
        private FuncionarioPossuiPontoAbertoSpecification deixouPontoAberto;
        private FuncionarioJaTrabalhouHojeSpecification jaTrabalhouHoje;

        public PontoService(PontoFactory pontoFactory, IDataHoraStrategy dataHoraStrategy, FuncionarioPossuiPontoAbertoSpecification pontoAbertoSpec,  FuncionarioJaTrabalhouHojeSpecification funcTrabSpec, SessaoLogin sessaoLogin)
        {
            this.pontoFactory = pontoFactory;
            this.dataHoraStrategy = dataHoraStrategy;
            this.deixouPontoAberto = pontoAbertoSpec;
            this.jaTrabalhouHoje = funcTrabSpec;
            this.jaTrabalhouHoje.Data = dataHoraStrategy.getDataHoraAtual();
            this.sessaoLogin = sessaoLogin;
        }

        public PontoDia iniciarDia()
        {            
            if (deixouPontoAberto.IsSatisfiedBy((Funcionario)sessaoLogin.UsuarioLogado))
                throw new DiaEmAbertoException(deixouPontoAberto.PontoDiaAbertoEncontrado);

            if (jaTrabalhouHoje.IsSatisfiedBy((Funcionario)sessaoLogin.UsuarioLogado))
                throw new PontoDiaJaExisteException(jaTrabalhouHoje.Data);

            return pontoFactory.criarPonto(dataHoraStrategy, sessaoLogin);
        }

        public void encerrarDia(PontoDia ponto)
        {
            if (ponto.algumIntervaloEmAberto())
                throw new IntervaloEmAbertoException(ponto.getIntervaloEmAberto());            

            ponto.Fim = dataHoraStrategy.getDataHoraAtual().TimeOfDay;
        }
    }
}
