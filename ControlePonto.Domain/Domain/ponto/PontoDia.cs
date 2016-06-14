using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.usuario;
using ControlePonto.Domain.usuario.funcionario;
using ControlePonto.Infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Domain.ponto
{
    public class PontoDia : Entity<ulong>
    {
        public DateTime Data { get; private set; }
        public TimeSpan Inicio { get; private set; }

        private TimeSpan? _Fim;
        public TimeSpan? Fim 
        {
            get
            {
                return _Fim;
            }
            set
            {
                Check.Require(!_Fim.HasValue);
                _Fim = value;
                isAberto = false;
            }
        }

        public Usuario Usuario { get; private set; }
        public bool isFeriado { get; private set; }
        public bool isAberto { get; private set; }

        public List<Intervalo> Intervalos { get; set; }

        public PontoDia(DateTime data, TimeSpan inicio, Usuario usuario)
        {
            base.checkPreConstructor();

            Data = data;
            Inicio = inicio;
            Usuario = usuario;
            isAberto = true;
            Intervalos = new List<Intervalo>();
        }

        public Intervalo getIntervalo(TipoIntervalo tipoIntervalo)
        {
            try
            {
                return Intervalos.Single(x => x.TipoIntervalo.Nome.Equals(tipoIntervalo.Nome));
            }
            catch (InvalidOperationException)
            {
                throw new IntervaloNaoRegistradoException(tipoIntervalo);
            }
        }

        public void registrarIntervalo(TipoIntervalo tipoIntervalo, IDataHoraStrategy dataHoraStrategy)
        {
            if (Intervalos.Exists(x => x.TipoIntervalo.Nome.Equals(tipoIntervalo.Nome)))
            {
                var intervalo = getIntervalo(tipoIntervalo);
                if (intervalo.Saida.HasValue)
                    throw new IntervaloJaRegistradoException(tipoIntervalo);

                intervalo.Saida = dataHoraStrategy.getDataHoraAtual().TimeOfDay;
            }
            else
            {
                Intervalos.Add(new Intervalo(tipoIntervalo, dataHoraStrategy.getDataHoraAtual().TimeOfDay));
            }
        }

        public bool algumIntervaloEmAberto()
        {
            return Intervalos.Any(x => !x.Saida.HasValue);
        }

        public Intervalo getIntervaloEmAberto()
        {
            return Intervalos.FirstOrDefault(x => !x.Saida.HasValue);
        }
    }
}
