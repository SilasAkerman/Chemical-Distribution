using System;
using System.Linq;
using System.Numerics;
using Raylib_cs;

namespace Chemical_Diffusion
{
    class Program
    {
        const int WIDTH = 1900;
        const int HEIGHT = 1000;
        const int SPACING = 5;

        const double DA = 1;
        const double DB = 0.5;
        const double FEED = 0.055;
        const double KILL = 0.062;
        //const double FEED = 0.0367;
        //const double KILL = 0.0649;
        const double MULTIPLIER = 1;

        int rows;
        int cols;

        Cell[,] grid;
        Cell[,] buffer;

        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.DoSomething();
        }

        public void DoSomething()
        {
            rows = HEIGHT / SPACING;
            cols = WIDTH / SPACING;
            grid = new Cell[rows, cols];
            buffer = new Cell[rows, cols];

            Raylib.InitWindow(WIDTH, HEIGHT, "Chemical Distribution");
            Raylib.SetTargetFPS(0);

            Setup();

            while (!Raylib.WindowShouldClose())
            {
                Display();
                for (int i = 0; i < 5; i++)
                {
                    Update();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                {
                    Setup();
                }
            }

            Raylib.CloseWindow();
        }

        void Display()
        {
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.RAYWHITE);
            foreach (Cell cell in grid)
            {
                cell.Display();
            }
            //Raylib.DrawFPS(200, 200);

            Raylib.EndDrawing();
        }

        void Update()
        {
            for (int row = 1; row < rows-1; row++)
            {
                for (int col = 1; col < cols-1; col++)
                {
                    buffer[row, col] = grid[row, col].CalculateNewCell(grid, DA, DB, FEED, KILL, MULTIPLIER); // Creates a whole new cell
                }
            }
            //grid = buffer; // This might cause issues
            grid = (Cell[,])buffer.Clone();
        }

        void Setup()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i, j] = new Cell(i, j, SPACING);
                    buffer[i, j] = new Cell(i, j, SPACING);
                    grid[i, j].A = 1;
                    grid[i, j].B = 0;
                    buffer[i, j].A = 1;
                    buffer[i, j].B = 0;
                }
            }

            //for (int i = rows / 2 - 20; i < rows / 2 + 20; i++)
            //{
            //    for (int j = cols / 2 - 10; j < cols / 2 + 10; j++)
            //    {
            //        grid[i, j].B = 1;
            //    }
            //}

            //for (int i = rows / 2 - 10; i < rows / 2 + 10; i++)
            //{
            //    for (int j = cols / 2 - 10; j < cols / 2 + 10; j++)
            //    {
            //        grid[i, j].B = 1;
            //    }
            //}

            //foreach (Cell cell in grid)
            //{
            //    if (cell.CalculateDistance(new Vector2(WIDTH / 2, HEIGHT / 2)) < 300)
            //    {
            //        cell.B = 1;
            //    }
            //}

            //for (int i = 20; i < 25; i++) // top
            //{
            //    for (int j = 20; j < cols - 20; j++)
            //    {
            //        grid[i, j].B = 1;
            //    }
            //}
            //for (int i = 20; i < rows - 20; i++) // right
            //{
            //    for (int j = cols - 25; j < cols - 20; j++)
            //    {
            //        grid[i, j].B = 1;
            //    }
            //}
            //for (int i = rows - 25; i < rows - 20; i++) // bottom
            //{
            //    for (int j = 20; j < cols - 20; j++)
            //    {
            //        grid[i, j].B = 1;
            //    }
            //}
            //for (int i = 20; i < rows - 20; i++) // left
            //{
            //    for (int j = 20; j < 25; j++)
            //    {
            //        grid[i, j].B = 1;
            //    }
            //}

            while (!Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && !Raylib.WindowShouldClose())
            {
                Display();
                if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    for (int row = 1; row < rows-1; row++)
                    {
                        for (int col = 1; col < cols-1; col++)
                        {
                            if (grid[row, col].CalculateDistance(Raylib.GetMousePosition()) < 10)
                            {
                                grid[row, col].B = 1;
                            }
                        }
                    }
                }
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                {
                    foreach (Cell cell in grid)
                    {
                        cell.B = 0;
                    }
                }
            }
        }
    }
}
