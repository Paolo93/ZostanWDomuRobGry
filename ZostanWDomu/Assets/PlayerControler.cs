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
    }

    private float previousHorizontal = 0.0f;
    private float previousVertical = 0.0f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Node markedNode = null;

        if (horizontal != 0.0f || vertical != 0.0f)
        {
            if(previousHorizontal != horizontal || previousVertical != vertical)
            {
                previousHorizontal = horizontal;
                previousVertical = vertical;
                foreach (Node node in GetLinkedNodes())
                {
                    node.UnMark();
                }
            }

            var end = transform.position + new Vector3(horizontal, vertical, 0);
            Debug.DrawLine(transform.position, end * 3, Color.green, 0.15f, false);

            var rayAngle = Mathf.Atan2(transform.position.x - horizontal, transform.position.y - vertical) * 180 / Mathf.PI;

            foreach (Node node in GetLinkedNodes())
            {
                var destinationAngle = Mathf.Atan2(transform.position.x - node.transform.position.x, transform.position.y - node.transform.position.y) * 180 / Mathf.PI;

                if (destinationAngle + 20 >= rayAngle && destinationAngle - 20 <= rayAngle)
                {
                    node.Mark();
                    markedNode = node;
                    break;
                }
                else
                {
                    node.UnMark();
                }
            }
        }

        if (Input.GetButtonDown("Fire1") && markedNode)
        {
            previousHorizontal = 0.0f;
            previousVertical = 0.0f;

            Debug.Log("MOVING TO NEW OBJECT");
            transform.position = markedNode.transform.position;
            currentNode = markedNode;
            markedNode.UnMark();
        }
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
        List<Node> links = new List<Node>();
        foreach (Transform child in currentNode.transform)
        {
            links.Add(child.GetComponent<Link>().destination);
        }

        return links;
    }
}
