using SavePort;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UntypedDataContainer), true)]
public class DataContainerInspector : Editor {

    private UntypedDataContainer container;

    private void OnEnable() {
        container = (UntypedDataContainer)target;
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Force Update")) {
            container.ForceUpdate();
        }
    }

}
