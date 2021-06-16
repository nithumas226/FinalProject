using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace FinalProject
{
    public partial class Form1 : Form
    {
        //Global Variables
        Rectangle player1 = new Rectangle(145, 550, 5, 10);
        Rectangle player2 = new Rectangle(445, 550, 5, 10);
        List<Rectangle> rightObjects = new List<Rectangle>();
        List<Rectangle> leftObjects = new List<Rectangle>();

        int playerSpeed = 7;
        int player1score = 0;
        int player2score = 0;
        int rightobjectsSpeed = 6;
        int leftobjectsSpeed = -6;
        int rightobjectCounter = 0;
        int leftobjectCounter = 0;

        bool wDown = false;
        bool sDown = false;
        bool upDown = false;
        bool downDown = false;

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush greenBrush = new SolidBrush(Color.Green);

        Random randGen = new Random();
        SoundPlayer musicPlayer = new SoundPlayer();
        string gameState = "starting";
        public Form1()
        {
            InitializeComponent();
            musicPlayer = new SoundPlayer(Properties.Resources.Intro);
            musicPlayer.Play();
        }


        public void GameInitialize()
        {
            titleLabel.Text = "";
            subtitleLable.Text = "";

            player1 = new Rectangle(145, 550, 5, 10);
            player2 = new Rectangle(445, 550, 5, 10);
            rightObjects.Clear();
            leftObjects.Clear();

            gameTimer.Enabled = true;
            gameState = "running";
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "starting" || gameState == "over")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "starting" || gameState == "over")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //Show Player Score
            p1scoreLabel.Text = $"{player1score}";
            p2scoreLabel.Text = $"{player2score}";

            //Move Players
            MovePlayer();

            //Moving Objects
            MoveObjects();

            //Adding Score
            AddScore();

            //Moving Player Back
            HitObjects();

            //Declaring Winner
            Winner();

            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "starting")
            {
                titleLabel.Text = "Welcome to Space Race!!";
                subtitleLable.Text = "First Player to 3 Points Wins \nPress Spacebar to Play or Esc to Exit";
            }
            else if (gameState == "running")
            {
                e.Graphics.FillRectangle(whiteBrush, player1);
                e.Graphics.FillRectangle(greenBrush, player2);
                for (int i = 0; i < rightObjects.Count(); i++)
                {
                    e.Graphics.FillRectangle(redBrush, rightObjects[i]);
                }
                for (int i = 0; i < leftObjects.Count(); i++)
                {
                    e.Graphics.FillRectangle(redBrush, leftObjects[i]);
                }
            }
            else if (gameState == "over")
            {
                if (player1score == 3)
                {
                    titleLabel.ForeColor = Color.White;
                    titleLabel.Text = "White has Won!!";
                    subtitleLable.Text = "Press Spacebar to Play Again or Esc to Exit";
                    player1score = 0;
                    player2score = 0;
                }
                else if (player2score == 3)
                {
                    titleLabel.ForeColor = Color.Green;
                    titleLabel.Text = "Green has Won!!";
                    subtitleLable.Text = "Press Spacebar to Play Again or Esc to Exit";
                    player1score = 0;
                    player2score = 0;
                }
                p1scoreLabel.Text = "";
                p2scoreLabel.Text = "";
            }
        }

        public void MovePlayer()
        {
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }

            if (upDown == true && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }

            if (downDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }
        }

        public void MoveObjects()
        {
            for (int i = 0; i < rightObjects.Count(); i++)
            {
                int x = rightObjects[i].X + rightobjectsSpeed;
                rightObjects[i] = new Rectangle(x, rightObjects[i].Y, 10, 5);
            }

            for (int i = 0; i < leftObjects.Count(); i++)
            {
                int x = leftObjects[i].X + leftobjectsSpeed;
                leftObjects[i] = new Rectangle(x, leftObjects[i].Y, 10, 5);
            }

            rightobjectCounter++;
            if (rightobjectCounter == 5)
            {
                int yCoordinate;
                yCoordinate = randGen.Next(75, 500);
                rightObjects.Add(new Rectangle(0, yCoordinate, 10, 5));
                rightobjectCounter = 0;
            }

            leftobjectCounter++;
            if (leftobjectCounter == 5)
            {
                int yCoordinate;
                yCoordinate = randGen.Next(75, 500);
                leftObjects.Add(new Rectangle(this.Width, yCoordinate, 10, 5));
                leftobjectCounter = 0;
            }
        }

        public void AddScore()
        {
            if (player1.Y <= 0)
            {
                player1score++;
                player1.Y = 550;
                musicPlayer = new SoundPlayer(Properties.Resources.pointUp);
                musicPlayer.Play();
            }

            if (player2.Y <= 0)
            {
                player2score++;
                player2.Y = 550;
                musicPlayer = new SoundPlayer(Properties.Resources.pointUp);
                musicPlayer.Play();
            }
        }

        public void HitObjects()
        {
            for (int i = 0; i < rightObjects.Count(); i++)
            {
                if (player1.IntersectsWith(rightObjects[i]))
                {
                    player1.Y = 550;
                    musicPlayer = new SoundPlayer(Properties.Resources.healthDown);
                    musicPlayer.Play();
                }
                else if (player2.IntersectsWith(rightObjects[i]))
                {
                    player2.Y = 550;
                    musicPlayer = new SoundPlayer(Properties.Resources.healthDown);
                    musicPlayer.Play();
                }
            }

            for (int i = 0; i < leftObjects.Count(); i++)
            {
                 if (player1.IntersectsWith(leftObjects[i]))
                 {
                   player1.Y = 550;
                   musicPlayer = new SoundPlayer(Properties.Resources.healthDown);
                   musicPlayer.Play();
                 }
                 else if (player2.IntersectsWith(leftObjects[i]))
                 {
                    player2.Y = 550;
                    musicPlayer = new SoundPlayer(Properties.Resources.healthDown);
                    musicPlayer.Play();
                 }
            }
        }
        
        public void Winner()
            {
                if (player1score == 3)
                {
                    gameTimer.Enabled = false;
                    musicPlayer = new SoundPlayer(Properties.Resources.win);
                    musicPlayer.Play();
                    gameState = "over";
                }

                if (player2score == 3)
                {
                    gameTimer.Enabled = false;
                    musicPlayer = new SoundPlayer(Properties.Resources.win);
                    musicPlayer.Play();
                    gameState = "over";
                }
            }
        }
    }
