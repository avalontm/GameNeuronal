using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameNeuronal.Sources
{
    
    public class BaseEnemy 
    {
        public int x { set; get; }
        public int y { set; get; }
        public int w { set; get; }
        public int h { set; get; }

        public int type { set; get; }

        internal Texture2D rect { set; get; }
        public Rectangle coll { set; get; }

        public BaseEnemy()
        {
            rect = new Texture2D(MainGame._graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

        }

        public virtual void Update(double speed)
        {
            if (x < -10)
            {
                MainGame.enemies.Remove(this);
            }
        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {

        }
    }
}
