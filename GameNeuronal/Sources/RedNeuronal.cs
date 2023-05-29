using Accord.Math;
using Accord.Math.Distances;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Microsoft.VisualBasic.Devices;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;

namespace GameNeuronal.Sources
{
    public class RedNeuronal
    {
        ActivationNetwork network { set; get; }
        BackPropagationLearning teacher;

        Dino player { set; get; }
        BaseEnemy enemy { set; get; }
        double thresholdDistance = 25;

        // Captura de datos del juego
        double[] GetInputData()
        {
            // Obtener los datos relevantes del juego
            double distance = CalculateDistanceToObstacle();
            double obstacleX = CalculateObstaclePositionX();
            double obstacleY = CalculateObstaclePositionY();
            double obstacleHeight = CalculateObstacleHeight();
            double obstacleWidth = CalculateObstacleWidth();
            double playerX = CalculatePlayerX();
            double playerY = CalculatePlayerY();
            double playerHeight = CalculatePlayerHeight();
            double playerWidth = CalculatePlayerWidth();
            double speed = CalculateGameSpeed();

            // Retornar los datos capturados como un array
            return new double[] { distance, obstacleX, obstacleY, obstacleHeight, obstacleWidth, playerX, playerY, playerHeight, playerWidth, speed };
        }

        public RedNeuronal(Dino dino)
        {
            int inputSize = 10; // Cantidad de neuronas de entrada
            int hiddenSize = 8; // Cantidad de neuronas en la capa oculta
            int outputSize = 2; // Cantidad de neuronas de salida
            double learningRate = 0.1;

            // Inicializar la red neuronal
            network = new ActivationNetwork(new SigmoidFunction(), inputSize, hiddenSize, outputSize);

            // Configurar el algoritmo de aprendizaje y la tasa de aprendizaje
            teacher = new BackPropagationLearning(network);
            teacher.LearningRate = learningRate;

            player = dino;
        }

        List<double[]> inputs = new List<double[]>();
        List<double[]> outputs = new List<double[]>();

        // Utilización de la red neuronal
        void MakeDecisions()
        {
            // Obtener los datos de entrada para la red neuronal
            double[] input = GetInputData();

            // Obtener las salidas de la red neuronal
            double[] output = network.Compute(input);

            inputs.Add(input);
            outputs.Add(output);


            // Interpretar las salidas y tomar decisiones
            bool shouldJump = output[0] >= 0.5;
            bool shouldDuck = output[1] >= 0.5;

            // Verificar condiciones adicionales y ajustar las decisiones
            if (shouldJump)
            {
                // Verificar si hay un obstáculo cercano
                if (CalculateDistanceToObstacle() < (this.player.x + thresholdDistance))
                {
                    // Saltar solo si la altura del obstáculo es menor que una altura máxima permitida
                    if ((CalculateObstaclePositionY()) < (this.player.y + this.player.h))
                    {
                        Jump();
                    }
                }
                else
                {
                    // Saltar por defecto si no hay condiciones adicionales
                    // Jump();
                }
            }

            if (shouldDuck)
            {
                // Verificar si hay un obstáculo cercano
                if (CalculateDistanceToObstacle() < (this.player.x + thresholdDistance))
                {
                    // Agacharse solo si la altura del obstáculo es mayor que una altura mínima permitida
                    if ((CalculateObstaclePositionY() + CalculateObstacleHeight()) > (this.player.y))
                    {
                        Duck();
                    }
                }
                else
                {
                    // Agacharse por defecto si no hay condiciones adicionales
                    //Duck();
                }
            }

        }

        public void Update()
        {
            double distance = 0;

            if (MainGame.enemies.Count > 0)
            {
                distance = CalculateDistanceToObstacle();

                enemy = MainGame.enemies[0];

                if (distance < 0)
                {
                    enemy = MainGame.enemies[1];
                }
            }


            // Utilizar la red neuronal para tomar decisiones en tiempo real
            MakeDecisions();

        }

        // Métodos para las acciones del jugador
        void Jump()
        {
            player.onJump();
        }

        void Duck()
        {
            // Implementar la acción de agacharse del jugador
            player.onDuck();
        }

        // Métodos para calcular la velocidad del juego
        public double CalculateGameSpeed()
        {
            return MainGame.speed;
        }

        public double CalculatePlayerX()
        {
            return player.x;
        }

        public double CalculatePlayerY()
        {
            return player.y;
        }

        public double CalculatePlayerWidth()
        {
            return player.w;
        }

        public double CalculatePlayerHeight()
        {
            return player.h;
        }

        // Métodos para calcular los datos relevantes del juego
        public double CalculateDistanceToObstacle()
        {
            double distance = 0; // Inicializar con un valor infinito para encontrar el obstáculo más cercano

            if (MainGame.enemies.Count > 0)
            {
                distance = MainGame.enemies[0].x - player.x;
                if (distance < 0)
                {
                    distance = MainGame.enemies[1].x - player.x;
                }
            }

            return distance;
        }

        public double CalculateObstaclePositionX()
        {
            // Implementar el cálculo de la posición X del obstáculo más cercano
            double posX = 0;

            if (enemy != null)
            {
                posX = enemy.x;
            }
            return posX;
        }

        public double CalculateObstaclePositionY()
        {
            // Implementar el cálculo de la posición Y del obstáculo más cercano
            double posY = 0;

            if (enemy != null)
            {
                posY = enemy.y;
            }
            return posY;
        }

        public double CalculateObstacleHeight()
        {
            // Implementar el cálculo de la altura del obstáculo más cercano
            double h = 0;

            if (enemy != null)
            {
                h = enemy.h;
            }
            return h;
        }

        public double CalculateObstacleWidth()
        {
            // Implementar el cálculo del ancho del obstáculo más cercano
            double w = 0;

            if (enemy != null)
            {
                w = enemy.w;
            }
            return w;
        }

        double[][] GetTrainingInput()
        {
            // Obtener los datos de entrada para el entrenamiento
            // como una matriz de dobles [cantidad de ejemplos][tamaño de entrada]

            return inputs.ToArray();
        }

        double[][] GetTrainingOutput()
        {
            // Obtener los datos de salida esperados para el entrenamiento
            // como una matriz de dobles [cantidad de ejemplos][tamaño de salida]

            return outputs.ToArray();
        }

        public void TrainNeuralNetwork()
        {
            if (player.last)
            {
                // Obtener los datos de entrenamiento
                double[][] input = GetTrainingInput();
                double[][] output = GetTrainingOutput();

                // Iterar el entrenamiento en varias épocas
                int maxEpochs = 1000;
                int epoch = 0;
                double error = double.PositiveInfinity;

                while (epoch < maxEpochs && error > 0.01)
                {
                    error = teacher.RunEpoch(input, output);
                    epoch++;
                }

                network.Save("train.mono");
            }
        }
    }
}
