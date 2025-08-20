using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SeatHandler;
using System.Collections.Generic;
using System.Text;

namespace WFAAPP
{
    public partial class MainWindow : Form
    {
        private TableLayoutPanel mainTablePanel;
        private TableLayoutPanel seatTablePanel;
        private Panel optionsPanel;
        
        private List<Button> selectedButtons = new List<Button>();
        private Map Seats = new Map();
        
        private readonly char[] rowLetters = {'O', 'N', 'M', 'L', 'K', 'J', 'I', 'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A'};
        private const int TotalRows = 15;
        private const int TotalSeatsPerRow = 40;
        
        public MainWindow()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false; 
            MinimizeBox = false;
            InitializeApplicationLayout();
            ResizeBegin += (this.MainWindow_ResizeBegin);
            ResizeEnd += (this.MainWindow_ResizeEnd);
        }

        private void InitializeApplicationLayout()
        {
            ConfigureMainLayout();
            this.Controls.Add(mainTablePanel);

            ConfigureSeatPanel();
            mainTablePanel.Controls.Add(seatTablePanel, 0, 0);

            ConfigureOptionsPanel();
            mainTablePanel.Controls.Add(optionsPanel, 0, 1);
            
            GenerateSeatButtons();
        }
        
        private void ConfigureMainLayout()
        {
            mainTablePanel = new TableLayoutPanel();
            mainTablePanel.Dock = DockStyle.Fill;
            mainTablePanel.ColumnCount = 1;
            mainTablePanel.RowCount = 2;
            
            mainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            mainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
        }

        private void ConfigureSeatPanel()
        {
            seatTablePanel = new TableLayoutPanel();
            seatTablePanel.Dock = DockStyle.Fill;
            seatTablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            seatTablePanel.BackColor = Color.LightGray;
            seatTablePanel.Margin = new Padding(2);

            seatTablePanel.ColumnCount = TotalSeatsPerRow;
            seatTablePanel.RowCount = TotalRows;
            
            seatTablePanel.ColumnStyles.Clear();
            for (int i = 0; i < TotalSeatsPerRow; i++)
            {
                seatTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / TotalSeatsPerRow));
            }

            seatTablePanel.RowStyles.Clear();
            for (int i = 0; i < TotalRows; i++)
            {
                seatTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / TotalRows));
            }
        }
        
        private void ConfigureOptionsPanel()
        {
            optionsPanel = new Panel();
            optionsPanel.Dock = DockStyle.Fill;
            optionsPanel.BackColor = Color.LightBlue;
            
            var buttonsOptionTable = new TableLayoutPanel();
            buttonsOptionTable.Dock = DockStyle.Fill;
            buttonsOptionTable.ColumnCount = 2;
            buttonsOptionTable.RowCount = 1;
            
            
            buttonsOptionTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            buttonsOptionTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            buttonsOptionTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            
            optionsPanel.Controls.Add(buttonsOptionTable);

            Button btnBook = new Button();
            btnBook.Text = "Reservar";
            btnBook.Dock = DockStyle.Fill;
            buttonsOptionTable.Controls.Add(btnBook, 0, 0);
            btnBook.Click += BookClicked;

            Button btnBoxOffice = new Button();
            btnBoxOffice.Text = "Faturamento";
            btnBoxOffice.Dock = DockStyle.Fill;
            buttonsOptionTable.Controls.Add(btnBoxOffice, 1, 0);
            btnBoxOffice.Click += ShowBoxOffice;
        }

        private void GenerateSeatButtons()
        {
            Font buttonFont = new Font("Arial", 7, FontStyle.Bold);
            
            seatTablePanel.SuspendLayout();
            
            for (int i = 0; i < TotalRows; i++)
            {
                for (int j = 0; j < TotalSeatsPerRow; j++)
                {
                    Button seatButton = new Button();
                    
                    seatButton.Text = $"{rowLetters[i]}{j + 1}";
                    seatButton.Font = buttonFont;
                    seatButton.BackColor = Color.LightGreen;
                    seatButton.Dock = DockStyle.Fill;
                    seatButton.Margin = new Padding(2);
                    seatButton.Tag = new int[] { i, j };
                    seatButton.Click += SelectSeats;
                    seatTablePanel.Controls.Add(seatButton, j, i);
                }
            }
            
            seatTablePanel.ResumeLayout(true);
        }

        private void SelectSeats(object sender, EventArgs e)
        {
            Button btnSelected = (Button)sender;
            
            if (btnSelected.BackColor == Color.LightGreen)
            {
                btnSelected.BackColor = Color.Yellow;
                selectedButtons.Add(btnSelected);
            }
            else if (btnSelected.BackColor == Color.Yellow)
            {
                btnSelected.BackColor = Color.LightGreen;
                selectedButtons.Remove(btnSelected);
            }
        }

        private void BookClicked(object sender, EventArgs e)
        {
            List<int[]> selectedSeatsData = selectedButtons.Select(btn => (int[])btn.Tag).ToList();
            var occupied = Seats.BookSeatsRange(selectedSeatsData);

            if (occupied.Count > 0)
            {
                foreach (var occupiedSeatData in occupied)
                {
                    var btn = seatTablePanel.GetControlFromPosition(occupiedSeatData[1], occupiedSeatData[0]) as Button;
                    if (btn != null)
                    {
                        btn.BackColor = Color.LightGreen;
                    }
                }
            }

            foreach (var btn in selectedButtons)
            {
                btn.BackColor = Color.Red;
            }
            selectedButtons.Clear();
        }

        private void ShowBoxOffice(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Total de assentos reservados: {Seats.CountBookSeats()}");
            sb.AppendLine($"O Faturamento total é de: R${Seats.BoxOffice}");
            MessageBox.Show(sb.ToString(), "Faturamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        // Evento que ocorre quando o redimensionamento começa
        private void MainWindow_ResizeBegin(object sender, EventArgs e)
        {
            // Suspende o layout do painel principal
            mainTablePanel.SuspendLayout();
        }

        // Evento que ocorre quando o redimensionamento termina
        private void MainWindow_ResizeEnd(object sender, EventArgs e)
        {
            // Ativa o layout novamente
            mainTablePanel.ResumeLayout(true);
        }
    }
}