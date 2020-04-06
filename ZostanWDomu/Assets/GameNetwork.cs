using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNetwork : MonoBehaviour
{
    public int totalLinks;
    public int brokenLinks;

    public Text timerText;
    public Text networkStateText;

    private float gameTime = 60.0f;

    public void AddLink()
    {
        totalLinks++;
        UpdateNetworkState();
    }

    public void AddBrokenLink()
    {
        brokenLinks++;
        UpdateNetworkState();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateNetworkState();
    }

    private void UpdateNetworkState()
    {
        if(brokenLinks > 0)
        {
            networkStateText.color = Color.red;
        }
        else
        {
            networkStateText.color = Color.blue;
        }

        networkStateText.text = "Network state " + ((float)(totalLinks - brokenLinks) / totalLinks).ToString("F1") + "%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        gameTime -= Time.fixedDeltaTime;
        timerText.text = "Time left: " + gameTime.ToString("F1");
        if(gameTime <= 0)
        {

        }
    }

    internal void LinkFixed()
    {
        brokenLinks--;
        UpdateNetworkState();
    }
}
