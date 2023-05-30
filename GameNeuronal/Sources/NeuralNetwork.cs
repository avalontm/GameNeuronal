using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Math;

public class NeuralNetwork
{
    private ActivationNetwork network;
    NguyenWidrow nguyen;

    public NeuralNetwork()
    {
        // Crear la red neuronal
        network = new ActivationNetwork(new SigmoidFunction(), 3, 3, 2);

        // Inicializar los pesos de la red neuronal de forma aleatoria
        nguyen = new NguyenWidrow(network);
        nguyen.Randomize();

    }

    public float[] Predict(float[] input)
    {
        // Normalizar los valores de entrada en el rango [0, 1]
        double[] normalizedInput = NormalizeInput(input, 0, 1);

        // Calcular las salidas de la red neuronal
        double[] output = network.Compute(normalizedInput);

        // Desnormalizar los valores de salida al rango original
        float[] denormalizedOutput = DenormalizeOutput(output, 0, 1);

        return denormalizedOutput;
    }

    public double[] NormalizeInput(float[] input, double inputMin, double inputMax)
    {
        double[] normalizedInput = new double[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            normalizedInput[i] = (input[i] - inputMin) / (inputMax - inputMin);
        }
        return normalizedInput;
    }

    public float[] DenormalizeOutput(double[] output, double outputMin, double outputMax)
    {
        float[] denormalizedOutput = new float[output.Length];
        for (int i = 0; i < output.Length; i++)
        {
            denormalizedOutput[i] = (float)(output[i] * (outputMax - outputMin) + outputMin);
        }
        return denormalizedOutput;
    }
}

