using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace IndexesPostgreSQL
{
    public partial class TestForm : Form
    {
        private readonly Dictionary<string, string> lessons = new Dictionary<string, string>()
        {
            { "Введение", "introduction" },
            { "Сканирование", "scan" },
            { "Индексное сканирование", "indexScan" },
            { "Создание индекса", "createIndex" },
            { "B-tree", "btree" },
            { "GiST", "gist" },
            { "Hash index", "hash" },
            { "Простые индексы", "simple" },
            { "Составные индексы", "complex" },
            { "Частичные индексы", "partial" },
            { "Включённые индексы", "included" },
        };
        WebBrowser testBrowser;
        Button closeButton;
        readonly string lesson;
        readonly string lessonName;

        public TestForm(string lesson, string lessonName)
        {
            this.lesson = lesson;
            this.lessonName = lessonName;
            InitializeComponent();
            Initializing();
        }

        private void Initializing()
        {
            closeButton = new Button()
            {
                Font = new System.Drawing.Font("Times New Roman", 12),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Text = "Выход",
                AutoSize = true,

            };
            closeButton.Size = new Size(100, 50);
            closeButton.Location = new Point(this.ClientSize.Width - 30 - closeButton.Width, this.ClientSize.Height - 10 - closeButton.Height);
            closeButton.Click += (s, k) =>
            {
                this.Close();
            };
            testBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
            };
            Controls.Add(closeButton);
            Controls.Add(testBrowser);
            testBrowser.Navigate(lesson);
            Text = lessonName;
            this.SizeChanged += (s, k) =>
            {
                closeButton.Location = new Point(this.ClientSize.Width - 30 - closeButton.Width, this.ClientSize.Height - 10 - closeButton.Height);
            };
        }
    }
}