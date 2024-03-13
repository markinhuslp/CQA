using Aspose.Pdf.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Pdf.Text;
using System.Text;
namespace CQA
{
    public  class ExportarPdf
    {
        public string ExportarPdfs(string caminho) 
        {
            string texto = string.Empty;
            Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(caminho);

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
                            // Percorrer cada célula na linha
                            foreach (AbsorbedCell cell in row.CellList)
                            {
                                // Faça um loop pelos fragmentos de texto
                                foreach (TextFragment fragment in cell.TextFragments)
                                {
                                   
                                    var sb = new StringBuilder();
                                    foreach (TextSegment seg in fragment.Segments)
                                        sb.Append(seg.Text);
                                    Console.Write($"{sb.ToString()}|");
                                    texto += sb.ToString();
                                }
                            }
                            texto += "\n";
                            Console.WriteLine();
                        }
                        else
                        {
                            // A linha está vazia, você pode adicionar o código desejado para lidar com linhas vazias aqui
                            Console.WriteLine("Linha Vazia");
                        }
                    }
                }
            }
            return texto;
        }

    }
    
}
