using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameNeuronal.Sources
{
    public class NeuralNetwork
    {
        private int inputSize;
        private int hiddenSize;
        private int outputSize;
        private float[,] weights1;
        private float[,] weights2;
        private float[] biases1;
        private float[] biases2;
        private Random random;
        float learningRate = 0.1f;
        float discountFactor = 0.9f;

        public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
        {
            this.inputSize = inputSize;
            this.hiddenSize = hiddenSize;
            this.outputSize = outputSize;

            // Inicializar pesos y sesgos de forma aleatoria
            weights1 = new float[inputSize, hiddenSize];
            weights2 = new float[hiddenSize, outputSize];
            biases1 = new float[hiddenSize];
            biases2 = new float[outputSize];
            random = new Random();

            InitializeWeightsAndBiases();
        }

        private void InitializeWeightsAndBiases()
        {
            for (int i = 0; i < inputSize; i++)
            {
                for (int j = 0; j < hiddenSize; j++)
                {
                    weights1[i, j] = (float)random.NextDouble() - 0.5f;
                }
            }

            for (int i = 0; i < hiddenSize; i++)
            {
                for (int j = 0; j < outputSize; j++)
                {
                    weights2[i, j] = (float)random.NextDouble() - 0.5f;
                }
            }

            for (int i = 0; i < hiddenSize; i++)
            {
                biases1[i] = (float)random.NextDouble() - 0.5f;
            }

            for (int i = 0; i < outputSize; i++)
            {
                biases2[i] = (float)random.NextDouble() - 0.5f;
            }
        }

        private float Sigmoid(float x)
        {
            return 1.0f / (1.0f + (float)Math.Exp(-x));
        }

        public float[] FeedForward(float[] inputs)
        {
            float[] hiddenLayer = new float[hiddenSize];
            float[] outputLayer = new float[outputSize];

            // Calcular valores de la capa oculta
            for (int i = 0; i < hiddenSize; i++)
            {
                float sum = 0.0f;
                for (int j = 0; j < inputSize; j++)
                {
                    sum += inputs[j] * weights1[j, i];
                }
                hiddenLayer[i] = Sigmoid(sum + biases1[i]);
            }

            // Calcular valores de la capa de salida
            for (int i = 0; i < outputSize; i++)
            {
                float sum = 0.0f;
                for (int j = 0; j < hiddenSize; j++)
                {
                    sum += hiddenLayer[j] * weights2[j, i];
                }
                outputLayer[i] = Sigmoid(sum + biases2[i]);
            }

            return outputLayer;
        }

        public void Train(int currentState, int currentAction, int nextState, float reward)
        {
            // Convertir los estados en representaciones de entrada adecuadas
            float[] inputCurrent = ConvertStateToInput(currentState);
            float[] inputNext = ConvertStateToInput(nextState);

            // Obtener las salidas correspondientes a los estados actual y siguiente
            float[] outputCurrent = FeedForward(inputCurrent);
            float[] outputNext = FeedForward(inputNext);

            // Calcular el objetivo de entrenamiento basado en el algoritmo Q-Learning
            float maxQNext = outputNext.Max();
            float targetQ = reward + discountFactor * maxQNext;

            // Calcular el error entre la salida actual y el objetivo de entrenamiento
            float[] targetOutput = (float[])outputCurrent.Clone();
            targetOutput[currentAction] = targetQ;
            float[] error = new float[outputSize];

            for (int i = 0; i < outputSize; i++)
            {
                error[i] = targetOutput[i] - outputCurrent[i];
            }

            // Actualizar los pesos y sesgos utilizando el algoritmo de retropropagación (backpropagation)
            // y el método de descenso del gradiente (gradient descent)
            for (int i = 0; i < hiddenSize; i++)
            {
                for (int j = 0; j < outputSize; j++)
                {
                    weights2[i, j] += learningRate * error[j] * outputCurrent[i];
                }
            }

            for (int i = 0; i < outputSize; i++)
            {
                biases2[i] += learningRate * error[i];
            }

            for (int i = 0; i < inputSize; i++)
            {
                for (int j = 0; j < hiddenSize; j++)
                {
                    float sum = 0.0f;
                    for (int k = 0; k < outputSize; k++)
                    {
                        sum += error[k] * weights2[j, k];
                    }
                    weights1[i, j] += learningRate * sum * inputCurrent[i] * (1 - inputCurrent[i]);
                }
            }

            for (int i = 0; i < hiddenSize; i++)
            {
                biases1[i] += learningRate * error[i];
            }
        }

        public int GetBestAction(int state)
        {
            float[] input = ConvertStateToInput(state);
            float[] output = FeedForward(input);

            // Obtener el índice de la acción con el mayor valor de salida
            int bestAction = 0;
            float bestValue = output[0];
            for (int i = 1; i < outputSize; i++)
            {
                if (output[i] > bestValue)
                {
                    bestAction = i;
                    bestValue = output[i];
                }
            }

            return bestAction;
        }

        float[] ConvertStateToInput(int state)
        {
            // Crear un arreglo para almacenar la representación de entrada
            float[] input = new float[inputSize];

            // Asignar valores a la representación de entrada según el estado
            // Aquí debes adaptar la lógica según cómo estás representando el estado en tu juego

            // Ejemplo genérico: Asignar un valor de 1.0f en la posición correspondiente al estado actual
            input[state] = 1.0f;

            return input;
        }
    }
}
