using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameNeuronal.Sources
{
    
    public class Bird : BaseEnemy
    {
        //Animacion
        int fotogramaActual;
        float tiempoTranscurrido;
        float tiempoCambioFotograma = 0.1f; // Cambia el fotograma cada 0.1 segundos
        int totalFotogramas = 2;

        public Bird()
        {
            Random rnd = new Random();

            x = 1350;
            w = 84;
            h = 40;
            type = (int)rnd.Next(3);

            switch (type)
            {
                case 0:
                    w = 30;
                    y = 365;
                    break;
                case 1:
                    w = 64;
                    y = 440;
                    break;
                case 2:
                    w = 98;
                    y = 465;
                    break;
            }

            coll = new Rectangle(x, y, w, h);
        }


        void Animation()
        {
            tiempoTranscurrido += (float)MainGame.time.ElapsedGameTime.TotalSeconds;

            if (tiempoTranscurrido >= tiempoCambioFotograma)
            {
                // Cambiar al siguiente fotograma de la animación
                fotogramaActual = (fotogramaActual + 1) % totalFotogramas;
                tiempoTranscurrido = 0;
            }
        }

        public override void Update(double speed)
        {
            base.Update(speed);

            x -= (int)speed;

            Animation();
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            base.Draw(_spriteBatch);

            coll = new Rectangle((int)x, (int)y, w, h);

            _spriteBatch.Draw(Animations.birds[fotogramaActual], coll, Color.White);
        }
    }
}
