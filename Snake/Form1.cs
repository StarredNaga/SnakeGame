using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private int _cellSize = 45;
        PictureBox rr = null;
        PictureBox[] snake = new PictureBox[400];
        int score = 0;
        PictureBox food = null;
        private int _dx = 0;
        private int _dy = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Score: 0";
            snake[0] = new PictureBox();
            snake[0].BackColor = Color.Green;
            snake[0].Size = new Size(_cellSize, _cellSize);
            snake[0].Location = new Point(90, 90);
            Controls.Add(snake[0]);

            SpawnFood();
            CreateMap();

            timer1.Interval = 500;
            timer1.Start();
        }

        private void CreateSnake()
        {
            snake[score] = new PictureBox();
            snake[score].BackColor = Color.Green;
            snake[score].Size = new Size(_cellSize, _cellSize);
            snake[score].Location = new Point(snake[score - 1].Location.X - (_dx * _cellSize), snake[score - 1].Location.Y - (_dy * _cellSize));
            Controls.Add(snake[score]);
        }

        private void CreateMap()
        {
            for (int i = 0; i < this.Width - 120; i += _cellSize)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.CadetBlue;
                pic.Size = new Size(2, this.Height - 30);
                pic.Location = new Point(i, 0);
                Controls.Add(pic);
            }

            for (int i = 0; i < this.Height; i += _cellSize)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.CadetBlue;
                pic.Size = new Size(this.Width - 160, 2);
                pic.Location = new Point(0, i);
                Controls.Add(pic);
            }
        }

        private void SpawnFood()
        {
            Random rand = new Random();

            int x = rand.Next(this.Width - _cellSize - 120);
            int y = rand.Next(this.Height - _cellSize);
            int tempX = x % _cellSize;
            int tempY = y % _cellSize;
            x -= tempX;
            y -= tempY;

            food = new PictureBox();
            food.BackColor = Color.Red;
            food.Size = new Size(_cellSize, _cellSize);
            food.Location = new Point(x, y);
            Controls.Add(food);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    _dx = 0;
                    _dy = -1;
                    break;

                case Keys.S:
                    _dx = 0;
                    _dy = 1;
                    break;

                case Keys.A:
                    _dy = 0;
                    _dx = -1;
                    break;

                case Keys.D:
                    _dy = 0;
                    _dx = 1;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }

            snake[0].Location = new Point(snake[0].Location.X + (_dx * _cellSize), snake[0].Location.Y + (_dy * _cellSize));

            if (snake[0].Location == food.Location)
            {
                EatFood();
                return;
            }

            if (snake[0].Location.X >= this.Width - 120 || snake[0].Location.X < 0 || snake[0].Location.Y > this.Height || snake[0].Location.Y < 0)
            {
                GameLose();
                return;
            }

            CheckLose();
        }

        private void EatFood()
        {
            Controls.Remove(food);
            SpawnFood();
            score++;
            CreateSnake();
            label1.Text = $"Score: {score}";
        }

        private void CheckLose()
        {
            for (int i = 0; i < score; i++)
            {
                if (snake[0].Location == snake[i].Location && i != 0) GameLose();
            }
        }

        private void GameLose()
        {
            _dx = 0;
            _dy = 0;

            for (int i = 0; i < score; i++)
            {
                Controls.Remove(snake[i]);
            }


            MessageBox.Show("You Lose");
            Application.Exit();
        }
    }
}
