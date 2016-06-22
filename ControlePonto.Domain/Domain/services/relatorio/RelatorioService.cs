using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlePonto.Domain.framework;

namespace ControlePonto.Domain.services.relatorio
{
    public class RelatorioService
    {
        private IPontoDiaRepository pontoRepository;

        public RelatorioService(ponto.IPontoDiaRepository pontoRepository)
        {
            this.pontoRepository = pontoRepository;
        }

        public CalendarioPonto gerarCalendario(Funcionario funcionario, DateTime inicio, DateTime fim)
        {
            var todosPontos = pontoRepository.findPontosNoIntervalo(funcionario, inicio, fim, true, false);            
            var diasFaltando = inicio.Range(fim).Except(todosPontos.Select(x => x.Data));

            var todosDias = todosPontos
                .Select(x => new DiaCalendarioPonto(x))
                .Concat(diasFaltando
                    .Select(x => new DiaCalendarioPonto(x))
                )
                .OrderBy(x => x.Data)
                .ToList();                        

            return new CalendarioPonto(funcionario, inicio, fim, todosDias);
        }
    }
}
