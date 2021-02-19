using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickingWarm : MonoBehaviour
{
    public RectTransform parent;

    public RectTransform rectTransform_TextLog;
    private RectTransform rectTransform;
    public GameObject[] worms;
    public int numPickWorm=0;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        foreach (GameObject worm in worms)
        {
            worm.GetComponent<Button>().onClick.AddListener(() =>
            {
                RectTransform rf = worm.GetComponent<RectTransform>();
                Vector3 scale = new Vector3(4, 4, 0);
                LeanTween.scale(rf, scale, 0.4f).setOnComplete(()=> {
                    worm.SetActive(false);
                    numPickWorm += 1;
                    if (numPickWorm == worms.Length)
                    {
                        compelete();
                    }
                });
            });
        }
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
                    numPickWorm = 0;
                    foreach (GameObject worm in worms)
                    {
                        worm.GetComponent<RectTransform>().LeanScale(new Vector3(1, 1, 0), 0.1f);
                        worm.SetActive(true);
                    }
                });
            });
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
