using System;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Configuration;
using System.Data.SqlTypes;
using static CQA.Program;


namespace CQA
{
    public partial class TelaInicial : Form
    {

        public TelaInicial()
        {
            InitializeComponent();

            Program.TaxaDiurna = ConfigurationManager.AppSettings["TAXA_DIURNA"];
            Program.TaxaNoturna = ConfigurationManager.AppSettings["TAXA_NOTURNA"];
            Program.TaxaDomingoFeriado = ConfigurationManager.AppSettings["TAXA_DOMINGO_E_FERIADOS"];
            Program.CaminhoBasePdfTrechos = ConfigurationManager.AppSettings["CAMINHO_PDF_BASE_TRECHOS"];
        }

        public void TelaInicial_Load(object sender, EventArgs e)
        {
            Program.ObterTrechos(Program.CaminhoBasePdfTrechos);
            Program.ExtrairPdfEscala();
            try
            {
                ExibirTrechosNoDataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao exibir trechos no DataGridView: {ex.Message}");
            }
       
        }
        private void ExibirTrechosNoDataGridView()
        {
            // Configurar o DataGridView
            dataGridView1.AutoGenerateColumns = false;
            //dgwTabelaVendas.DataSource = Venda;

            dataGridView1.Columns.Add("CIA", "CIA");
            dataGridView1.Columns.Add("Origem", "Origem");
            dataGridView1.Columns.Add("Destino", "Destino");
            dataGridView1.Columns.Add("KM", "KM");

            foreach (var item in trechoViagem)
            {
                dataGridView1.Rows.Add(item.cia, item.origem, item.destino, item.km);
                // Adicione mais células conforme necessário
            }

            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns.Add("Data", "Data Agendada");
            dataGridView2.Columns.Add("Origem", "Origem");
            dataGridView2.Columns.Add("HorarioOrigem", "Horario Partida");
            dataGridView2.Columns.Add("Destino", "Destino");
            dataGridView2.Columns.Add("HorarioDestino", "Horario Chegada");

            DateTime dataMinima = new DateTime(0001, 01, 01);

            foreach (var item in escalaViagem)
            {
                dataGridView2.Rows.Add(item.DataVoo.ToString("dd/MM/yyyy"), item.Origem, item.HorarioOrigem.ToString("HH:mm:ss"), item.Destino, item.HorarioDestino.ToString("HH:mm:ss"));
                int linha = dataGridView2.Rows.Count -1 ;
                // Verificar a condição desejada para a formatação condicional
                if (item.DataVoo.Date > dataMinima)
                {
                    // Aplicar a formatação condicional para toda a grade
                    dataGridView2.Rows[linha].DefaultCellStyle.BackColor  = Color.Red;
                    dataGridView2.Rows[linha].DefaultCellStyle.ForeColor = Color.White;

                    TimeSpan  diferenca = item.DataVoo.Date - DateTime.Now;
                    if (diferenca.Days < 7 && diferenca.Days > 0) 
                    {
                        dataGridView2.Rows[linha].DefaultCellStyle.BackColor = Color.Purple;
                        dataGridView2.Rows[linha].DefaultCellStyle.ForeColor = Color.White;

                    }
                }

                // Adicionar a nova linha ao DataGridView
               

               
            }

        }

    
    }

}
