using System;
using UnityEngine;

public enum NetworkObjectState
{
    Fixed,
    Broken
}

public class Link : MonoBehaviour
{
    [SerializeField]
    public NetworkObjectState networkObjectState = NetworkObjectState.Fixed;

    [SerializeField]
    public Node destination = null;

    [HideInInspector]
    [SerializeField]
    private Node parent = null;
    private LineRenderer lineRenderer;
    Vector3 connectionPoint1;
    Vector3 connectionPoint2;
    Color linkColor;

    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    void Start()
    {
        parent = GetComponentInParent<Node>();
        if (!parent || !destination)
            return;

        gradient = new Gradient();

        SpriteRenderer parentRenderer = GetComponentInParent<SpriteRenderer>();
        SpriteRenderer destinationRenderer = destination.GetComponent<SpriteRenderer>();

        connectionPoint1 = GetConnectionPoint(destination, parent, destinationRenderer.bounds.size[0] / 2.0f);
        connectionPoint2 = GetConnectionPoint(parent, destination, parentRenderer.bounds.size[0] / 2.0f);

        lineRenderer = GetComponent<LineRenderer>();
        linkColor = networkObjectState == NetworkObjectState.Fixed ? Color.blue : Color.red;
        lineRenderer.startColor = linkColor;
        lineRenderer.endColor = linkColor;
        lineRenderer.SetPositions(new []{ connectionPoint1, connectionPoint2 });
        lineRenderer.transform.position = (connectionPoint1 - connectionPoint2) / 2f;

        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        destination.LinkedBy(parent);
    }

    private void FixedUpdate()
    {
        Color color = networkObjectState == NetworkObjectState.Fixed ? Color.blue : Color.red;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public bool IsFixed()
    {
        return networkObjectState == NetworkObjectState.Fixed;
    }

    private Vector3 GetConnectionPoint(Node node1, Node node2, float radius)
    {
        var p1 = node1.transform.position;
        var p2 = node2.transform.position;

        var angle = Mathf.Atan2(p1.y - p2.y, p1.x - p2.x);
        return p1 - new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
    }

    internal void SetFixedPercentage(float percent)
    {
        //Debug.Log("Fixing: " + percent);
        if (percent == 0.0f)
        {
            linkColor = Color.gray;
            lineRenderer.startColor = linkColor;
            lineRenderer.endColor = linkColor;
            lineRenderer.SetPositions(new[] { connectionPoint1, connectionPoint2 });
        }
        else if (percent >= 1.0f)
        {
            networkObjectState = NetworkObjectState.Fixed;
            parent.SetState(NetworkObjectState.Fixed);
            destination.SetState(NetworkObjectState.Fixed);
        }
        else
        {
            lineRenderer.startColor = gradient.Evaluate(percent);
            lineRenderer.endColor = gradient.Evaluate(percent);
        }
    }
}
