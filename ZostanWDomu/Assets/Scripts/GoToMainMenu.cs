using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMainMenu : MonoBehaviour
{
    void Update()
    {
        if(Input.anyKey)
        {
            Loader.Load("0");
        }
    }
}
