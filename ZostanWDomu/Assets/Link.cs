using UnityEngine;

public class Link : MonoBehaviour
{
    public Node destination;

    public const float maxAngle = 20;

    private Node parent;
    private LineRenderer lineRenderer;

    void Start()
    {
        parent = GetComponentInParent<Node>();
        SpriteRenderer parentRenderer = GetComponentInParent<SpriteRenderer>();
        SpriteRenderer destinationRenderer = destination.GetComponent<SpriteRenderer>();

        Vector3 connectionPoint1 = GetConnectionPoint(destination, parent, destinationRenderer.bounds.size[0] / 2.0f);
        Vector3 connectionPoint2 = GetConnectionPoint(parent, destination, parentRenderer.bounds.size[0] / 2.0f);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPositions(new []{ connectionPoint1, connectionPoint2 });
    }

    private Vector3 GetConnectionPoint(Node node1, Node node2, float radius)
    {
        var p1 = node1.transform.position;
        var p2 = node2.transform.position;

        var angle = Mathf.Atan2(p1.y - p2.y, p1.x - p2.x);
        return p1 - new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
    }
}
