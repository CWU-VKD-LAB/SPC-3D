using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SFB;

public class resetScene : MonoBehaviour
{
    public MaterialController controller;
    public Placebuttons valueUpdater;
    public Text cubeText;
    int cubeCount;
    public ReadFileData dataSetter;

    private void Start()
    {
        cubeCount = controller.cubesPerRow;
        cubeText.text = ""+ cubeCount;
    }

    public void fillCubesPerRow(string cubeInp)
    {
        int tempCubeCount = controller.cubesPerRow;
        if (int.TryParse(cubeInp, out _))
            cubeCount = int.Parse(cubeInp);
        if (cubeCount == 0)
            cubeCount = tempCubeCount;
        resetSceneVisual();
    }

    public void resetSceneVisual()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("cube");
        foreach (GameObject cube in cubes)
            Destroy(cube);
        controller.cubesPerRow = cubeCount;
        controller.init(false);
        valueUpdater.updateVals();
    }

    public void resetSceneData()
    {
        var extensions = new[] {
                new ExtensionFilter("Text Files", "txt", "csv", "data" )
        };
        string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        if (path.Length == 0)
            return;
        dataSetter.init(path[0]);

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("cube");
        foreach (GameObject cube in cubes)
            Destroy(cube);
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("button");
        foreach (GameObject button in buttons)
            Destroy(button);

        controller.cubesPerRow = cubeCount;
        controller.init(true);
        valueUpdater.init();
        valueUpdater.updateVals();
    }
}
