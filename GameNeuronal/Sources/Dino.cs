using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
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
        public bool last { set; get; }
        Texture2D rect { set; get; }
        public Rectangle coll { set; get; }

        Color color = Color.White;

        private int distanciaCactus;
        private int currentState;
        private int currentAction;
        private int prevState;
        private int prevAction;

        //Red Reunoral (Inteligencia Artificial)
        private NeuralNetwork neuralNetwork;

        public Dino()
        {
            neuralNetwork = new NeuralNetwork();

            rect = new Texture2D(MainGame._graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });

            Reset();

        }

        public void Reset()
        {
            Random rnd = new Random();

            x = 200 + rnd.Next(-80, 80);
            y = 450;
            w = 80;
            h = 86;

            distanciaCactus = 0;
            currentState = 0;
            currentAction = 0;
            prevState = 0;
            prevAction = 0;

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

                // Actualiza el agente de IA
                TomarDecision();

            }
        }

        BaseEnemy GetEnemy()
        {
            if (MainGame.enemies.Count > 0)
            {
                int distance = CalculateDistanceToObstacle();

                if (distance < 0)
                {
                    return MainGame.enemies[1];
                }
                else
                {
                    return MainGame.enemies[0];
                }
            }

            return null;
        }

        public int CalculateDistanceToObstacle()
        {
            int distance = 0; // Inicializar con un valor infinito para encontrar el obstáculo más cercano

            if (MainGame.enemies.Count > 0)
            {
                distance = MainGame.enemies[0].x - x;
                if (distance < 0)
                {
                    distance = MainGame.enemies[1].x - x;
                }
            }

            return distance;
        }

        public int CalculateObstaclePositionX()
        {
            int x = 0; 

            if (MainGame.enemies.Count > 0)
            {
                int distance = CalculateDistanceToObstacle();

                if (distance < 0)
                {
                    x = MainGame.enemies[1].x;
                }else
                {
                    x = MainGame.enemies[0].x;
                }
            }

            return x;
        }

        public int CalculateObstaclePositionY()
        {
            int y = 0;

            if (MainGame.enemies.Count > 0)
            {
                int distance = CalculateDistanceToObstacle();

                if (distance < 0)
                {
                    y = MainGame.enemies[1].y;
                }
                else
                {
                    y = MainGame.enemies[0].y;
                }
            }

            return y;
        }


        public int CalculateObstacleWidth()
        {
            int w = 0;

            if (MainGame.enemies.Count > 0)
            {
                int distance = CalculateDistanceToObstacle();

                if (distance < 0)
                {
                    w = MainGame.enemies[1].w;
                }
                else
                {
                    w = MainGame.enemies[0].w;
                }
            }

            return w;
        }

        public int CalculateObstacleHeight()
        {
            int h = 0;

            if (MainGame.enemies.Count > 0)
            {
                int distance = CalculateDistanceToObstacle();

                if (distance < 0)
                {
                    h = MainGame.enemies[1].h;
                }
                else
                {
                   h = MainGame.enemies[0].h;
                }
            }

            return h;
        }

        public void TomarDecision()
        {
            // Obtener las características del entorno (por ejemplo, la posición del dinosaurio y el cactus)
            float[] input = GetEnvironmentFeatures();

            // Obtener la predicción de la red neuronal
            float[] output = neuralNetwork.Predict(input);


            if (currentAction == 1)
            {
                onJump();
            }
            else if (currentAction == 2)
            {
                onDuck();
            }
            else
            {
                // No hacer nada
            }

        }

        float[] GetEnvironmentFeatures()
        {
            float[] features = new float[3];
            features[0] = CalculateDistanceToObstacle();
            features[1] = CalculateObstacleHeight();
            features[2] = CalcularRecompensa();
            return features;
        }

        int CalcularRecompensa()
        {
            onCollition();

            if (dead)
            {
                // El dinosaurio colisionó con un obstáculo (cactus)
                return -1; // Recompensa negativa por colisión
            }
            else
            {
                // El dinosaurio realizó un salto exitoso sin colisionar
                return 1; // Recompensa positiva por salto exitoso
            }
        }

        void ActualizarEstado(int distanciaCactus)
        {

            currentState = MapDistanceToState(distanciaCactus);
        }


        int MapDistanceToState(int distanciaCactus)
        {

            var enemy = GetEnemy();

            if(enemy == null)
            {
                return 0;
            }
            if (distanciaCactus <= 100)
            {
                if (enemy.h >= 30)
                {
                    return 4;
                }
                return 1;
            }
            else if (distanciaCactus <= 160)
            {
                if (enemy.h >= 30)
                {
                    return 4;
                }
                return 2;
            }
            else if (distanciaCactus <= 220)
            {
                if (enemy.h >= 30)
                {
                    return 4;
                }
                return 3;
            }
            else
            {
                return 0;
            }
        }


        public int ObtenerAccionActual()
        {
            return currentAction;
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
