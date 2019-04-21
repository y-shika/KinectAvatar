using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VL_OVRLipSyncMicInput))]
public class OVRLipSyncMicInputInspector : Editor
{

    OVRLipSyncMicInput _OVRLipSyncMicInput = null;

    void OnEnable()
    {
        _OVRLipSyncMicInput = target as OVRLipSyncMicInput;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Mic Device", GUILayout.Width(120));
            _OVRLipSyncMicInput.selectedDeviceIndex = EditorGUILayout.Popup(_OVRLipSyncMicInput.selectedDeviceIndex, Microphone.devices);
        }
    }
}
