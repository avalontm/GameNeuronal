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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameNeuronal.Sources
{
    public class Dinosaur
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _jumpVelocity;
        private const float _gravity = 1.2f;

        public Vector2 Position => _position;
        public bool IsJumping { get; private set; }
        NeuralNetwork neuralNetwork;
        QLearning qLearning;

        public Dinosaur(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
            _jumpVelocity = 0f;
            IsJumping = false;

            // Crea una instancia de la red neuronal
            int numStats = 8; // Por ejemplo, distancia al cactus y velocidad del juego
            int hiddenSize = 4; // Número de neuronas en la capa oculta
            int numActions = 2; // Número de acciones posibles (saltar, no saltar, agacharse)
            neuralNetwork = new NeuralNetwork(numStats, hiddenSize, numActions);

            // Definir el algoritmo Q-learning
            qLearning = new QLearning(numStats, numActions, 0.1f,0.9f);

        }

        public void Update(float speed)
        {
            if (IsJumping)
            {
                _jumpVelocity += _gravity;
                _position.Y += _jumpVelocity;

                if (_position.Y >= Game1.WindowHeight - _texture.Height)
                {
                    _position.Y = Game1.WindowHeight - _texture.Height;
                    _jumpVelocity = 0f;
                    IsJumping = false;
                }
            }

            TomarDesicion(speed);

        }

        void TomarDesicion(float speed)
        {
            // Obtener el estado actual del juego
            int currentState = GetGameState();
            int prevState = currentState;

            // Obtener la acción actual del algoritmo Q-learning
            int currentAction = qLearning.GetAction(currentState);

            // Realizar la acción en el juego y obtener la recompensa
            float reward = PerformAction(currentAction);

            // Obtener el nuevo estado del juego
            int nextState = GetGameState();

            // Actualizar el algoritmo Q-learning con la nueva transición
            qLearning.UpdateQValue(prevState, currentState, currentAction, nextState, reward);

            // Actualizar la red neuronal con la nueva transición
            neuralNetwork.Train(currentState, currentAction, nextState, reward);

            // Tomar una nueva decisión basada en la red neuronal actualizada
            int newAction = neuralNetwork.GetBestAction(nextState);

            // Actualizar la acción actual para la siguiente iteración
            currentAction = newAction;

        }

        int GetGameState()
        {
            if(IsJumping)
            {
                return 1;
            }

          return  0;
        }

        void Duck()
        {
            Debug.WriteLine($"[Duck]");
        }

        private float GetCactusDistance()
        {
            float distance = 0;

            if (Game1._cacti.Count > 0)
            {
                distance = Game1._cacti[0].Position.X - Position.X;

                if (distance < 0)
                {
                    if (Game1._cacti.Count > 1)
                    {
                        distance = Game1._cacti[1].Position.X - Position.X;
                    }
                }

            }

            return distance;
        }

        private int GetCactusState()
        {
            float closestDistance = GetCactusDistance();

            if (closestDistance < 350)
            {
                return 1; // Cactus close
            }
            else
            {
                return 0; // Cactus far
            }
        }

        float PerformAction(int currentAction)
        {
            // Implementa la lógica de recompensa basada en el éxito del salto, colisiones, etc.
            // Aquí tienes un ejemplo básico:
            if (IsColliding())
            {
                Game1.deads ++;
                return -1f; // Colisión, recompensa negativa
            }
            else
            {
                return 0f; // Sin colisión, recompensa neutral
            }
        }

        int offset = 20;
        bool IsColliding()
        {
            Rectangle dinosaurBounds = new Rectangle((int)Position.X+ offset, (int)Position.Y+ offset, _texture.Width- offset, _texture.Height- offset);

            foreach (var cactus in Game1._cacti)
            {
                Rectangle cactusBounds = new Rectangle((int)cactus.Position.X+ offset, (int)cactus.Position.Y+ offset, Game1._cactusTexture.Width- offset, Game1._cactusTexture.Height- offset);

                if (dinosaurBounds.Intersects(cactusBounds))
                {
                    return true; // Colisión detectada
                }
            }

            return false; // No hay colisión
        }

        public void Jump()
        {
            if (!IsJumping)
            {
                _jumpVelocity = -25f;
                IsJumping = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
