using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace lab_03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Add("id", "ID");
            dataGridView1.Columns.Add("name", "Name");
            dataGridView1.Columns.Add("age", "Age");
            List<Person> people = new List<Person>
            {
                new Person { ID = "1", Name = "Nguyen Van A", Age = 20 },
                new Person { ID = "2", Name = "Tran Thi B", Age = 22 },
                new Person { ID = "3", Name = "Le Van C", Age = 25 }
            };

            foreach (var person in people)
            {
                dataGridView1.Rows.Add(person.ID, person.Name, person.Age);
            }
        }
        private void btn_them_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();  
            if (form2.NewPerson != null)  
            {
                dataGridView1.Rows.Add(form2.NewPerson.ID, form2.NewPerson.Name, form2.NewPerson.Age);
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                Person selectedPerson = new Person
                {
                    ID = selectedRow.Cells["id"].Value.ToString(),
                    Name = selectedRow.Cells["name"].Value.ToString(),
                    Age = int.Parse(selectedRow.Cells["age"].Value.ToString())
                };
                Form2 form2 = new Form2
                {
                    PersonToEdit = selectedPerson  
                };
                if (form2.ShowDialog() == DialogResult.OK)
                {
                   
                    if (form2.EditedPerson != null)
                    {
                        selectedRow.Cells["id"].Value = form2.EditedPerson.ID;
                        selectedRow.Cells["name"].Value = form2.EditedPerson.Name;
                        selectedRow.Cells["age"].Value = form2.EditedPerson.Age;
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    public class Person
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
