using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";

    [MenuItem("Graph/Dialogue graph")]
    public static void OpenDialogueGraphWindow(string graphName = "")
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");

        if (graphName == "") return;

        window.SetFileName(graphName);
        window.RequestDataOperation(false);
        window.rootVisualElement.Q<Toolbar>().Q<TextField>().SetValueWithoutNotify(graphName);
    }

    private void GenerateToolbar()
    {
        var toolBar = new Toolbar();

        toolBar.style.width = 170;
        toolBar.style.height = 999;
        
        toolBar.style.flexDirection = FlexDirection.Column;
        toolBar.style.alignSelf = Align.FlexEnd;
        
        var fileLabel = new Label("Title") { text = "File Options" };
        fileLabel.style.alignSelf = Align.Center;
        fileLabel.style.paddingTop = 10;
        fileLabel.style.paddingBottom = 10;
        toolBar.Add(fileLabel);
        
        var fileNameTextField = new TextField();
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolBar.Add(fileNameTextField);

        toolBar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolBar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        var objectLabel = new Label("Title") { text = "Create Object" };
        objectLabel.style.alignSelf = Align.Center;
        objectLabel.style.paddingTop = 10;
        objectLabel.style.paddingBottom = 10;
        toolBar.Add(objectLabel);
        
        toolBar.Add(new Button(() => _graphView.AddDialogueNode("Dialogue Node")) { text = "Create Dialogue Node" });
        toolBar.Add(new Button(() => _graphView.AddQuestNode("Quest Node")) { text = "Create Quest Node" });
        toolBar.Add(new Button(() => Debug.Log("Not Implemented")) { text = "Create Execution Node" });
        
        rootVisualElement.Add(toolBar);
    }
    

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid File Name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        if (save)
            saveUtility.SaveGraph(_fileName);
        else
            saveUtility.LoadGraph(_fileName);
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void SetFileName(string fileName)
    {
        _fileName = fileName;
    }
}