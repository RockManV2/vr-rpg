
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueVendor))]
public class DialogueVendorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueVendor vendor = (DialogueVendor)target;
        
        GUILayout.Label(vendor.Guid);
        
        if(vendor.Guid == string.Empty)
            if(GUILayout.Button("Generate vendor Guid"))
                GenerateNewGuid(vendor);
        
        if(Resources.Load<DialogueContainer>($"Dialogue/{vendor.Guid}") == null)
            if(GUILayout.Button("Generate new dialogue asset"))
                GenerateNewDialogueAsset(vendor);
        
        if(vendor.Dialogue != null)
            if(GUILayout.Button("Open dialogue asset"))
                OpenDialogueAsset(vendor);
    }

    private void GenerateNewGuid(DialogueVendor vendor)
    {
        vendor.Guid = Guid.NewGuid().ToString();
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
