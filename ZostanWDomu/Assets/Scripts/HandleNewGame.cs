using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleNewGame : MonoBehaviour
{
    public string nextLevel;

    public void StartGame()
    {
        Loader.Load(nextLevel);
    }
}
