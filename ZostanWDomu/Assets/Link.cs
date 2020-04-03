using UnityEngine;

public class Link : MonoBehaviour
{
    public Node destination;
    private Node parent;
    private LineRenderer lineRenderer;

    void Start()
    {
        parent = gameObject.GetComponentInParent<Node>();

        Vector3 pos1 = destination.transform.position;
        Vector3 pos2 = parent.transform.position;

        transform.position = (pos1 + pos2) / 2;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPositions(new []{ pos1, pos2 });
    }
}
