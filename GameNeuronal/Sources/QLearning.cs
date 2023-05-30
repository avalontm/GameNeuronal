using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameNeuronal.Sources
{
    public class QLearning
    {
        private float[,] qTable;
        private float learningRate;
        private float discountFactor;
        private int numStates;
        private int numActions;

        public QLearning(int numStates, int numActions, float learningRate, float discountFactor)
        {
            this.numStates = numStates;
            this.numActions = numActions;
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;

            qTable = new float[numStates, numActions];

            // Inicializar la tabla Q con valores iniciales
            for (int state = 0; state < numStates; state++)
            {
                for (int action = 0; action < numActions; action++)
                {
                    qTable[state, action] = 0f;
                }
            }
        }

        public int GetBestAction(int state)
        {
            int bestAction = 0;
            float bestQValue = qTable[state, 0];

            // Encontrar la mejor acción basada en los valores Q
            for (int action = 1; action < numActions; action++)
            {
                if (qTable[state, action] > bestQValue)
                {
                    bestAction = action;
                    bestQValue = qTable[state, action];
                }
            }

            return bestAction;
        }

        public void UpdateQValue(int prevState, int prevAction, int currentState, float reward)
        {
            // Calcular el nuevo valor Q utilizando el algoritmo Q-Learning
            float currentQValue = qTable[prevState, prevAction];
            float maxFutureQValue = GetMaxQValue(currentState);
            float newQValue = currentQValue + learningRate * (reward + discountFactor * maxFutureQValue - currentQValue);

            // Actualizar el valor Q en la tabla
            qTable[prevState, prevAction] = newQValue;
        }

        private float GetMaxQValue(int state)
        {
            float maxQValue = qTable[state, 0];

            // Encontrar el valor Q máximo para el estado dado
            for (int action = 1; action < numActions; action++)
            {
                if (qTable[state, action] > maxQValue)
                {
                    maxQValue = qTable[state, action];
                }
            }

            return maxQValue;
        }
    }
}
