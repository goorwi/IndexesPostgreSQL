using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace IndexesPostgreSQL
{
    public partial class BTreeEmulator : Form
    {
        private DrawBox _box;
        private ITree<SimpleIndexValue> SimpleTree = null;
        private ITree<ComplexIndexValue> ComplexTree;
        private ITree<PartialIndexValue> PartialTree;
        private ITree<IncludedIndexValue> IncludedTree;
        private bool isCreatedTree = false;
        private readonly string typeIndex;

        private TextBox firstInsertBox;
        private TextBox secondInsertBox;
        private Button insertButton;

        private TextBox firstDeleteBox;
        private TextBox secondDeleteBox;
        private Button deleteButton;

        private RadioButton uniqueButton;

        private Button clearTreeButton;

        private TextBox minValueCondition;
        private Label labelCondition;
        private TextBox maxValueCondition;

        private TextBox firstField;
        private TextBox secondField;
        private TextBox showField;
        private Button showButton;
        private Panel includedPanel;
        private ListBox showIncludedRecords;

        public BTreeEmulator(string typeIndex)
        {
            InitializeComponent();
            this.typeIndex = typeIndex;
            Text = $"Эмуляция дерева. {typeIndex}";
        }

        private void InitializeActions()
        {
            var font = new Font("Times New Roman", 12);
            switch (typeIndex)
            {
                case "Простой индекс":
                    InitializeSimpleIndexControls(font);
                    break;
                case "Составной индекс":
                    InitializeComplexIndexControls(font);
                    break;
                case "Частичный индекс":
                    InitializePartialIndexControls(font);
                    break;
                case "Включённый индекс":
                    InitializeIncludedIndexControls(font);
                    break;
            }
        }

        private void InitializeSimpleIndexControls(Font font)
        {
            firstInsertBox = CreateTextBox(new Point(10, 10), font, 0);
            firstInsertBox.KeyPress += insertBox_KeyPress;
            insertButton = CreateButton(new Point(firstInsertBox.Right + 10, 10), font, "Вставить", 1, insertButton_Click);
            firstDeleteBox = CreateTextBox(new Point(insertButton.Right + 10, 10), font, 2);
            firstDeleteBox.KeyPress += deleteBox_KeyPress;
            deleteButton = CreateButton(new Point(firstDeleteBox.Right + 10, 10), font, "Удалить", 3, deleteButton_Click);
            uniqueButton = CreateRadioButton(new Point(deleteButton.Right + 10, 10), font, "Уникальные значения", 4);
            clearTreeButton = CreateButton(new Point(actionPanel.Right - 100 - 10, 10), font, "Сбросить", 5, clearTreeButton_Click);

            actionPanel.Controls.AddRange(new Control[] { firstInsertBox, insertButton, firstDeleteBox, deleteButton, uniqueButton, clearTreeButton });
        }

        private void InitializeComplexIndexControls(Font font)
        {
            firstInsertBox = CreateTextBox(new Point(10, 10), font, 0);
            firstInsertBox.KeyPress += insertBox_KeyPress;
            secondInsertBox = CreateTextBox(new Point(firstInsertBox.Right + 10, 10), font, 1);
            secondInsertBox.KeyPress += insertBox_KeyPress;
            insertButton = CreateButton(new Point(secondInsertBox.Right + 10, 10), font, "Вставить", 2, insertButton_Click);
            firstDeleteBox = CreateTextBox(new Point(insertButton.Right + 10, 10), font, 3);
            firstDeleteBox.KeyPress += deleteBox_KeyPress;
            secondDeleteBox = CreateTextBox(new Point(firstDeleteBox.Right + 10, 10), font, 4);
            secondDeleteBox.KeyPress += deleteBox_KeyPress;
            deleteButton = CreateButton(new Point(secondDeleteBox.Right + 10, 10), font, "Удалить", 5, deleteButton_Click);
            uniqueButton = CreateRadioButton(new Point(deleteButton.Right + 10, 10), font, "Уникальные значения", 6);
            clearTreeButton = CreateButton(new Point(actionPanel.Right - 100 - 10, 10), font, "Сбросить", 7, clearTreeButton_Click);

            actionPanel.Controls.AddRange(new Control[] { firstInsertBox, secondInsertBox, insertButton, firstDeleteBox, secondDeleteBox, deleteButton, uniqueButton, clearTreeButton });
        }

        private void InitializePartialIndexControls(Font font)
        {
            firstInsertBox = CreateTextBox(new Point(10, 10), font, 0);
            firstInsertBox.KeyPress += insertBox_KeyPress;
            insertButton = CreateButton(new Point(firstInsertBox.Right + 10, 10), font, "Вставить", 1, insertButton_Click);
            minValueCondition = CreateTextBox(new Point(insertButton.Right + 10, 10), font, 5);
            minValueCondition.Enter += RemovePlaceholderText;
            minValueCondition.Leave += SetPlaceholderText;
            minValueCondition.KeyPress += insertBox_KeyPress;
            labelCondition = CreateLabel(new Point(minValueCondition.Right + 10, 10), font, "<= (элемент) <=", 6);
            maxValueCondition = CreateTextBox(new Point(labelCondition.Right + 25, 10), font, 7);
            maxValueCondition.Enter += RemovePlaceholderText;
            maxValueCondition.Leave += SetPlaceholderText;
            maxValueCondition.KeyPress += insertBox_KeyPress;
            firstDeleteBox = CreateTextBox(new Point(maxValueCondition.Right + 10, 10), font, 2);
            firstDeleteBox.KeyPress += deleteBox_KeyPress;
            firstDeleteBox.Enabled = false;
            deleteButton = CreateButton(new Point(firstDeleteBox.Right + 10, 10), font, "Удалить", 3, deleteButton_Click);
            deleteButton.Enabled = false;
            uniqueButton = CreateRadioButton(new Point(deleteButton.Right + 10, 10), font, "Уникальные значения", 4);
            clearTreeButton = CreateButton(new Point(actionPanel.Right - 100 - 10, 10), font, "Сбросить", 8, clearTreeButton_Click);

            actionPanel.Controls.AddRange(new Control[] { firstInsertBox, insertButton, firstDeleteBox, deleteButton, uniqueButton, minValueCondition, labelCondition, maxValueCondition, clearTreeButton });

            SetPlaceholderText(minValueCondition, null);
            SetPlaceholderText(maxValueCondition, null);
        }

        private void InitializeIncludedIndexControls(Font font)
        {
            firstInsertBox = CreateTextBox(new Point(10, 10), font, 0);
            firstInsertBox.KeyPress += insertBox_KeyPress;
            firstField = CreateTextBox(new Point(firstInsertBox.Right + 10, 10), font, 0, 150);
            firstField.KeyPress += insertBox_KeyPress;
            firstField.Enter += RemovePlaceholderText;
            firstField.Leave += SetPlaceholderText;
            secondField = CreateTextBox(new Point(firstField.Right + 10, 10), font, 0, 150);
            secondField.KeyPress += insertBox_KeyPress;
            secondField.Enter += RemovePlaceholderText;
            secondField.Leave += SetPlaceholderText;
            insertButton = CreateButton(new Point(secondField.Right + 10, 10), font, "Вставить", 1, insertButton_Click);
            firstDeleteBox = CreateTextBox(new Point(insertButton.Right + 10, 10), font, 2);
            firstDeleteBox.KeyPress += deleteBox_KeyPress;
            firstDeleteBox.Enabled = false;
            deleteButton = CreateButton(new Point(firstDeleteBox.Right + 10, 10), font, "Удалить", 3, deleteButton_Click);
            deleteButton.Enabled = false;
            showField = CreateTextBox(new Point(deleteButton.Right + 10, 10), font, 0);
            showField.Enabled = false;
            showField.KeyPress += (s, k) =>
            {
                if (k.KeyChar == (char)Keys.Enter)
                {
                    ShowButton_Click(s, k);
                    k.Handled = true;
                }
            };
            showButton = CreateButton(new Point(showField.Right + 10, 10), font, "Показать", 3, ShowButton_Click);
            showButton.Enabled = false;
            uniqueButton = CreateRadioButton(new Point(showButton.Right + 10, 10), font, "Уникальные значения", 4);
            clearTreeButton = CreateButton(new Point(actionPanel.Right - 100 - 10, 10), font, "Сбросить", 5, clearTreeButton_Click);
            includedPanel = CreatePanel(new Size(150, this.ClientSize.Height - actionPanel.Height), DockStyle.Left);
            showIncludedRecords = new ListBox { Font = font, Dock = DockStyle.Fill };
            includedPanel.Controls.Add(showIncludedRecords);
            this.Controls.Add(includedPanel);

            this.Controls.Remove(actionPanel);
            actionPanel = CreatePanel(new Size(this.ClientSize.Width, 50), DockStyle.Bottom);
            this.Controls.Add(actionPanel);
            actionPanel.Controls.AddRange(new Control[] { firstInsertBox, insertButton, firstDeleteBox, deleteButton, firstField, secondField, showField, showButton, uniqueButton, clearTreeButton });

            this.Controls.Remove(drawPanel);
            drawPanel = CreatePanel(new Size(this.ClientSize.Width - includedPanel.Width - storyPanel.Width, this.ClientSize.Height - actionPanel.Height), DockStyle.None, new Point(includedPanel.Right, 0));
            _box = new DrawBox { Dock = DockStyle.Fill };
            drawPanel.Controls.Add(_box);
            this.Controls.Add(drawPanel);

            SetPlaceholderText(firstField, null);
            SetPlaceholderText(secondField, null);
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(showField.Text))
            {
                _ = MessageBox.Show("Введите элемент!", "Ошибка!");
                return;
            }

            int? value;
            if (showField.Text == "null")
                value = null;
            else if (!int.TryParse(showField.Text, out int val))
            {
                _ = MessageBox.Show("Введите числовое значение!", "Ошибка!");
                return;
            }
            else
                value = val;

            var x = new IncludedIndexValue(value, null, null);
            var values = IncludedTree.Search(x);

            if (values.Count == 0)
            {
                _ = MessageBox.Show("Записи не найдены!", "Ошибка!");
                return;
            }

            showIncludedRecords.Items.Clear();
            foreach (var record in values)
            {
                _ = showIncludedRecords.Items.Add($"-> {record.Value?.ToString() ?? "null"}  ->  {record.firstField?.ToString() ?? "null"}  :  {record.secondField?.ToString() ?? "null"}");
            }
        }

        private void RemovePlaceholderText(object sender, EventArgs e)
        {
            if (typeIndex == "Частичный индекс")
            {
                if (sender is TextBox textBox && textBox.ForeColor == Color.Gray)
                {
                    textBox.Text = string.Empty;
                    textBox.ForeColor = Color.Black;
                }
            }
            else if (typeIndex == "Включённый индекс")
            {
                if (sender is TextBox textBox && textBox.ForeColor == Color.Gray)
                {
                    textBox.Text = string.Empty;
                    textBox.ForeColor = Color.Black;
                }
            }
        }

        private void SetPlaceholderText(object sender, EventArgs e)
        {
            if (typeIndex == "Частичный индекс")
            {
                if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.ForeColor = Color.Gray;
                    if (textBox == minValueCondition)
                        textBox.Text = "min";
                    else if (textBox == maxValueCondition)
                        textBox.Text = "max";
                }
            }
            else if (typeIndex == "Включённый индекс")
            {
                if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.ForeColor = Color.Gray;
                    textBox.Text = "Включённое поле";
                }
            }
        }

        private TextBox CreateTextBox(Point location, Font font, int tabIndex, int width = 100, HorizontalAlignment textAlign = HorizontalAlignment.Left)
        {
            return new TextBox
            {
                Location = location,
                Font = font,
                TabIndex = tabIndex,
                Width = width,
                TextAlign = textAlign
            };
        }

        private Button CreateButton(Point location, Font font, string text, int tabIndex, EventHandler clickHandler)
        {
            var button = new Button
            {
                Location = location,
                Font = font,
                Text = text,
                TabIndex = tabIndex,
                AutoSize = true
            };
            button.Click += clickHandler;
            return button;
        }

        private RadioButton CreateRadioButton(Point location, Font font, string text, int tabIndex)
        {
            return new RadioButton
            {
                Location = location,
                Font = font,
                Text = text,
                TabIndex = tabIndex,
                AutoSize = true
            };
        }

        private Label CreateLabel(Point location, Font font, string text, int tabIndex)
        {
            return new Label
            {
                Location = location,
                Font = font,
                Text = text,
                TabIndex = tabIndex,
                AutoSize = true
            };
        }

        private Panel CreatePanel(Size size, DockStyle dock, Point? location = null)
        {
            return new Panel
            {
                Size = size,
                Dock = dock,
                Location = location ?? Point.Empty
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            drawPanel.Dock = DockStyle.None;
            _box = new DrawBox
            {
                Dock = DockStyle.Fill,
            };
            drawPanel.Controls.Add(_box);

            storyPanel.Dock = DockStyle.Right;
            storyListBox.Dock = DockStyle.Fill;
            storyPanel.Controls.Add(storyListBox);

            actionPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - storyPanel.Height);

            // Устанавливаем привязку drawPanel к краям формы
            drawPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;

            // Устанавливаем размер drawPanel в соответствии с размерами клиентской области формы
            drawPanel.Size = new Size(this.ClientSize.Width - storyPanel.Width, this.ClientSize.Height - actionPanel.Height);

            // Подписываемся на событие изменения размера формы, чтобы обновлять размер drawPanel
            this.SizeChanged += (s, k) =>
            {
                drawPanel.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - actionPanel.Height);
            };

            this.Resize += (s, k) =>
            {
                _box.Invalidate();
            };
            InitializeActions();
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            switch (typeIndex)
            {
                case "Простой индекс":
                    {
                        if (firstInsertBox.Text == "null" || int.TryParse(firstInsertBox.Text, out _))
                        {
                            var value = firstInsertBox.Text == "null" ? (int?)null : int.Parse(firstInsertBox.Text);
                            if (SimpleTree == null)
                            {
                                SimpleTree = CreateTree<SimpleIndexValue>();
                                uniqueButton.Enabled = false;
                            }
                            var val = new SimpleIndexValue(value);
                            SimpleTree.Insert(val);
                            _box.Print<BTree<int>>(SimpleTree);

                            if (uniqueButton.Checked)
                            {
                                if (!storyListBox.Items.Contains($"-> {val}"))
                                    _ = storyListBox.Items.Add($"-> {val}");
                                firstInsertBox.Text = "";
                                _ = firstInsertBox.Focus();
                            }
                            else
                            {
                                _ = storyListBox.Items.Add($"-> {val}");
                                firstInsertBox.Text = "";
                                _ = firstInsertBox.Focus();
                            }
                        }
                        else
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                        break;
                    }
                case "Составной индекс":
                    {
                        int? firstVal, secondVal;
                        if (firstInsertBox.Text == "null" || int.TryParse(firstInsertBox.Text, out _))
                            firstVal = firstInsertBox.Text == "null" ? (int?)null : int.Parse(firstInsertBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (secondInsertBox.Text == "null" || int.TryParse(secondInsertBox.Text, out _))
                            secondVal = secondInsertBox.Text == "null" ? (int?)null : int.Parse(secondInsertBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (ComplexTree == null)
                        {
                            ComplexTree = CreateTree<ComplexIndexValue>();
                            uniqueButton.Enabled = false;
                        }
                        var value = new ComplexIndexValue(firstVal, secondVal);
                        ComplexTree.Insert(value);
                        _box.Print<BTree<int>>(ComplexTree);

                        if (uniqueButton.Checked)
                        {
                            if (!storyListBox.Items.Contains($"-> {value}"))
                                _ = storyListBox.Items.Add($"-> {value}");
                            firstInsertBox.Text = "";
                            secondInsertBox.Text = "";
                            _ = firstInsertBox.Focus();
                        }
                        else
                        {
                            _ = storyListBox.Items.Add($"-> {value}");
                            firstInsertBox.Text = "";
                            secondInsertBox.Text = "";
                            _ = firstInsertBox.Focus();
                        }


                        break;
                    }
                case "Частичный индекс":
                    {
                        int? main, min, max;
                        if (firstInsertBox.Text == "null" || int.TryParse(firstInsertBox.Text, out _))
                            main = firstInsertBox.Text == "null" ? (int?)null : int.Parse(firstInsertBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (minValueCondition.Text == "null" || int.TryParse(minValueCondition.Text, out _))
                            min = minValueCondition.Text == "null" ? (int?)null : int.Parse(minValueCondition.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (maxValueCondition.Text == "null" || int.TryParse(maxValueCondition.Text, out _))
                            max = maxValueCondition.Text == "null" ? (int?)null : int.Parse(maxValueCondition.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (Nullable.Compare(min, max) <= 0)
                        {
                            if (Nullable.Compare(min, main) <= 0 && Nullable.Compare(main, max) <= 0)
                            {
                                if (PartialTree == null)
                                {
                                    PartialTree = CreateTree<PartialIndexValue>();
                                    minValueCondition.Enabled = false; maxValueCondition.Enabled = false;
                                    uniqueButton.Enabled = false;
                                    firstDeleteBox.Enabled = true;
                                    deleteButton.Enabled = true;
                                }
                                var value = new PartialIndexValue(main);
                                PartialTree.Insert(value);
                                _box.Print<BTree<int>>(PartialTree);

                                if (uniqueButton.Checked)
                                {
                                    if (!storyListBox.Items.Contains($"-> {value}"))
                                        _ = storyListBox.Items.Add($"-> {value}");
                                    firstInsertBox.Text = "";
                                    _ = firstInsertBox.Focus();
                                }
                                else
                                {
                                    _ = storyListBox.Items.Add($"-> {value}");
                                    firstInsertBox.Text = "";
                                    _ = firstInsertBox.Focus();
                                }
                            }
                            else
                            {
                                _ = MessageBox.Show("Элемент не соответствует условию!", "Ошибка!");
                                return;
                            }
                        }
                        else
                            _ = MessageBox.Show("Некорректное условие!", "Ошибка!");

                        break;
                    }
                case "Включённый индекс":
                    {
                        int? val, first, second;
                        if (firstInsertBox.Text == "null" || int.TryParse(firstInsertBox.Text, out _))
                            val = firstInsertBox.Text == "null" ? (int?)null : int.Parse(firstInsertBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (firstField.Text == "null" || int.TryParse(firstField.Text, out _))
                            first = firstField.Text == "null" ? (int?)null : int.Parse(firstField.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (secondField.Text == "null" || int.TryParse(secondField.Text, out _))
                            second = secondField.Text == "null" ? (int?)null : int.Parse(secondField.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        if (IncludedTree == null)
                        {
                            IncludedTree = CreateTree<IncludedIndexValue>();
                            uniqueButton.Enabled = false;
                            firstDeleteBox.Enabled = true;
                            deleteButton.Enabled = true;
                            showField.Enabled = true;
                            showButton.Enabled = true;
                        }
                        var value = new IncludedIndexValue(val, first, second);
                        IncludedTree.Insert(value);
                        _box.Print<BTree<int>>(IncludedTree);

                        if (uniqueButton.Checked)
                        {
                            firstInsertBox.Text = "";
                            firstField.Text = "";
                            secondField.Text = "";
                            _ = firstInsertBox.Focus();
                            foreach (var item in storyListBox.Items)
                            {
                                if (item.ToString().Substring(0, $"-> {value.Value?.ToString() ?? "null"}".Length) == $"-> {value.Value?.ToString() ?? "null"}")
                                    return;
                            }
                            _ = storyListBox.Items.Add($"-> {value.Value?.ToString() ?? "null"}  ->  {value.firstField?.ToString() ?? "null"}  :  {value.secondField?.ToString() ?? "null"}");
                        }
                        else
                        {
                            _ = storyListBox.Items.Add($"-> {value.Value?.ToString() ?? "null"}  ->  {value.firstField?.ToString() ?? "null"}  :  {value.secondField?.ToString() ?? "null"}");
                            firstInsertBox.Text = "";
                            firstField.Text = "";
                            secondField.Text = "";
                            _ = firstInsertBox.Focus();
                        }

                        SetPlaceholderText(firstField, null);
                        SetPlaceholderText(secondField, null);

                        break;
                    }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            switch (typeIndex)
            {
                case "Простой индекс":
                    {
                        if (firstDeleteBox.Text == "null" || int.TryParse(firstDeleteBox.Text, out var x))
                        {
                            var value = firstDeleteBox.Text == "null" ? (int?)null : int.Parse(firstDeleteBox.Text);
                            var val = new SimpleIndexValue(value);

                            SimpleTree.Remove(val);
                            _box.Print<BTree<int>>(SimpleTree);

                            _ = storyListBox.Items.Add($"<- {val}");
                            firstDeleteBox.Text = "";
                            _ = firstInsertBox.Focus();

                            if (SimpleTree.GetAllNodes().ToList().Sum(node => node.Keys.Count) == 0)
                            {
                                drawPanel.Controls.Clear();
                                _box = new DrawBox
                                {
                                    Dock = DockStyle.Fill,
                                };
                                drawPanel.Controls.Add(_box);
                            }
                        }
                        else
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                        break;
                    }
                case "Составной индекс":
                    {
                        int? firstVal, secondVal;
                        if (firstDeleteBox.Text == "null" || int.TryParse(firstDeleteBox.Text, out var x))
                            firstVal = firstDeleteBox.Text == "null" ? (int?)null : int.Parse(firstDeleteBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }
                        if (secondDeleteBox.Text == "null" || int.TryParse(secondDeleteBox.Text, out var y))
                            secondVal = secondDeleteBox.Text == "null" ? (int?)null : int.Parse(secondDeleteBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        var value = new ComplexIndexValue(firstVal, secondVal);

                        ComplexTree.Remove(value);
                        _box.Print<BTree<int>>(ComplexTree);

                        _ = storyListBox.Items.Add($"<- {value}");
                        firstDeleteBox.Text = "";
                        secondDeleteBox.Text = "";
                        _ = firstInsertBox.Focus();

                        if (ComplexTree.GetAllNodes().ToList().Sum(node => node.Keys.Count) == 0)
                        {
                            drawPanel.Controls.Clear();
                            _box = new DrawBox
                            {
                                Dock = DockStyle.Fill,
                            };
                            drawPanel.Controls.Add(_box);
                        }

                        break;
                    }
                case "Частичный индекс":
                    {
                        int? main;
                        if (firstDeleteBox.Text == "null" || int.TryParse(firstDeleteBox.Text, out var x))
                            main = firstDeleteBox.Text == "null" ? (int?)null : int.Parse(firstDeleteBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        var value = new PartialIndexValue(main);

                        PartialTree.Remove(value);
                        _box.Print<BTree<int>>(PartialTree);

                        _ = storyListBox.Items.Add($"<- {value}");
                        firstDeleteBox.Text = "";
                        _ = firstInsertBox.Focus();

                        if (PartialTree.GetAllNodes().ToList().Sum(node => node.Keys.Count) == 0)
                        {
                            drawPanel.Controls.Clear();
                            _box = new DrawBox
                            {
                                Dock = DockStyle.Fill,
                            };
                            drawPanel.Controls.Add(_box);
                        }

                        break;
                    }
                case "Включённый индекс":
                    {
                        int? val;
                        if (firstDeleteBox.Text == "null" || int.TryParse(firstDeleteBox.Text, out var x))
                            val = firstDeleteBox.Text == "null" ? (int?)null : int.Parse(firstDeleteBox.Text);
                        else
                        {
                            _ = MessageBox.Show("Введите значение!", "Ошибка!");
                            return;
                        }

                        var value = new IncludedIndexValue(val, null, null);

                        var item = IncludedTree.Search(value).FirstOrDefault();
                        if (item != null)
                        {
                            IncludedTree.Remove(value);
                            _box.Print<BTree<int>>(IncludedTree);

                            _ = storyListBox.Items.Add($"<- {item.Value?.ToString() ?? "null"}  ->  {item.firstField?.ToString() ?? "null"}  :  {item.secondField?.ToString() ?? "null"}");
                            firstDeleteBox.Text = "";
                            _ = firstInsertBox.Focus();

                            if (IncludedTree.GetAllNodes().ToList().Sum(node => node.Keys.Count) == 0)
                            {
                                drawPanel.Controls.Clear();
                                _box = new DrawBox
                                {
                                    Dock = DockStyle.Fill,
                                };
                                drawPanel.Controls.Add(_box);
                            }
                        }

                        break;
                    }
            }

        }

        private void insertBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Enter))
            {
                insertButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void deleteBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Enter))
            {
                deleteButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void clearTreeButton_Click(object sender, EventArgs e)
        {
            switch (typeIndex)
            {
                case "Простой индекс":
                    {
                        // Очистка всех элементов управления и переменных
                        drawPanel.Controls.Clear();
                        storyListBox.Items.Clear();
                        uniqueButton.Enabled = true;
                        uniqueButton.Checked = false;
                        isCreatedTree = false;
                        firstInsertBox.Text = "";
                        firstDeleteBox.Text = "";

                        // Пересоздание DrawBox
                        _box = new DrawBox
                        {
                            Dock = DockStyle.Fill,
                        };
                        drawPanel.Controls.Add(_box);

                        // Очистка дерева
                        SimpleTree = null;

                        break;
                    }
                case "Составной индекс":
                    {
                        // Очистка всех элементов управления и переменных
                        drawPanel.Controls.Clear();
                        storyListBox.Items.Clear();
                        uniqueButton.Enabled = true;
                        uniqueButton.Checked = false;
                        isCreatedTree = false;
                        firstInsertBox.Text = "";
                        secondInsertBox.Text = "";
                        firstDeleteBox.Text = "";
                        secondDeleteBox.Text = "";
                        // Пересоздание DrawBox
                        _box = new DrawBox
                        {
                            Dock = DockStyle.Fill,
                        };
                        drawPanel.Controls.Add(_box);

                        // Очистка дерева
                        ComplexTree = null;

                        break;
                    }
                case "Частичный индекс":
                    {
                        // Очистка всех элементов управления и переменных
                        drawPanel.Controls.Clear();
                        storyListBox.Items.Clear();
                        uniqueButton.Enabled = true;
                        uniqueButton.Checked = false;
                        isCreatedTree = false;
                        firstInsertBox.Text = "";
                        firstDeleteBox.Text = "";
                        firstDeleteBox.Enabled = false;
                        deleteButton.Enabled = false;
                        minValueCondition.Text = "";
                        minValueCondition.Enabled = true;
                        maxValueCondition.Text = "";
                        maxValueCondition.Enabled = true;

                        // Пересоздание DrawBox
                        _box = new DrawBox
                        {
                            Dock = DockStyle.Fill,
                        };
                        drawPanel.Controls.Add(_box);

                        // Очистка дерева
                        PartialTree = null;

                        break;
                    }
                case "Включённый индекс":
                    {
                        // Очистка всех элементов управления и переменных
                        drawPanel.Controls.Clear();
                        storyListBox.Items.Clear();
                        showIncludedRecords.Items.Clear();
                        uniqueButton.Enabled = true;
                        uniqueButton.Checked = false;
                        isCreatedTree = false;

                        firstInsertBox.Text = "";
                        firstDeleteBox.Text = "";
                        firstDeleteBox.Enabled = false;
                        deleteButton.Enabled = false;
                        firstField.Text = "";
                        secondField.Text = "";
                        showField.Text = "";
                        showField.Enabled = false;
                        showButton.Enabled = false;
                        SetPlaceholderText(firstField, null);
                        SetPlaceholderText(secondField, null);

                        // Пересоздание DrawBox
                        _box = new DrawBox
                        {
                            Dock = DockStyle.Fill,
                        };
                        drawPanel.Controls.Add(_box);

                        // Очистка дерева
                        IncludedTree = null;

                        break;
                    }
            }

        }

        private ITree<T> CreateTree<T>() where T : IComparable<T>
        {
            switch (typeIndex)
            {
                case "Простой индекс":
                    return new BTree<SimpleIndexValue>(new TreeConfiguration(), 3, uniqueButton.Checked) as ITree<T>;
                case "Составной индекс":
                    return new BTree<ComplexIndexValue>(new TreeConfiguration(), 3, uniqueButton.Checked) as ITree<T>;
                case "Частичный индекс":
                    return new BTree<PartialIndexValue>(new TreeConfiguration(), 3, uniqueButton.Checked) as ITree<T>;
                case "Включённый индекс":
                    return new BTree<IncludedIndexValue>(new TreeConfiguration(), 3, uniqueButton.Checked) as ITree<T>;
                default:
                    throw new InvalidOperationException("Unknown index type.");
            }
        }
    }
}
