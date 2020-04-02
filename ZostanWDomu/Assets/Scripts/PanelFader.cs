using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    [SerializeField]
    private bool isFaded = false;
    [SerializeField]
    public float fadeDuration = 0.5f;

    void Start()
    {
        Fade();
    }


    public void Fade()
    {
        var canvGroup = GetComponent<CanvasGroup>();

        StartCoroutine(PerformFade(canvGroup, canvGroup.alpha, isFaded ? 1 : 0));

        isFaded = !isFaded;

    }

    public IEnumerator PerformFade(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;

        while(counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / fadeDuration);
            yield return null;
        }
    }


}
