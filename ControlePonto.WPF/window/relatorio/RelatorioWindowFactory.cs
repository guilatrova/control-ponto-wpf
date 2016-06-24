﻿using ControlePonto.Domain.factories;
using ControlePonto.Domain.factories.services;
using ControlePonto.Domain.services.relatorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.WPF.window.relatorio
{
    public static class RelatorioWindowFactory
    {
        public static RelatorioWindow criarRelatorioWindow()
        {
            return new RelatorioWindow(new RelatorioViewModel(
                RepositoryFactory.criarUsuarioRepository(),
                RelatorioServiceFactory.criarRelatorioService()
            ));
        }
    }
}
