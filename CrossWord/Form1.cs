namespace CrossWord
{
    public partial class Form1 : Form
    {
        private const int gridSize = 15;
        private Button[,] gridButtons = new Button[gridSize, gridSize];
        private List<string> wordsList = new List<string>();
        private HashSet<Point> selectedCells = new HashSet<Point>();

        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
            LoadWordsFromFile("words.txt");
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    var button = new Button
                    {
                        Size = new Size(30, 30),
                        Location = new Point(i * 30, j * 30),
                        Tag = new Point(i, j)
                    };
                    button.Click += GridButton_Click;
                    gridButtons[i, j] = button;
                    Controls.Add(button);
                }
            }

            var generateButton = new Button
            {
                Text = "Сгенерировать кроссворд",
                Location = new Point(0, gridSize * 30 + 10),
                Size = new Size(gridSize * 30, 30)
            };
            generateButton.Click += GenerateButton_Click;
            Controls.Add(generateButton);

            var clearButton = new Button
            {
                Text = "Очистить",
                Location = new Point(0, gridSize * 30 + 40),
                Size = new Size(gridSize * 30, 30)
            };
            clearButton.Click += ClearButton_Click;
            Controls.Add(clearButton);
        }

        private void LoadWordsFromFile(string filePath)
        {
            try
            {
                wordsList = File.ReadAllLines(filePath).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка при чтении файла: " + e.Message);
            }
        }

        private void GridButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var location = (Point)button.Tag;

            if (selectedCells.Contains(location))
            {
                selectedCells.Remove(location);
                button.BackColor = DefaultBackColor;
            }
            else
            {
                selectedCells.Add(location);
                button.BackColor = Color.LightBlue;
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // Сортировка слов по убыванию длины
            var wordsToPlace = new List<string>(wordsList.OrderByDescending(word => word.Length));
            var usedWords = new HashSet<string>();

            foreach (var word in wordsToPlace)
            {
                if (usedWords.Contains(word))
                    continue;

                bool placed = false;

                // Попытка вставить слово горизонтально в выбранные ячейки
                for (int i = 0; i < gridSize && !placed; i++)
                {
                    for (int j = 0; j <= gridSize - word.Length && !placed; j++)
                    {
                        if (CanPlaceWordHorizontally(word, i, j))
                        {
                            PlaceWordHorizontally(word, i, j);
                            usedWords.Add(word);
                            placed = true;
                        }
                    }
                }

                // Попытка вставить слово вертикально в выбранные ячейки
                for (int j = 0; j < gridSize && !placed; j++)
                {
                    for (int i = 0; i <= gridSize - word.Length && !placed; i++)
                    {
                        if (CanPlaceWordVertically(word, i, j))
                        {
                            PlaceWordVertically(word, i, j);
                            usedWords.Add(word);
                            placed = true;
                        }
                    }
                }
            }
        }

        private bool CanPlaceWordHorizontally(string word, int row, int col)
        {
            // Проверка, помещается ли слово в выбранные ячейки
            if (col + word.Length > gridSize)
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                var cell = new Point(row, col + i);

                if (!selectedCells.Contains(cell) ||
                    (gridButtons[row, col + i].Text != "" && gridButtons[row, col + i].Text != word[i].ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private void PlaceWordHorizontally(string word, int row, int col)
        {
            for (int i = 0; i < word.Length; i++)
            {
                gridButtons[row, col + i].Text = word[i].ToString();
                selectedCells.Add(new Point(row, col + i));
            }
        }

        private bool CanPlaceWordVertically(string word, int row, int col)
        {
            // Проверка, помещается ли слово в выбранные ячейки
            if (row + word.Length > gridSize)
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                var cell = new Point(row + i, col);

                if (!selectedCells.Contains(cell) ||
                    (gridButtons[row + i, col].Text != "" && gridButtons[row + i, col].Text != word[i].ToString()))
                {
                    return false;
                }
            }
            return true;
        }

        private void PlaceWordVertically(string word, int row, int col)
        {
            for (int i = 0; i < word.Length; i++)
            {
                gridButtons[row + i, col].Text = word[i].ToString();
                selectedCells.Add(new Point(row + i, col));
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearGrid();
        }

        private void ClearGrid()
        {
            foreach (var button in gridButtons)
            {
                button.Text = "";
                button.BackColor = DefaultBackColor;
            }
            selectedCells.Clear();
        }
    }
}
