using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.ponto;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public class PontoService
    {
        protected PontoFactory pontoFactory;
        protected SessaoLogin sessaoLogin;
        protected IDataHoraStrategy dataHoraStrategy;
        protected IPontoDiaRepository pontoRepository;
        protected FuncionarioPossuiPontoAbertoSpecification deixouPontoAberto;
        protected FuncionarioJaTrabalhouHojeSpecification jaTrabalhouHoje;

        public PontoService(PontoFactory pontoFactory, IDataHoraStrategy dataHoraStrategy, FuncionarioPossuiPontoAbertoSpecification pontoAbertoSpec,  FuncionarioJaTrabalhouHojeSpecification funcTrabSpec, SessaoLogin sessaoLogin, IPontoDiaRepository pontoRepository)
        {
            this.pontoFactory = pontoFactory;
            this.dataHoraStrategy = dataHoraStrategy;
            this.deixouPontoAberto = pontoAbertoSpec;
            this.jaTrabalhouHoje = funcTrabSpec;
            this.jaTrabalhouHoje.Data = dataHoraStrategy.getDataHoraAtual();
            this.sessaoLogin = sessaoLogin;
            this.pontoRepository = pontoRepository;
        }

        public DiaTrabalho iniciarDia()
        {            
            if (deixouPontoAberto.IsSatisfiedBy((Funcionario)sessaoLogin.UsuarioLogado))
                throw new DiaEmAbertoException(deixouPontoAberto.PontoDiaAbertoEncontrado);

            if (jaTrabalhouHoje.IsSatisfiedBy((Funcionario)sessaoLogin.UsuarioLogado))
                throw new PontoDiaJaExisteException(jaTrabalhouHoje.Data);

            var ponto = pontoFactory.criarDiaTrabalho(dataHoraStrategy, sessaoLogin);
            pontoRepository.save(ponto);
            return ponto as DiaTrabalho;
        }

        public void encerrarDia(DiaTrabalho ponto)
        {
            if (ponto.algumIntervaloEmAberto())
                throw new IntervaloEmAbertoException(ponto.getIntervaloEmAberto());            

            ponto.Fim = dataHoraStrategy.getDataHoraAtual().TimeOfDay;
            pontoRepository.save(ponto);
        }

        public DiaTrabalho recuperarPontoAbertoFuncionario(Funcionario funcionario)
        {
            return pontoRepository.findPontoAberto(funcionario, dataHoraStrategy.getDataHoraAtual());
        }

        public void registrarIntervalo(TipoIntervalo tipoIntervalo, DiaTrabalho ponto)
        {
            ponto.registrarIntervalo(tipoIntervalo, dataHoraStrategy);
            pontoRepository.save(ponto);
        }

        public DiaFolga darFolgaPara(Funcionario funcionario, DateTime data, string descricao)
        {
            if (data.Date < DateTime.Today)
                throw new FolgaDiaInvalidoException(data);

            var folga = pontoFactory.criarDiaFolga(funcionario, data, descricao);
            pontoRepository.save(folga);

            return folga;
        }
    }
}
