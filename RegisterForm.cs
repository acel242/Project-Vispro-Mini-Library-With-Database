namespace Tugas_1_Kelompok_2

{
    public partial class RegisterForm : Form
    {
        private DatabaseHelper db;

        public RegisterForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text;
            string fullname = txtFullName.Text;
            string phone = txtPhone.Text;

            // Validasi input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(fullname))
            {
                MessageBox.Show("Username, password, email, dan nama lengkap harus diisi!");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Password dan konfirmasi password tidak sama!");
                return;
            }

            if (password.Length < 3)
            {
                MessageBox.Show("Password minimal 3 karakter!");
                return;
            }

            // Cek apakah username sudah ada
            string checkQuery = $"SELECT COUNT(*) FROM users WHERE username='{username}' OR email='{email}'";
            var count = db.GetSingleValue(checkQuery);

            if (count != null && Convert.ToInt32(count) > 0)
            {
                MessageBox.Show("Username atau email sudah digunakan!");
                return;
            }

            // Generate user ID
            string userid = "USR" + DateTime.Now.ToString("yyyyMMddHHmmss");

            string query = $@"INSERT INTO users (user_id, username, email, password_hash, full_name, phone_number, role, status) 
                            VALUES ('{userid}', '{username}', '{email}', '{password}', '{fullname}', '{phone}', 'USER', 'PENDING')";

            if (db.ExecuteQuery(query))
            {
                MessageBox.Show("Pendaftaran berhasil! Akun Anda menunggu persetujuan staff.");
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
            else
            {
                MessageBox.Show("Pendaftaran gagal! Silakan coba lagi.");
            }
        }

        private void btnGoToLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            btnRegister = new Button();
            btnGoToLogin = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Arial", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(100, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(200, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "DAFTAR AKUN BARU";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            lblUsername.Location = new Point(50, 70);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(100, 20);
            lblUsername.TabIndex = 1;
            lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(150, 70);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(180, 27);
            txtUsername.TabIndex = 2;
            // 
            // lblPassword
            // 
            lblPassword.Location = new Point(50, 110);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(100, 20);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(150, 110);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(180, 27);
            txtPassword.TabIndex = 4;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.Location = new Point(50, 150);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(100, 20);
            lblConfirmPassword.TabIndex = 5;
            lblConfirmPassword.Text = "Konfirmasi Password:";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(150, 150);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '*';
            txtConfirmPassword.Size = new Size(180, 27);
            txtConfirmPassword.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.Location = new Point(50, 190);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 20);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(150, 190);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(180, 27);
            txtEmail.TabIndex = 8;
            // 
            // lblFullName
            // 
            lblFullName.Location = new Point(50, 230);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(100, 20);
            lblFullName.TabIndex = 9;
            lblFullName.Text = "Nama Lengkap:";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(150, 230);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(180, 27);
            txtFullName.TabIndex = 10;
            // 
            // lblPhone
            // 
            lblPhone.Location = new Point(50, 270);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(100, 20);
            lblPhone.TabIndex = 11;
            lblPhone.Text = "No. Telepon:";
            // 
            // txtPhone
            // 
            txtPhone.Location = new Point(150, 270);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(180, 27);
            txtPhone.TabIndex = 12;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.LightGreen;
            btnRegister.Location = new Point(150, 320);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(80, 30);
            btnRegister.TabIndex = 13;
            btnRegister.Text = "Daftar";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // btnGoToLogin
            // 
            btnGoToLogin.BackColor = Color.Transparent;
            btnGoToLogin.FlatStyle = FlatStyle.Flat;
            btnGoToLogin.Location = new Point(100, 360);
            btnGoToLogin.Name = "btnGoToLogin";
            btnGoToLogin.Size = new Size(180, 25);
            btnGoToLogin.TabIndex = 14;
            btnGoToLogin.Text = "Sudah punya akun? Login";
            btnGoToLogin.UseVisualStyleBackColor = false;
            btnGoToLogin.Click += btnGoToLogin_Click;
            // 
            // RegisterForm
            // 
            ClientSize = new Size(382, 403);
            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(lblConfirmPassword);
            Controls.Add(txtConfirmPassword);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblFullName);
            Controls.Add(txtFullName);
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
            Controls.Add(btnRegister);
            Controls.Add(btnGoToLogin);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Daftar - Mini Library";
            Load += RegisterForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle, lblUsername, lblPassword, lblConfirmPassword, lblEmail, lblFullName, lblPhone;
        private TextBox txtUsername, txtPassword, txtConfirmPassword, txtEmail, txtFullName, txtPhone;
        private Button btnRegister, btnGoToLogin;

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}