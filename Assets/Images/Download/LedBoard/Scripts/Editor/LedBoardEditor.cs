using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LedBoardScript))]
public class LedBoardEditor : Editor
{
    private LedBoardScript LBS;

    private SerializedProperty pLedText;
    private SerializedProperty pdistLetFieldsH, pdistLetFieldsV;
    private SerializedProperty pVerHor, pIsBlink;
    private SerializedProperty pDeltaBlinkTime;
    private SerializedProperty pIsMove, pMoveTo;
    private SerializedProperty pDeltaTimeMove;
    private SerializedProperty pLedColor, pBoardColor;
    private SerializedProperty pLedScale;
    private SerializedProperty pLedType;

    private void OnEnable()
    {
        LBS = target as LedBoardScript;
        LBS.PrefabLed = (GameObject)Resources.Load("Led");
        LBS.PrefabLetterField = (GameObject)Resources.Load("LetterField");
        LBS.PrefOnMaterial = (Material)Resources.Load("OnMaterial");
        LBS.PrefOffMaterial = (Material)Resources.Load("OffMaterial");

        pLedText = serializedObject.FindProperty("LedText");
        pLedType = serializedObject.FindProperty("LedType");
        pdistLetFieldsH = serializedObject.FindProperty("distLetFielsH");
        pdistLetFieldsV = serializedObject.FindProperty("distLetFielsV");
        pVerHor = serializedObject.FindProperty("isHorizontal");
        pIsBlink = serializedObject.FindProperty("isBlink");
        pDeltaBlinkTime = serializedObject.FindProperty("DeltaBlinkTime");
        pIsMove = serializedObject.FindProperty("isMove");
        pMoveTo = serializedObject.FindProperty("MoveTo");
        pDeltaTimeMove = serializedObject.FindProperty("DeltaMoveTime");
        pLedColor = serializedObject.FindProperty("LedColor");
        pBoardColor = serializedObject.FindProperty("BoardColor");
        pLedScale = serializedObject.FindProperty("LedScale");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        
        EditorGUI.BeginChangeCheck();
        pLedText.stringValue = EditorGUILayout.TextField("Led text", pLedText.stringValue);
        pLedType.enumValueIndex = EditorGUILayout.Popup("Led type", pLedType.enumValueIndex, pLedType.enumDisplayNames);

        EditorGUILayout.LabelField("Board orientation", EditorStyles.boldLabel);
        pVerHor.boolValue = EditorGUILayout.Toggle("Horizontal", pVerHor.boolValue);
        if (pVerHor.boolValue)
        {
            pdistLetFieldsH.intValue = EditorGUILayout.IntSlider("Letter distance", pdistLetFieldsH.intValue, 0, 10);
            pdistLetFieldsV.intValue = 0;
        }
        else
        {
            pdistLetFieldsV.intValue = EditorGUILayout.IntSlider("Letter distance", pdistLetFieldsV.intValue, 0, 10);
            pdistLetFieldsH.intValue = 0;
        }

        EditorGUILayout.LabelField("Blink", EditorStyles.boldLabel);
        pIsBlink.boolValue = EditorGUILayout.Toggle("Blink", pIsBlink.boolValue);
        pDeltaBlinkTime.floatValue = EditorGUILayout.FloatField("Blink Time", pDeltaBlinkTime.floatValue);

        EditorGUILayout.LabelField("Move", EditorStyles.boldLabel);
        pIsMove.boolValue = EditorGUILayout.Toggle("is Move", pIsMove.boolValue);
        pMoveTo.enumValueIndex = EditorGUILayout.Popup("Move to",pMoveTo.enumValueIndex, pMoveTo.enumDisplayNames);
        pDeltaTimeMove.floatValue = EditorGUILayout.FloatField("DeltaMoveTime", pDeltaTimeMove.floatValue);

        EditorGUILayout.LabelField("Led settings", EditorStyles.boldLabel);
        pLedColor.colorValue = EditorGUILayout.ColorField("LedColor", pLedColor.colorValue);
        LBS.PrefOnMaterial.color = pLedColor.colorValue;
        pBoardColor.colorValue = EditorGUILayout.ColorField("Board color", pBoardColor.colorValue);
        pLedScale.vector3Value = EditorGUILayout.Vector3Field("Led scale", pLedScale.vector3Value);

        serializedObject.ApplyModifiedProperties();
        if (EditorGUI.EndChangeCheck())
        {
            LBS.BoardConstructor();
        }

        if (GUILayout.Button("Rebuild"))
        {
            LBS.BoardConstructor();
        }

        serializedObject.ApplyModifiedProperties();
    }

}
