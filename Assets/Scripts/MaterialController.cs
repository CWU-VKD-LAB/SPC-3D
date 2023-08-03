using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;

//Handles all Visuals derived from the two vectors, X1 and A. In all honesty this could be split up for organization sake. Ill leave that up to future programmers
public class MaterialController : MonoBehaviour
{
    struct Vertex
    {
        public Vector4 pos;
        public Vector3 color;
    }

    int vertexSize = sizeof(float) * 7;

    int vertsPerVector;
    Vertex[] vectorVertArr;
    ComputeBuffer vectorVertBuff;

    int vertsPerCoord = 6;
    Vertex[] coordVertArr;
    ComputeBuffer coordVertBuff;

    int vertsPerGLCLLine;
    Vertex[] glclVertArr;
    ComputeBuffer glclVertBuff;

    private int PAIR_COORDS; //The number of subcoordinates
    private float high_fx = 0;
    private float VALUE_TO_CUBE_FRAC_SCALE; //cubeLengthWidth (always 5 unless scaled in editor) / maximum point height
    private const float X_SCALE = 5;
    private const float Y_SCALE = 5;
    public VisualToggleController visTogElements;
    public GameObject prefabPoint;
    public GameObject prefabVector;
    public GameObject prefabSubCoord;
    public GameObject prefabGLCL;
    public Material rulePlaneC1;
    public Material rulePlaneC2;
    public Material vectorMat;
    public Material coordMat;
    public Material glclMat;

    GameObject[] subCoordinateCubes;

    GameObject[] coordPointArr;// = new GameObject[100];
    GameObject[] attribContribPointArr;
    //public Material pointMat;
    GameObject[] classLinePlanes;// = new GameObject[100];
    GameObject[] glclLinePlanes;
    //public Text[] restOfFuncTexts;
    Material HeightPlane;
    //public Text fX;


    private Vector3[] classColors = { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1)};
    private float[] fxi;// = new float[50];//f(x) input
    private float[][] fxiParts;// = new float[50][];
    private float[][][] data; //= new float [50,4];
    //data normalized by column
    private float[][][] normPerAttribData;// = new float[50,4];
    private float[] aVals;// = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
    private float[] normAVals;// = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    private float[] ruleC1 = { 1, 0, 1, 0 };
    private float[] ruleC2 = { 1, 0, 1, 0 };
    private float c = 0.5f;
    private float[,] pointHeight;// = new float[100, 100];
    public int[] order;
    public int cubesPerRow = 3;

    float xShift = 2.5f;
    float yShift = 2.836596f;
    float zShift = 2.5f;// 0.008552f;

    void Start()
    {
        init(true);
    }

    public void init(bool newScene)
    {
        high_fx = 0;

        if(newScene)
            order = new int[ReadFileData.attribCount];
        int nProcessID = Process.GetCurrentProcess().Id;

        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { Vector3.zero };
        mesh.triangles = new int[] { 0, 0, 0 };

        //intitialize data
        data = ReadFileData.data;
        fxi = new float[ReadFileData.setCount];
        fxiParts = new float[ReadFileData.setCount][];
        int setCount = ReadFileData.setCount;
        PAIR_COORDS = Mathf.CeilToInt(ReadFileData.attribCount / 2);
        vertsPerVector = PAIR_COORDS;
        vertsPerGLCLLine = ReadFileData.attribCount + 1;

        //pointHeight = new float[PAIR_COORDS, 4];
        aVals = new float[ReadFileData.attribCount];
        normAVals = new float[ReadFileData.attribCount];

        for (int i = 0; i < ReadFileData.attribCount; i++)
        {
            if (newScene)
            {
                order[i] = i;
                aVals[i] = 1;
                normAVals[i] = 1;
            }
        }

        subCoordinateCubes = new GameObject[PAIR_COORDS];
        coordPointArr = new GameObject[setCount * PAIR_COORDS];// multiplied by two because it there are two subcoordinates for every 4 values (which makes up one xVal)
        classLinePlanes = new GameObject[setCount * PAIR_COORDS];
        attribContribPointArr = new GameObject[setCount * PAIR_COORDS];
        GameObject[] tempPoints = new GameObject[setCount * PAIR_COORDS];

        int swapDir = 1;
        Vector3 curPos = new Vector3(0, 0, 0);
        HeightPlane = prefabSubCoord.transform.GetChild(1).gameObject.GetComponent<Renderer>().sharedMaterial;
        for (int i = 0; i < PAIR_COORDS; i++)
        {
            subCoordinateCubes[i] = Instantiate(prefabSubCoord);
            subCoordinateCubes[i].transform.position = curPos;
            //HeightPlane = subCoordinateCubes[i].transform.GetChild(1).gameObject.GetComponent<Renderer>().material;
            if ((i + 1) % cubesPerRow == 0 /*&& i > 0*/)
            {
                swapDir *= -1;
                curPos += new Vector3(0, -7f, -9);
                continue;
            }
            curPos += new Vector3(9f * swapDir, 0, 0);
        }
        int pointIndex = 0;
        for (int i = 0; i < setCount; i++)
        {
            //the two points: one in 1st sub coordinate, and one in the second
            for (int j = 0; j < PAIR_COORDS; j++)
            {

                tempPoints[pointIndex] = Instantiate(prefabPoint, subCoordinateCubes[j].transform);
                //setting the two points up
                tempPoints[pointIndex].transform.localPosition = new Vector3(0, 0, 0);
                coordPointArr[pointIndex] = tempPoints[pointIndex].transform.GetChild(1).gameObject;

                attribContribPointArr[pointIndex] = tempPoints[pointIndex].transform.GetChild(2).gameObject;

                //setting the two classLinePlanes up

                classLinePlanes[pointIndex] = tempPoints[pointIndex].transform.GetChild(0).gameObject;

                pointIndex++;
            }
        }
        //visTogElements.attribContrib = attribContribPointArr;
        //visTogElements.coordPlanes = classLinePlanes;
        //visTogElements.GLCLLines = glclLinePlanes;
        //visTogElements.vector = VectorArr;

        updateCubes();
    }

    public void updateCubes()
    {
        //rulePlaneC1.SetFloat("_TX", ruleC1[0]);
        //rulePlaneC1.SetFloat("_BX", ruleC1[1]);
        //rulePlaneC1.SetFloat("_TY", ruleC1[2]);
        //rulePlaneC1.SetFloat("_BY", ruleC1[3]);

        //rulePlaneC2.SetFloat("_TX", ruleC2[0]);
        //rulePlaneC2.SetFloat("_BX", ruleC2[1]);
        //rulePlaneC2.SetFloat("_TY", ruleC2[2]);
        //rulePlaneC2.SetFloat("_BY", ruleC2[3]);

        setHeightPlaneCorners();
        normPerAttribData = normalize(data);
        normAVals = normalize(aVals);

        classLines();
    }

    void classLines()
    {
        vectorVertArr = new Vertex[ReadFileData.setCount * PAIR_COORDS];
        coordVertArr = new Vertex[ReadFileData.setCount * PAIR_COORDS * vertsPerCoord];
        glclVertArr = new Vertex[ReadFileData.setCount * PAIR_COORDS * vertsPerGLCLLine];
        int pointIndex = 0;
        //for each class
        for (int i = 0; i < data.Length; i++)
        {
            Vector3 classColor = classColors[i];
            //for each set
            for (int j = 0; j < data[i].Length; j++)
            {
                int fxiIndex = j + (i * ReadFileData.classCount);
                fxi[fxiIndex] = setFX(aVals, data, i, j);
                fxiParts[fxiIndex] = setFXParts(aVals, data, i, j);

                //Material vec = VectorArr[vectorIndex].GetComponent<Renderer>().material;
                //vec.SetColor("_Color", classColor);
                bool skipNextMat = false;
                bool outsideRules = false;
                //for each sub-coordinate
                for (int k = 0; k < PAIR_COORDS; k++)
                {
                    int kInd = order[k * 2];
                    int kIndHigh = order[k * 2 + 1];
                    coordPointArr[pointIndex].transform.localPosition = new Vector3((float)normPerAttribData[i][j][kInd] * X_SCALE - xShift, (float)normPerAttribData[i][j][kIndHigh] * Y_SCALE - yShift, (float)(fxi[fxiIndex] * VALUE_TO_CUBE_FRAC_SCALE) - zShift);
                    Vector3 currentCoordLocal = coordPointArr[pointIndex].transform.localPosition;
                    Vector3 currentCoordGlobal = coordPointArr[pointIndex].transform.position;

                    //=============================GLCL=========================//
                    Vector3 curCoordXY = new Vector3(currentCoordLocal.x, currentCoordLocal.y, 0);

                    setGLCLLines(subCoordinateCubes[k], curCoordXY, pointIndex * vertsPerGLCLLine, classColor, i, j, currentCoordGlobal);

                    //===========================Vector=========================//

                    vectorVertArr[pointIndex].pos = currentCoordGlobal;
                    vectorVertArr[pointIndex].color = classColor;
                    //==========================coordLines===============================//


                    setCoordPlane(currentCoordLocal, subCoordinateCubes[k], (float)(c * high_fx) * VALUE_TO_CUBE_FRAC_SCALE - 2.5f, pointIndex * vertsPerCoord);

                    pointIndex++;
                }
                
            }
        }
        DisplayData();
    }

    void DisplayData()
    {
        vectorVertBuff = new ComputeBuffer(vectorVertArr.Length, vertexSize);
        vectorVertBuff.SetData(vectorVertArr);
        vectorMat.SetBuffer("verti", vectorVertBuff);
        vectorMat.SetInt("vertsPerVector", vertsPerVector);
        vectorMat.SetFloat("transparency", 1);

        coordVertBuff = new ComputeBuffer(coordVertArr.Length, vertexSize);
        coordVertBuff.SetData(coordVertArr);
        coordMat.SetBuffer("verti", coordVertBuff);
        coordMat.SetInt("vertsPerVector", vertsPerCoord);
        coordMat.SetFloat("transparency", 0.2f);

        glclVertBuff = new ComputeBuffer(glclVertArr.Length, vertexSize);
        glclVertBuff.SetData(glclVertArr);
        glclMat.SetBuffer("verti", glclVertBuff);
        glclMat.SetInt("vertsPerVector", vertsPerGLCLLine);
        glclMat.SetFloat("transparency", 1);
    }

    void OnRenderObject()
    {
        vectorMat.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.LineStrip, vertsPerVector, ReadFileData.setCount);

        coordMat.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.LineStrip, vertsPerCoord, ReadFileData.setCount * PAIR_COORDS);

        glclMat.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.LineStrip, vertsPerGLCLLine, ReadFileData.setCount * PAIR_COORDS);
    }

    //calcultes f(x) based off "A" vector and "X" vector
    //will have to create dynamic "a" index. 
    //have to add the cubes like below
    void setGLCLLines(GameObject cube, Vector3 curCoordXY, int index, Vector3 classColor, int dataI, int dataJ, Vector3 finalPoint)
    {
        glclVertArr[index].color = classColor;

        glclVertArr[index].pos = new Vector3(0, 0, 0 - zShift) + curCoordXY;
        glclVertArr[index].pos = cube.transform.localToWorldMatrix.MultiplyPoint(glclVertArr[index].pos);
        for (int i = 0; i < vertsPerGLCLLine - 1; i++)
        {
            glclVertArr[index + i + 1].pos = setGLCLPoint(glclVertArr[index + i].pos, data[dataI][dataJ][order[i]], aVals[order[i]], normAVals[order[i]]);
            glclVertArr[index + i + 1].color = classColor;
        }
    }

    Vector3 setGLCLPoint( Vector3 endP, float data, float aval, float normAval)
    {
        float height = (float)(aval * data * VALUE_TO_CUBE_FRAC_SCALE);
        float topRAngle = Mathf.Acos(normAval);
        float bottomLength = data * VALUE_TO_CUBE_FRAC_SCALE * Mathf.Sin(topRAngle);
        endP += new Vector3(bottomLength, height, 0);

        return endP;
    }

    float setFX(float[] a, float[][][] x, int c, int set)
    {
        float f_x = 0;// a[0] * x[c][set][0] + a[1] * x[c][set][1] + a[2] * x[c][set][2] + a[3] * x[c][set][3];
        for(int i = 0; i < ReadFileData.attribCount; i++)
        {
            float A = a[order[i]];
            f_x += A * x[c][set][order[i]];
        }
        if(f_x > high_fx)
        {
            high_fx = f_x;
            VALUE_TO_CUBE_FRAC_SCALE = 5f / high_fx;
        }
        return f_x;
    }

    float[] setFXParts(float[] a, float[][][] x, int c, int set)
    {
        float[] f_x_PerSubCoord = new float[PAIR_COORDS];// { a[0] * x[c][set][0] + a[1] * x[c][set][1], a[2] * x[c][set][2] + a[3] * x[c][set][3]};
        int i = 0;
        int n = 0;
        while(i < PAIR_COORDS)
        {
            f_x_PerSubCoord[n] = a[order[i]] * x[c][set][order[i]] + a[order[i + 1]] * x[c][set][order[i + 1]];
            i += 2;
            n += 1;
        }

        return f_x_PerSubCoord;
    }

    //finds the rest of the height function to set f(x) to zero. For example, f_12(X) = a1x1 + a2x2 + restOfVectors - f(x)= 0. This finds restOfVectors for f_12(x), f_34(x), f_56(x). 
    /*double[] setRestOfFunctions(double[] a, double[] x)
    {
        double[] rof = new double[3];//rest of function
        rof[0] = (a[2] * x[2] + a[3] * x[3] + a[4] * x[4] + a[5] * x[5]); //f12
        rof[1] = (a[0] * x[0] + a[1] * x[1] + a[4] * x[4] + a[5] * x[5]); //f34
        rof[2] = (a[0] * x[0] + a[1] * x[1] + a[2] * x[2] + a[3] * x[3]); //f56
        return rof;
    }*/
    //sets each corner of the yellow height planes at position (0,0), (0,1) (1,1), (1,0) for each subcoordinate
    void setHeightPlaneCorners()
    {
        float newC = (float)-(c * high_fx) * VALUE_TO_CUBE_FRAC_SCALE; //for fitting purposes into the cubes of the subcoordinates. Otherwise it does not match Fx in heigh
        //pointHeight[i, 0] = newC;// setPointHeight(0, 0, aVals[2 * i], aVals[2 * i + 1], restOfFunction[i]); //(0,0)
        //pointHeight[i, 1] = newC;// setPointHeight(1, 0, aVals[2 * i], aVals[2 * i + 1], restOfFunction[i]); //(1,0)
        //pointHeight[i, 2] = newC;// setPointHeight(1, 1, aVals[2 * i], aVals[2 * i + 1], restOfFunction[i]); //(1,1)
        //pointHeight[i, 3] = newC;// setPointHeight(0, 1, aVals[2 * i], aVals[2 * i + 1], restOfFunction[i]); //(0,1);
        HeightPlane.SetFloat("_Pos0", newC);
        HeightPlane.SetFloat("_Pos1", newC);
        HeightPlane.SetFloat("_Pos2", newC);
        HeightPlane.SetFloat("_Pos3", newC);
    }

    //sets the height of a corner for the height plane.
    /*double setPointHeight(double x1, double x2, double a1, double a2, double rof)
    {
        double height = (x1 * a1 + x2 * a2 + rof - fxi) * 2.5f;// * 5;
        return height;
    }*/

    //sets the values for coordPlane shader
    //need index for coord arr
    void setCoordPlane(Vector3 pos, GameObject cube, float C, int index)
    {
        //The values, -2.5, and -2.84 properly set the lines within their subcoordinate
        //The addition/subtraction of 0.01 just allows for some offset of the coordlines so that they don't z-fight with other lines.
        Vector3 pos0 = new Vector3(-xShift, pos.y, -zShift /*+ 0.01f*/);
        Vector3 pos1 = new Vector3(pos.x /*- 0.01f*/, -yShift, -zShift /*+ 0.01f*/);
        Vector3 pos2 = new Vector3(pos.x /*- 0.01f*/, pos.y, -zShift/*+ 0.01f*/);
        Vector3 pos3 = new Vector3(pos.x, pos.y /*+ 0.01f*/, pos.z);
        Vector3 CPlane = new Vector3(pos.x, pos.y /*+ 0.01f*/, C);

        pos0 = cube.transform.localToWorldMatrix.MultiplyPoint(pos0);
        pos1 = cube.transform.localToWorldMatrix.MultiplyPoint(pos1);
        pos2 = cube.transform.localToWorldMatrix.MultiplyPoint(pos2);
        pos3 = cube.transform.localToWorldMatrix.MultiplyPoint(pos3);

        //red line
        coordVertArr[index].pos = pos0;
        coordVertArr[index].color = new Vector3(1f,0f,0f);
        coordVertArr[index + 1].pos = pos2;
        coordVertArr[index + 1].color = new Vector3(1f, 0f, 0f);

        //green line
        coordVertArr[index + 2].pos = pos1;
        coordVertArr[index + 2].color = new Vector3(0f, 1f, 0f);
        coordVertArr[index + 3].pos = pos2;
        coordVertArr[index + 3].color = new Vector3(0f, 1f, 0f);

        //blue line
        coordVertArr[index + 4].pos = pos2;
        coordVertArr[index + 4].color = new Vector3(0.76f, 0.76f, 0.76f);
        coordVertArr[index + 5].pos = pos3;
        coordVertArr[index + 5].color = new Vector3(0.76f, 0.76f, 0.76f);
    }

    float[][][] normalize(float[][][] x)
    {
        float[][][] normArr = new float[x.Length][][];

        float[] maxs = ReadFileData.maxAttribNums;

        for(int i = 0; i < normArr.Length; i++)
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

    float[] normalize(float[] x)
    {
        float[] normArr = new float[x.Length];

        float max = -1;
        foreach(int i in x)
        {
            if(Mathf.Abs(i) > max)
            {
                max = i;
            }
        }

        for (int i = 0; i < normArr.Length; i++)
        {
            normArr[i] =Mathf.Abs(max) > 1? x[i] / max : x[i];
        }
        return normArr;
    }

    //These set of functions read in the A vector from the UI, one element at a time
    public void readA(float[] aInp)
    {
        aVals = aInp;
        //updateCubes();
    }

    //reads in the C input
    public void readC(float cInp)
    {
        c = cInp;
        //updateCubes();
    }

    //reads the rules for coord 1
    public void readX1C1(string xInp)
    {
        if (float.TryParse(xInp, out _))
            ruleC1[0] = float.Parse(xInp);
    }
    public void readX2C1(string xInp)
    {
        if (float.TryParse(xInp, out _))
            ruleC1[1] = float.Parse(xInp);
    }
    public void readY1C1(string yInp)
    {
        if (float.TryParse(yInp, out _))
            ruleC1[2] = float.Parse(yInp);
    }
    public void readY2C1(string yInp)
    {
        if (float.TryParse(yInp, out _))
            ruleC1[3] = float.Parse(yInp);
    }

    //reads the rules for coord2
    public void readX1C2(string xInp)
    {
        if (float.TryParse(xInp, out _))
            ruleC2[0] = float.Parse(xInp);
    }
    public void readX2C2(string xInp)
    {
        if (float.TryParse(xInp, out _))
            ruleC2[1] = float.Parse(xInp);
    }
    public void readY1C2(string yInp)
    {
        if (float.TryParse(yInp, out _))
            ruleC2[2] = float.Parse(yInp);
    }
    public void readY2C2(string yInp)
    {
        if (float.TryParse(yInp, out _))
            ruleC2[3] = float.Parse(yInp);
    }

    /*//These set of functions read in the X vector from the UI, one element at a time
    public void readX(string xInp)
    {
       xVals[0] = double.Parse(xInp);
    }
    public void readY(string yInp)
    {
        xVals[1] = double.Parse(yInp);
    }
    public void readZ(string zInp)
    {
        xVals[2] = double.Parse(zInp);
    }
    public void readW(string wInp)
    {
        xVals[3] = double.Parse(wInp);
    }
    public void readV(string vInp)
    {
        xVals[4] = double.Parse(vInp);
    }
    public void readU(string uInp)
    {
        xVals[5] = double.Parse(uInp);
    }*/
    private void OnDestroy()
    {
        vectorVertBuff.Release();
        coordVertBuff.Release();
    }
}
