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
        //eventually will be used retrieved
        fileName = "brstCncrModShort.csv";
        //myFilePath = Application.dataPath + "/FileData/" + fileName;
        myFilePath = "C:\\Users\\infer\\Desktop\\Github2\\Boris\\SPC-3D\\Assets\\FileData\\" + fileName;
        UnityEngine.Debug.Log(myFilePath);
        int nProcessID = Process.GetCurrentProcess().Id;
        UnityEngine.Debug.Log("proc: " + nProcessID);
        ReadFromTheFile();
    }

    void ReadFromTheFile()
    {
        string[] rows = File.ReadAllLines(myFilePath);
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
        //setosa = new float[setosaCount, 4];
        //versicolor = new float[versiCount, 4];
        //virginica = new float[virgCount, 4];

        ////looks through every line
        //for (int i = 0; i < rows.Length; i++)
        //{
        //    numberString[i] = rows[i].Split(',');
        //    //changes string to float values
        //    if (rows[i].Contains("setosa"))
        //    {
        //        setosa = seperateIntoIrisArrays(numberString, setosa, i, 0, "setosa");
        //    }
        //    else if (rows[i].Contains("versicolor"))
        //    {
        //        versicolor = seperateIntoIrisArrays(numberString, versicolor, i, setosaCount, "versicolor");
        //    }
        //    else if (rows[i].Contains("virginica"))
        //    {
        //        virginica = seperateIntoIrisArrays(numberString, virginica, i, versiCount + setosaCount, "virginica");
        //    }
        //}
    }
}
