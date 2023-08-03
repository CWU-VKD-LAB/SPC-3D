using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

public class ReadFileData : MonoBehaviour
{
    //public float[] xVals;
    string myFilePath, fileName;
    //Number of elements in each class
    Dictionary<string, int> classNum = new Dictionary<string, int>();
    //make an array for the max number of each attribute
    static public float[] maxAttribNums;
    //make a number for the number of classes
    static public int classCount;
    //make a number for the number of rows
    static public int setCount;
    //make a number for the number of attributes
    static public int attribCount;

    //the plan: have the program look at every string. If it is new, then make it a new value starting at 0 and going up by 1.
    //for the array below, the first attribute represents the class, the second the row, and the third the value 
    static public float[][][] data;


    //static public float[,] setosa;
    //static public float[,] versicolor;
    //static public float[,] virginica;

    // Start is called before the first frame update
    void Awake()
    { 
        ////eventually will be user retrieved
        //fileName = "SimpleMushroomDataReal.csv";
        ////myFilePath = Application.dataPath + "/FileData/" + fileName;
        //myFilePath = "C:\\Users\\infer\\Desktop\\Github2\\SPC-3D\\Assets\\FileData\\" + fileName;
        //init(myFilePath);

        init("");
    }

    public void init(string path)
    {
        UnityEngine.Debug.Log(path);
        classCount = 0;
        classNum.Clear();
        setCount = 0;
        attribCount = 0;
        if (path == "")
        {
            classCount = 3;
            classNum.Clear();
            setCount = 15;
            attribCount = 6;
            maxAttribNums = new float[attribCount];

            data = new float[3][][];
            int count = 0;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new float[5][];
                for (int j = 0; j < data[i].Length; j++)
                {
                    data[i][j] = new float[6];
                    for (int k = 0; k < data[i][j].Length; k++)
                    {
                        data[i][j][k] = i * j * k + 1;
                        if (data[i][j][k] > maxAttribNums[k])
                        {
                            maxAttribNums[k] = data[i][j][k];
                        }
                    }
                    count++;
                }
            }
            return;
        }
        ReadFromTheFile(path);
    }

    void ReadFromTheFile(string path)
    {
        string[] rows = File.ReadAllLines(path);
        int dataColumns = rows[0].Split(',').Length;
        attribCount = dataColumns - 1; // "-1" to account for the class column, which is not an attribute
        setCount = rows.Length - 1;//TEMP
        string[][] numberString = new string[setCount][];
        maxAttribNums = new float[attribCount];
        List<string> classNames = new List<string>();
        for (int i = 0; i < setCount; i++)
        {
            numberString[i] = rows[i].Split(',');
            string className = numberString[i][dataColumns - 1];
            if (!classNum.ContainsKey(className))
            {
                classNum.Add(className, 1);
                classNames.Add(className);
                classCount++;
            }
            else
            {
                classNum[className] += 1;
            }
        }

        data = new float[classCount][][];
        int count = 0;
        for(int i = 0; i < data.Length; i++)
        {
            data[i] = new float[classNum[classNames[i]]][];
            for (int j = 0; j < data[i].Length; j++)
            {
                data[i][j] = new float[attribCount];
                for (int k = 0; k < data[i][j].Length; k++)
                {
                    float.TryParse(numberString[count][k], out data[i][j][k]);
                    if (data[i][j][k] > maxAttribNums[k])
                    {
                        maxAttribNums[k] = data[i][j][k];
                    }
                }
                count++;
            }
        }
    }

}
