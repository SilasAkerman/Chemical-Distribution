using System;
using System.Numerics;
using Raylib_cs;

namespace Chemical_Diffusion
{
    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }

        private double _a;
        private double _b;
        public double A 
        { 
            get 
            { 
                return _a; 
            } 
            set 
            {
                _a = Math.Clamp(value, 0, 1);
            } 
        }
        public double B 
        { 
            get 
            { 
                return _b; 
            } 
            set 
            { 
                _b = Math.Clamp(value, 0, 1); 
            } 
        }

        int row;
        int col;
        int size;

        public Cell(int aRow, int aCol, int aSize)
        {
            row = aRow;
            col = aCol;
            size = aSize;

            X = col * size;
            Y = row * size;
        }

        public void Display()
        {
            int opacity = (int)Math.Floor((A - B) * 255);
            opacity = Math.Clamp(opacity, 0, 255);
            Color color = new Color(opacity, opacity, opacity, 255);
            Raylib.DrawRectangle(X, Y, size, size, color);
        }

        public Cell CalculateNewCell(Cell[,] grid, double dA, double dB, double feed, double kill, double multiplier)
        {
            Cell cell = new Cell(row, col, size);
            cell.A = A + (dA * LaplaceA(grid) - A * B * B + feed * (1 - A)) * multiplier;
            cell.B = B + (dB * LaplaceB(grid) + A * B * B - (kill + feed) * B) * multiplier;
            return cell;
        }

        private double LaplaceA(Cell[,] grid)
        {
            double sumA = 0;
            sumA += -A;
            sumA += grid[row - 1, col].A * 0.2;
            sumA += grid[row + 1, col].A * 0.2;
            sumA += grid[row, col + 1].A * 0.2;
            sumA += grid[row, col - 1].A * 0.2;
            sumA += grid[row - 1, col - 1].A * 0.05;
            sumA += grid[row + 1, col - 1].A * 0.05;
            sumA += grid[row - 1, col + 1].A * 0.05;
            sumA += grid[row + 1, col + 1].A * 0.05;
            return sumA;
        }

        private double LaplaceB(Cell[,] grid)
        {
            double sumB = 0;
            sumB += -B;
            sumB += grid[row - 1, col].B * 0.2;
            sumB += grid[row + 1, col].B * 0.2;
            sumB += grid[row, col + 1].B * 0.2;
            sumB += grid[row, col - 1].B * 0.2;
            sumB += grid[row - 1, col - 1].B * 0.05;
            sumB += grid[row + 1, col - 1].B * 0.05;
            sumB += grid[row - 1, col + 1].B * 0.05;
            sumB += grid[row + 1, col + 1].B * 0.05;
            return sumB;
        }

        public double CalculateDistance(Vector2 mouse)
        {
            Vector2 pos = new Vector2(X, Y);
            return Vector2.Distance(pos, mouse);
        }
    }
}
