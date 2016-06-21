using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using System;
using ControlePonto.Infrastructure.utils;

namespace ControlePonto.Domain.ponto
{
    public class PontoFactory
    {
        private IPontoDiaRepository repository;

        public PontoFactory(IPontoDiaRepository repository)
        {
            this.repository = repository;
        }

        public DiaTrabalho criarDiaTrabalho(IDataHoraStrategy dataHoraStrategy, SessaoLogin sessaoLogin)
        {
            DateTime dt = dataHoraStrategy.getDataHoraAtual();
            if (repository.existePontoDia(sessaoLogin.UsuarioLogado as Funcionario, dt))
                throw new PontoDiaJaExisteException(dt);

            return new DiaTrabalho(dt.Date, dt.TimeOfDay, sessaoLogin.UsuarioLogado as Funcionario);
        }

        public PontoDia criarDiaFolga(Funcionario funcionario, DateTime data, string descricao)
        {
            Check.Require(!string.IsNullOrWhiteSpace(descricao), "A descrição deve ser válida");

            if (repository.existePontoDia(funcionario, data))
                throw new PontoDiaJaExisteException(data);
                        
            return new DiaFolga(funcionario, data, descricao);
        }
    }
}
