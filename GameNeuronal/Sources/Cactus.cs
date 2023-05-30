using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameNeuronal.Sources
{
    public class Cactus
    {
        private Texture2D _texture;
        private Vector2 _position;

        public Vector2 Position => _position;

        public Cactus(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;

        }

        public void Update(float _speed)
        {
            _position.X -= _speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
