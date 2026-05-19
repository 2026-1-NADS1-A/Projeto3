using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PrototipoMessier
{
    public partial class PacoteForms : Form
    {
        private TextBox txtID, txtNome;
        private Button btnSalvar, btnNovo;
        private DataGridView grdPacotes;
        private CheckedListBox checkedListBox1;
        private Label lblStatus, lblContador;
        private Panel painelTop, painelHero, painelForm, painelTabela, painelFooter;

        private readonly Color corFundo = Color.FromArgb(240, 242, 245);
        private readonly Color corBranco = Color.White;
        private readonly Color corBorda = Color.FromArgb(229, 231, 235);
        private readonly Color corTeal = Color.FromArgb(0, 201, 177);
        private readonly Color corTexto = Color.FromArgb(17, 17, 30);
        private readonly Color corCinza = Color.FromArgb(136, 136, 136);
        private readonly Color corVerde = Color.FromArgb(0, 135, 110);
        private readonly Color corVermelho = Color.FromArgb(224, 82, 82);
        private readonly Color corEscuro = Color.FromArgb(26, 26, 46);

        public PacoteForms()
        {
            InitializeComponent();
            MontarInterface();
        }

        private void MontarInterface()
        {
            this.Text = "Game Catalog — Pacotes";
            this.BackColor = corFundo;
            this.Font = new Font("Segoe UI", 9);
            this.Size = new Size(900, 720);
            this.MinimumSize = new Size(800, 620);
            this.StartPosition = FormStartPosition.CenterScreen;

            // === TOP BAR ===
            painelTop = new Panel();
            painelTop.BackColor = corBranco;
            painelTop.Dock = DockStyle.Top;
            painelTop.Height = 58;
            painelTop.Paint += (s, e) =>
            {
                using (Pen p = new Pen(corBorda, 1))
                    e.Graphics.DrawLine(p, 0, painelTop.Height - 1, painelTop.Width, painelTop.Height - 1);
            };

            Panel logoCircle = CriarCirculo("GC", corTeal, new Point(20, 11), 36);
            Label lblLogoText = new Label();
            lblLogoText.Text = "Game Catalog";
            lblLogoText.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblLogoText.ForeColor = corTexto;
            lblLogoText.AutoSize = true;
            lblLogoText.Location = new Point(64, 18);

            Panel avatarCircle = CriarCirculo("AL", corTeal, new Point(0, 11), 36);
            avatarCircle.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            painelTop.Controls.AddRange(new Control[] { logoCircle, lblLogoText, avatarCircle });
            painelTop.Resize += (s, e) =>
                avatarCircle.Location = new Point(painelTop.Width - 60, 11);

            // === HERO ===
            painelHero = new Panel();
            painelHero.BackColor = corBranco;
            painelHero.Dock = DockStyle.Top;
            painelHero.Height = 64;
            painelHero.Paint += PainelHero_Paint;

            Label lblBemVindo = new Label();
            lblBemVindo.Text = "Olá Aluno, gerencie seus pacotes!";
            lblBemVindo.Font = new Font("Segoe UI", 13);
            lblBemVindo.ForeColor = corTexto;
            lblBemVindo.AutoSize = false;
            lblBemVindo.TextAlign = ContentAlignment.MiddleCenter;
            lblBemVindo.Dock = DockStyle.Fill;
            painelHero.Controls.Add(lblBemVindo);

            // === PAINEL FORMULÁRIO ===
            painelForm = new Panel();
            painelForm.BackColor = corBranco;
            painelForm.Dock = DockStyle.Top;
            painelForm.Height = 220;
            painelForm.Paint += PainelCard_Paint;

            Label lblFormTitulo = new Label();
            lblFormTitulo.Text = "📦  Novo pacote";
            lblFormTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFormTitulo.ForeColor = corTexto;
            lblFormTitulo.AutoSize = true;
            lblFormTitulo.Location = new Point(28, 16);

            Label lblId = CriarLabel("ID", new Point(28, 50));
            Label lblNome = CriarLabel("NOME DO PACOTE", new Point(138, 50));
            Label lblJogos = CriarLabel("CATEGORIAS INCLUÍDAS", new Point(28, 118));

            txtID = CriarCampo(new Point(28, 68), 100);
            txtNome = CriarCampo(new Point(138, 68), 700);

            checkedListBox1 = new CheckedListBox();
            checkedListBox1.Location = new Point(28, 138);
            checkedListBox1.Size = new Size(810, 48);
            checkedListBox1.BackColor = Color.FromArgb(247, 248, 250);
            checkedListBox1.ForeColor = corTexto;
            checkedListBox1.Font = new Font("Segoe UI", 9);
            checkedListBox1.BorderStyle = BorderStyle.FixedSingle;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.MultiColumn = true;
            checkedListBox1.Items.AddRange(new object[] {
                "Ação", "Aventura", "RPG", "Esporte", "Corrida", "Luta", "Puzzle", "Terror"
            });

            btnNovo = CriarBotao("Limpar", new Point(638, 174), corCinza, corBranco, corBorda);
            btnSalvar = CriarBotao("Salvar pacote", new Point(748, 174), corBranco, corTeal, corTeal);
            btnSalvar.Width = 120;
            btnNovo.Click += btnNovo_Click;
            btnSalvar.Click += btnSalvar_Click;

            lblStatus = new Label();
            lblStatus.Text = "";
            lblStatus.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblStatus.ForeColor = corVerde;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(30, 182);

            painelForm.Controls.AddRange(new Control[] {
                lblFormTitulo,
                lblId, txtID, lblNome, txtNome,
                lblJogos, checkedListBox1,
                btnNovo, btnSalvar, lblStatus
            });

            // === PAINEL TABELA ===
            painelTabela = new Panel();
            painelTabela.BackColor = corFundo;
            painelTabela.Dock = DockStyle.Fill;
            painelTabela.Padding = new Padding(28, 16, 28, 16);

            Panel cardTabela = new Panel();
            cardTabela.BackColor = corBranco;
            cardTabela.Dock = DockStyle.Fill;
            cardTabela.Paint += PainelCard_Paint;

            Panel tabelaHeader = new Panel();
            tabelaHeader.BackColor = corBranco;
            tabelaHeader.Dock = DockStyle.Top;
            tabelaHeader.Height = 48;
            tabelaHeader.Paint += (s, e) =>
            {
                using (Pen p = new Pen(corBorda, 1))
                    e.Graphics.DrawLine(p, 0, tabelaHeader.Height - 1, tabelaHeader.Width, tabelaHeader.Height - 1);
            };

            Label lblTabelaTitulo = new Label();
            lblTabelaTitulo.Text = "Pacotes cadastrados";
            lblTabelaTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTabelaTitulo.ForeColor = corTexto;
            lblTabelaTitulo.AutoSize = true;
            lblTabelaTitulo.Location = new Point(20, 14);

            lblContador = new Label();
            lblContador.Text = "0 pacotes";
            lblContador.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblContador.ForeColor = corVerde;
            lblContador.AutoSize = true;
            lblContador.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            tabelaHeader.Controls.AddRange(new Control[] { lblTabelaTitulo, lblContador });
            tabelaHeader.Resize += (s, e) =>
                lblContador.Location = new Point(tabelaHeader.Width - lblContador.Width - 20, 16);

            grdPacotes = new DataGridView();
            grdPacotes.Dock = DockStyle.Fill;
            ConfigurarGrid();

            cardTabela.Controls.Add(grdPacotes);
            cardTabela.Controls.Add(tabelaHeader);
            painelTabela.Controls.Add(cardTabela);

            // === FOOTER ===
            painelFooter = new Panel();
            painelFooter.BackColor = corBranco;
            painelFooter.Dock = DockStyle.Bottom;
            painelFooter.Height = 56;
            painelFooter.Paint += (s, e) =>
            {
                using (Pen p = new Pen(corBorda, 1))
                    e.Graphics.DrawLine(p, 0, 0, painelFooter.Width, 0);
            };

            Button btnSair = CriarBotao("Sair", new Point(28, 10), corBranco, corEscuro, corEscuro);
            Button btnCadastrar = CriarBotao("+ Novo pacote", new Point(0, 10), corBranco, corTeal, corTeal);
            btnCadastrar.Width = 140;
            btnCadastrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            painelFooter.Controls.AddRange(new Control[] { btnSair, btnCadastrar });
            painelFooter.Resize += (s, e) =>
                btnCadastrar.Location = new Point(painelFooter.Width - 168, 10);

            this.Controls.Add(painelTabela);
            this.Controls.Add(painelForm);
            this.Controls.Add(painelHero);
            this.Controls.Add(painelTop);
            this.Controls.Add(painelFooter);
        }

        private void PainelHero_Paint(object sender, PaintEventArgs e)
        {
            int m = 28;
            using (Pen p = new Pen(corBorda, 1))
                e.Graphics.DrawRectangle(p, m, 8, painelHero.Width - m * 2, painelHero.Height - 16);
        }

        private void PainelCard_Paint(object sender, PaintEventArgs e)
        {
            Panel pan = sender as Panel;
            using (Pen p = new Pen(corBorda, 1))
                e.Graphics.DrawRectangle(p, 1, 1, pan.Width - 2, pan.Height - 2);
        }

        private Panel CriarCirculo(string texto, Color cor, Point pos, int size)
        {
            Panel p = new Panel();
            p.Size = new Size(size, size);
            p.Location = pos;
            p.BackColor = cor;

            Label lbl = new Label();
            lbl.Text = texto;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lbl.ForeColor = Color.White;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            p.Controls.Add(lbl);

            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush b = new SolidBrush(cor))
                    e.Graphics.FillEllipse(b, 0, 0, p.Width - 1, p.Height - 1);
            };
            return p;
        }

        private void ConfigurarGrid()
        {
            grdPacotes.ColumnCount = 3;
            grdPacotes.Columns[0].Name = "ID";
            grdPacotes.Columns[0].Width = 90;
            grdPacotes.Columns[1].Name = "Nome";
            grdPacotes.Columns[1].Width = 220;
            grdPacotes.Columns[2].Name = "Categorias";
            grdPacotes.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            grdPacotes.ReadOnly = true;
            grdPacotes.AllowUserToAddRows = false;
            grdPacotes.AllowUserToDeleteRows = false;
            grdPacotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdPacotes.MultiSelect = false;
            grdPacotes.BorderStyle = BorderStyle.None;
            grdPacotes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdPacotes.GridColor = Color.FromArgb(240, 242, 245);
            grdPacotes.BackgroundColor = corBranco;
            grdPacotes.RowHeadersVisible = false;

            grdPacotes.EnableHeadersVisualStyles = false;
            grdPacotes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
            grdPacotes.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(170, 170, 170);
            grdPacotes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            grdPacotes.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            grdPacotes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grdPacotes.ColumnHeadersHeight = 38;

            grdPacotes.DefaultCellStyle.BackColor = corBranco;
            grdPacotes.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            grdPacotes.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grdPacotes.DefaultCellStyle.Padding = new Padding(10, 4, 0, 4);
            grdPacotes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 252, 248);
            grdPacotes.DefaultCellStyle.SelectionForeColor = corTexto;
            grdPacotes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 248, 250);
            grdPacotes.RowTemplate.Height = 40;
        }

        private Label CriarLabel(string texto, Point pos)
        {
            return new Label
            {
                Text = texto,
                Location = pos,
                AutoSize = true,
                ForeColor = corCinza,
                Font = new Font("Segoe UI", 7, FontStyle.Bold)
            };
        }

        private TextBox CriarCampo(Point pos, int largura)
        {
            TextBox txt = new TextBox();
            txt.Location = pos;
            txt.Width = largura;
            txt.Font = new Font("Segoe UI", 9);
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = Color.FromArgb(247, 248, 250);
            txt.ForeColor = corTexto;
            return txt;
        }

        private Button CriarBotao(string texto, Point pos, Color corTxt, Color corBg, Color corBrd)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Location = pos;
            btn.Size = new Size(100, 34);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = corBg;
            btn.ForeColor = corTxt;
            btn.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btn.FlatAppearance.BorderColor = corBrd;
            btn.FlatAppearance.BorderSize = 1;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void MostrarStatus(string msg, Color cor)
        {
            lblStatus.Text = msg;
            lblStatus.ForeColor = cor;

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
            t.Interval = 3000;
            t.Tick += (s, e) => { lblStatus.Text = ""; t.Stop(); t.Dispose(); };
            t.Start();
        }

        private void AtualizarContador()
        {
            int n = grdPacotes.Rows.Count;
            lblContador.Text = n + (n == 1 ? " pacote" : " pacotes");
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtNome.Text = "";
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemCheckState(i, CheckState.Unchecked);
            txtID.Focus();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MostrarStatus("Preencha ID e Nome.", corVermelho);
                return;
            }

            string categorias = "";
            foreach (var item in checkedListBox1.CheckedItems)
                categorias += item.ToString() + " | ";
            categorias = categorias.TrimEnd(' ', '|');

            grdPacotes.Rows.Add(txtID.Text.Trim(), txtNome.Text.Trim(), categorias);

            int ultima = grdPacotes.Rows.Count - 1;
            grdPacotes.ClearSelection();
            grdPacotes.Rows[ultima].Selected = true;
            grdPacotes.FirstDisplayedScrollingRowIndex = ultima;

            MostrarStatus("\"" + txtNome.Text.Trim() + "\" salvo com sucesso!", corVerde);
            AtualizarContador();
            btnNovo_Click(sender, e);
        }
    }
}