using GameNeuronal.Sources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;


namespace GameNeuronal
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Texture2D _dinosaurTexture;
        public static Texture2D _cactusTexture;

        private Dinosaur _dinosaur;
        public static List<Cactus> _cacti;

        private float _speed = 5f;
        private float _elapsedTime = 0f;
        private float _spawnInterval = 1.5f;
        private SpriteFont _font;

        private Random _random;
        public static int WindowHeight;
        public static int deads;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            WindowHeight = _graphics.PreferredBackBufferHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _random = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
           
            _font = Content.Load<SpriteFont>("default");
            _dinosaurTexture = Content.Load<Texture2D>("dinosaur");
            _cactusTexture = Content.Load<Texture2D>("cactus");

            _dinosaur = new Dinosaur(_dinosaurTexture, new Vector2(100, _graphics.PreferredBackBufferHeight - _dinosaurTexture.Height));
            _cacti = new List<Cactus>();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime >= _spawnInterval)
            {
                _cacti.Add(new Cactus(_cactusTexture, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight - _cactusTexture.Height)));
                _elapsedTime = 0f;
            }

            for (int i = 0; i < _cacti.Count; i++)
            {
                _cacti[i].Update(_speed);

                if (_cacti[i].Position.X + _cactusTexture.Width < 0)
                {
                    _cacti.RemoveAt(i);
                    i--;
                }
            }

            _dinosaur.Update(_speed);

            //_speed += 0.005f;
            _spawnInterval = (float)GetRandomNumber(1.5, 2.2);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            _dinosaur.Draw(_spriteBatch);

            foreach (var cactus in _cacti)
            {
                cactus.Draw(_spriteBatch);
            }

            _spriteBatch.DrawString(_font, $"Errores: {deads}", new Vector2(_graphics.PreferredBackBufferWidth - 150, 40), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
