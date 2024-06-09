using System;
using System.Drawing;
using System.Windows.Forms;

namespace IndexesPostgreSQL
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeButtons();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            welcomeLabel.Location = new Point(this.ClientSize.Width / 2 - welcomeLabel.Size.Width / 2, welcomeLabel.Location.Y + 10);
        }

        private void InitializeButtons()
        {
            Button btnLesson1 = new Button
            {
                Text = "Введение",
                Location = new Point(20, welcomeLabel.Location.Y + 50),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson1.Click += (sender, e) => OpenLessonForm("introduction");

            Button btnLesson2 = new Button
            {
                Text = "Сканирование",
                Location = new Point(20, btnLesson1.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson2.Click += (sender, e) => OpenLessonForm("scan");

            Button btnLesson3 = new Button
            {
                Text = "Индексное сканирование",
                Location = new Point(20, btnLesson2.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson3.Click += (sender, e) => OpenLessonForm("indexScan");

            Button btnLesson4 = new Button
            {
                Text = "Создание индекса",
                Location = new Point(20, btnLesson3.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson4.Click += (sender, e) => OpenLessonForm("createIndex");

            Button btnLesson5 = new Button
            {
                Text = "B-tree",
                Location = new Point(20, btnLesson4.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson5.Click += (sender, e) => OpenLessonForm("btree");

            Button btnLesson6 = new Button
            {
                Text = "GiST",
                Location = new Point(20, btnLesson5.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson6.Click += (sender, e) => OpenLessonForm("gist");

            Button btnLesson7 = new Button
            {
                Text = "Hash index",
                Location = new Point(20, btnLesson6.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson7.Click += (sender, e) => OpenLessonForm("hash");

            Button btnLesson8 = new Button
            {
                Text = "Простые индексы",
                Location = new Point(20, btnLesson7.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson8.Click += (sender, e) => OpenLessonForm("simple");

            Button btnLesson9 = new Button
            {
                Text = "Составные индексы",
                Location = new Point(20, btnLesson8.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson9.Click += (sender, e) => OpenLessonForm("complex");

            Button btnLesson10 = new Button
            {
                Text = "Частичные индексы",
                Location = new Point(20, btnLesson9.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson10.Click += (sender, e) => OpenLessonForm("partial");

            Button btnLesson11 = new Button
            {
                Text = "Включённые индексы",
                Location = new Point(20, btnLesson10.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson11.Click += (sender, e) => OpenLessonForm("included");

            Button btnLesson12 = new Button
            {
                Text = "Заключение",
                Location = new Point(20, btnLesson11.Location.Y + 35),
                Size = new Size(this.ClientSize.Width - 40, 30),
                Font = new Font("Times New Roman", 11)
            };
            btnLesson12.Click += (sender, e) => OpenLessonForm("ending");

            // Добавление кнопок на форму
            this.Controls.Add(btnLesson1);
            this.Controls.Add(btnLesson2);
            this.Controls.Add(btnLesson3);
            this.Controls.Add(btnLesson4);
            this.Controls.Add(btnLesson5);
            this.Controls.Add(btnLesson6);
            this.Controls.Add(btnLesson7);
            this.Controls.Add(btnLesson8);
            this.Controls.Add(btnLesson9);
            this.Controls.Add(btnLesson10);
            this.Controls.Add(btnLesson11);
            this.Controls.Add(btnLesson12);
        }

        private void OpenLessonForm(string lessonType)
        {
            this.Hide();
            LessonForm lessonForm = new LessonForm(lessonType);
            lessonForm.Show();
            lessonForm.FormClosing += (sender, e) =>
            {
                this.Show();
            };
        }
    }
}
