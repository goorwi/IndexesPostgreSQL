using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IndexesPostgreSQL
{
    public partial class LessonForm : Form
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
        private Panel menuPanel;
        private Button toggleMenuButton;
        private ListBox lessonsListBox;
        private WebBrowser lessonBrowser;
        private TableLayoutPanel navigationPanel;
        private Button prevButton;
        private Button testButton;
        private Button nextButton;
        private bool isMenuVisible = false;

        string path = Directory.GetCurrentDirectory().Replace("bin\\Debug", "Lessons\\");
        string lessonType;
        public LessonForm(string LessonType)
        {
            this.lessonType = LessonType;
            InitializeComponent();
            Initializing();
            LoadLessons();
            this.lessonsListBox.SelectedIndex = lessonsListBox.Items.IndexOf(lessons.Where(x => x.Value == lessonType).First().Key);
        }

        private void LessonForm_Load(object sender, EventArgs e)
        {
            lessonBrowser.Navigate(path + lessonType + ".html");
            Text = $"{lessons.Where(x => x.Value == lessonType).First().Key}";
        }

        private void Initializing()
        {
            // Panel for menu
            menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                Visible = false // Initially hidden
            };

            // Toggle button for menu
            toggleMenuButton = new Button
            {
                Text = "Меню",
                Dock = DockStyle.Left,
                Width = 50
            };
            toggleMenuButton.Click += ToggleMenuButton_Click;

            // ListBox for lessons
            lessonsListBox = new ListBox
            {
                Font = new Font("Times New Roman", 12),
                Dock = DockStyle.Fill
            };
            lessonsListBox.SelectedIndexChanged += LessonsListBox_SelectedIndexChanged;

            // WebBrowser for lesson content
            lessonBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill
            };

            // TableLayoutPanel for navigation buttons
            navigationPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                ColumnCount = 3,
                RowCount = 1
            };
            navigationPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            navigationPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            navigationPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            // Navigation buttons
            prevButton = new Button
            {
                Text = "Предыдущий урок",
                Dock = DockStyle.Fill
            };
            prevButton.Click += PrevButton_Click;

            testButton = new Button
            {
                Text = "Тест",
                Dock = DockStyle.Fill
            };
            testButton.Click += TestButton_Click;
            switch (lessonType)
            {
                case "simple":
                    {
                        testButton.Text = "Эмуляция дерева";
                        break;
                    }
                case "complex":
                    {
                        testButton.Text = "Эмуляция дерева";
                        break;
                    }
                case "partial":
                    {
                        testButton.Text = "Эмуляция дерева";
                        break;
                    }
                case "included":
                    {
                        testButton.Text = "Эмуляция дерева";
                        break;
                    }
            };

            nextButton = new Button
            {
                Text = "Следующий урок",
                Dock = DockStyle.Fill
            };
            nextButton.Click += NextButton_Click;

            // Add buttons to the TableLayoutPanel
            navigationPanel.Controls.Add(prevButton, 0, 0);
            navigationPanel.Controls.Add(testButton, 1, 0);
            navigationPanel.Controls.Add(nextButton, 2, 0);

            // Add controls to the panels
            menuPanel.Controls.Add(lessonsListBox);

            // Add panels and controls to the form
            Controls.Add(lessonBrowser);
            Controls.Add(navigationPanel);
            Controls.Add(toggleMenuButton);
            Controls.Add(menuPanel);
        }

        private void LoadLessons()
        {
            // Add lessons to the ListBox (these should be your lesson titles or identifiers)
            lessonsListBox.Items.AddRange(lessons.Keys.ToArray());
        }

        private void LessonsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Load the selected lesson content
            int selectedIndex = lessonsListBox.SelectedIndex;
            if (selectedIndex >= 0)
            {
                // Here, you would load the actual lesson content based on the selected index
                lessons.TryGetValue(lessonsListBox.Items[selectedIndex].ToString(), out lessonType);
                lessonBrowser.Navigate(path + lessonType + ".html");
                Text = $"{lessonsListBox.Items[selectedIndex]}";

                switch (lessonType)
                {
                    case "simple":
                        {
                            testButton.Text = "Эмуляция дерева";
                            break;
                        }
                    case "complex":
                        {
                            testButton.Text = "Эмуляция дерева";
                            break;
                        }
                    case "partial":
                        {
                            testButton.Text = "Эмуляция дерева";
                            break;
                        }
                    case "included":
                        {
                            testButton.Text = "Эмуляция дерева";
                            break;
                        }
                    default:
                        {
                            testButton.Text = "Тест";
                            break;
                        }
                };
            }
        }

        private void ToggleMenuButton_Click(object sender, EventArgs e)
        {
            // Toggle menu visibility
            isMenuVisible = !isMenuVisible;
            menuPanel.Visible = isMenuVisible;
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            // Navigate to the previous lesson
            if (lessonsListBox.SelectedIndex > 0)
            {
                lessonsListBox.SelectedIndex--;
            }
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            switch (lessonType)
            {
                case "simple":
                    {
                        break;
                    }
                case "complex":
                    {
                        break;
                    }
                case "partial":
                    {
                        break;
                    }
                case "included":
                    {
                        break;
                    }
                default:
                    {
                        this.Hide();
                        // Navigate to the test page
                        string testPath = Directory.GetCurrentDirectory().Replace("bin\\Debug", "Tests\\") + lessonType + ".html";
                        var lessonName = lessons.Where(x => x.Value == lessonType).First().Key;
                        TestForm test = new TestForm(testPath, lessonName);
                        test.Show();
                        test.FormClosing += (s, arg) =>
                        {
                            this.Show();
                        };
                        break;
                    }
            }
        }

        private void Test_FormClosing(object sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            // Navigate to the next lesson
            if (lessonsListBox.SelectedIndex < lessonsListBox.Items.Count - 1)
            {
                lessonsListBox.SelectedIndex++;
            }
        }

    }
}
