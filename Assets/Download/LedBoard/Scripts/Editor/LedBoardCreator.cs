using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LedBoardCreator : Editor
{
    [MenuItem("Tools/Create Led Board")]
    private static void CreateLedBoard()
    {
        GameObject ledboardPrefab = Instantiate((GameObject)Resources.Load("LedBoard"));
        ledboardPrefab.name = "NewLedBoard";
        ledboardPrefab.GetComponent<LedBoardScript>().BoardConstructor();
    }
}
