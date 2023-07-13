using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vectorDisplay : MonoBehaviour
{
    public ComputeShader comp;
    public Material mat;
    private float[][][] data; //= new float [50,4];

    int vecHandle;
    int groupCount;

    struct VectorPoint
    {
        public float x1;
        public float x2;
        public float x3;
        public float x4;
        public float x5;
        public float x6;
        public float x7;
        public float x8;
        public float x9;
        public float x10;
        public float x11;
        public float x12;
        public float x13;
        public float x14;
        public float x15;
        public float x16;
        public float rowClass;
    }

    struct AVals
    {
        public int x1;
        public int x2;
        public int x3;
        public int x4;
        public int x5;
        public int x6;
        public int x7;
        public int x8;
    }

    VectorPoint[] vectorsArray;
    ComputeBuffer vectorBuff;
    ComputeBuffer vertexBuff;

    int highestVal;
    int vectorPointSize = sizeof(int) * 17;
    int vertSize = (3) * sizeof(float);
    int setCount;
    //number of attributes divided by two
    int vertCount;
    // Start is called before the first frame update
    void Start()
    {
        vertCount = ReadFileData.attribCount / 2;
        data = ReadFileData.data;
        setCount = ReadFileData.setCount;
        vecHandle = comp.FindKernel("placeVectors");

        data = normalize(data);

        init();
    }

    void init()
    {
        vectorsArray = new VectorPoint[setCount];

        int n = 0;
        int maxNum = -1;
        for(int i = 0; i < data.Length; i++)
        {
            for(int j = 0; j < data[i].Length; j++)
            {
                vectorsArray[n].rowClass = i;
                for(int k = 0; k < data[i][j].Length; k++)
                {
                    if((int)data[i][j][k] > maxNum)
                    {
                        maxNum = (int)data[i][j][k];
                    }
                    switch (k)
                    {
                        case 0:
                            vectorsArray[n].x1 = (int)data[i][j][k];
                            break;
                        case 1:
                            vectorsArray[n].x2 = (int)data[i][j][k];
                            break;
                        case 2:
                            vectorsArray[n].x3 = (int)data[i][j][k];
                            break;
                        case 3:
                            vectorsArray[n].x4 = (int)data[i][j][k];
                            break;
                        case 4:
                            vectorsArray[n].x5 = (int)data[i][j][k];
                            break;
                        case 5:
                            vectorsArray[n].x6 = (int)data[i][j][k];
                            break;
                        case 6:
                            vectorsArray[n].x7 = (int)data[i][j][k];
                            break;
                        case 7:
                            vectorsArray[n].x8 = (int)data[i][j][k];
                            break;
                        case 8:
                            vectorsArray[n].x9 = (int)data[i][j][k];
                            break;
                        case 9:
                            vectorsArray[n].x10 = (int)data[i][j][k];
                            break;
                        case 10:
                            vectorsArray[n].x11 = (int)data[i][j][k];
                            break;
                        case 11:
                            vectorsArray[n].x12 = (int)data[i][j][k];
                            break;
                        case 12:
                            vectorsArray[n].x13 = (int)data[i][j][k];
                            break;
                        case 13:
                            vectorsArray[n].x14 = (int)data[i][j][k];
                            break;
                        case 14:
                            vectorsArray[n].x15 = (int)data[i][j][k];
                            break;
                        case 15:
                            vectorsArray[n].x16 = (int)data[i][j][k];
                            break;
                        default:
                            break;
                    }
                }
                n++;
            }
        }
        vectorBuff = new ComputeBuffer(setCount, vectorPointSize);
        vertexBuff = new ComputeBuffer(setCount * vertCount, vertSize);
        vectorBuff.SetData(vectorsArray);

        uint threadGroupSizeX;
        comp.GetKernelThreadGroupSizes(vecHandle, out threadGroupSizeX, out _, out _);
        groupCount = setCount / (int)threadGroupSizeX;
        if(groupCount < 1)
        {
            groupCount = 1;
        }
        comp.SetBuffer(vecHandle, "vec", vectorBuff);
        comp.SetBuffer(vecHandle, "vrt", vertexBuff);
        comp.SetInt("highestVal", maxNum);
        comp.Dispatch(vecHandle, groupCount, 1, 1);

        mat.SetBuffer("verti", vertexBuff);
    }

    float[][][] normalize(float[][][] x)
    {
        float[][][] normArr = new float[x.Length][][];

        float[] maxs = ReadFileData.maxAttribNums;

        for (int i = 0; i < normArr.Length; i++)
        {
            normArr[i] = new float[x[i].Length][];
            for (int j = 0; j < normArr[i].Length; j++)
            {
                normArr[i][j] = new float[x[i][j].Length];
                for (int k = 0; k < normArr[i][j].Length; k++)
                {
                    normArr[i][j][k] = x[i][j][k] / maxs[k];
                }
            }
        }
        return normArr;
    }

    void OnRenderObject()
    {
        mat.SetPass(0);
        //here could be an issue
                                             //.Lines      8
        Graphics.DrawProceduralNow(MeshTopology.LineStrip, 8, setCount);
    }

    private void OnDestroy()
    {
        if(vectorBuff != null)
        {
            vectorBuff.Release();
        }
        if(vertexBuff != null)
        {
            vertexBuff.Release();
        }
    }
}
