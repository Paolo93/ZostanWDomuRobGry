using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Node : MonoBehaviour
{
    public NetworkObjectState nodeState = NetworkObjectState.Fixed;
    public Light2D nodeLight;

    private SpriteRenderer spriteRenderer;
    private static HashSet<Node> nodes = new HashSet<Node>();
    private HashSet<Node> linkedByNodes = new HashSet<Node>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.blue;

        foreach (Transform t in transform)
        {
            Link link = t.GetComponent<Link>();
            if (link)
            {
                if (!link.IsFixed())
                {
                    SetState(NetworkObjectState.Broken);
                }
            }
        }
   

        nodes.Add(this);
    }

    public void SetState(NetworkObjectState state)
    {
        nodeState = state;
        if(nodeState == NetworkObjectState.Fixed)
        {
            nodeLight.pointLightOuterRadius = 1.1f;
        }
        else if (nodeState == NetworkObjectState.Broken)
        {
            nodeLight.pointLightOuterRadius = 0.3f;
        }
    }

    internal void LinkedBy(Node neighbor)
    {
        foreach (Transform t in neighbor.transform)
        {
            var link = t.GetComponent<Link>();
            if(link && link.destination == this)
            {
                if (!link.IsFixed())
                {
                    SetState(NetworkObjectState.Broken);
                }
            }
        }

        linkedByNodes.Add(neighbor);
    }

    internal bool IsBrokenConnection(Node neighbor)
    {
        foreach (Transform t in neighbor.transform)
        {
            var link = t.GetComponent<Link>();
            if (link && link.destination == this && !link.IsFixed())
            {
                return true;
            }
        }

        foreach (Transform t in transform)
        {
            var link = t.GetComponent<Link>();
            if (link && link.destination == neighbor && !link.IsFixed())
            {
                return true;
            }
        }

        return false;
    }

    internal Link GetBrokenConnection(Node neighbor)
    {
        foreach (Transform t in neighbor.transform)
        {
            var link = t.GetComponent<Link>();
            if (link && link.destination == this)
            {
                return link;
            }
        }

        foreach (Transform t in transform)
        {
            var link = t.GetComponent<Link>();
            if (link && link.destination == neighbor)
            {
                return link;
            }
        }

        return null;
    }

    public void Mark()
    {
        spriteRenderer.color = Color.yellow;
    }

    public void UnMark()
    {
        spriteRenderer.color = Color.blue;
        SetState(nodeState);
    }

    public static HashSet<Node> GetNodes()
    {
        return nodes;
    }

    internal HashSet<Node> GetLinkedBy()
    {
        return linkedByNodes;
    }
}
