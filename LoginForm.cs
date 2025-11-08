
using System;
using System.Windows.Forms;

namespace Tugas_1_Kelompok_2
{
    public partial class LoginForm : Form
    {
        private DatabaseHelper db;
        public static LoginForm Instance;

        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnGoToRegister;
        private Button btnExit;

        public LoginForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
            Instance = this;
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblUsername = new Label();
            lblPassword = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnGoToRegister = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Arial", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(100, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(200, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "MINI LIBRARY";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(50, 100);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(78, 20);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(50, 140);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(73, 20);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(130, 97);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(150, 27);
            txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(130, 137);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(150, 27);
            txtPassword.TabIndex = 4;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.LightBlue;
            btnLogin.Location = new Point(130, 180);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(80, 30);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnGoToRegister
            // 
            btnGoToRegister.BackColor = Color.Transparent;
            btnGoToRegister.FlatStyle = FlatStyle.Flat;
            btnGoToRegister.Location = new Point(80, 220);
            btnGoToRegister.Name = "btnGoToRegister";
            btnGoToRegister.Size = new Size(180, 25);
            btnGoToRegister.TabIndex = 6;
            btnGoToRegister.Text = "Belum punya akun? Daftar";
            btnGoToRegister.UseVisualStyleBackColor = false;
            btnGoToRegister.Click += btnGoToRegister_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.LightCoral;
            btnExit.Location = new Point(130, 260);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(80, 30);
            btnExit.TabIndex = 7;
            btnExit.Text = "Keluar Aplikasi";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // LoginForm
            // 
            ClientSize = new Size(350, 350);
            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnGoToRegister);
            Controls.Add(btnExit);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login - Mini Library";
            FormClosing += LoginForm_FormClosing;
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username dan password harus diisi!");
                return;
            }

            try
            {
                string query = $"SELECT user_id, username, role, status FROM users WHERE username='{username}' AND password_hash='{password}' AND status='ACTIVE'";
                var result = db.GetData(query);

                if (result.Rows.Count > 0)
                {
                    string role = result.Rows[0]["role"].ToString();
                    string userid = result.Rows[0]["user_id"].ToString();

                    this.Hide();

                    if (role == "STAFF" || role == "ADMIN")
                    {
                        StaffForm staffForm = new StaffForm(userid, role);
                        staffForm.Show();
                    }
                    else
                    {
                        UserForm userForm = new UserForm(userid);
                        userForm.Show();
                    }

                    txtUsername.Clear();
                    txtPassword.Clear();
                }
                else
                {
                    MessageBox.Show("Login gagal! Periksa username dan password.");
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnGoToRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Application.Exit();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}