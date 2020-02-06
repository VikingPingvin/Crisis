using Crisis.Model;
using System;
using System.Windows.Forms;

namespace Crisis.View
{
    public partial class LoginForm : Form
    {
        private readonly CrisisModel model;

        public string Username => emailBox.Text;
        public string Password => passwordBox.Text;

        public event Action LoginPressed;
        public event Action RegisterSubmitPressed;

        public string Error
        {
            get => errorStatus.Text;
            set => errorStatus.Text = value;
        }

        public LoginForm(CrisisModel model)
        {
            this.model = model;
            InitializeComponent();
            Error = string.Empty;
        }

        private void login_Click(object sender, EventArgs e)
        {
            LoginPressed?.Invoke();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterSubmitPressed?.Invoke();
        }
    }
}
