using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQA
{
    public class TrechosKm
    {
        public int Km { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public string Cia { get; set; }
        public void ReceberTrechosDoPdf(string Texto)
        {
            string[] Informacoes = Texto.Split(' ');
            Cia = Informacoes[0];
            Origem = Informacoes[1];
            Destino = Informacoes[2];
            Km = Convert.ToInt32(Informacoes[3]);
        }
    }
  
    
}
