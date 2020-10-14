using System.Windows.Forms;
using CheckersProject;

namespace DamkaForms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public int DeskSize
        {
            get
            {
                int size = 0;

                foreach (RadioButton radio in groupBoxSize.Controls)
                {
                    if (radio.Checked)
                    {
                        int.TryParse(radio.Name.Substring(11).ToString(), out size);
                        break;
                    }
                }

                return size;
            }
        }

        public string NamePlayer1
        {
            get { return textBoxPlayer1.Text; }
        }

        public string NamePlayer2
        {
            get { return textBoxPlayer2.Text; }
        }

        public bool IsTwoPlayers
        {
            get { return checkBoxTwoPlayers.Checked; }
        }

        private void checkBoxTwoPlayersCheckedChanged(object sender, System.EventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;

            if (checkbox.Checked)
            {
                textBoxPlayer2.Enabled = true;
                textBoxPlayer2.Text = string.Empty;
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                textBoxPlayer2.Text = "Computer";
            }
        }

        private void ButtonDone_Click(object sender, System.EventArgs e)
        {
            if (textBoxPlayer1.Text != string.Empty && textBoxPlayer2.Text != string.Empty)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter player name");
            }
        }  
    }
}
