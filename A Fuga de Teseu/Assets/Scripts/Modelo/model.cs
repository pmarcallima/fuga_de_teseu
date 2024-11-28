using System;
using System.Linq;
using UnityEngine;

public class LogisticRegressionModel
{
    private double[] weights;
    private double bias;

    public LogisticRegressionModel(int featureCount)
    {
        System.Random random = new System.Random();
        weights = new double[featureCount];
        for (int i = 0; i < featureCount; i++)
        {
            weights[i] = random.NextDouble() * 0.1; // Inicialização aleatória pequena
        }
        bias = 0.0; // Inicialização do bias
    }

    private double Sigmoid(double z) => 1 / (1 + Math.Exp(-z));

    public void Train(double[][] X, double[] y, int epochs, double learningRate)
    {
        int m = X.Length; // número de exemplos
        int n = X[0].Length; // número de features

        for (int epoch = 0; epoch < epochs; epoch++)
        {
            double[] gradients = new double[n];
            double biasGradient = 0;

            for (int i = 0; i < m; i++)
            {
                double linearModel = bias + X[i].Zip(weights, (xi, wi) => xi * wi).Sum();
                double prediction = Sigmoid(linearModel);
                double error = prediction - y[i];

                // Calculando os gradientes
                for (int j = 0; j < n; j++) gradients[j] += error * X[i][j];
                biasGradient += error;
            }

            // Atualizando os pesos e o bias
            for (int j = 0; j < n; j++) weights[j] -= learningRate * gradients[j] / m;
            bias -= learningRate * biasGradient / m;

            // Opcional: exibir o progresso da perda
            if (epoch % 1000 == 0)
            {
                double loss = 0;
                for (int i = 0; i < m; i++)
                {
                    double prediction = Sigmoid(bias + X[i].Zip(weights, (xi, wi) => xi * wi).Sum());
                    loss += -y[i] * Math.Log(prediction) - (1 - y[i]) * Math.Log(1 - prediction);
                }
                loss /= m;
                Debug.Log($"Epoch {epoch}, Loss: {loss:F4}");
            }
        }
    }

    public double Predict(double[] x)
    {
        double linearModel = bias + x.Zip(weights, (xi, wi) => xi * wi).Sum();
        return Sigmoid(linearModel);
    }
}