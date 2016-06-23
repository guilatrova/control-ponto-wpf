﻿using ControlePonto.Domain.ponto;
using ControlePonto.Domain.usuario.funcionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlePonto.Domain.framework;
using ControlePonto.Domain.feriado;

namespace ControlePonto.Domain.services.relatorio
{
    public class RelatorioService
    {
        private IPontoDiaRepository pontoRepository;
        private FeriadoService feriadoService;

        public RelatorioService(ponto.IPontoDiaRepository pontoRepository, FeriadoService feriadoService)
        {
            this.pontoRepository = pontoRepository;
            this.feriadoService = feriadoService;
        }

        public CalendarioPonto gerarCalendario(Funcionario funcionario, DateTime inicio, DateTime fim)
        {
            var todosPontos = pontoRepository.findPontosNoIntervalo(funcionario, inicio, fim, true, false);
            var diasFaltando = inicio.Range(fim).Except(todosPontos.Select(x => x.Data));
            var feriadosNaoTrabalhados = diasFaltando.Where(x => feriadoService.isFeriado(x));

            //Se é um feriado, não deve ser contado como falta
            diasFaltando = diasFaltando.Except(feriadosNaoTrabalhados);

            var todosDias = todosPontos
                .Select(x => criarDia(x))
                .Concat(
                    diasFaltando.Select(x => criarDia(x))
                )
                .Concat(feriadosNaoTrabalhados
                    .Select(x => 
                        criarDia(feriadoService.getFeriado(x))
                    )
                )
                .OrderBy(x => x.Data)
                .ToList();                        

            return new CalendarioPonto(funcionario, inicio, fim, todosDias);
        }

        private DiaCalendario criarDia(DateTime date)
        {
            return new DiaCalendarioFalta(date);
        }

        private DiaCalendario criarDia(PontoDia ponto)
        {
            return new DiaCalendarioPonto(ponto);
        }

        private DiaCalendario criarDia(Feriado feriado)
        {
            return new DiaCalendarioFeriado(feriado);
        }
    }
}
