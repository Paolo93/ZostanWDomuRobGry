using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControler : MonoBehaviour
{
    public float movementAngleTolerance = 30.0f;
    public Node currentNode;

    void Start()
    {
        if(currentNode == null)
        {
            currentNode = GetClosestNode(Node.GetNodes());
            if(currentNode != null)
            transform.position = currentNode.transform.position;
        }
    }

    private float previousHorizontal = 0.0f;
    private float previousVertical = 0.0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    Node markedNode = null;
    bool moveRequested = false;
    private bool fixingNode;
    
    Link brokenConnection;

    void Update()
    {
        if (currentNode == null)
        {
            currentNode = GetClosestNode(Node.GetNodes());
            if(currentNode != null)
                transform.position = currentNode.transform.position;
        }

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

                if (destinationAngle + movementAngleTolerance >= rayAngle && destinationAngle - movementAngleTolerance <= rayAngle)
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

        if (Input.GetButtonDown("Fire2") && markedNode && markedNode.IsBrokenConnection(currentNode))
        {
            fixingNode = true;
            
            brokenConnection = markedNode.GetBrokenConnection(currentNode);
            Debug.Log("Started fixing");
            AudioManager.instance.Play("sfx_fix_attempt");
        }

        if (fixingNode)
        {
            UpdateFixingLink();
        }
    }

    private void UpdateFixingLink()
    {
        if (Input.GetButton("Fire2"))
        {
            if(brokenConnection.HoldingButton())
            {
                fixingNode = false;
                brokenConnection = null;
            }
        }
        else if(Input.GetButtonUp("Fire2"))
        {
            Debug.Log("released");
            if (brokenConnection.ButtonReleased())
            {
                //not fixed
                fixingNode = false;
                brokenConnection = null;
            }
            else if (brokenConnection.fixType == FixType.holdButton)
            {
                //not fixed, button held too short
                fixingNode = false;
                brokenConnection = null;
            }
            else
            {
                //potential multitap
            }
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
            AudioManager.instance.Play("sfx_movement");
        }

        moveRequested = false;
        markedNode = null;
    }

    private Node GetClosestNode(HashSet<Node> nodes)
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

    private HashSet<Node> GetLinkedNodes()
    {
        if(currentNode == null)
            currentNode = GetClosestNode(Node.GetNodes());

        HashSet<Node> nodes = new HashSet<Node>(currentNode.GetLinkedBy());
        foreach (Transform child in currentNode.transform)
        {
            var link = child.GetComponent<Link>();
            if (link && link.destination)
            {
                nodes.Add(child.GetComponent<Link>().destination);
            }
        }

        return nodes;
    }
}
