using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrayFilter : MonoBehaviour
{
    static public float[][][] FilterData(float[][][] dataArray, string userConditionString)
    {
        if (userConditionString.Equals(string.Empty))
        {
            return CopyArray(dataArray);
        }

        float[][][] copiedArray = CopyArray(dataArray);

        string[] orConditions = userConditionString.Split('^');
        List<List<Func<float[], bool>>> conditionGroups = new List<List<Func<float[], bool>>>();

        foreach (string orCondition in orConditions)
        {
            string[] andConditions = orCondition.Trim().Split('&');
            List<Func<float[], bool>> conditionFuncs = new List<Func<float[], bool>>();

            foreach (string andCondition in andConditions)
            {
                conditionFuncs.Add(dataRecord => EvaluateCondition(dataRecord, andCondition));
            }

            conditionGroups.Add(conditionFuncs);
        }

        for (int i = 0; i < copiedArray.Length; i++)
        {
            copiedArray[i] = copiedArray[i].Where(innerRecord =>
                conditionGroups.Any(group =>
                    group.All(conditionFunc => conditionFunc(innerRecord))
                )
            ).ToArray();
        }

        foreach (float[][] record in copiedArray)
        {
            foreach (float[] innerRecord in record)
            {
                Debug.Log($"Filtered Record: [{string.Join(", ", innerRecord)}]");
            }
        }

        return copiedArray;
    }

    static bool EvaluateCondition(float[] dataRecord, string condition)
    {
        string[] tokens = condition.Split('=');
        string indexString = tokens[0].Trim().Substring(1); // Extract index from Xn
        int index;

        if (int.TryParse(indexString, out index))
        {
            index--; // Adjust index to zero-based
            float value;

            if (float.TryParse(tokens[1].Trim(), out value))
            {
                return index >= 0 && index < dataRecord.Length && dataRecord[index] == value;
            }
            else
            {
                Debug.LogError($"Invalid value format in condition: {condition}");
                return false;
            }
        }
        else
        {
            Debug.LogError($"Invalid index format in condition: {condition}");
            return false;
        }
    }

    static float[][][] CopyArray(float[][][] source)
    {
        float[][][] copy = new float[source.Length][][];

        for (int i = 0; i < source.Length; i++)
        {
            copy[i] = new float[source[i].Length][];
            for (int j = 0; j < source[i].Length; j++)
            {
                copy[i][j] = new float[source[i][j].Length];
                Array.Copy(source[i][j], copy[i][j], source[i][j].Length);
            }
        }

        return copy;
    }
}
