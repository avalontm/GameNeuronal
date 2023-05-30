using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GameNeuronal.Sources
{
    public class Dinosaurio
    {
        private Texture2D texture;
        public Vector2 Position;
        private Vector2 velocity;
        private bool isJumping;

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);

        public Dinosaurio(Texture2D texture, Vector2 Position)
        {
            this.texture = texture;
            this.Position = Position;
            velocity = Vector2.Zero;
            isJumping = false;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Aplicar la gravedad
            velocity.Y += 9.8f * deltaTime;

            // Actualizar la posición según la velocidad
            Position += velocity * deltaTime;

            // Verificar si el dinosaurio está en el suelo
            if (Position.Y >= 400)
            {
                Position.Y = 400;
                velocity.Y = 0;
                isJumping = false;
            }
        }

        public void Jump()
        {
            if (!isJumping)
            {
                velocity.Y = -300;
                isJumping = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
