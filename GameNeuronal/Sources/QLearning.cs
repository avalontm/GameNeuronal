using System;
using System.Collections.Generic;
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

        public QLearning(int numStates, int numActions, float learningRate, float discountFactor)
        {
            qTable = new float[numStates, numActions];
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
        }

        public int GetAction(int state)
        {
            int bestAction = GetBestAction(state);
            return bestAction;
        }

        public int GetBestAction(int state)
        {
            int bestAction = 0;
            float bestQValue = qTable[state, 0];

            for (int action = 1; action < qTable.GetLength(1); action++)
            {
                if (qTable[state, action] > bestQValue)
                {
                    bestAction = action;
                    bestQValue = qTable[state, action];
                }
            }

            return bestAction;
        }

        public void UpdateQValue(int prevState, int prevAction, int currentState, int currentAction, float reward)
        {
            float prevQValue = qTable[prevState, prevAction];
            float maxQValue = qTable[currentState, GetBestAction(currentState)];
            float newQValue = prevQValue + learningRate * (reward + discountFactor * maxQValue - prevQValue);

            qTable[prevState, prevAction] = newQValue;
        }
    }
}
