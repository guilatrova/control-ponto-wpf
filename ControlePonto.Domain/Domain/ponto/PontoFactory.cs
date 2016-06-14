using ControlePonto.Domain.services.login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public class PontoFactory
    {
        public PontoDia criarPonto(IDataHoraStrategy dataHoraStrategy, SessaoLogin sessaoLogin)
        {
            DateTime dt = dataHoraStrategy.getDataHoraAtual();

            return new PontoDia(dt.Date, dt.TimeOfDay, sessaoLogin.UsuarioLogado);
        }
    }
}
