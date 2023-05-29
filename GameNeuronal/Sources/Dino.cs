using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Linq;

namespace GameNeuronal.Sources
{
    public class Dino
    {
        public int x { set; get; }
        public int y { set; get; }
        public int w { set; get; }
        public int h { set; get; }
        public bool jumping { set; get; }
        public bool crounching { set; get; }
        public float jump_stage { set; get; }
        public bool dead { set; get; }
        public bool last { set;get; }
        Texture2D rect { set; get; }
        public Rectangle coll { set; get; }

        //Red Reunoral (Inteligencia Artificial)
        public RedNeuronal redNeuronal { private set; get; }
        Color color = Color.White;

        public Dino()
        {
            rect = new Texture2D(MainGame._graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

            Start();

            redNeuronal = new RedNeuronal(this);
        }

        public void Start()
        {
            Random rnd = new Random();

            x = 200 + rnd.Next(-80, 80);
            y = 450;
            w = 80;
            h = 86;

            jumping = false;
            crounching = false;

            color = Color.White;
            coll = new Rectangle(x, y, w, h);
            dead = false;
            last = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!dead)
            {
                if (jumping)
                {
                    y = 448 - (int)f(jump_stage);
                    jump_stage += 0.03f;
                    if (jump_stage > 1)
                    {
                        jumping = false;
                        crounching = false;
                        jump_stage = 0;
                        y = 450;
                    }
                }
                else if (crounching)
                {
                    crounching = false;
                    y -= 34;
                    w = 80;
                    h = 86;
                }
                else
                {
                    jumping = false;
                    crounching = false;
                    jump_stage = 0;
                    y = 450;
                }

                /*
                    if (Inputs.IsKeyPressed(Keys.Up, true))
                    {
                        onJump();
                    }
                    if (Inputs.IsPressed(Keys.Down))
                    {
                        onDuck();
                    }
                */

                redNeuronal.Update();

                onCollition();

            }
        }

        void onCollition()
        {
            for (int c = 0; c < MainGame.enemies.Count; c++)
            {
                if (Collition(MainGame.enemies[c]))
                {
                    if (MainGame.players.Where(x => !x.dead).Count() == 1)
                    {
                        last = true;
                    }
                    color = Color.Red;
                    dead = true;
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (!dead)
            {
                coll = new Rectangle(x, y, w, h);

                _spriteBatch.Draw(rect, coll, color);
            }
        }

        float f(float x)
        {
            return (-4 * x * (x - 1) * 172);
        }

        bool Collition(BaseEnemy enemy)
        {
            return this.coll.Intersects(enemy.coll);
        }

        public void onJump()
        {
            if (!jumping)
            {
                jumping = true;
                crounching = false;
            }
        }

        public void onDuck()
        {
            if (!jumping)
            {
                jumping = false;
                crounching = true;
                y += 34;
                w = 110;
                h = 52;
            }
        }
    }
}
