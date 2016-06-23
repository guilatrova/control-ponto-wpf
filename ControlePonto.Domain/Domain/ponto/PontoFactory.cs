using ControlePonto.Domain.ponto.trabalho;
using ControlePonto.Domain.ponto.folga;
using ControlePonto.Domain.services.login;
using ControlePonto.Domain.usuario.funcionario;
using System;
using ControlePonto.Infrastructure.utils;
using ControlePonto.Domain.feriado;

namespace ControlePonto.Domain.ponto
{
    public class PontoFactory
    {
        private IPontoDiaRepository repository;
        private FeriadoService feriadoService;

        public PontoFactory(IPontoDiaRepository repository, FeriadoService feriadoService)
        {
            this.repository = repository;
            this.feriadoService = feriadoService;
        }

        public DiaTrabalho criarDiaTrabalho(IDataHoraStrategy dataHoraStrategy, SessaoLogin sessaoLogin)
        {
            DateTime dt = dataHoraStrategy.getDataHoraAtual();
            if (repository.existePontoDia(sessaoLogin.UsuarioLogado as Funcionario, dt))
                throw new PontoDiaJaExisteException(dt);

            if (feriadoService.isFeriado(dt))
                return new DiaTrabalhoFeriado(feriadoService.getFeriado(dt), dt.TimeOfDay, sessaoLogin.UsuarioLogado as Funcionario);

            return new DiaTrabalhoComum(dt.Date, dt.TimeOfDay, sessaoLogin.UsuarioLogado as Funcionario);
        }

        public DiaFolga criarDiaFolga(Funcionario funcionario, DateTime data, string descricao)
        {
            Check.Require(!string.IsNullOrWhiteSpace(descricao), "A descrição deve ser válida");

            if (repository.existePontoDia(funcionario, data))
                throw new PontoDiaJaExisteException(data);
                        
            return new DiaFolga(funcionario, data, descricao);
        }
    }
}
