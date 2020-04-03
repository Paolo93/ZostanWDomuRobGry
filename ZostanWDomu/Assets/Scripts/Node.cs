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

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetState(nodeState);
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
        else
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
}
