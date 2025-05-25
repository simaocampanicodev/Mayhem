using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(OrderDisplay))]
public class OrderDisplayEditor : Editor
{
    private OrderDisplay orderDisplay;
    
    private void OnEnable()
    {
        orderDisplay = (OrderDisplay)target;
    }
    
    private void OnSceneGUI()
    {
        SerializedProperty positionsProp = serializedObject.FindProperty("displayPositions");
        
        if (positionsProp != null)
        {
            for (int i = 0; i < positionsProp.arraySize; i++)
            {
                SerializedProperty positionProp = positionsProp.GetArrayElementAtIndex(i);
                if (positionProp.objectReferenceValue != null)
                {
                    Transform posTransform = (Transform)positionProp.objectReferenceValue;
                    Vector3 pos = posTransform.position;
                    
                    Handles.color = new Color(0f, 1f, 0f, 0.5f);
                    Handles.DrawWireDisc(pos, Vector3.forward, 0.5f);
                    
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.white;
                    style.fontSize = 20;
                    style.fontStyle = FontStyle.Bold;
                    style.alignment = TextAnchor.MiddleCenter;
                    Handles.Label(pos, i.ToString(), style);
                }
            }
        }
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Configurações de Display de Pedidos", EditorStyles.boldLabel);
        
        DrawDefaultInspector();
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Testar Display de Todos os Pedidos"))
        {
            TestAllOrderDisplays();
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    
    private void TestAllOrderDisplays()
    {
        SerializedProperty positionsProp = serializedObject.FindProperty("displayPositions");
        
        if (Application.isPlaying && positionsProp != null)
        {
            for (int i = 0; i < positionsProp.arraySize; i++)
            {
                SilhouetteOrder.OrderType orderType = (SilhouetteOrder.OrderType)(i % 3);
                orderDisplay.ShowOrder(i, orderType);
            }
        }
    }
}
#endif