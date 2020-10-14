using System;
using System.Drawing;
using System.Windows.Forms;
using CheckersProject;

namespace DamkaForms
{
    public partial class GameForm : Form
    {
        private Button[,] m_Cells;
        private DamkaController m_GameController;
        private bool m_IsWarriorSelected;
        private bool m_IsNewWarriorSelectionAvailable;
        private Button m_WarriorButton;
        private Button m_ButtonSelected;

        public GameForm(DamkaController i_GameController)
        {
            InitializeComponent();
            m_GameController = i_GameController;
            m_Cells = new Button[m_GameController.Size, m_GameController.Size];
            m_IsWarriorSelected = false;
            m_IsNewWarriorSelectionAvailable = true;
            m_GameController.DeskUpdate += new Action<eSymbols[,], int>(updateDesk);
            m_GameController.AlertShow += new Action<string>(showAlert);
            m_GameController.CurrentPlayerChange += new Action<object>(whenPlayerChanged);
            m_GameController.GameOver += new Action<eGameStatus>(gameOver);
            createDesk(m_GameController.Size);            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Show();
            m_GameController.startSession();
        }

        private void createDesk(int i_Size)
        {
            const int buttonSize = 70;

            Width = (i_Size + 2) * buttonSize;
            Height = (i_Size + 2) * buttonSize;

            for (int i = 0; i < i_Size; i++)
            {
                for (int j = 0; j < i_Size; j++)
                {
                    Button button = new Button();
                    button.Name = string.Format("button{0}{1}", i, j);
                    button.Location = new Point(j * buttonSize + buttonSize, i * buttonSize + buttonSize);
                    button.Size = new Size(buttonSize, buttonSize);

                    if ((i + j) % 2 == 0)
                    {
                        button.BackColor = Color.Black;
                        button.Enabled = false;
                    }
                    else
                    {
                        button.Font = new Font(button.Font.FontFamily, 30, FontStyle.Bold);
                        button.BackColor = Color.White;
                        button.Click += new EventHandler(OnButtonPress);
                    }

                    m_Cells[i, j] = button;
                    Controls.Add(button);
                }
            }

            labelPlayer1 = new Label();
            labelPlayer1.Location = new Point(buttonSize * 2, buttonSize / 2);
            labelPlayer1.Text = string.Format("{0}: {1}", m_GameController.Player1Name, m_GameController.PointsPlayer1);
            labelPlayer1.Font = new Font(labelPlayer1.Font.FontFamily, 18, FontStyle.Regular);
            labelPlayer1.Name = "labelPlayer1";
            labelPlayer1.AutoSize = true;
            labelPlayer1.ForeColor = Color.BlueViolet;
            Controls.Add(labelPlayer1);

            labelPlayer2 = new Label();
            labelPlayer2.Location = new Point((i_Size - 1) * buttonSize, buttonSize / 2);
            labelPlayer2.Text = string.Format("{0}: {1}", m_GameController.Player2Name, m_GameController.PointsPlayer2);
            labelPlayer2.Font = new Font(labelPlayer2.Font.FontFamily, 18, FontStyle.Regular);
            labelPlayer2.Name = "labelPlayer2";
            labelPlayer2.AutoSize = true;
            labelPlayer2.ForeColor = Color.BlueViolet;
            Controls.Add(labelPlayer2);
        }

        private void updateDesk(eSymbols[,] i_Desk, int i_Size)
        {
            for (int i = 0; i < i_Size; i++)
            {
                for (int j = 0; j < i_Size; j++)
                {
                    m_Cells[i, j].Text = ((char)i_Desk[i, j]).ToString();
                    if (!m_Cells[i, j].Text.Equals(" "))
                    {
                        m_Cells[i, j].ForeColor = Color.Red;
                    }
                }
            }

            if (m_IsWarriorSelected)
            {
                m_WarriorButton.BackColor = Color.White;
                m_ButtonSelected.BackColor = Color.DodgerBlue;
                m_WarriorButton = m_ButtonSelected;
                m_ButtonSelected = null;
                m_IsNewWarriorSelectionAvailable = false;
            }

            this.Refresh();
        }

        private void OnButtonPress(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (!m_IsWarriorSelected)
            {
                if (!button.Text.Equals(" ")) // select warrior
                {
                    button.BackColor = Color.DodgerBlue;
                    m_WarriorButton = button;
                    m_IsWarriorSelected = true;
                }
                else
                {
                    MessageBox.Show("Please choose your warrior to move!");
                }
            }
            else
            {
                if (button == m_WarriorButton && m_IsNewWarriorSelectionAvailable) // unselect warrior
                {
                    button.BackColor = Color.White;
                    m_WarriorButton = null;
                    m_IsWarriorSelected = false;
                    m_IsNewWarriorSelectionAvailable = true;
                }
                else if (button.Text.Equals(" "))
                {
                    m_ButtonSelected = button;
                    // true if all posible kicks done.
                    if (m_GameController.PlayerMove(string.Format("{0}{1}", m_WarriorButton.Name.Substring(6, 2), button.Name.Substring(6, 2))))
                    {
                        m_WarriorButton.BackColor = Color.White;
                        m_WarriorButton = null;
                        m_ButtonSelected = null;
                        m_IsWarriorSelected = false;
                        m_IsNewWarriorSelectionAvailable = true;
                        m_GameController.TurnOver();
                        m_GameController.checkGameStatus();
                    }
                }
                else if (m_IsNewWarriorSelectionAvailable)
                {
                    m_ButtonSelected = null;
                    MessageBox.Show("Selected cell must be empty!");
                }
                else
                {
                    m_ButtonSelected = null;
                    MessageBox.Show("You must kick again!");
                }
            }
        }

        private void disableOpponentCells(object i_Opponnet)
        {
            if (m_GameController.GameMode == eGameMode.HUMAN)
            {
                foreach (Button cell in m_Cells)
                {
                    if (cell.Text.Equals(((char)(i_Opponnet as User).Symbol).ToString()) || cell.Text.Equals(((char)(i_Opponnet as User).SymbolKing).ToString()))
                    {
                        cell.Enabled = false;
                    }
                    else if (!(cell.BackColor == Color.Black))
                    {
                        cell.Enabled = true;
                    }
                }
            }
            else
            {
                foreach (Button cell in m_Cells)
                {
                    if (cell.Text.Equals(((char)(i_Opponnet as ComputerDummy).Symbol).ToString()) || cell.Text.Equals(((char)(i_Opponnet as ComputerDummy).SymbolKing).ToString()))
                    {
                        cell.Enabled = false;
                    }
                    else if (!(cell.BackColor == Color.Black))
                    {
                        cell.Enabled = true;
                    }
                }
            }
        }

        private void showAlert(string i_Message)
        {
            MessageBox.Show(i_Message);
        }

        private void whenPlayerChanged(object i_Player)
        {
            disableOpponentCells(i_Player);
        }

        private void gameOver(eGameStatus i_GameStatus)
        {
            DialogResult dialogResult = DialogResult.None;

            switch (i_GameStatus)
            {
                case eGameStatus.TIE:
                    dialogResult = MessageBox.Show(string.Format("Tie!{0}Another Round?", Environment.NewLine), "Damka", MessageBoxButtons.YesNo);
                    break;
                case eGameStatus.WIN_PLAYER1:
                    dialogResult = MessageBox.Show(string.Format("{0} Won!{1}Another Round?", m_GameController.Player1Name, Environment.NewLine), "Damka", MessageBoxButtons.YesNo);
                    break;
                case eGameStatus.WIN_PLAYER2:
                    dialogResult = MessageBox.Show(string.Format("{0} Won{1}Another Round?", m_GameController.Player2Name, Environment.NewLine), "Damka", MessageBoxButtons.YesNo);
                    break;
            }

            if (dialogResult == DialogResult.Yes)
            {
                labelPlayer1.Text = string.Format("{0}: {1}", m_GameController.Player1Name, m_GameController.PointsPlayer1);
                labelPlayer2.Text = string.Format("{0}: {1}", m_GameController.Player2Name, m_GameController.PointsPlayer2);
            }
            else if (dialogResult == DialogResult.No)
            {
                Application.Exit();
            }
        }
    }
}
