using Laba6.ModelEF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba6
{
    public partial class FormAddUPDMD : Form
    {
        public FormAddUPDMD()
        {
            InitializeComponent();
        }
        public static Model1 DB = new Model1();
        public List<Table_Motorbike> Table_Motorbike_datasource { get; private set; }

        private string Pic_Name;
        private Table_Motorbike currentMotorbike;

        private void FormShowMot_Load(object sender, EventArgs e)
        {
            Table_Motorbike_datasource = DB.Table_Motorbike.ToList();
            List<string> dictBrand = new List<string>();
            foreach (Table_Motorbike item in Table_Motorbike_datasource)
                dictBrand.Add(item.Brand);
            dictBrand = dictBrand.Distinct().ToList();
            if (currentMotorbike != null)
            {
                textBox1.Text = currentMotorbike.Model;
                textBox2.Text = currentMotorbike.Price.ToString();
                textBox3.Text = currentMotorbike.Horsepower.ToString();
                textBox4.Text = currentMotorbike.Mileage.ToString();

                if (!string.IsNullOrEmpty(currentMotorbike.Picture))
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(currentMotorbike.Picture);
                        Pic_Name = currentMotorbike.Picture;
                    }
                    catch { }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы изображений (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png";
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                Pic_Name = openFileDialog.FileName;
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormShowMot form = new FormShowMot();
            form.Visible = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!");
                return;
            }
            try
            {
                Convert.ToInt32(textBox4.Text);
                Convert.ToInt32(textBox3.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("В полях Л/С и Пробег могут быть только целочисленные данные!");
                return;
            }

            try
            {
                Convert.ToSingle(textBox2.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("В поле Цена могут быть только числа с плавающей точкой!");
                return;
            }
            if (currentMotorbike == null && !File.Exists(Pic_Name))
            {
                MessageBox.Show("Невозможно найти файл изображения!");
                return;
            }

            try
            {
                if (currentMotorbike == null)
                {
                    Table_Motorbike motorbike = new Table_Motorbike();
                    motorbike.ID = GetNextId();
                    motorbike.Brand = "Custom Brand"; 
                    motorbike.Model = textBox1.Text;
                    motorbike.Price = Convert.ToSingle(textBox2.Text);
                    motorbike.Horsepower = Convert.ToInt32(textBox3.Text);
                    motorbike.Mileage = Convert.ToInt32(textBox4.Text);

                    if (!string.IsNullOrEmpty(Pic_Name))
                    {
                        string newFileName = $@"Pictures\{GetNextId()}{Path.GetExtension(Pic_Name)}";
                        File.Copy(Pic_Name, newFileName, true);
                        motorbike.Picture = newFileName;
                    }

                    DB.Table_Motorbike.Add(motorbike);
                }
                else
                {
                    currentMotorbike.Model = textBox1.Text;
                    currentMotorbike.Price = Convert.ToSingle(textBox2.Text);
                    currentMotorbike.Horsepower = Convert.ToInt32(textBox3.Text);
                    currentMotorbike.Mileage = Convert.ToInt32(textBox4.Text);

                    if (!string.IsNullOrEmpty(Pic_Name) && Pic_Name != currentMotorbike.Picture)
                    {
                        string newFileName = $@"Pictures\{currentMotorbike.ID}{Path.GetExtension(Pic_Name)}";
                        File.Copy(Pic_Name, newFileName, true);
                        currentMotorbike.Picture = newFileName;
                    }
                }

                DB.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");

                FormShowMot form = new FormShowMot();
                form.Visible = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private int GetNextId()
        {
            if (!Table_Motorbike_datasource.Any())
                return 1;
            return Table_Motorbike_datasource.Max(x => x.ID) + 1;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 46)
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',' && e.KeyChar != '.')
                e.Handled = true;
        }
    }
}
