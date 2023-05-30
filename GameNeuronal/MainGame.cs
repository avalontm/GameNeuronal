using GameNeuronal.Sources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameNeuronal
{
    public class MainGame : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static IList<Dino> players;
        public static IList<BaseEnemy> enemies;
        public static double speed = 12;
        public static bool GameOver = false;

        SpriteBatch _spriteBatch;
        StageBackground background;
        SpriteFont font;

        int every_sec = 0;
        int generation = 0;
        int alive = 0;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.SynchronizeWithVerticalRetrace = false;
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

            Inputs.GetState();

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
            GraphicsDevice.Clear(Color.CornflowerBlue);
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
            Random rnd = new Random();

            if (rnd.Next(10) == 0)
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
            if (players.Count > 0)
            {
                _spriteBatch.DrawString(font, $"(Obstacle) Distance: {players[0].CalculateDistanceToObstacle()}", new Vector2(20, 20), Color.Black);
                _spriteBatch.DrawString(font, $"(Obstacle) X: {players[0].CalculateObstaclePositionX()}", new Vector2(20, 40), Color.Black);
                _spriteBatch.DrawString(font, $"(Obstacle) Y: {players[0].CalculateObstaclePositionY()}", new Vector2(20, 60), Color.Black);
                _spriteBatch.DrawString(font, $"(Obstacle) Width: {players[0].CalculateObstacleWidth()}", new Vector2(20, 80), Color.Black);
                _spriteBatch.DrawString(font, $"(Obstacle) Height: {players[0].CalculateObstacleHeight()}", new Vector2(20, 100), Color.Black);
                _spriteBatch.DrawString(font, $"(Dino) Y: {players[0].y}", new Vector2(20, 120), Color.Black);
                _spriteBatch.DrawString(font, $"(Game) Speed: {speed}", new Vector2(20, 140), Color.Black);

            }

            _spriteBatch.DrawString(font, $"Generation: {generation}", new Vector2(_graphics.PreferredBackBufferWidth - 150, 20), Color.Black);
            _spriteBatch.DrawString(font, $"Alive: {alive}", new Vector2(_graphics.PreferredBackBufferWidth - 150, 40), Color.Black);
        }
    }
}