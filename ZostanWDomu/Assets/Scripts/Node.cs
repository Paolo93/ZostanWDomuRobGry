using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Fixed,
    Broken
}

public class Node : MonoBehaviour
{
    public NodeState nodeState = NodeState.Fixed;
    private SpriteRenderer spriteRenderer;
    private static List<Node> nodes = new List<Node>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetState(nodeState);
        nodes.Add(this);
    }

    void Update()
    {

    }

    public void SetState(NodeState state)
    {
        nodeState = state;
        if(nodeState == NodeState.Fixed)
        {
            spriteRenderer.color = Color.blue;
        }
        else if (nodeState == NodeState.Broken)
        {
            spriteRenderer.color = Color.gray;
        }
    }

    public void ToggleState()
    {
        if(nodeState == NodeState.Fixed)
        {
            SetState(NodeState.Broken);
        }
        else
        {
            SetState(NodeState.Fixed);
        }
    }

    public void Mark()
    {
        spriteRenderer.color = Color.yellow;
    }

    public void UnMark()
    {
        SetState(nodeState);
    }

    public static List<Node> GetNodes()
    {
        return nodes;
    }
}
