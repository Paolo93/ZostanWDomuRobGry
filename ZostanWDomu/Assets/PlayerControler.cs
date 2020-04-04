using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public Node currentNode;

    void Start()
    {
        if(currentNode == null)
        {
            currentNode = GetClosestNode(Node.GetNodes());
        }
        transform.position = currentNode.transform.position;
    }

    private float previousHorizontal = 0.0f;
    private float previousVertical = 0.0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    Node markedNode = null;
    bool moveRequested = false;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0.0f || vertical != 0.0f)
        {
            var rayAngle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;

            foreach (Node node in GetLinkedNodes())
            {
                var p1 = transform.position;
                var p2 = node.transform.position;
                var destinationAngle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * Mathf.Rad2Deg;
                Debug.Log(string.Format("RA={0} DA={1} DA+={2} DA-={3}", rayAngle, destinationAngle, destinationAngle + 20, destinationAngle - 20));

                if (destinationAngle + 20.0f >= rayAngle && destinationAngle - 20.0f <= rayAngle)
                {
                    node.Mark();
                    markedNode = node;
                    break;
                }
            }
        }

        if (Input.GetButtonDown("Fire1") && markedNode)
        {
            moveRequested = true;
        }
    }

    private void FixedUpdate()
    {
        var end = new Vector3(horizontal, vertical, 0).normalized * 3;
        Debug.DrawLine(transform.position, transform.position + end,  Color.green);

        if (previousHorizontal != horizontal || previousVertical != vertical)
        {
            previousHorizontal = horizontal;
            previousVertical = vertical;
            foreach (Node node in GetLinkedNodes())
            {
                node.UnMark();
            }
        }

        if (moveRequested)
        {
            previousHorizontal = 0.0f;
            previousVertical = 0.0f;

            Debug.Log("MOVING TO NEW OBJECT");
            currentNode = markedNode;
            markedNode.UnMark();
            transform.position = markedNode.transform.position;
        }

        moveRequested = false;
        markedNode = null;
    }

    private Node GetClosestNode(List<Node> nodes)
    {
        Node closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Node node in nodes)
        {
            float dist = Vector3.Distance(node.transform.position, currentPos);
            if (dist < minDist)
            {
                closest = node;
                minDist = dist;
            }
        }

        return closest;
    }

    private List<Node> GetLinkedNodes()
    {
        List<Node> nodes = new List<Node>(currentNode.GetLinkedBy());
        foreach (Transform child in currentNode.transform)
        {
            nodes.Add(child.GetComponent<Link>().destination);
        }

        return nodes;
    }
}
