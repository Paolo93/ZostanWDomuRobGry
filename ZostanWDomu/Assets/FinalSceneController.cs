using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneController : MonoBehaviour
{
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void End()
    {
        Loader.Load("0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
