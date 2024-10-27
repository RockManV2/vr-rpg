
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueVendor))]
public class DialogueVendorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueVendor vendor = (DialogueVendor)target;
        
        GUILayout.Label($"Vendor Guid:\n{vendor.Guid}");
        
        if(Resources.Load<DialogueContainer>($"Dialogue/{vendor.Guid}") == null)
            if(GUILayout.Button("Generate new dialogue asset"))
                GenerateNewDialogueAsset(vendor);
        
        if(vendor.Dialogue != null)
            if(GUILayout.Button("Open dialogue asset"))
                OpenDialogueAsset(vendor);
    }
    
    private void GenerateNewDialogueAsset(DialogueVendor vendor)
    {
        var dialogueContainer = CreateInstance<DialogueContainer>();
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/Dialogue/{vendor.Guid}.asset");
        AssetDatabase.SaveAssets();
    }

    private void OpenDialogueAsset(DialogueVendor vendor)
    {
        DialogueGraph.OpenDialogueGraphWindow(vendor.Guid);
    }
}
