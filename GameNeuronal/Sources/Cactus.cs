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
    public class Cactus : BaseEnemy
    {
        public Cactus()
        {
            Random rnd = new Random();

            x = 1350;
            type = (int)rnd.Next(6);

            if (type < 3)
            {
                h = 66;
                y = 470;
            }
            else
            {
                h = 96;
                y = 440;
            }

            switch (type)
            {
                case 0:
                    w = 30;
                    break;
                case 1:
                    w = 64;
                    break;
                case 2:
                    w = 98;
                    break;
                case 3:
                    w = 46;
                    break;
                case 4:
                    w = 96;
                    break;
                case 5:
                    w = 146;
                    break;
            }

            coll = new Rectangle(x, y, w, h);
        }


        public override void Update(double speed)
        {
            base.Update(speed);
            x -= (int)speed;

        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);
            coll = new Rectangle((int)x, (int)y, w, h);

            _spriteBatch.Draw(rect, coll, Color.Green);
        }
    }
}
