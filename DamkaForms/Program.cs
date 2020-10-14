using System;
using CheckersProject;
using System.Windows.Forms;

namespace DamkaForms
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SettingsForm m_SettingsForm = new SettingsForm();
            DialogResult dResult = m_SettingsForm.ShowDialog();
            if (dResult == DialogResult.OK)
            {
                DamkaController m_DamkaController = new DamkaController(m_SettingsForm.DeskSize, m_SettingsForm.NamePlayer1, m_SettingsForm.NamePlayer2, m_SettingsForm.IsTwoPlayers);
                Application.Run(new GameForm(m_DamkaController));
            }
        }
    }
}
