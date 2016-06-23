using ControlePonto.Domain.ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Tests.mocks
{
    public class DataHoraMockListStrategy : IDataHoraStrategy
    {
        private Queue<DateTime> queueDateTimes = new Queue<DateTime>();
        public int Count { get { return queueDateTimes.Count; } }

        public DataHoraMockListStrategy(params DateTime[] dateTimes)
        {
            foreach (DateTime dateTime in dateTimes)
                queueDateTimes.Enqueue(dateTime);            
        }

        public DateTime getDataHoraAtual()
        {
            return queueDateTimes.Dequeue();
        }
    }
}
