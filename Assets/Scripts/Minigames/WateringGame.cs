using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WateringGame : MonoBehaviour
{
    public Slider sliderTime;

    public float timeWatering;
    float time=0;
    public Animator animator;
    public RectTransform rectTransform_TextLog;
    private RectTransform rectTransform;
  

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        sliderTime.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator watering()
    {
        Debug.Log("watering");
        animator.SetBool("isWatering", true);
        time = 0;
        sliderTime.value = 0;
        while (time < timeWatering)
        {
            yield return new WaitForSeconds(1);
            time += 1;
            if (time < timeWatering)
            {
                sliderTime.value = (float)(time / timeWatering);
                Debug.Log("dsdsd" + (float)(time / timeWatering));
            }
        }
        animator.SetBool("isWatering", false);
        sliderTime.value = 1;
        // LeanTween.moveY(textLog, 2, 1);
        LeanTween.move(rectTransform_TextLog, new Vector3(0f, 0f, 0f), 1f).setEaseInOutCubic().setOnComplete(() =>
        {
            LeanTween.delayedCall(0.5f, () =>
            {
                rectTransform_TextLog.position = new Vector3(rectTransform_TextLog.position.x, -350f, rectTransform_TextLog.position.z);
                LeanTween.move(rectTransform, new Vector3(rectTransform.position.x, -600, rectTransform.position.z), 1).setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    sliderTime.value = 0;
                    GameManager.instance.spawnCoint_EXP(new Vector2(-18, 5), 6, 2);
                });
            });
        });
    }

    public void btnWatering()
    {
        StartCoroutine(watering());
    }
}
