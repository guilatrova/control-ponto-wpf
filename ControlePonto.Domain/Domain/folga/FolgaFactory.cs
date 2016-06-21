using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.folga
{
    public class FolgaFactory
    {
        private IFolgaRepository folgaRepository;
        private PontoFactory pontoFactory;        

        public FolgaFactory(IFolgaRepository repository, PontoFactory pontoFactory)
        {
            this.pontoFactory = pontoFactory;
            this.folgaRepository = repository;
        }

        public Folga criarFolga(Funcionario funcionario, DateTime data, string descricao)
        {
            Check.Require(!string.IsNullOrWhiteSpace(descricao), "A descrição deve ser válida");

            var folga = folgaRepository.findFolgaForDate(funcionario, data);
            if (folga != null)
                throw new FolgaJaExisteException(funcionario, data, folga.Descricao);

            var ponto = pontoFactory.criarDiaFolga(funcionario, data);

            return new Folga(funcionario, ponto, descricao.Trim());
        }
    }
}
