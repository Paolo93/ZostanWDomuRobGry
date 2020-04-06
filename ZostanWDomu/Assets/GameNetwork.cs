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

    public string thisMap;
    public string nextMap;

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

        networkStateText.text = "Network state " + (((float)(totalLinks - brokenLinks) / (float)totalLinks)*100.0f).ToString("F1") + "%";
    }

    private void FixedUpdate()
    {
        gameTime -= Time.fixedDeltaTime;
        timerText.text = "Time left: " + gameTime.ToString("F1");
        if(gameTime <= 0)
        {
            Loader.Load(thisMap);
        }
    }

    internal void LinkFixed()
    {
        brokenLinks--;
        UpdateNetworkState();

        if(brokenLinks == 0)
        {
            Loader.Load(nextMap);
        }
    }
}
