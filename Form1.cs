using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjektZtp
{
    public partial class BattleshipGameForm : Form
    {
        private UserControl currentControl;
        GameBuilder gameBuilder = new GameBuilder();

        public BattleshipGameForm()
        {
            InitializeComponent();

            currentControl = new AiSetupControl(this, gameBuilder);
            this.Controls.Add(currentControl);
            CenterControl(currentControl); // Wycentrowanie kontrolki
        }

        public void ShowCurrentControl(UserControl control)
        {
            this.Controls.Remove(currentControl);
            currentControl = control;
            this.Controls.Add(currentControl);
            CenterControl(currentControl); // Wycentrowanie nowej kontrolki
        }

        private void CenterControl(Control control)
        {
            // Obliczanie środka formularza i ustawianie lokalizacji kontrolki
            control.Left = (this.ClientSize.Width - control.Width) / 2;
            control.Top = (this.ClientSize.Height - control.Height) / 2;
        }

        public void ResetGame()
        {
            // Resetowanie komponentów związanych z grą
            gameBuilder = new GameBuilder();  // Ponowne zainicjowanie obiektu GameBuilder

            // Tworzenie nowego obiektu formularza i ustawienie kontrolki
            var newAiSetupControl = new AiSetupControl(this, gameBuilder);
            ShowCurrentControl(newAiSetupControl);  // Pokazuje nowy ekran ustawiania gry

            // Opcjonalnie: wyczyszczenie wyników, jeśli są wyświetlane na formularzu
            // W takim przypadku można tutaj dodać kod czyszczący wyniki (np. labelki)
            // scoreLabel.Text = string.Empty;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Upewnij się, że aktualna kontrolka pozostaje wycentrowana przy zmianie rozmiaru okna
            if (currentControl != null)
            {
                CenterControl(currentControl);
            }
        }
    }
}