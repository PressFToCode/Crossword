using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;

namespace CrossWordTests
{
    [TestClass]
    public class CrossWordTests
    {
        private const int gridSize = 15;
        private Button[,] gridButtons;
        private HashSet<Point> selectedCells;

        [TestInitialize]
        public void TestInitialize()
        {
            gridButtons = new Button[gridSize, gridSize];
            selectedCells = new HashSet<Point>();

            // Инициализация сетки кнопок
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    gridButtons[i, j] = new Button { Text = "" };
                }
            }
        }

        [TestMethod]
        public void TestCanPlaceWordHorizontally()
        {
            // Пример теста для горизонтальной вставки
            string word = "TEST";
            int row = 0;
            int col = 0;

            // Отметить ячейки как выбранные
            for (int i = 0; i < word.Length; i++)
            {
                selectedCells.Add(new Point(row, col + i));
            }

            bool result = CanPlaceWordHorizontally(word, row, col);
            Assert.IsTrue(result);

            // Test overlapping with existing words
            gridButtons[row, col + 2].Text = "S";
            result = CanPlaceWordHorizontally(word, row, col);
            Assert.IsTrue(result);

            // Test conflicting overlap
            gridButtons[row, col + 2].Text = "X";
            result = CanPlaceWordHorizontally(word, row, col);
            Assert.IsFalse(result);

            // Test word too long
            result = CanPlaceWordHorizontally("TOOLONGWORD", row, col);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestCanPlaceWordVertically()
        {
            // Пример теста для вертикальной вставки
            string word = "TEST";
            int row = 0;
            int col = 0;

            // Отметить ячейки как выбранные
            for (int i = 0; i < word.Length; i++)
            {
                selectedCells.Add(new Point(row + i, col));
            }

            bool result = CanPlaceWordVertically(word, row, col);
            Assert.IsTrue(result);

            // Test overlapping with existing words
            gridButtons[row + 2, col].Text = "S";
            result = CanPlaceWordVertically(word, row, col);
            Assert.IsTrue(result);

            // Test conflicting overlap
            gridButtons[row + 2, col].Text = "X";
            result = CanPlaceWordVertically(word, row, col);
            Assert.IsFalse(result);

            // Test word too long
            result = CanPlaceWordVertically("TOOLONGWORD", row, col);
            Assert.IsFalse(result);
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
    }

    // Вспомогательный класс для имитации кнопок
    public class Button
    {
        public string Text { get; set; }
    }
}
