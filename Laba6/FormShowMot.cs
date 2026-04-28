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
using Laba6.ModelEF;

namespace Laba6
{
    public partial class FormShowMot : Form
    {
        public FormShowMot()
        {
            InitializeComponent();
        }

        public static Model1 DB = new Model1();
        private void button2_Click(object sender, EventArgs e)
        {
            if (DB.Table_Motorbike.ToList().Count == 0)
            {
                MessageBox.Show("Данные отстутствуют!");
                return;
            }
            Table_Motorbike CurrentMoto = DB.Table_Motorbike.Find((int)dataGridView1.CurrentRow.Cells[0].Value);
            DialogResult result = MessageBox.Show(
                $"Вы действительно хотите удалить объект с ID = {CurrentMoto.ID}",
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    DB.Table_Motorbike.Remove(CurrentMoto);
                    DB.SaveChanges();

                    File.Delete($"Pictures\\{CurrentMoto.Picture}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    tableMotorbikeBindingSource.DataSource = DB.Table_Motorbike.ToList();
                    pictureBox1.Image = null;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddUPDMD form = new FormAddUPDMD();
            this.Visible = false;
            form.Show();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (DB.Table_Motorbike.ToList().Count == 0) return;
            int ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            pictureBox1.Image = Image.FromFile($"Pictures\\{DB.Table_Motorbike.Find(ID).Picture}");
        }

        private void FormShowMot_Load(object sender, EventArgs e)
        {
            tableMotorbikeBindingSource.DataSource = DB.Table_Motorbike.ToList();
            if (DB.Table_Motorbike.ToList().Count == 0) return;
            int ID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            try
            {
                pictureBox1.Image = Image.FromFile($"Pictures\\{DB.Table_Motorbike.Find(ID).Picture}");
            }
            catch (Exception)
            {
                pictureBox1.Image = null;
            }
        }
    }
}
