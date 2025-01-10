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
    public partial class MainGameControl : UserControl
    {
        BattleshipGameForm gameForm;
        public MainGameControl(BattleshipGameForm battleshipGameForm)
        {
            InitializeComponent();
            gameForm = battleshipGameForm;
        }

        private void initGame()
        {

        }
    }
}
