using System;
using UnityEngine;

public enum NetworkObjectState
{
    Fixed,
    Broken
}

public enum FixType
{
    multiPress,
    holdButton
}

public class Link : MonoBehaviour
{
    [SerializeField]
    public NetworkObjectState networkObjectState = NetworkObjectState.Fixed;
    [SerializeField]
    public FixType fixType = FixType.holdButton;

    [SerializeField]
    public Node destination = null;

    [SerializeField]
    public Color fixedColor = Color.blue;
    [SerializeField]
    public Color multiPress_brokenColor = Color.yellow;
    [SerializeField]
    public Color holdButton_brokenColor = Color.red;
    Color brokenColor;

    [SerializeField]
    public float timeToFix = 1.5f;
    [SerializeField]
    public int buttonPressToFix = 5;

    private const float multiPressFixDelta = 0.3f;

    [HideInInspector]
    [SerializeField]
    private Node parent = null;
    private LineRenderer lineRenderer;
    Vector3 connectionPoint1;
    Vector3 connectionPoint2;

    Gradient gradient = new Gradient();

    void Start()
    {
        parent = GetComponentInParent<Node>();
        if (!parent || !destination)
            return;

        brokenColor = fixType == FixType.holdButton ? holdButton_brokenColor : multiPress_brokenColor;
        Color linkColor = networkObjectState == NetworkObjectState.Fixed ? fixedColor : brokenColor;

        SpriteRenderer parentRenderer = GetComponentInParent<SpriteRenderer>();
        SpriteRenderer destinationRenderer = destination.GetComponent<SpriteRenderer>();

        connectionPoint1 = GetConnectionPoint(destination, parent, destinationRenderer.bounds.size[0] / 2.0f);
        connectionPoint2 = GetConnectionPoint(parent, destination, parentRenderer.bounds.size[0] / 2.0f);

        lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.startColor = linkColor;
        lineRenderer.endColor = linkColor;
        lineRenderer.SetPositions(new []{ connectionPoint1, connectionPoint2 });
        lineRenderer.transform.position = (connectionPoint1 - connectionPoint2) / 2f;

        GradientColorKey[] colorKey = new GradientColorKey[2] { new GradientColorKey(brokenColor, 0.0f), new GradientColorKey(fixedColor, 1.0f) };
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2] { new GradientAlphaKey(1.0f, 0f), new GradientAlphaKey(1f, 1f) };
        gradient.SetKeys(colorKey, alphaKey);

        destination.LinkedBy(parent);
    }

    private void FixedUpdate()
    {
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

    float timer = float.PositiveInfinity;
    int pressedButton = 0;

    public bool HoldingButton()
    {
        if (networkObjectState == NetworkObjectState.Fixed)
            return true;

        if (timer == float.PositiveInfinity)
            timer = Time.time;

        if (fixType == FixType.holdButton)
        {
            return SetFixedPercentage((Time.time - timer) / timeToFix);
        }
        else
        {
            return false;
        }
    }

    internal bool ButtonReleased()
    {
        if (networkObjectState == NetworkObjectState.Fixed)
            return true;

        if (fixType == FixType.holdButton)
        {
            SetFixedPercentage(0f);
            timer = float.PositiveInfinity;
            return false;
        }

        if(Time.time - timer < multiPressFixDelta)
        {
            timer = float.PositiveInfinity;
            return SetFixedPercentage((float)pressedButton++ / (float)buttonPressToFix);
        }
        else
        {
            //Debug.Log("Press delta: " + (Time.time - timer));
            pressedButton = 0;
        }
        timer = float.PositiveInfinity;
        return false;
    }

    internal bool SetFixedPercentage(float percent)
    {
        //Debug.Log("Fix " + percent * 100.0f + "%");
        if (percent >= 1.0f)
        {
            networkObjectState = NetworkObjectState.Fixed;
            parent.SetState(NetworkObjectState.Fixed);
            destination.SetState(NetworkObjectState.Fixed);
            AudioManager.instance.Play("sfx_fix_success");

            return true;
        }
        else
        {
            lineRenderer.startColor = gradient.Evaluate(percent);
            lineRenderer.endColor = gradient.Evaluate(percent);
        }

        return false;
    }
}
