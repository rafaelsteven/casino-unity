using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class probabilidad : MonoBehaviour
{
    private System.Random random = new System.Random();

    public int WeightedRandomChoice(int[] weights)
    {
        float[] invertedWeights = new float[weights.Length];
        float totalInvertedWeight = 0.0f;

        for (int i = 0; i < weights.Length; i++)
        {
            invertedWeights[i] = 1.0f / weights[i];
            totalInvertedWeight += invertedWeights[i];
        }

        float[] weightedProbabilities = new float[weights.Length];
        float cumulativeProbability = 0.0f;

        for (int i = 0; i < weights.Length; i++)
        {
            weightedProbabilities[i] = invertedWeights[i] / totalInvertedWeight;
            cumulativeProbability += weightedProbabilities[i];
            weightedProbabilities[i] = cumulativeProbability;
        }

        float randomValue = (float)random.NextDouble();

        for (int i = 0; i < weights.Length; i++)
        {
            if (randomValue < weightedProbabilities[i])
            {
                return i;
            }
        }

        return weights.Length - 1;
    }

    public void ejecutar()
    {

        int[] weights = new int[] { 100, 200,10, 20, 30, 40, 50 }; // Ajusta los pesos según tus necesidades
        int randomIndex = WeightedRandomChoice(weights);
        Debug.Log("RANDO PROBABILIDAD: "+ weights[randomIndex] +"--" + randomIndex);
    }
    public void ejecutar2()
    {

        int[] weights = new int[] { 100, 200, 10, 20, 30, 40, 50 }; // Ajusta los pesos según tus necesidades
        int count = weights.Length;
        int randomIndex = UnityEngine.Random.Range(0, count);
        Debug.Log("RANDO NORMAL: " + weights[randomIndex] + "--" + randomIndex);
    }
} 
