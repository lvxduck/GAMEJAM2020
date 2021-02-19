using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingGame : MonoBehaviour
{
    public Slider slider;
    public RectTransform rectTransform_TextLog;
    private RectTransform rectTransform;
    private bool isDone;
    private LTDescr x;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slider.value = 0;
        isDone = false;
    }
    public void compelete()
    {
        LeanTween.move(rectTransform_TextLog, new Vector3(0f, 0f, 0f), 1f).setEaseInOutCubic().setOnComplete(() =>
        {
            LeanTween.delayedCall(0.5f, () =>
            {
                rectTransform_TextLog.position = new Vector3(rectTransform_TextLog.position.x, -350f, rectTransform_TextLog.position.z);
                LeanTween.move(rectTransform, new Vector3(rectTransform.position.x, -600, rectTransform.position.z), 1).setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    GameManager.instance.spawnCoint_EXP(new Vector2(-13.5f, 5), 6, 2);
                    slider.value = 0;
                });
            });
        });
    }
    public void btnStart()
    {
        isDone = false;
        x = LeanTween.value(0, 1, 2).setOnUpdate((float x) =>
        {
            if(!isDone)
                slider.value = x;
        });
    }

    public void btnStop()
    {
        if (!isDone)
        {
            if (slider.value > 0.45f && slider.value < 0.55f)
            {
                isDone = true;
                compelete();
            }
            else
            {
                //LeanTween.removeTween(x.id);
                LeanTween.cancel(x.id);
                LeanTween.value(0, 1, 2).setOnUpdate((float x) =>
                {
                    if (!isDone)
                        slider.value = x;
                });
            }
        }
    }

    void Update()
    {
        
    }
}
