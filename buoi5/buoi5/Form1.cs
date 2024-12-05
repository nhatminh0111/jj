using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace buoi5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder containing text files";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;

                    // Tìm các file văn bản trong thư mục
                    string[] textFiles = Directory.GetFiles(selectedPath, "*.txt");

                    if (textFiles.Length == 0)
                    {
                        MessageBox.Show("No text files found in the selected folder!", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Đọc nội dung file văn bản đầu tiên và hiển thị vào TextBox
                    richTextBox1.Text = File.ReadAllText(textFiles[0]);
                    this.Text = Path.GetFileName(textFiles[0]); // Cập nhật tiêu đề Form
                }
            }

        }

        private void tittleHorizoltleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void cassadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical   );
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
   
 
                Application.Exit(); // Thoát chương trình
            
        

    }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("dd/MM/yyyy/ hh:mm");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.Text = "Tahoma";
            toolStripComboBox2.Text = "14";

            // Thêm danh sách font vào ComboBox
            foreach (FontFamily font in new InstalledFontCollection().Families)
            {
                toolStripComboBox1.Items.Add(font.Name);
            }

            // Thêm danh sách cỡ chữ
            List<int> listSize = new List<int> { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in listSize)
            {
                toolStripComboBox2.Items.Add(size);
            }


        }



        private void sapXepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;
            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richTextBox1.ForeColor = fontDlg.Color;
                richTextBox1.Font = fontDlg.Font;
            }
        }

        private void tạoMớiVănBảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear(); // Xóa nội dung trong TextBox
            this.Text = "Untitled Document"; // Đặt lại tiêu đề của Form
        }

        private void lưuNộiDungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
                saveFileDialog.Title = "Save a Text File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Ghi nội dung từ TextBox vào file
                    File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Text = Path.GetFileName(saveFileDialog.FileName); // Cập nhật tiêu đề Form
                }
            }

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedItem != null)
            {
                string selectedFont = toolStripComboBox1.SelectedItem.ToString();
                ChangeTextFont(new Font(selectedFont, richTextBox1.Font.Size, richTextBox1.Font.Style));
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ChangeTextFont(Font newFont)
        {
            if (richTextBox1.SelectionLength > 0) // Nếu có văn bản được chọn
            {
                richTextBox1.SelectionFont = newFont; // Thay đổi font của đoạn văn bản được chọn
            }
            else // Nếu không có văn bản nào được chọn
            {
                richTextBox1.Font = newFont; // Thay đổi font của toàn bộ nội dung
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            if (richTextBox1.SelectionFont != null)
            {
                FontStyle style = richTextBox1.SelectionFont.Style;
                style ^= FontStyle.Underline; // Thay đổi trạng thái Underline
                ChangeTextFont(new Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size, style));
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont != null)
            {
                FontStyle style = richTextBox1.SelectionFont.Style;
                style ^= FontStyle.Italic; // Thay đổi trạng thái Italic
                ChangeTextFont(new Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size, style));
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont != null)
            {
                FontStyle style = richTextBox1.SelectionFont.Style;
                style ^= FontStyle.Bold; // Thay đổi trạng thái Bold
                ChangeTextFont(new Font(richTextBox1.SelectionFont.FontFamily, richTextBox1.SelectionFont.Size, style));
            }
        }

        private void toolStripComboBox2_Click(object sender, EventArgs e)
        {

            if (toolStripComboBox2.SelectedItem != null && float.TryParse(toolStripComboBox2.SelectedItem.ToString(), out float newSize))
            {
                ChangeTextFont(new Font(richTextBox1.Font.FontFamily, newSize, richTextBox1.Font.Style));
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear(); // Xóa nội dung trong TextBox
            this.Text = "Untitled Document"; // Đặt lại tiêu đề của Form
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files|*.txt|All Files|*.*";
                saveFileDialog.Title = "Save a Text File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Ghi nội dung từ TextBox vào file
                    File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Text = Path.GetFileName(saveFileDialog.FileName); // Cập nhật tiêu đề Form
                }
            }
        }
    }
}
