using GameNeuronal.Sources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace GameNeuronal
{
    public class MainGame : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static IList<Dino> players;
        public static IList<BaseEnemy> enemies;
        public static GameTime time { private set; get; }
        float speedStart = 12;
        public static float speed = 12;
        public static bool GameOver = false;

        SpriteBatch _spriteBatch;
        StageBackground background;
        SpriteFont font;

        ProbabilidadPorcentaje probabilidad;
        int every_sec = 0;
        int generation = 0;
        int alive = 0;
        Dino playerTarget;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.SynchronizeWithVerticalRetrace = false;

            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("default");

            probabilidad = new ProbabilidadPorcentaje();
            Animations.Load(Content);

            background = new StageBackground();
            players = new List<Dino>();
            enemies = new List<BaseEnemy>();

            for (int i = 0; i < 1000; i++)
            {
                players.Add(new Dino());
            }

            alive = players.Count;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time = gameTime;

            InputManager.GetState();

            if (GameOver)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    GameStart();
                }
            }

            onGameOver();

            if (!GameOver)
            {
                // TODO: Add your update logic here
                background.Update(gameTime);

                for (int p = 0; p < players.Count; p++)
                {
                    players[p].Update(gameTime);
                }

                for (int c = 0; c < enemies.Count; c++)
                {
                    enemies[c].Update(speed);
                }

                if (every_sec > 60)
                {
                    every_sec = 0;
                    Spawn_Enemy();
                }

                every_sec += 1;
                speed += (float)(0.00000025f * gameTime.TotalGameTime.TotalSeconds);
            }
        }

        void GameStart()
        {
            for (int c = 0; c < enemies.Count; c++)
            {
                enemies.Remove(enemies[c]);
            }

            for (int i = 0; i < players.Count; i++)
            {
                players[i].Reset();
            }

            generation++;
            every_sec = 0;
            speed = speedStart;
            GameOver = false;
        }

        void onGameOver()
        {
            alive = players.Where(x => !x.dead).Count();

            if (alive == 0)
            {
                GameOver = true;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();

            background.Draw(_spriteBatch);
            // TODO: Add your drawing code here

            for (int p = 0; p < players.Count; p++)
            {
                players[p].Draw(_spriteBatch);
            }

            for (int c = 0; c < enemies.Count; c++)
            {
                enemies[c].Draw(_spriteBatch);
            }

            DrawDebug();

            _spriteBatch.End();
            base.Draw(gameTime);
        }


        void Spawn_Enemy()
        {
            if (probabilidad.GenerarConProbabilidad(20))
            {
                enemies.Add(new Bird());
            }
            else
            {
                enemies.Add(new Cactus());
            }
        }

        void DrawDebug()
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].dead)
                {
                    playerTarget = players[i];
                    break;
                }
            }

            _spriteBatch.DrawString(font, $"(Obstacle) Distance: {playerTarget.CalculateDistanceToObstacle()}", new Vector2(10, 20), Color.Black);
            _spriteBatch.DrawString(font, $"(Obstacle) X: {playerTarget.CalculateObstaclePositionX()}", new Vector2(10, 40), Color.Black);
            _spriteBatch.DrawString(font, $"(Obstacle) Y: {playerTarget.CalculateObstaclePositionY()}", new Vector2(10, 60), Color.Black);
            _spriteBatch.DrawString(font, $"(Obstacle) Width: {playerTarget.CalculateObstacleWidth()}", new Vector2(10, 80), Color.Black);
            _spriteBatch.DrawString(font, $"(Obstacle) Height: {playerTarget.CalculateObstacleHeight()}", new Vector2(10, 100), Color.Black);
            _spriteBatch.DrawString(font, $"(Dino) Y: {playerTarget.y}", new Vector2(10, 120), Color.Black);
            _spriteBatch.DrawString(font, $"(Game) Speed: {speed}", new Vector2(10, 140), Color.Black);

            _spriteBatch.DrawString(font, $"Generation: {generation}", new Vector2(_graphics.PreferredBackBufferWidth - 250, 20), Color.Black);
            _spriteBatch.DrawString(font, $"Alive: {alive}", new Vector2(_graphics.PreferredBackBufferWidth - 220, 40), Color.Black);
        }
    }
}