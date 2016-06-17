﻿using ControlePonto.Domain.intervalo;
using ControlePonto.Domain.jornada;
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
        #region Propriedades

        public virtual DateTime Data { get; protected set; }
        public virtual TimeSpan Inicio { get; protected set; }

        private TimeSpan? _Fim;
        public virtual TimeSpan? Fim 
        {
            get
            {
                return _Fim;
            }
            set
            {
                Check.Require(!_Fim.HasValue, "O horário do ponto não pode ser alterado");
                _Fim = value;                
            }
        }

        public virtual Usuario Usuario { get; protected set; }
        public virtual bool isFeriado { get; protected set; }
        public virtual bool isAberto 
        {
            get
            {
                return !_Fim.HasValue;
            }
        }

        public virtual bool deleteme { get; set; }

        public virtual ICollection<Intervalo> Intervalos { get; set; }

        #endregion

        protected PontoDia() { }

        public PontoDia(DateTime data, TimeSpan inicio, Usuario usuario)
        {
            base.checkPreConstructor();
            Check.Require(usuario != null, "O usuário não deve ser nulo");

            Data = data;
            Inicio = inicio;
            Usuario = usuario;            
            Intervalos = new List<Intervalo>();
        }
        
        public virtual void registrarIntervalo(TipoIntervalo tipoIntervalo, IDataHoraStrategy dataHoraStrategy)
        {
            if (intervaloFoiRegistrado(tipoIntervalo))
            {
                var intervalo = getIntervalo(tipoIntervalo);
                if (intervalo.Saida.HasValue)
                    throw new IntervaloJaRegistradoException(tipoIntervalo);

                intervalo.Saida = dataHoraStrategy.getDataHoraAtual().TimeOfDay;
            }
            else
            {
                if (algumIntervaloEmAberto())
                    throw new IntervaloEmAbertoException(getIntervaloEmAberto());

                Intervalos.Add(new Intervalo(tipoIntervalo, dataHoraStrategy.getDataHoraAtual().TimeOfDay));
            }
        }

        public virtual Intervalo getIntervalo(TipoIntervalo tipoIntervalo)
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

        public virtual bool intervaloFoiRegistrado(TipoIntervalo tipoIntervalo)
        {
            return Intervalos.Any(x => x.TipoIntervalo.Nome.Equals(tipoIntervalo.Nome));
        }       

        public virtual bool algumIntervaloEmAberto()
        {
            return Intervalos.Any(x => x.isAberto);
        }

        public virtual Intervalo getIntervaloEmAberto()
        {
            return Intervalos.FirstOrDefault(x => x.isAberto);
        }

        public virtual TimeSpan calcularHorasTrabalhadas()
        {
            if (isAberto) throw new DiaEmAbertoException(this);

            var descanso = new TimeSpan(Intervalos.Sum(x => x.Saida.Value.Subtract(x.Entrada).Ticks));
            var trabalhado = Fim.Value.Subtract(Inicio);

            return trabalhado.Subtract(descanso);
        }

        public virtual TimeSpan calcularHorasExtras(JornadaTrabalho jornada)
        {
            if (isAberto) throw new DiaEmAbertoException(this);

            DiaJornada diaJornada = jornada.getDia(Data.DayOfWeek);
            var trabalhado = calcularHorasTrabalhadas();
            var esperado = diaJornada.calcularHorasTrabalhoEsperado();

            return trabalhado.Subtract(esperado);
        }
    }
}
