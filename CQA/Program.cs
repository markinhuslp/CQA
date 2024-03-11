using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Globalization;
using System.Text;
using static CQA.TelaInicial;
using Aspose.Pdf.Text;
using System.Text.RegularExpressions;

namespace CQA
{
    internal static class Program
    {
        public static string TaxaDiurna { get; set; }
        public static string TaxaNoturna { get; set; }
        public static string TaxaDomingoFeriado { get; set; }
        public static string CaminhoBasePdfTrechos { get; set; }
        public static List<trechos> trechoViagem { get; set; } = new List<trechos>();
        public static List<Escala> escalaViagem { get; set; } = new List<Escala>();
      
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new TelaInicial());
        }
        public struct trechos
        {
            public int km;
            public string origem;
            public string destino;
            public string cia;
        }
        public struct Escala
        {
            public DateTime DataVoo { get; set; }
            public string Origem { get; set; }
            public string Destino {  get; set; }
            public DateTime HorarioOrigem {  get; set; }
            public DateTime HorarioDestino { get; set; }
        }
        public static void ObterTrechos(string CaminhoPdfTrechos)
        {
            string textoExtraido = ExtrairTextoDoPDF(CaminhoBasePdfTrechos);
            string[] linhas = textoExtraido.Split('\n');
            List<string> Textos = new List<string>();

            foreach (string line in linhas)
            {
                int numeroDePalavras = line.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
                string linha = ((numeroDePalavras == 8 || numeroDePalavras == 4) && (!line.Contains("CIA") && !line.Contains("Página"))) ? $"{line}\n" : string.Empty;
                if (linha != string.Empty)
                {
                    Calcular(linha);
                }
            }
           
        }
        static string ExtrairTextoDoPDF(string caminhoDoPDF)
        {
            StringBuilder texto = new StringBuilder();

            using (PdfReader pdfReader = new PdfReader(caminhoDoPDF))
            {
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    for (int pagina = 1; pagina <= pdfDocument.GetNumberOfPages(); pagina++)
                    {
                        LocationTextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                        PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);
                        parser.ProcessPageContent(pdfDocument.GetPage(pagina));
                        texto.AppendLine(strategy.GetResultantText());
                    }
                }
            }

            return texto.ToString();
        }

        public static void ExtrairPdfEscala()
        {
            Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document("C:\\USR\\CrewRosterReport-2.pdf");

            // Percorra as páginas                      
            foreach (var page in pdfDocument.Pages)
            {
                // Crie um absorvente de mesa e visite a página
                Aspose.Pdf.Text.TableAbsorber absorber = new Aspose.Pdf.Text.TableAbsorber();
                absorber.Visit(page);

                // Faça um loop em cada tabela absorvida 
                foreach (AbsorbedTable table in absorber.TableList)
                {
                    Console.WriteLine("Table");

                    // Percorrer cada linha na tabela
                    foreach (AbsorbedRow row in table.RowList)
                    {
                        // Verificar se a linha contém texto
                        if (row.CellList.Count > 0)
                        {
                            // Extrair dados das colunas 0, 8 e 9
                            string coluna0 = "";
                            string coluna8 = "";
                            string coluna9 = "";

                            if (row.CellList.Count > 0 && row.CellList.Count > 7 && row.CellList.Count > 8)
                            {
                                coluna0 = ObterTexto(row.CellList[0]);
                                coluna8 = ObterTexto(row.CellList[7]);
                                coluna9 = ObterTexto(row.CellList[8]);

                               if (!coluna8.Contains("Stn"))
                               {
                                    string[] OrigemHorario = coluna8.Split(" ");
                                    string[] DestinoHorario = coluna9.Split("  ");
                                    string origem = OrigemHorario[0],
                                            HorarioOrigem = OrigemHorario[2],
                                            destino = DestinoHorario[0],
                                            HorarioDestino = DestinoHorario[1];
                                    int indiceParenteseOrigem = HorarioOrigem.IndexOf("(");
                                    int indiceParenteseDestino = HorarioDestino.IndexOf("(");
                                    
                                    // Se houver um caractere de parêntese, extrair a parte antes dele, caso contrário, use a string completa
                                    HorarioOrigem = (indiceParenteseOrigem != -1) ? HorarioOrigem.Substring(0, indiceParenteseOrigem) : HorarioOrigem;
                                    HorarioDestino = (indiceParenteseDestino != -1) ? HorarioDestino.Substring(0, indiceParenteseDestino) : HorarioDestino;

                                    Escala escala = new Escala
                                    {
                                        DataVoo = coluna0 == "" ? DateTime.MinValue.Date : DateTime.Parse(coluna0).Date,
                                        Origem = OrigemHorario[0],
                                        Destino = DestinoHorario[0],
                                        
                                        HorarioOrigem = DateTime.Parse(HorarioOrigem),
                                        HorarioDestino = DateTime.Parse(HorarioDestino),

                                    };
                                    escalaViagem.Add(escala);
                                    // Faça o que quiser com os dados extraídos
                               }



                            }
                        }
                       
                    }
                }
            }

            // Função auxiliar para obter o texto de uma célula
            static string ObterTexto(AbsorbedCell cell)
            {
                var sb = new StringBuilder();
                foreach (TextFragment fragment in cell.TextFragments)
                {
                    if (fragment != null)
                    {
                        foreach (TextSegment seg in fragment.Segments)
                            sb.Append(seg.Text);
                    }
                }
                return sb.ToString();
            }

        }
       
        public static void Calcular(string palavra)
        {
            palavra = Regex.Replace(palavra,"  ", "");
            palavra = Regex.Replace(palavra, "\n", "");
            string[] texto = palavra.Split(' ');
            
            if (texto.Length == 9)
            {
                trechos trecho = new trechos
                {
                    cia = texto[0],
                    origem = texto[1],
                    destino = texto[2],
                    km = Convert.ToInt32(texto[3])
                };
                trechoViagem.Add(trecho);
                trechos trecho2 = new trechos
                {
                    cia = texto[4],
                    origem = texto[5],
                    destino = texto[6],
                    km = Convert.ToInt32(texto[7])
                };
                trechoViagem.Add(trecho2);
            }
            else
            {
                trechos trecho = new trechos
                {
                    cia = texto[0],
                    origem = texto[1],
                    destino = texto[2],
                    km = Convert.ToInt32(texto[3])
                };
                trechoViagem.Add(trecho);
            }

        }

        static double CalcularGanhoTotal(DateTime horaInicio, DateTime horaFim)
        {
            double ganhoTotal = 0;
            DateTime horaLimite1 = new DateTime(horaInicio.Year, horaInicio.Month, horaInicio.Day, 6, 0, 0);
            DateTime horaLimite2 = new DateTime(horaInicio.Year, horaInicio.Month, horaInicio.Day, 18, 0, 0);
            DateTime horaLimite3 = new DateTime(horaInicio.Year, horaInicio.Month, horaInicio.Day, 5, 59, 59);

            if (horaInicio < horaLimite1 && horaFim > horaLimite3)
            {
                // O voo abrange as duas faixas horárias
                TimeSpan tempoFaixa1 = (horaLimite1 < horaFim) ? horaLimite1 - horaInicio : TimeSpan.Zero;
                TimeSpan tempoFaixa2 = (horaFim > horaLimite2) ? horaFim - horaLimite2 : TimeSpan.Zero;
                TimeSpan tempoFaixa3 = horaFim - horaLimite3;

                ganhoTotal = (tempoFaixa1.TotalHours + tempoFaixa2.TotalHours) * 10 + tempoFaixa3.TotalHours * 20;
            }
            else if (horaInicio < horaLimite2 && horaFim > horaLimite1)
            {
                // O voo abrange apenas a primeira faixa horária
                TimeSpan tempoFaixa1 = (horaLimite1 < horaFim) ? horaLimite1 - horaInicio : TimeSpan.Zero;
                TimeSpan tempoFaixa2 = (horaFim > horaLimite2) ? horaFim - horaLimite2 : TimeSpan.Zero;

                ganhoTotal = (tempoFaixa1.TotalHours + tempoFaixa2.TotalHours) * 10;
            }
            else if (horaInicio < horaLimite3 && horaFim > horaLimite2)
            {
                // O voo abrange apenas a segunda faixa horária
                TimeSpan tempoFaixa2 = (horaFim > horaLimite2) ? horaFim - horaLimite2 : TimeSpan.Zero;
                TimeSpan tempoFaixa3 = horaFim - horaLimite3;

                ganhoTotal = tempoFaixa2.TotalHours * 10 + tempoFaixa3.TotalHours * 20;
            }
            else
            {
                // O voo não abrange nenhuma faixa horária completa
                TimeSpan tempoTotal = horaFim - horaInicio;
                ganhoTotal = tempoTotal.TotalHours * 10;  // Utilizando a taxa padrão antes das 18h
            }

            return ganhoTotal;
        }
    }
}