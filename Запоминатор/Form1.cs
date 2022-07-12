using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Учитель_английского
{
    public partial class Form1 : Form
    {
        Random Rnd = new Random();
        Dictionary<string, string> GeneralDict = new Dictionary<string, string>();
        Dictionary<string, string> CurrentDict = new Dictionary<string, string>();
        int number;
        string answer;

        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox2.BackColor = Color.White;
            label1.Visible = false;
            label2.Visible = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            if (GeneralDict.Count != 0)
            {
                button5.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e) // Открыть файл
        {
            OpenFileDialog ofd = new OpenFileDialog();

            label2.Visible = false;
            ofd.Filter = "txt files (*.txt)|*.txt";
            ofd.ShowDialog();
            string path = ofd.FileName;
            
            if (path != "")
            {
                CurrentDict.Clear();
                GeneralDict.Clear();
                FillOutDictionaries(path);
                Start();
            }
            ofd = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        } // Выход

        private void button3_Click(object sender, EventArgs e)
        {
            label2.Visible = false;
            button5.Enabled = true;
        
            ChangeLanguage();
            
            number = Rnd.Next(0, CurrentDict.Count - 1);
            textBox1.Text = CurrentDict.ElementAt(number).Value;
            answer = CurrentDict.ElementAt(number).Key;

            textBox2.Clear();
            textBox2.BackColor = Color.White;
            label1.Text = CurrentDict.Count + "/" + GeneralDict.Count;
            label1.Visible = true;
            textBox2.Focus();
        } //Сменить язык

        private void button4_Click(object sender, EventArgs e) // Продолжить
        {
            if (textBox2.Text.ToLower() == answer)
            {
                label2.Visible = false;
                button5.Enabled = true;
                
                textBox2.Clear();
                textBox2.BackColor = Color.White;
                label1.Text = CurrentDict.Count - 1 + "/" + GeneralDict.Count;
                CurrentDict.Remove(answer);

                ChangeWord();
            }
            else
            {
                textBox2.BackColor = Color.White;
                Thread.Sleep(250);
                textBox2.BackColor = Color.Red;
                textBox2.Clear();
            }
        }

        private void button5_Click(object sender, EventArgs e) // Показать слово
        {
            button5.Enabled = false;
            label2.Text = answer;
            label2.Visible = true;
        }

        private void FillOutDictionaries (string path)
        {
            StreamReader ReadFile = new StreamReader(path);
            string Word;
            string[] Words;

            try
            {
                do
                {
                Word = ReadFile.ReadLine();
                
                    if (Word != null)
                    {
                        Word = Word.ToLower();
                        Words = Word.Split('-').ToArray();
                        Words[0] = Words[0].TrimEnd(' ');
                        Words[1] = Words[1].TrimEnd(' ');
                        Words[0] = Words[0].TrimStart(' ');
                        Words[1] = Words[1].TrimStart(' ');

                        if (Words.Length == 2)
                        {
                            GeneralDict.Add(Words[0], Words[1]);
                            CurrentDict.Add(Words[0], Words[1]);
                        }
                        else
                        {
                            MessageBox.Show("Выбран неверный файл");
                            return;
                        }
                    }
                }
                while (Word != null);

            }
                catch
            {
                MessageBox.Show("Выбран неверный файл");
            }
        }

        private void Start ()
        {
            if (GeneralDict.Count != 0)
            {
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;

                number = Rnd.Next(0, CurrentDict.Count - 1);
                textBox1.Text = CurrentDict.ElementAt(number).Value;
                answer = CurrentDict.ElementAt(number).Key;

                label1.Visible = true;
                label1.Text = CurrentDict.Count + "/" + GeneralDict.Count;
                textBox2.Enabled = true;
                textBox2.Focus();
            }
        }

        private void ChangeLanguage ()
        {
            CurrentDict.Clear();

            for (int i = 0; i < GeneralDict.Count; i++)
            {
                try
                {
                    CurrentDict.Add(GeneralDict.ElementAt(i).Value, GeneralDict.ElementAt(i).Key);
                }
                catch
                {
                    CurrentDict.Add(GeneralDict.ElementAt(i).Value + 1, GeneralDict.ElementAt(i).Key);
                }
            }
            GeneralDict.Clear();

            for (int i = 0; i < CurrentDict.Count; i++)
            {
                GeneralDict.Add(CurrentDict.ElementAt(i).Key, CurrentDict.ElementAt(i).Value);
            }
        }

        private void ChangeWord ()
        {
            if (CurrentDict.Count > 0)
            {
                number = Rnd.Next(0, CurrentDict.Count - 1);
                textBox1.Text = CurrentDict.ElementAt(number).Value;
                answer = CurrentDict.ElementAt(number).Key.ToLower();
            }
            else
            {
                foreach (var a in GeneralDict) CurrentDict.Add(a.Key, a.Value);

                number = Rnd.Next(0, CurrentDict.Count - 1);
                while (CurrentDict.ElementAt(number).Key == answer)
                {
                    number = Rnd.Next(0, CurrentDict.Count - 1);
                }

                label1.Text = CurrentDict.Count + "/" + GeneralDict.Count;
                textBox1.Text = CurrentDict.ElementAt(number).Value;
                answer = CurrentDict.ElementAt(number).Key.ToLower();
            }
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Формат входного файла: \r'word - перевод'");
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4_Click(this, EventArgs.Empty);
            }
        }
    }
}
