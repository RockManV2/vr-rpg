
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

    private void GenerateToolBar()
    {
        var toolBar = new Toolbar();

        var fileNameTextField = new TextField("FileName");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolBar.Add(fileNameTextField);

        toolBar.Add(new Button(() => RequestDataOperation(true)){text = "Save Data"});
        toolBar.Add(new Button(() => RequestDataOperation(false)){text = "Load Data"});
        
        var nodeCreateButton = new Button(() => { _graphView.CreateNode("Dialogue Node"); });
        nodeCreateButton.text = "Create Node";
        toolBar.Add(nodeCreateButton);
        
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
        
        if(save)
            saveUtility.SaveGraph(_fileName);
        else
            saveUtility.LoadGraph(_fileName);
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
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
