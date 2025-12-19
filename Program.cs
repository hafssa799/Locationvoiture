using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LocationVoitures.BackOffice.Forms;

namespace LocationVoitures.BackOffice
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Ensure Database Schema
            LocationVoitures.BackOffice.DAL.MigrationHelper.EnsurePhotoColumnExists();

            // Afficher l'écran de bienvenue animé
            WelcomeForm welcomeForm = new WelcomeForm();
            if (welcomeForm.ShowDialog() == DialogResult.OK)
            {
                // Afficher le formulaire de connexion après le bienvenue
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
