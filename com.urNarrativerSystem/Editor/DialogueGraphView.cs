﻿using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueGraphView : GraphView
{
    private DialogueGraph dialogueGraph;
    private NodeSearchWindow searchWindow;



    public DialogueGraphView(DialogueGraph _editorWindow)
    {
        dialogueGraph = _editorWindow;

        styleSheets.Add(Resources.Load<StyleSheet>(path: "DialogueGraph"));
        styleSheets.Add(Resources.Load<StyleSheet>(path: "Node"));

        SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        AddSearchWindow();

    }

    private void AddSearchWindow()
    {
        searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        searchWindow.Init(dialogueGraph,this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        Port startPortView = startPort;

        ports.ForEach((port) => { Port portView = port;
            if (startPortView != portView && startPortView.node != portView.node && startPortView.direction != port.direction) 
            {
                compatiblePorts.Add(port);
            }
        });
        return compatiblePorts;
    }

    public void LanguageReload()
    {
        List<DialogueNode> dialogueNodes = nodes.ToList().Where(node => node is DialogueNode).Cast<DialogueNode>().ToList();
        foreach (DialogueNode dialogueNode in dialogueNodes)
        {
            dialogueNode.ReloadLanguage();
        }
    }


    
    public StartNode CreateStartNode(Vector2 _pos)
    {
        StartNode tmp = new StartNode(_pos, dialogueGraph, this);
        return tmp;
    }

    public EndNode CreateEndNode(Vector2 _pos)
    {
        EndNode tmp = new EndNode(_pos, dialogueGraph, this);
        return tmp;
    }

    public EventNode CreateEventNode(Vector2 _pos)
    {
        EventNode tmp = new EventNode(_pos, dialogueGraph, this);
        return tmp;
    }

    public DialogueNode CreateDialogueNode(Vector2 _pos)
    {
        DialogueNode tmp = new DialogueNode(_pos, dialogueGraph, this);
        return tmp;
    }

    public ScriptNode CreateScriptNode(Vector2 _pos)
    {
        ScriptNode tmp = new ScriptNode(_pos, dialogueGraph, this);
        return tmp;
    }



}
