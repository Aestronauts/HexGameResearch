using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGrid))]
[CanEditMultipleObjects]
public class HexGridEditor : Editor {
	SerializedProperty gridSize;
	SerializedProperty radius;
	SerializedProperty baseElevation;
	SerializedProperty maxElevation;
	SerializedProperty flatTop;
	SerializedProperty material;
    SerializedProperty xOrg;
    SerializedProperty yOrg;
    SerializedProperty scale;
    
    void OnEnable() {
        gridSize = serializedObject.FindProperty("gridSize");
        radius = serializedObject.FindProperty("radius");
        baseElevation = serializedObject.FindProperty("baseElevation");
        maxElevation = serializedObject.FindProperty("maxElevation");
        flatTop = serializedObject.FindProperty("flatTop");
        material = serializedObject.FindProperty("material");
        xOrg = serializedObject.FindProperty("xOrg");
        yOrg = serializedObject.FindProperty("yOrg");
        scale = serializedObject.FindProperty("scale");
    }

    public override void OnInspectorGUI() {
        HexGrid t = (HexGrid)target;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Grid Settings");
        EditorGUILayout.PropertyField(gridSize);
        EditorGUILayout.LabelField("Tile Settings");
        EditorGUILayout.PropertyField(radius);
        EditorGUILayout.PropertyField(baseElevation);
        EditorGUILayout.PropertyField(maxElevation);
        EditorGUILayout.PropertyField(flatTop);
        EditorGUILayout.PropertyField(material);
        EditorGUILayout.LabelField("Noise Settings");
        EditorGUILayout.PropertyField(xOrg);
        EditorGUILayout.PropertyField(yOrg);
        EditorGUILayout.PropertyField(scale);

        if (EditorGUI.EndChangeCheck()) {
            t.GenerateGrid();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
