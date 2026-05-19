using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PrototipoMessier
{
    public partial class JogosForms : Form
    {
        private TextBox txtID, txtNome, txtDescricao;
        private Button btnSalvar, btnNovo;
        private DataGridView grdJogos;
        private Label lblStatus, lblContador;
        private Panel painelTop, painelHero, painelForm, painelTabela, painelFooter;

        private readonly Color corFundo = Color.FromArgb(240, 242, 245);
        private readonly Color corBranco = Color.White;
        private readonly Color corBorda = Color.FromArgb(229, 231, 235);
        private readonly Color corTeal = Color.FromArgb(0, 201, 177);
        private readonly Color corTealEsc = Color.FromArgb(0, 168, 143);
        private readonly Color corTexto = Color.FromArgb(17, 17, 30);
        private readonly Color corCinza = Color.FromArgb(136, 136, 136);
        private readonly Color corVerde = Color.FromArgb(0, 135, 110);
        private readonly Color corVermelho = Color.FromArgb(224, 82, 82);
        private readonly Color corEscuro = Color.FromArgb(26, 26, 46);

        public JogosForms()
        {
            InitializeComponent();
            MontarInterface();
        }

        private void MontarInterface()
        {
            this.Text = "Game Catalog";
            this.BackColor = corFundo;
            this.Font = new Font("Segoe UI", 9);
            this.Size = new Size(900, 680);
            this.MinimumSize = new Size(800, 580);
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

            Panel avatarCircle = CriarCirculo("AL", corTeal, new Point(0, 0), 34);
            Label lblUser = new Label();
            lblUser.Text = "Aluno";
            lblUser.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblUser.ForeColor = corTexto;
            lblUser.AutoSize = true;

            Label lblEditar = new Label();
            lblEditar.Text = "(editar perfil)";
            lblEditar.Font = new Font("Segoe UI", 8);
            lblEditar.ForeColor = corTeal;
            lblEditar.AutoSize = true;
            lblEditar.Cursor = Cursors.Hand;

            Panel userArea = new Panel();
            userArea.BackColor = Color.Transparent;
            userArea.Size = new Size(180, 40);
            userArea.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            painelTop.Controls.AddRange(new Control[] { logoCircle, lblLogoText });
            painelTop.Resize += (s, e) =>
            {
                avatarCircle.Location = new Point(painelTop.Width - 60, 12);
            };
            painelTop.Controls.Add(avatarCircle);

            // === HERO ===
            painelHero = new Panel();
            painelHero.BackColor = corBranco;
            painelHero.Dock = DockStyle.Top;
            painelHero.Height = 64;
            painelHero.Padding = new Padding(20, 0, 20, 0);
            painelHero.Paint += PainelHero_Paint;

            Label lblBemVindo = new Label();
            lblBemVindo.Text = "Olá Aluno, bem-vindo de volta!";
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
            painelForm.Height = 170;
            painelForm.Padding = new Padding(28, 16, 28, 16);
            painelForm.Paint += PainelCard_Paint;

            Label lblFormTitulo = new Label();
            lblFormTitulo.Text = "🎮  Novo jogo";
            lblFormTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFormTitulo.ForeColor = corTexto;
            lblFormTitulo.AutoSize = true;
            lblFormTitulo.Location = new Point(28, 16);

            Label lblId = CriarLabel("ID", new Point(28, 50));
            Label lblNome = CriarLabel("NOME DO JOGO", new Point(138, 50));
            Label lblDesc = CriarLabel("DESCRIÇÃO", new Point(428, 50));

            txtID = CriarCampo(new Point(28, 68), 100);
            txtNome = CriarCampo(new Point(138, 68), 280);
            txtDescricao = CriarCampo(new Point(428, 68), 400);

            btnNovo = CriarBotao("Limpar", new Point(638, 112), corCinza, corBranco, corBorda);
            btnSalvar = CriarBotao("Salvar jogo", new Point(748, 112), corBranco, corTeal, corTeal);
            btnNovo.Click += btnNovo_Click;
            btnSalvar.Click += btnSalvar_Click;

            lblStatus = new Label();
            lblStatus.Text = "";
            lblStatus.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblStatus.ForeColor = corVerde;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(30, 120);

            painelForm.Controls.AddRange(new Control[] {
                lblFormTitulo, lblId, txtID, lblNome, txtNome,
                lblDesc, txtDescricao, btnNovo, btnSalvar, lblStatus
            });

            // === PAINEL TABELA ===
            painelTabela = new Panel();
            painelTabela.BackColor = corFundo;
            painelTabela.Dock = DockStyle.Fill;
            painelTabela.Padding = new Padding(28, 16, 28, 16);

            Panel cardTabela = new Panel();
            cardTabela.BackColor = corBranco;
            cardTabela.Dock = DockStyle.Fill;
            cardTabela.Padding = new Padding(0);
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
            lblTabelaTitulo.Text = "Jogos cadastrados";
            lblTabelaTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTabelaTitulo.ForeColor = corTexto;
            lblTabelaTitulo.AutoSize = true;
            lblTabelaTitulo.Location = new Point(20, 14);

            lblContador = new Label();
            lblContador.Text = "0 jogos";
            lblContador.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblContador.ForeColor = corVerde;
            lblContador.AutoSize = true;
            lblContador.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            tabelaHeader.Controls.AddRange(new Control[] { lblTabelaTitulo, lblContador });
            tabelaHeader.Resize += (s, e) =>
                lblContador.Location = new Point(tabelaHeader.Width - lblContador.Width - 20, 16);

            grdJogos = new DataGridView();
            grdJogos.Dock = DockStyle.Fill;
            ConfigurarGrid();

            cardTabela.Controls.Add(grdJogos);
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
            Button btnCadastrar = CriarBotao("+ Cadastrar jogo", new Point(0, 10), corBranco, corTeal, corTeal);
            btnCadastrar.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btnCadastrar.Width = 150;
            btnCadastrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            painelFooter.Controls.AddRange(new Control[] { btnSair, btnCadastrar });
            painelFooter.Resize += (s, e) =>
                btnCadastrar.Location = new Point(painelFooter.Width - 178, 10);

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
            lbl.Font = new Font("Segoe UI", size < 36 ? 8 : 9, FontStyle.Bold);
            lbl.ForeColor = Color.White;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            p.Controls.Add(lbl);
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush b = new SolidBrush(cor))
                {
                    e.Graphics.FillEllipse(b, 0, 0, p.Width - 1, p.Height - 1);
                }
            };
            p.Region = new Region(new GraphicsPath());
            return p;
        }

        private void ConfigurarGrid()
        {
            grdJogos.ColumnCount = 3;
            grdJogos.Columns[0].Name = "ID";
            grdJogos.Columns[0].Width = 90;
            grdJogos.Columns[1].Name = "Nome";
            grdJogos.Columns[1].Width = 250;
            grdJogos.Columns[2].Name = "Descrição";
            grdJogos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            grdJogos.ReadOnly = true;
            grdJogos.AllowUserToAddRows = false;
            grdJogos.AllowUserToDeleteRows = false;
            grdJogos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdJogos.MultiSelect = false;
            grdJogos.BorderStyle = BorderStyle.None;
            grdJogos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdJogos.GridColor = Color.FromArgb(240, 242, 245);
            grdJogos.BackgroundColor = corBranco;
            grdJogos.RowHeadersVisible = false;

            grdJogos.EnableHeadersVisualStyles = false;
            grdJogos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
            grdJogos.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(170, 170, 170);
            grdJogos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            grdJogos.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            grdJogos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grdJogos.ColumnHeadersHeight = 38;

            grdJogos.DefaultCellStyle.BackColor = corBranco;
            grdJogos.DefaultCellStyle.ForeColor = Color.FromArgb(51, 51, 51);
            grdJogos.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            grdJogos.DefaultCellStyle.Padding = new Padding(10, 4, 0, 4);
            grdJogos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 252, 248);
            grdJogos.DefaultCellStyle.SelectionForeColor = corTexto;
            grdJogos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 248, 250);
            grdJogos.RowTemplate.Height = 40;
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

        private Button CriarBotao(string texto, Point pos, Color corTexto, Color corBg, Color corBorda)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Location = pos;
            btn.Size = new Size(100, 34);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = corBg;
            btn.ForeColor = corTexto;
            btn.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btn.FlatAppearance.BorderColor = corBorda;
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
            int n = grdJogos.Rows.Count;
            lblContador.Text = n + (n == 1 ? " jogo" : " jogos");
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtNome.Text = "";
            txtDescricao.Text = "";
            txtID.Focus();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MostrarStatus("Preencha ID e Nome.", corVermelho);
                return;
            }

            grdJogos.Rows.Add(txtID.Text.Trim(), txtNome.Text.Trim(), txtDescricao.Text.Trim());

            int ultima = grdJogos.Rows.Count - 1;
            grdJogos.ClearSelection();
            grdJogos.Rows[ultima].Selected = true;
            grdJogos.FirstDisplayedScrollingRowIndex = ultima;

            MostrarStatus("\"" + txtNome.Text.Trim() + "\" salvo com sucesso!", corVerde);
            AtualizarContador();
            btnNovo_Click(sender, e);
        }
    }
}