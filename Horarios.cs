using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQA
{
    using System;

    public class Horarios
    {
        public TimeSpan Hora18 { get; set; } = new TimeSpan(18, 0, 0);
        public TimeSpan Hora06 { get; set; } = new TimeSpan(6, 0, 0);

        public Tuple<decimal, decimal, decimal> CalcularValorTrabalho(DateTime HoraInicio, DateTime HoraFim, int KM)
        {
            decimal valorCheioDiurnoKm = KM * Convert.ToDecimal(Program.TaxaDiurna) / 1000000;
            decimal valorCheioNoturnoKm = KM * Convert.ToDecimal(Program.TaxaNoturna) / 1000000;
            decimal valDiurno = 0, valNoturno = 0, valTotal = 0;
            // Verifica se o horário de início está dentro do período diurno
            bool diurnoInicio = HoraInicio.TimeOfDay >= Hora06 && HoraInicio.TimeOfDay < Hora18;

            // Verifica se o horário de término está dentro do período diurno
            bool diurnoFim = HoraFim.TimeOfDay >= Hora06 && HoraFim.TimeOfDay < Hora18;

            // Calcula a duração do trabalho em horas
            TimeSpan duracaoTrabalho = HoraFim - HoraInicio;
            decimal horasTrabalhadas = Convert.ToDecimal(duracaoTrabalho.TotalHours);

            // Verifica se o trabalho abrange ambos os períodos (diurno e noturno)
            if ((diurnoInicio && !diurnoFim) || (!diurnoInicio && diurnoFim))
            {
                // Calcula o valor até o fim do período diurno
                TimeSpan duracaoDiurna = Hora18 - HoraInicio.TimeOfDay;
                decimal horasDiurnas = Convert.ToDecimal(duracaoDiurna.TotalHours);
                decimal percentualHoraDiurna = horasDiurnas / horasTrabalhadas;
                decimal valorDiurno = percentualHoraDiurna * valorCheioDiurnoKm;

                // Calcula o valor para o restante do período (noturno)
                TimeSpan duracaoNoturna = HoraFim.TimeOfDay - Hora18;
                decimal horasNoturnas = Convert.ToDecimal(duracaoNoturna.TotalHours);
                decimal percentualHoraNoturna = horasNoturnas / horasTrabalhadas;
                decimal valorNoturno = percentualHoraNoturna * valorCheioNoturnoKm;

                valDiurno = valorDiurno;

                valNoturno = valorNoturno;
            }
            else
            {
                // O trabalho está completamente dentro do período diurno ou noturno

                if (diurnoInicio && diurnoFim)
                {
                    valDiurno = valorCheioDiurnoKm;
                }
                else
                {
                    valNoturno = valorCheioNoturnoKm;
                }
            }
            valNoturno = valNoturno > 0 ? valNoturno : 0;
            valDiurno = valDiurno > 0 ? valDiurno : 0;
            return Tuple.Create(valTotal =  valDiurno + valNoturno,valDiurno,valNoturno);
        }


    }

}
