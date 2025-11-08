using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Tugas_1_Kelompok_2
{
    public partial class StaffForm : Form
    {
        private DatabaseHelper db;
        private string staffId;
        private string staffRole;

        // Controls declaration
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private DataGridView dataGridPendingUsers;
        private DataGridView dataGridAllLoans;
        private DataGridView dataGridOverdue;
        private DataGridView dataGridAllUsers;
        private Button btnApproveUser;
        private Button btnRejectUser;
        private Button btnRefresh;
        private Button btnLogout;
        private Button btnAddBook;
        private Button btnUpdateRole;
        private Button btnDeleteUser;
        private TextBox txtTitle;
        private TextBox txtAuthor;
        private TextBox txtCategory;
        private NumericUpDown numericCopies;
        private ComboBox comboRole;
        private ComboBox comboStatus;
        private Label lblTitle;
        private Label lblAuthor;
        private Label lblCategory;
        private Label lblCopies;
        private Label label13;
        private Label label14;

        public StaffForm(string staffId, string staffRole)
        {
            InitializeComponent();
            db = new DatabaseHelper();
            this.staffId = staffId;
            this.staffRole = staffRole;

            this.Text = $"Mini Library - {staffRole}";

            LoadPendingUsers();
            LoadAllLoans();
            LoadOverdueBooks();
            LoadAllUsers();

            // Sembunyikan tab manage users jika bukan admin
            if (staffRole != "ADMIN")
            {
                tabControl1.TabPages.Remove(tabPage5);
            }
        }

        private void LoadPendingUsers()
        {
            string query = "SELECT user_id, username, email, full_name, registration_date FROM users WHERE status = 'PENDING'";
            DataTable dt = db.GetData(query);
            dataGridPendingUsers.DataSource = dt;
        }

        private void LoadAllLoans()
        {
            string query = @"SELECT bl.loan_id, u.username, b.title, bl.borrow_date, bl.due_date, bl.status 
                           FROM book_loans bl 
                           JOIN users u ON bl.user_id = u.user_id 
                           JOIN books b ON bl.book_id = b.book_id 
                           ORDER BY bl.borrow_date DESC";
            DataTable dt = db.GetData(query);
            dataGridAllLoans.DataSource = dt;
        }

        private void LoadOverdueBooks()
        {
            string query = @"SELECT bl.loan_id, u.username, b.title, bl.due_date, 
                           DATEDIFF(CURDATE(), bl.due_date) as days_overdue
                           FROM book_loans bl 
                           JOIN users u ON bl.user_id = u.user_id 
                           JOIN books b ON bl.book_id = b.book_id 
                           WHERE bl.status = 'BORROWED' AND CURDATE() > bl.due_date";
            DataTable dt = db.GetData(query);
            dataGridOverdue.DataSource = dt;
        }

        private void LoadAllUsers()
        {
            string query = "SELECT user_id, username, email, full_name, role, status, current_borrow_count FROM users";
            DataTable dt = db.GetData(query);
            dataGridAllUsers.DataSource = dt;
        }

        private void btnApproveUser_Click(object sender, EventArgs e)
        {
            if (dataGridPendingUsers.CurrentRow == null)
            {
                MessageBox.Show("Pilih user yang akan disetujui!");
                return;
            }

            string userId = dataGridPendingUsers.CurrentRow.Cells["user_id"].Value?.ToString() ?? "";
            string username = dataGridPendingUsers.CurrentRow.Cells["username"].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("Data user tidak valid!");
                return;
            }

            string query = $"UPDATE users SET status = 'ACTIVE' WHERE user_id = '{userId}'";
            if (db.ExecuteQuery(query))
            {
                MessageBox.Show($"User {username} berhasil disetujui!");
                LoadPendingUsers();
            }
        }

        private void btnRejectUser_Click(object sender, EventArgs e)
        {
            if (dataGridPendingUsers.CurrentRow == null)
            {
                MessageBox.Show("Pilih user yang akan ditolak!");
                return;
            }

            string userId = dataGridPendingUsers.CurrentRow.Cells["user_id"].Value?.ToString() ?? "";
            string username = dataGridPendingUsers.CurrentRow.Cells["username"].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("Data user tidak valid!");
                return;
            }

            DialogResult result = MessageBox.Show($"Yakin tolak user {username}?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            string query = $"DELETE FROM users WHERE user_id = '{userId}'";
            if (db.ExecuteQuery(query))
            {
                MessageBox.Show($"User {username} berhasil ditolak!");
                LoadPendingUsers();
            }
        }

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string category = txtCategory.Text.Trim();
            int copies = (int)numericCopies.Value;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author))
            {
                MessageBox.Show("Judul dan penulis harus diisi!");
                return;
            }

            string bookId = "BK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string query = $@"INSERT INTO books (book_id, title, author, category, total_copies, available_copies) 
                            VALUES ('{bookId}', '{title}', '{author}', '{category}', {copies}, {copies})";

            if (db.ExecuteQuery(query))
            {
                MessageBox.Show("Buku berhasil ditambahkan!");
                txtTitle.Clear();
                txtAuthor.Clear();
                txtCategory.Clear();
                numericCopies.Value = 1;
            }
        }

        private void btnUpdateRole_Click(object sender, EventArgs e)
        {
            if (dataGridAllUsers.CurrentRow == null)
            {
                MessageBox.Show("Pilih user yang akan diupdate!");
                return;
            }

            string userId = dataGridAllUsers.CurrentRow.Cells["user_id"].Value?.ToString() ?? "";
            string username = dataGridAllUsers.CurrentRow.Cells["username"].Value?.ToString() ?? "";
            string newRole = comboRole.Text;
            string newStatus = comboStatus.Text;

            if (string.IsNullOrEmpty(newRole) || string.IsNullOrEmpty(newStatus))
            {
                MessageBox.Show("Pilih role dan status!");
                return;
            }

            string query = $@"UPDATE users 
                            SET role = '{newRole}', status = '{newStatus}'
                            WHERE user_id = '{userId}'";

            if (db.ExecuteQuery(query))
            {
                MessageBox.Show($"User {username} berhasil diupdate!");
                LoadAllUsers();
                LoadPendingUsers();
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dataGridAllUsers.CurrentRow == null)
            {
                MessageBox.Show("Pilih user yang akan dihapus!");
                return;
            }

            string userId = dataGridAllUsers.CurrentRow.Cells["user_id"].Value?.ToString() ?? "";
            string username = dataGridAllUsers.CurrentRow.Cells["username"].Value?.ToString() ?? "";

            if (MessageBox.Show($"Yakin hapus user {username}?", "Konfirmasi",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string query = $"DELETE FROM users WHERE user_id = '{userId}'";
                if (db.ExecuteQuery(query))
                {
                    MessageBox.Show($"User {username} berhasil dihapus!");
                    LoadAllUsers();
                    LoadPendingUsers();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPendingUsers();
            LoadAllLoans();
            LoadOverdueBooks();
            LoadAllUsers();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (LoginForm.Instance != null)
            {
                this.Hide();
                LoginForm.Instance.Show();
            }
        }

        private void InitializeComponent()
        {
            // Initialize all controls
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.tabPage2 = new TabPage();
            this.tabPage3 = new TabPage();
            this.tabPage4 = new TabPage();
            this.tabPage5 = new TabPage();
            this.dataGridPendingUsers = new DataGridView();
            this.dataGridAllLoans = new DataGridView();
            this.dataGridOverdue = new DataGridView();
            this.dataGridAllUsers = new DataGridView();
            this.btnApproveUser = new Button();
            this.btnRejectUser = new Button();
            this.btnRefresh = new Button();
            this.btnLogout = new Button();
            this.btnAddBook = new Button();
            this.btnUpdateRole = new Button();
            this.btnDeleteUser = new Button();
            this.txtTitle = new TextBox();
            this.txtAuthor = new TextBox();
            this.txtCategory = new TextBox();
            this.numericCopies = new NumericUpDown();
            this.comboRole = new ComboBox();
            this.comboStatus = new ComboBox();
            this.lblTitle = new Label();
            this.lblAuthor = new Label();
            this.lblCategory = new Label();
            this.lblCopies = new Label();
            this.label13 = new Label();
            this.label14 = new Label();

            this.SuspendLayout();

            // StaffForm
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Text = "Mini Library - Staff";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // tabControl1
            this.tabControl1.Location = new System.Drawing.Point(10, 10);
            this.tabControl1.Size = new System.Drawing.Size(860, 500);

            // tabPage1 - Pending Users
            this.tabPage1.Text = "Pending Users";
            this.tabPage1.BackColor = Color.White;

            // dataGridPendingUsers
            this.dataGridPendingUsers.Location = new System.Drawing.Point(10, 10);
            this.dataGridPendingUsers.Size = new System.Drawing.Size(800, 350);
            this.dataGridPendingUsers.ReadOnly = true;
            this.dataGridPendingUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridPendingUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // btnApproveUser
            this.btnApproveUser.Location = new System.Drawing.Point(10, 370);
            this.btnApproveUser.Size = new System.Drawing.Size(100, 30);
            this.btnApproveUser.Text = "Setujui User";
            this.btnApproveUser.BackColor = Color.LightGreen;
            this.btnApproveUser.Click += new EventHandler(btnApproveUser_Click);

            // btnRejectUser
            this.btnRejectUser.Location = new System.Drawing.Point(120, 370);
            this.btnRejectUser.Size = new System.Drawing.Size(100, 30);
            this.btnRejectUser.Text = "Tolak User";
            this.btnRejectUser.BackColor = Color.LightCoral;
            this.btnRejectUser.Click += new EventHandler(btnRejectUser_Click);

            // Add controls to tabPage1
            this.tabPage1.Controls.Add(dataGridPendingUsers);
            this.tabPage1.Controls.Add(btnApproveUser);
            this.tabPage1.Controls.Add(btnRejectUser);

            // tabPage2 - All Loans
            this.tabPage2.Text = "Semua Peminjaman";
            this.tabPage2.BackColor = Color.White;

            // dataGridAllLoans
            this.dataGridAllLoans.Location = new System.Drawing.Point(10, 10);
            this.dataGridAllLoans.Size = new System.Drawing.Size(800, 400);
            this.dataGridAllLoans.ReadOnly = true;
            this.dataGridAllLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add controls to tabPage2
            this.tabPage2.Controls.Add(dataGridAllLoans);

            // tabPage3 - Overdue Books
            this.tabPage3.Text = "Buku Terlambat";
            this.tabPage3.BackColor = Color.White;

            // dataGridOverdue
            this.dataGridOverdue.Location = new System.Drawing.Point(10, 10);
            this.dataGridOverdue.Size = new System.Drawing.Size(800, 400);
            this.dataGridOverdue.ReadOnly = true;
            this.dataGridOverdue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Add controls to tabPage3
            this.tabPage3.Controls.Add(dataGridOverdue);

            // tabPage4 - Add Book
            this.tabPage4.Text = "Tambah Buku";
            this.tabPage4.BackColor = Color.White;

            // lblTitle
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Size = new System.Drawing.Size(80, 20);
            this.lblTitle.Text = "Judul:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(100, 20);
            this.txtTitle.Size = new System.Drawing.Size(200, 20);

            // lblAuthor
            this.lblAuthor.Location = new System.Drawing.Point(20, 60);
            this.lblAuthor.Size = new System.Drawing.Size(80, 20);
            this.lblAuthor.Text = "Penulis:";

            // txtAuthor
            this.txtAuthor.Location = new System.Drawing.Point(100, 60);
            this.txtAuthor.Size = new System.Drawing.Size(200, 20);

            // lblCategory
            this.lblCategory.Location = new System.Drawing.Point(20, 100);
            this.lblCategory.Size = new System.Drawing.Size(80, 20);
            this.lblCategory.Text = "Kategori:";

            // txtCategory
            this.txtCategory.Location = new System.Drawing.Point(100, 100);
            this.txtCategory.Size = new System.Drawing.Size(200, 20);

            // lblCopies
            this.lblCopies.Location = new System.Drawing.Point(20, 140);
            this.lblCopies.Size = new System.Drawing.Size(80, 20);
            this.lblCopies.Text = "Jumlah:";

            // numericCopies
            this.numericCopies.Location = new System.Drawing.Point(100, 140);
            this.numericCopies.Size = new System.Drawing.Size(100, 20);
            this.numericCopies.Minimum = 1;
            this.numericCopies.Maximum = 100;
            this.numericCopies.Value = 1;

            // btnAddBook
            this.btnAddBook.Location = new System.Drawing.Point(100, 180);
            this.btnAddBook.Size = new System.Drawing.Size(100, 30);
            this.btnAddBook.Text = "Tambah Buku";
            this.btnAddBook.BackColor = Color.LightBlue;
            this.btnAddBook.Click += new EventHandler(btnAddBook_Click);

            // Add controls to tabPage4
            this.tabPage4.Controls.Add(lblTitle);
            this.tabPage4.Controls.Add(txtTitle);
            this.tabPage4.Controls.Add(lblAuthor);
            this.tabPage4.Controls.Add(txtAuthor);
            this.tabPage4.Controls.Add(lblCategory);
            this.tabPage4.Controls.Add(txtCategory);
            this.tabPage4.Controls.Add(lblCopies);
            this.tabPage4.Controls.Add(numericCopies);
            this.tabPage4.Controls.Add(btnAddBook);

            // tabPage5 - Manage Users
            this.tabPage5.Text = "Manage Users";
            this.tabPage5.BackColor = Color.White;

            // dataGridAllUsers
            this.dataGridAllUsers.Location = new System.Drawing.Point(10, 10);
            this.dataGridAllUsers.Size = new System.Drawing.Size(800, 300);
            this.dataGridAllUsers.ReadOnly = true;
            this.dataGridAllUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridAllUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // label13 - Role
            this.label13.Location = new System.Drawing.Point(10, 320);
            this.label13.Size = new System.Drawing.Size(50, 20);
            this.label13.Text = "Role:";

            // comboRole
            this.comboRole.Location = new System.Drawing.Point(60, 320);
            this.comboRole.Size = new System.Drawing.Size(100, 20);
            this.comboRole.Items.AddRange(new object[] { "USER", "STAFF", "ADMIN" });

            // label14 - Status
            this.label14.Location = new System.Drawing.Point(170, 320);
            this.label14.Size = new System.Drawing.Size(50, 20);
            this.label14.Text = "Status:";

            // comboStatus
            this.comboStatus.Location = new System.Drawing.Point(220, 320);
            this.comboStatus.Size = new System.Drawing.Size(100, 20);
            this.comboStatus.Items.AddRange(new object[] { "PENDING", "ACTIVE", "SUSPENDED" });

            // btnUpdateRole
            this.btnUpdateRole.Location = new System.Drawing.Point(330, 320);
            this.btnUpdateRole.Size = new System.Drawing.Size(100, 25);
            this.btnUpdateRole.Text = "Update Role";
            this.btnUpdateRole.Click += new EventHandler(btnUpdateRole_Click);

            // btnDeleteUser
            this.btnDeleteUser.Location = new System.Drawing.Point(440, 320);
            this.btnDeleteUser.Size = new System.Drawing.Size(100, 25);
            this.btnDeleteUser.Text = "Hapus User";
            this.btnDeleteUser.BackColor = Color.LightCoral;
            this.btnDeleteUser.Click += new EventHandler(btnDeleteUser_Click);

            // Add controls to tabPage5
            this.tabPage5.Controls.Add(dataGridAllUsers);
            this.tabPage5.Controls.Add(label13);
            this.tabPage5.Controls.Add(comboRole);
            this.tabPage5.Controls.Add(label14);
            this.tabPage5.Controls.Add(comboStatus);
            this.tabPage5.Controls.Add(btnUpdateRole);
            this.tabPage5.Controls.Add(btnDeleteUser);

            // Add tabs to tabControl
            this.tabControl1.TabPages.Add(tabPage1);
            this.tabControl1.TabPages.Add(tabPage2);
            this.tabControl1.TabPages.Add(tabPage3);
            this.tabControl1.TabPages.Add(tabPage4);
            this.tabControl1.TabPages.Add(tabPage5);

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(640, 520);
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.Text = "Refresh Data";
            this.btnRefresh.Click += new EventHandler(btnRefresh_Click);

            // btnLogout
            this.btnLogout.Location = new System.Drawing.Point(750, 520);
            this.btnLogout.Size = new System.Drawing.Size(100, 30);
            this.btnLogout.Text = "LOGOUT";
            this.btnLogout.BackColor = Color.LightCoral;
            this.btnLogout.Click += new EventHandler(btnLogout_Click);

            // Add controls to form
            this.Controls.Add(tabControl1);
            this.Controls.Add(btnRefresh);
            this.Controls.Add(btnLogout);

            this.ResumeLayout(false);
        }
    }
}