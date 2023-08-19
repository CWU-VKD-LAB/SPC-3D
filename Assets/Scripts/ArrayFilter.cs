using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrayFilter : MonoBehaviour
{
    static public bool[] involvedAttribute;

    static public float[][][] FilterData(float[][][] dataArray, string userConditionString)
    {
        involvedAttribute = new bool[ReadFileData.attribCount];
        for (int i = 0; i < involvedAttribute.Length; i++)
        {
            involvedAttribute[i] = false;
        }

        if (userConditionString.Equals(string.Empty))
        {
            setInvolvedAttributesToAllTrue();
            return CopyArray(dataArray);
        }

        float[][][] combinedResults = null;

        string[] ruleStrings = userConditionString.Split('+');

        foreach (string ruleString in ruleStrings)
        {
            string rule = ruleString.Trim();
            float[][][] filteredData = ApplyRule(dataArray, rule);

            if (combinedResults == null)
            {
                combinedResults = filteredData;
            }
            else
            {
                combinedResults = CombineResults(combinedResults, filteredData);
            }
        }

        return combinedResults;
    }

    static float[][][] ApplyRule(float[][][] dataArray, string rule)
    {
        float[][][] copiedArray = CopyArray(dataArray);

        string[] orConditions = rule.Split('|');
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

        return copiedArray;
    }

    static float[][][] CombineResults(float[][][] result1, float[][][] result2)
    {
        if (result1.Length != result2.Length)
        {
            Debug.LogError("Both result arrays should have the same number of classes.");
            return null;
        }

        float[][][] combinedResults = new float[result1.Length][][];

        for (int i = 0; i < result1.Length; i++)
        {
            HashSet<float[]> uniqueCases = new HashSet<float[]>(new ArrayEqualityComparer());
            uniqueCases.UnionWith(result1[i]);
            uniqueCases.UnionWith(result2[i]);

            combinedResults[i] = uniqueCases.ToArray();
        }

        return combinedResults;
    }

    class ArrayEqualityComparer : IEqualityComparer<float[]>
    {
        public bool Equals(float[] x, float[] y)
        {
            if (x.Length != y.Length)
                return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(float[] obj)
        {
            unchecked
            {
                int hash = 17;
                foreach (float value in obj)
                {
                    hash = hash * 23 + value.GetHashCode();
                }
                return hash;
            }
        }
    }

    static bool EvaluateCondition(float[] dataRecord, string condition)
    {
        string[] operators = { "!=", "=" };

        foreach (string op in operators)
        {
            string[] tokens = condition.Split(new string[] { op }, StringSplitOptions.None);

            if (tokens.Length == 2)
            {
                string attributeToken = tokens[0].Trim();
                string valueToken = tokens[1].Trim();

                if (attributeToken.StartsWith("X") && int.TryParse(attributeToken.Substring(1), out int index))
                {
                    index--;
                    if (index + 1 <= involvedAttribute.Length)
                        involvedAttribute[index] = true;
                    if (index >= 0 && index < dataRecord.Length)
                    {
                        if (float.TryParse(valueToken, out float value))
                        {
                            switch (op)
                            {
                                case "=":
                                    return dataRecord[index] == value;
                                case "!=":
                                    return dataRecord[index] != value;
                            }
                        }
                        else
                        {
                            Debug.LogError($"Invalid value format in condition: {condition}");
                            return false;
                        }
                    }
                    else
                    {
                        Debug.Log($"Invalid attribute index in condition: {condition}");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError($"Invalid attribute format in condition: {condition}");
                    return false;
                }
            }
        }

        Debug.LogError($"Invalid condition format: {condition}");
        return false;
    }

    static void setInvolvedAttributesToAllTrue()
    {
        for (int i = 0; i < involvedAttribute.Length; i++)
        {
            involvedAttribute[i] = true;
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
