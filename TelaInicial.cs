using System;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Configuration;
using System.Data.SqlTypes;
using static CQA.Program;
using System.Text.RegularExpressions;
using Aspose.OCR;
using System.Windows.Forms;


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
            Program.CaminhoPlanejamentoMensal = ConfigurationManager.AppSettings["CAMINHO_PDF_ESCALA"];
        }

        public void TelaInicial_Load(object sender, EventArgs e)
        {
            PictureBox gifCarregamento = new PictureBox();
            gifCarregamento.Image = Properties.Resources.aviaoCarregando;

            Task.Run(async () =>
            {

                try
                {
                    Invoke(new Action(() =>
                    {
                        Control ctr = tlpCentral.GetControlFromPosition(1, 1);
                        tlpCentral.Controls.Remove(ctr);
                        //PictureBox img = new();
                        //img.Anchor = AnchorStyles.None;
                        //img.SizeMode = PictureBoxSizeMode.StretchImage;
                        //img.Image = Properties.Resources.aviaoCarregando;
                        //img.Width = tlpCentral.GetColumnWidths()[1] / 2;
                        //img.Height = tlpCentral.GetRowHeights()[1] / 1;
                        //tlpCentral.Controls.Add(img, 1, 1);
                    }));

                    await Program.ObterTrechos(Program.CaminhoBasePdfTrechos);
                    await Program.ExtrairPdfEscala();
                    Invoke(new Action(() =>
                    ExibirTrechosNoDataGridView()));

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocorreu um erro ao exibir trechos no DataGridView: {ex.Message}");
                }
            });


        }

        private async Task ExibirTrechosNoDataGridView()
        {



            Thread.Sleep(1000);
            dataGridView2.Dock = DockStyle.Fill;
            tlpCentral.Controls.Add(dataGridView2, 1, 1);


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
            dataGridView2.Columns.Add("Codigo", "ID");
            dataGridView2.Columns.Add("Data", "Data Agendada");
            dataGridView2.Columns.Add("MARCACAO", "Regime");
            dataGridView2.Columns.Add("Origem", "Origem");
            dataGridView2.Columns.Add("HorarioOrigem", "Horario Partida");
            dataGridView2.Columns.Add("Destino", "Destino");
            dataGridView2.Columns.Add("HorarioDestino", "Horario Chegada");
            dataGridView2.Columns.Add("KM", "Quilometragem");
            dataGridView2.Columns.Add("Ganhos", "Ganhos");
            dataGridView2.Columns.Add("GanhoDiurno", "Ganho Diurno");
            dataGridView2.Columns.Add("GanhoNoturno", "Ganho Noturno");

            foreach (var item in escalaViagem)
            {
                try
                {
                    Program.trechos tr = Program.trechoViagem.FirstOrDefault(v => v.origem == item.Origem && v.destino == item.Destino);
                    dataGridView2.Rows.Add(item.codigo, item.DataVoo.ToString("dd/MM/yyyy"), $"N", item.Origem, item.HorarioOrigem.ToString("HH:mm:ss"), item.Destino, item.HorarioDestino.ToString("HH:mm:ss"), $"{tr.km} Km", $"R$ 0,00", $"R$ 0,00", $"R$ 0,00");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro\n{ex.Message}");
                }
            }
            RecalcularDataGrid();

        }
        // Supondo que você tenha um botão em sua interface do usuário para realizar a ação
        private void RecalcularDataGrid()
        {

            DateTime dataMinima = new DateTime(0001, 01, 01);
            decimal valReceber = 0;
            foreach (DataGridViewRow linha in dataGridView2.Rows)
            {
                // Supondo que as colunas que você precisa manipular sejam "DataVoo" e "Km"
                DateTime dataVoo = Convert.ToDateTime(linha.Cells["Data"].Value);
                DateTime HoraEmbarque = Convert.ToDateTime(linha.Cells["HorarioOrigem"].Value);
                DateTime HoraDesembarque = Convert.ToDateTime(linha.Cells["HorarioDestino"].Value);


                string km = linha.Cells["KM"].Value.ToString();
                km = Regex.Replace(km, "[^0-9]", "");
                // Calcula os ganhos com base nas condições específicas
                Tuple<decimal, decimal, decimal> ganhos;
                if (dataVoo > dataMinima && Convert.ToInt32(km) > 1)
                {
                    ganhos = Program.CalcularGanhoTotal(HoraEmbarque, HoraDesembarque, Convert.ToInt32(km)); // Preencha com os parâmetros corretos
                    linha.Cells["Ganhos"].Value = $"R$ {ganhos.Item1.ToString("F2")}";
                    linha.Cells["GanhoDiurno"].Value = $"R$ {ganhos.Item2.ToString("F2")}";
                    linha.Cells["GanhoNoturno"].Value = $"R$ {ganhos.Item3.ToString("F2")}";
                }
                else
                {
                    ganhos = Tuple.Create(0.0m, 0.0m, 0.0m);
                }

                // Atualiza o valor de valReceber com base nos ganhos
                valReceber += ganhos.Item1;

                // Verifica se a condição é atendida para alterar as propriedades visuais da linha
                if (dataVoo > dataMinima)
                {
                    linha.DefaultCellStyle.BackColor = Color.Purple;
                    linha.DefaultCellStyle.ForeColor = Color.White;
                }
            }
            lblTotalReceber.Text = valReceber.ToString("F2");
        }
        private void CalcularGanhos(DataGridView dt, int linha, string nomeColuna, string valor)
        {
            dt.Rows[linha].Cells[nomeColuna].Value = valor;
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            RecalcularDataGrid();
        }

      
    }

}
