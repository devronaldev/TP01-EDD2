using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SeatHandler;
using System.Collections.Generic;

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
        private const int TotalSeatsPerRow = 10;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeApplicationLayout();
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
            
            mainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            mainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        }

        private void ConfigureSeatPanel()
        {
            seatTablePanel = new TableLayoutPanel();
            seatTablePanel.Dock = DockStyle.Fill;
            seatTablePanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            seatTablePanel.BackColor = Color.LightGray;

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

            Button btnBook = new Button();
            btnBook.Text = "Reservar";
            btnBook.Dock = DockStyle.Top;
            optionsPanel.Controls.Add(btnBook);
            btnBook.Click += BookClicked;
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
                    seatButton.Tag = new int[] { i, j }; // Use a propriedade Tag
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
    }
}