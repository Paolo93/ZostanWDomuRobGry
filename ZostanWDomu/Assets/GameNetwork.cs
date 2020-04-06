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

    public GameObject endingCanvas;
    public float canvasDisplayTime;
    public GameObject[] itemsToFadeOut;

    public string thisMap;
    public string nextMap;

    public float gameTime = 60.0f;
    public bool ending = false;

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
        if (ending)
            return;

        gameTime -= Time.fixedDeltaTime;
        timerText.text = "Time left: " + gameTime.ToString("F1");
        if(gameTime <= 0)
        {
            Loader.Load(thisMap);
        }
    }

    IEnumerator ShowCanvas()
    {
        ending = true;
        foreach(var sound in AudioManager.instance.sounds)
        {
            sound.volume = 0.1f;
        }

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (var item in itemsToFadeOut)
        {
            item.SetActive(false);
        }

        endingCanvas.SetActive(true);
        yield return new WaitForSeconds(canvasDisplayTime);
        endingCanvas.SetActive(false);

        foreach (var sound in AudioManager.instance.sounds)
        {
            sound.volume = 1f;
        }

        Loader.Load(nextMap);
    }

    internal void LinkFixed()
    {
        brokenLinks--;
        UpdateNetworkState();

        if(brokenLinks == 0)
        {
            StartCoroutine(ShowCanvas());
        }
    }
}
