using System;
using System.Windows.Forms;

namespace lab_03
{
    public partial class Form2 : Form
    {
        public Person PersonToEdit { get; set; }
        public Person EditedPerson { get; set; }
        public Person NewPerson { get; set; }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (PersonToEdit != null)
            {
                txt_id.Text = PersonToEdit.ID;
                txt_name.Text = PersonToEdit.Name;
                txt_age.Text = PersonToEdit.Age.ToString();
            }
        }
        private void btn_dongy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_id.Text) || string.IsNullOrEmpty(txt_name.Text) || string.IsNullOrEmpty(txt_age.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (PersonToEdit != null)
            {

                EditedPerson = new Person
                {
                    ID = txt_id.Text,
                    Name = txt_name.Text,
                    Age = int.Parse(txt_age.Text)
                };

                DialogResult = DialogResult.OK;  
                this.Close();  
            }
            else
            {

                NewPerson = new Person
                {
                    ID = txt_id.Text,
                    Name = txt_name.Text,
                    Age = int.Parse(txt_age.Text)
                };

                DialogResult = DialogResult.OK;  
                this.Close();  
            }
        }
        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
