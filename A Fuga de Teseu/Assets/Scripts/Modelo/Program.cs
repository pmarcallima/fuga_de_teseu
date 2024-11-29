using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class LogisticRegression : MonoBehaviour
{
    static private string projectPath = Application.streamingAssetsPath;
    private string trainFilePath = Application.streamingAssetsPath + @"\dados.csv";  
    public string testFilePath = Application.streamingAssetsPath + @"\dados_player.csv"; 

    private static LogisticRegressionModel model;  // Tornando o modelo estático para persistir durante a execução
    private static double[] minValues;              // Também tornando os valores de normalização estáticos
    private static double[] maxValues;              

    // Variável estática para garantir que o treinamento ocorra apenas uma vez
    private static bool isModelTrained = false;

void Awake()
    {
        // Impede que o GameObject com este script seja destruído ao carregar uma nova cena
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!isModelTrained)  // Verifica se o modelo já foi treinado
        {
            Debug.Log("Iniciando treinamento do modelo...");
            TrainModel(trainFilePath);
            isModelTrained = true;  // Marca o modelo como treinado
        }
        else
        {
            Debug.Log("Modelo já treinado, realizando previsões...");
        }

        Debug.Log("Realizando previsões...");
        PredictFromFile(testFilePath);
    }

    private void TrainModel(string trainFilePath)
    {
        // Carregar os dados de treinamento
        var (X_train, y_train) = ReadCsvWithLabels(trainFilePath);

        // Verificar consistência dos dados
        if (X_train.Length == 0 || X_train[0].Length == 0)
        {
            Debug.LogError("Erro: os dados de treinamento estão vazios ou com formato incorreto.");
            return;
        }

        // Normalizar as features de treinamento
        (minValues, maxValues) = NormalizeFeatures(X_train);

        model = new LogisticRegressionModel(X_train[0].Length);
        model.Train(X_train, y_train, epochs: 1000, learningRate: 0.1);
        Debug.Log("Modelo treinado com sucesso.");
    }

    public int PredictFromFile(string testFilePath)
    {
int predictedClass = -1;
        // Verificar se o modelo foi treinado
        if (model == null)
        {
            Debug.LogError("Erro: o modelo não foi treinado. Não é possível realizar previsões.");
            return -1;
        }

        var X_test = ReadCsvWithoutLabels(testFilePath);

        // Verificar consistência dos dados
        if (X_test.Length == 0 || X_test[0].Length == 0)
        {
            Debug.LogError("Erro: os dados de teste estão vazios ou com formato incorreto.");
            return -1;
        }

        // Normalizar os dados de teste com os valores mínimos e máximos obtidos no treinamento
        NormalizeFeatures(X_test, minValues, maxValues);
 
        // Realizar previsões
        for (int i = 0; i < X_test.Length; i++)
        {
            double prediction = model.Predict(X_test[i]);
            predictedClass = prediction >= 0.5 ? 1 : 0;
            Debug.Log($"Entrada: {string.Join(", ", X_test[i])} | Probabilidade: {prediction:F4} | Classe: {predictedClass}");
        }
 	return predictedClass;
    }

    // Função para ler os dados de treinamento (com rótulos)
    static (double[][] X, double[] y) ReadCsvWithLabels(string filePath)
    {
        var X = new List<double[]>();
        var y = new List<double>();

        using (var reader = new StreamReader(filePath))
        {
            string header = reader.ReadLine(); // Ignora o cabeçalho
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (values.Length < 2)
                {
                    Debug.LogError($"Erro: linha com formato inesperado: {line}");
                    continue;
                }

                X.Add(values.Take(values.Length - 1).Select(double.Parse).ToArray());
                y.Add(double.Parse(values.Last()));
            }
        }

        return (X.ToArray(), y.ToArray());
    }

    // Função para ler os dados de teste (sem labels)
    static double[][] ReadCsvWithoutLabels(string filePath)
    {
        var X = new List<double[]>();

        using (var reader = new StreamReader(filePath))
        {
            string header = reader.ReadLine(); // Ignora o cabeçalho
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                if (values.Length < 1)
                {
                    Debug.LogError($"Erro: linha com formato inesperado: {line}");
                    continue;
                }

                X.Add(values.Select(double.Parse).ToArray());
            }
        }

        return X.ToArray();
    }

    // Função para normalizar as features e retornar os valores mínimos e máximos
    static (double[] minValues, double[] maxValues) NormalizeFeatures(double[][] X)
    {
        int m = X.Length;
        int n = X[0].Length;

        double[] minValues = new double[n];
        double[] maxValues = new double[n];

        for (int j = 0; j < n; j++)
        {
            minValues[j] = X.Min(row => row[j]);
            maxValues[j] = X.Max(row => row[j]);
        }

        for (int j = 0; j < n; j++)
        {
            for (int i = 0; i < m; i++)
            {
                if (maxValues[j] != minValues[j])
                {
                    X[i][j] = (X[i][j] - minValues[j]) / (maxValues[j] - minValues[j]);
                }
            }
        }

        return (minValues, maxValues);
    }

    static void NormalizeFeatures(double[][] X, double[] minValues, double[] maxValues)
    {
        int m = X.Length;
        int n = X[0].Length;

        for (int j = 0; j < n; j++)
        {
            for (int i = 0; i < m; i++)
            {
                if (maxValues[j] != minValues[j])
                {
                    X[i][j] = (X[i][j] - minValues[j]) / (maxValues[j] - minValues[j]);
                }
            }
        }
    }
}