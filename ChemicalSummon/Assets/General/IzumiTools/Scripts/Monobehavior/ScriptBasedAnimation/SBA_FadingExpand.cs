using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBA_FadingExpand : MonoBehaviour
{
    public float expandRate = 1.2F;
    public CanvasGroup fadingGroup;
    [Min(0)]
    public float timeLength = 0.1F;
    [Min(0)]
    public float power = 1;
    //data
    float passedTime = float.MaxValue;
    bool isBeforeReach;
    Vector3 originalScale;
    public void StartAnimation()
    {
        if(isBeforeReach)
            transform.localScale = originalScale;
        gameObject.SetActive(true);
        passedTime = 0;
        originalScale = transform.localScale;
        isBeforeReach = true;
    }
    // Start is called before the first frame update
    void FixedUpdate()
    {
        if (passedTime < timeLength)
        {
            float timePassedRate = passedTime / timeLength;
            float rate = Mathf.Pow(timePassedRate, power);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * expandRate, rate);
            fadingGroup.alpha = 1 - rate;
            passedTime += Time.fixedDeltaTime;
        }
        if (isBeforeReach && passedTime >= timeLength)
        {
            isBeforeReach = false;
            gameObject.SetActive(false);
            transform.localScale = originalScale;
            //TODO: multi time expand
        }
    }
}
