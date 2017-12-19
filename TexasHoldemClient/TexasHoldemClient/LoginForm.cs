using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TexasHoldemClient.view
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string player_login = login.Text;
            int port = int.Parse(textBox1.Text);
            PokerWindow pokerForm = new PokerWindow(port, player_login);
            pokerForm.Show();
        }

        public void global_FormClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
