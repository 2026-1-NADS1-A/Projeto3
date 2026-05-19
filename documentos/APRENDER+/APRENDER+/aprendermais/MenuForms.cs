using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PrototipoMessier
{
    public partial class Form1 : Form
    {
        JogosForms frmJogos;
        PacoteForms frmPacotes;

        private readonly Color corFundo = Color.FromArgb(240, 242, 245);
        private readonly Color corBranco = Color.White;
        private readonly Color corBorda = Color.FromArgb(229, 231, 235);
        private readonly Color corTeal = Color.FromArgb(0, 201, 177);
        private readonly Color corTexto = Color.FromArgb(17, 17, 30);
        private readonly Color corCinza = Color.FromArgb(136, 136, 136);
        private readonly Color corEscuro = Color.FromArgb(26, 26, 46);

        public Form1()
        {
            InitializeComponent();
            frmJogos = new JogosForms();
            frmPacotes = new PacoteForms();
            MontarInterface();
        }

        private void MontarInterface()
        {
            this.Text = "Game Catalog";
            this.BackColor = corFundo;
            this.Font = new Font("Segoe UI", 9);
            this.Size = new Size(860, 600);
            this.MinimumSize = new Size(760, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            // === TOP BAR ===
            Panel painelTop = new Panel();
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
            Panel painelHero = new Panel();
            painelHero.BackColor = corBranco;
            painelHero.Dock = DockStyle.Top;
            painelHero.Height = 72;
            painelHero.Paint += (s, e) =>
            {
                int m = 28;
                using (Pen p = new Pen(corBorda, 1))
                    e.Graphics.DrawRectangle(p, m, 10, painelHero.Width - m * 2, painelHero.Height - 20);
            };

            Label lblBemVindo = new Label();
            lblBemVindo.Text = "Olá Aluno, bem-vindo de volta!";
            lblBemVindo.Font = new Font("Segoe UI", 13);
            lblBemVindo.ForeColor = corTexto;
            lblBemVindo.AutoSize = false;
            lblBemVindo.TextAlign = ContentAlignment.MiddleCenter;
            lblBemVindo.Dock = DockStyle.Fill;
            painelHero.Controls.Add(lblBemVindo);

            // === LABEL SEÇÃO ===
            Label lblSecao = new Label();
            lblSecao.Text = "Selecione o que deseja acessar";
            lblSecao.Font = new Font("Segoe UI", 9);
            lblSecao.ForeColor = corCinza;
            lblSecao.AutoSize = false;
            lblSecao.TextAlign = ContentAlignment.MiddleCenter;
            lblSecao.Dock = DockStyle.Top;
            lblSecao.Height = 36;

            // === GRID DE CARDS ===
            Panel painelCards = new Panel();
            painelCards.BackColor = corFundo;
            painelCards.Dock = DockStyle.Fill;
            painelCards.Padding = new Padding(28, 8, 28, 8);

            Panel gridCards = new Panel();
            gridCards.BackColor = corFundo;
            gridCards.Dock = DockStyle.Fill;

            var cards = new (string icone, string titulo, string sub, Color cor, EventHandler acao)[]
            {
                ("🎮", "Jogos",    "Cadastrar e listar jogos",    Color.FromArgb(224, 250, 245), jogosToolStripMenuItem_Click),
                ("📦", "Pacotes",  "Gerenciar pacotes",           Color.FromArgb(240, 235, 255), pacotesToolStripMenuItem_Click),
                ("⭐", "Favoritos","Seus jogos favoritos",         Color.FromArgb(255, 243, 232), null),
                ("📊", "Relatórios","Visualizar dados",            Color.FromArgb(232, 244, 255), null),
                ("🛟", "Suporte",  "Central de ajuda",            Color.FromArgb(253, 232, 245), null),
                ("⚙️", "Config.",  "Configurações do sistema",    Color.FromArgb(242, 242, 242), null),
            };

            int cols = 3;
            int cardW = 220;
            int cardH = 140;
            int gapX = 20;
            int gapY = 16;
            int startX = 0;
            int startY = 8;

            for (int i = 0; i < cards.Length; i++)
            {
                var c = cards[i];
                int col = i % cols;
                int row = i / cols;

                Panel card = new Panel();
                card.Size = new Size(cardW, cardH);
                card.Location = new Point(startX + col * (cardW + gapX),
                                          startY + row * (cardH + gapY));
                card.BackColor = corBranco;
                card.Cursor = Cursors.Hand;
                card.Tag = c.acao;
                card.Paint += Card_Paint;
                card.MouseEnter += Card_MouseEnter;
                card.MouseLeave += Card_MouseLeave;
                card.Click += Card_Click;

                Panel iconBox = new Panel();
                iconBox.Size = new Size(52, 52);
                iconBox.Location = new Point(20, 22);
                iconBox.BackColor = c.cor;
                iconBox.BorderStyle = BorderStyle.None;

                Label lblIcone = new Label();
                lblIcone.Text = c.icone;
                lblIcone.Font = new Font("Segoe UI", 18);
                lblIcone.Dock = DockStyle.Fill;
                lblIcone.TextAlign = ContentAlignment.MiddleCenter;
                lblIcone.BackColor = Color.Transparent;
                iconBox.Controls.Add(lblIcone);

                Label lblTitulo = new Label();
                lblTitulo.Text = c.titulo;
                lblTitulo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblTitulo.ForeColor = corTexto;
                lblTitulo.AutoSize = true;
                lblTitulo.Location = new Point(20, 84);

                Label lblSub = new Label();
                lblSub.Text = c.sub;
                lblSub.Font = new Font("Segoe UI", 8);
                lblSub.ForeColor = corCinza;
                lblSub.AutoSize = true;
                lblSub.Location = new Point(20, 106);

                foreach (Control ctrl in new Control[] { iconBox, lblTitulo, lblSub })
                {
                    ctrl.MouseEnter += Card_MouseEnter;
                    ctrl.MouseLeave += Card_MouseLeave;
                    ctrl.Click += Card_Click;
                    ctrl.Tag = card;
                }

                card.Controls.AddRange(new Control[] { iconBox, lblTitulo, lblSub });
                gridCards.Controls.Add(card);
            }

            painelCards.Controls.Add(gridCards);

            // centraliza o grid quando a janela redimensiona
            gridCards.Resize += (s, e) =>
            {
                int totalW = cols * cardW + (cols - 1) * gapX;
                int offX = Math.Max(0, (gridCards.Width - totalW) / 2);
                foreach (Control ctrl in gridCards.Controls)
                {
                    int col = gridCards.Controls.IndexOf(ctrl) % cols;
                    int row = gridCards.Controls.IndexOf(ctrl) / cols;
                    ctrl.Location = new Point(offX + col * (cardW + gapX),
                                              startY + row * (cardH + gapY));
                }
            };

            // === FOOTER ===
            Panel painelFooter = new Panel();
            painelFooter.BackColor = corBranco;
            painelFooter.Dock = DockStyle.Bottom;
            painelFooter.Height = 56;
            painelFooter.Paint += (s, e) =>
            {
                using (Pen p = new Pen(corBorda, 1))
                    e.Graphics.DrawLine(p, 0, 0, painelFooter.Width, 0);
            };

            Button btnSair = CriarBotao("Sair", new Point(28, 11), corBranco, corEscuro, corEscuro);
            btnSair.Click += sairToolStripMenuItem_Click;

            Panel logoCentro = CriarCirculo("GC", corEscuro, new Point(0, 12), 32);
            logoCentro.Anchor = AnchorStyles.Top | AnchorStyles.None;

            painelFooter.Controls.AddRange(new Control[] { btnSair, logoCentro });
            painelFooter.Resize += (s, e) =>
                logoCentro.Location = new Point((painelFooter.Width - 32) / 2, 12);

            // === MONTAR ===
            this.Controls.Add(painelCards);
            this.Controls.Add(lblSecao);
            this.Controls.Add(painelHero);
            this.Controls.Add(painelTop);
            this.Controls.Add(painelFooter);
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            Panel card = sender as Panel;
            using (Pen p = new Pen(corBorda, 1))
                e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
        }

        private void Card_MouseEnter(object sender, EventArgs e)
        {
            Panel card = (sender is Panel p && p.Tag is EventHandler) ? p : (sender as Control)?.Tag as Panel ?? sender as Panel;
            if (card != null)
            {
                card.BackColor = Color.FromArgb(247, 255, 253);
                foreach (Control c in card.Controls) c.BackColor = Color.Transparent;
            }
        }

        private void Card_MouseLeave(object sender, EventArgs e)
        {
            Panel card = (sender is Panel p && p.Tag is EventHandler) ? p : (sender as Control)?.Tag as Panel ?? sender as Panel;
            if (card != null)
            {
                card.BackColor = corBranco;
                foreach (Control c in card.Controls) c.BackColor = Color.Transparent;
            }
        }

        private void Card_Click(object sender, EventArgs e)
        {
            Panel card = (sender is Panel p && p.Tag is EventHandler) ? p : (sender as Control)?.Tag as Panel ?? sender as Panel;
            EventHandler acao = card?.Tag as EventHandler;
            acao?.Invoke(sender, e);
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

            p.Paint += (s, ev) =>
            {
                ev.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush b = new SolidBrush(cor))
                    ev.Graphics.FillEllipse(b, 0, 0, p.Width - 1, p.Height - 1);
            };
            return p;
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

        private void jogosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmJogos.ShowDialog();
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pacotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPacotes.ShowDialog();
        }
    }
}