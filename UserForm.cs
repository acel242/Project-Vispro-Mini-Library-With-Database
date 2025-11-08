using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Tugas_1_Kelompok_2

{
    public partial class UserForm : Form
    {
        private DatabaseHelper db;
        private string userId;
        private string selectedLoanId;

        private DataGridView dataGridBooks;
        private DataGridView dataGridMyLoans;
        private Button btnBorrow;
        private Button btnReturn;
        private Button btnRefresh;
        private Button btnLogout;
        private Label lblSelectedBook;
        private GroupBox groupBox1;
        private GroupBox groupBox2;

        public UserForm(string userId)
        {
            InitializeComponent();
            db = new DatabaseHelper();
            this.userId = userId;
            LoadBooks();
            LoadMyLoans();
        }

        private void InitializeComponent()
        {
            dataGridBooks = new DataGridView();
            dataGridMyLoans = new DataGridView();
            btnBorrow = new Button();
            btnReturn = new Button();
            btnRefresh = new Button();
            btnLogout = new Button();
            lblSelectedBook = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)dataGridBooks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridMyLoans).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridBooks
            // 
            dataGridBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridBooks.BackgroundColor = Color.LightCyan;
            dataGridBooks.ColumnHeadersHeight = 29;
            dataGridBooks.Location = new Point(15, 25);
            dataGridBooks.Name = "dataGridBooks";
            dataGridBooks.ReadOnly = true;
            dataGridBooks.RowHeadersWidth = 51;
            dataGridBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridBooks.Size = new Size(770, 150);
            dataGridBooks.TabIndex = 0;
            // 
            // dataGridMyLoans
            // 
            dataGridMyLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridMyLoans.BackgroundColor = Color.LightYellow;
            dataGridMyLoans.ColumnHeadersHeight = 29;
            dataGridMyLoans.Location = new Point(15, 25);
            dataGridMyLoans.Name = "dataGridMyLoans";
            dataGridMyLoans.ReadOnly = true;
            dataGridMyLoans.RowHeadersWidth = 51;
            dataGridMyLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridMyLoans.Size = new Size(770, 150);
            dataGridMyLoans.TabIndex = 0;
            dataGridMyLoans.CellContentClick += dataGridMyLoans_CellContentClick;
            dataGridMyLoans.SelectionChanged += dataGridMyLoans_SelectionChanged;
            // 
            // btnBorrow
            // 
            btnBorrow.BackColor = Color.LightGreen;
            btnBorrow.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnBorrow.Location = new Point(20, 230);
            btnBorrow.Name = "btnBorrow";
            btnBorrow.Size = new Size(120, 35);
            btnBorrow.TabIndex = 3;
            btnBorrow.Text = "PINJAM BUKU";
            btnBorrow.UseVisualStyleBackColor = false;
            btnBorrow.Click += btnBorrow_Click;
            // 
            // btnReturn
            // 
            btnReturn.BackColor = Color.LightCoral;
            btnReturn.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnReturn.Location = new Point(150, 230);
            btnReturn.Name = "btnReturn";
            btnReturn.Size = new Size(120, 50);
            btnReturn.TabIndex = 4;
            btnReturn.Text = "KEMBALIKAN BUKU";
            btnReturn.UseVisualStyleBackColor = false;
            btnReturn.Click += btnReturn_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnRefresh.Location = new Point(280, 230);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(80, 35);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "REFRESH";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.LightCoral;
            btnLogout.Font = new Font("Arial", 9F, FontStyle.Bold);
            btnLogout.Location = new Point(720, 570);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(100, 35);
            btnLogout.TabIndex = 6;
            btnLogout.Text = "LOGOUT";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // lblSelectedBook
            // 
            lblSelectedBook.Font = new Font("Arial", 9F, FontStyle.Bold);
            lblSelectedBook.ForeColor = Color.Blue;
            lblSelectedBook.Location = new Point(20, 540);
            lblSelectedBook.Name = "lblSelectedBook";
            lblSelectedBook.Size = new Size(500, 25);
            lblSelectedBook.TabIndex = 2;
            lblSelectedBook.Text = "Tidak ada buku dipilih";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGridBooks);
            groupBox1.Font = new Font("Arial", 9F, FontStyle.Bold);
            groupBox1.Location = new Point(20, 20);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(800, 200);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "BUKU TERSEEDIA (Untuk Dipinjam)";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridMyLoans);
            groupBox2.Font = new Font("Arial", 9F, FontStyle.Bold);
            groupBox2.Location = new Point(20, 280);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(800, 250);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "BUKU YANG SEDANG DIPINJAM (Untuk Dikembalikan)";
            // 
            // UserForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(850, 650);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Controls.Add(lblSelectedBook);
            Controls.Add(btnBorrow);
            Controls.Add(btnReturn);
            Controls.Add(btnRefresh);
            Controls.Add(btnLogout);
            Name = "UserForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mini Library - User";
            ((System.ComponentModel.ISupportInitialize)dataGridBooks).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridMyLoans).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void LoadBooks()
        {
            string query = "SELECT book_id, title, author, available_copies FROM books WHERE available_copies > 0 AND status = 'AVAILABLE'";
            DataTable dt = db.GetData(query);
            dataGridBooks.DataSource = dt;
            dataGridBooks.ClearSelection();
        }

        private void LoadMyLoans()
        {
            string query = $@"SELECT 
                bl.loan_id, 
                bl.book_id,
                b.title, 
                DATE_FORMAT(bl.borrow_date, '%d-%m-%Y') as borrow_date, 
                DATE_FORMAT(bl.due_date, '%d-%m-%Y') as due_date, 
                bl.status,
                DATEDIFF(CURDATE(), bl.due_date) as days_overdue
                FROM book_loans bl 
                JOIN books b ON bl.book_id = b.book_id 
                WHERE bl.user_id = '{userId}' 
                AND bl.status = 'BORROWED'
                ORDER BY bl.borrow_date DESC";

            DataTable dt = db.GetData(query);
            dataGridMyLoans.DataSource = dt;
            dataGridMyLoans.ClearSelection();
            selectedLoanId = null;
            lblSelectedBook.Text = "Tidak ada buku dipilih";
        }

        private void dataGridMyLoans_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridMyLoans.CurrentRow != null && dataGridMyLoans.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dataGridMyLoans.CurrentRow;
                if (row.Cells[0].Value != null && row.Cells[2].Value != null)
                {
                    selectedLoanId = row.Cells[0].Value.ToString();
                    string bookTitle = row.Cells[2].Value.ToString();
                    string dueDate = row.Cells[4].Value?.ToString() ?? "-";
                    lblSelectedBook.Text = $"Buku dipilih: {bookTitle} (Jatuh tempo: {dueDate})";
                }
            }
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            if (dataGridBooks.CurrentRow == null)
            {
                MessageBox.Show("Pilih buku dari tabel BUKU TERSEEDIA!");
                return;
            }

            DataGridViewRow row = dataGridBooks.CurrentRow;
            string bookId = row.Cells[0].Value?.ToString() ?? "";
            string bookTitle = row.Cells[1].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(bookId)) return;

            // Cek batas peminjaman
            string checkQuery = $"SELECT current_borrow_count FROM users WHERE user_id = '{userId}'";
            var currentCount = db.GetSingleValue(checkQuery);

            if (currentCount != null && Convert.ToInt32(currentCount) >= 2)
            {
                MessageBox.Show("Maksimal 2 buku!");
                return;
            }

            DialogResult result = MessageBox.Show($"Pinjam buku '{bookTitle}'?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            string loanId = "LOAN" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string borrowQuery = $@"
                INSERT INTO book_loans (loan_id, user_id, book_id, borrow_date, due_date, status) 
                VALUES ('{loanId}', '{userId}', '{bookId}', NOW(), DATE_ADD(NOW(), INTERVAL 7 DAY), 'BORROWED');
                
                UPDATE books SET available_copies = available_copies - 1 
                WHERE book_id = '{bookId}';
                
                UPDATE users SET current_borrow_count = current_borrow_count + 1 
                WHERE user_id = '{userId}'";

            if (db.ExecuteQuery(borrowQuery))
            {
                MessageBox.Show("Buku berhasil dipinjam!");
                LoadBooks();
                LoadMyLoans();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedLoanId))
            {
                MessageBox.Show("Pilih buku dari tabel BUKU YANG DIPINJAM!");
                return;
            }

            string findQuery = $@"
                SELECT bl.loan_id, bl.book_id, b.title, bl.status 
                FROM book_loans bl 
                JOIN books b ON bl.book_id = b.book_id 
                WHERE bl.loan_id = '{selectedLoanId}'";

            DataTable dt = db.GetData(findQuery);

            if (dt.Rows.Count == 0) return;

            string loanId = dt.Rows[0]["loan_id"].ToString();
            string bookId = dt.Rows[0]["book_id"].ToString();
            string bookTitle = dt.Rows[0]["title"].ToString();
            string status = dt.Rows[0]["status"].ToString();

            if (status != "BORROWED")
            {
                MessageBox.Show("Buku sudah dikembalikan!");
                return;
            }

            DialogResult result = MessageBox.Show($"Kembalikan buku '{bookTitle}'?", "Konfirmasi", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes) return;

            string returnQuery = $@"
                UPDATE book_loans SET return_date = NOW(), status = 'RETURNED'
                WHERE loan_id = '{loanId}';
                
                UPDATE books SET available_copies = available_copies + 1 
                WHERE book_id = '{bookId}';
                
                UPDATE users SET current_borrow_count = current_borrow_count - 1 
                WHERE user_id = '{userId}'";

            if (db.ExecuteQuery(returnQuery))
            {
                MessageBox.Show("Buku berhasil dikembalikan!");
                LoadBooks();
                LoadMyLoans();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBooks();
            LoadMyLoans();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (LoginForm.Instance != null)
            {
                this.Hide();
                LoginForm.Instance.Show();
            }
        }

        private void dataGridMyLoans_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}