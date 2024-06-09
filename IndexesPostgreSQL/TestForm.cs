using System.Collections.Generic;
using System.Windows.Forms;

namespace IndexesPostgreSQL
{
    public partial class TestForm : Form
    {
        private Dictionary<string, string> lessons = new Dictionary<string, string>()
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
        string lesson;
        string lessonName;

        public TestForm(string lesson, string lessonName)
        {
            this.lesson = lesson;
            this.lessonName = lessonName;
            InitializeComponent();
            Initializing();
        }

        private void Initializing()
        {
            testBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
            };
            Controls.Add(testBrowser);
            testBrowser.Navigate(lesson);
            Text = lessonName;
        }
    }
}