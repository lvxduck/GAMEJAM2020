using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //UI
    public GameObject panel_equipment;
    public GameObject panel_equipment_weapon;
    public GameObject panel_equipment_armor;
    public GameObject panel_black;
    public GameObject panel_introduce;
    public GameObject joystick;
    public GameObject btnTask;

    public SpriteRenderer sr_weapon;
    public Sprite[] sprite_weapons;
    public SpriteRenderer sr_armor;
    public Sprite[] sprite_armor;

    //
    private float time;
    private int day;
    private bool isSpawnMonster;

    //monster
    public GameObject monsterLevel1;
    public GameObject monsterLevel2;
    public GameObject monsterLevel3;
    public GameObject monsterLevel4;
    public GameObject monsterBoss;

    //
    public GameObject coinPrefab;
    public GameObject EXPPrefab;
    public GameObject HPPrefab;

    private int[,] infoWave = {{3,0,0,0,0},{4,1,0,0,0},{5,3,0,0,0},{7,5,1,0,0},{7,5,2,1,0},{9,7,3,2,0},{7,7,4,3,0},{7,8,5,3,0},{5,10,6,4,0},{3,12,7,5,1} };
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        time = 0;
        day = 1;
        isSpawnMonster = false;
        panel_black.SetActive(false);
        panel_equipment.SetActive(false);
        intiListenerBtnPnlEquipment();

        activeIntroduce();
    }

    private void activeIntroduce()
    {
        panel_black.SetActive(true);
        panel_introduce.SetActive(true);
        joystick.SetActive(false);
        btnTask.SetActive(false);
        LeanTween.move(panel_introduce.GetComponent<RectTransform>(), new Vector3(0f, 40, 0f), 0.2f);
    }

    public void btnOKIntroduce()
    {
        LeanTween.move(panel_introduce.GetComponent<RectTransform>(), new Vector3(0f, -600, 0f), 0.2f);
        panel_black.SetActive(false);
        joystick.SetActive(true);
        btnTask.SetActive(true);
    }

    private void intiListenerBtnPnlEquipment()
    {
        for(int i = 0; i <= 5; i++)
        {
            int type = i;
            GameObject item = panel_equipment_weapon.transform.GetChild(i).gameObject;
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("ITEM" + type);
                PlayerController.instance.type_weapon = type;
                sr_weapon.sprite = sprite_weapons[type];
            });
        }
        for (int i = 0; i <= 5; i++)
        {
            int type = i;
            GameObject item = panel_equipment_armor.transform.GetChild(i).gameObject;
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("armor" + type);
                PlayerController.instance.type_armor = type;
                sr_armor.sprite = sprite_armor[type];
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += 0.02f;
        // moi ngay co 4 phut
        // quai sinh sau moi ngay
        if (time - (day*4*60) >= 3)
        {
            if (!isSpawnMonster)
            {
                spawnMonster(day+1);
                day += 1;
            }
        }

    }

    public void onCompleteTask(int type)
    {

    }

    public void spawnCoint_EXP(Vector2 pos , int numberCoint, int numberEXP)
    {
        for(int i = 1; i <= numberCoint; i++)
        {
            Instantiate(coinPrefab, randomPos(pos,1.5f), Quaternion.identity);
        }
        for (int i = 1; i <= numberEXP; i++)
        {
            Instantiate(EXPPrefab, randomPos(pos, 1.5f), Quaternion.identity);
        }
    }

    private Vector3 randomPos(Vector2 pos, float radius)
    {
        float x = Random.Range(-radius, radius);
        float y = Random.Range(-radius, radius);
        return new Vector3(pos.x + x, pos.y + y,0);
    }

    private void spawnMonster(int wave)
    {
        for(int i = 1; i <= infoWave[wave - 1, 0];i++)
        {
            Instantiate(monsterLevel1, randomPos(), Quaternion.identity);
        }
        for (int i = 1; i <= infoWave[wave - 1, 1]; i++)
        {
            Instantiate(monsterLevel2, randomPos(), Quaternion.identity);
        }
        for (int i = 1; i <= infoWave[wave - 1, 2]; i++)
        {
            Instantiate(monsterLevel3, randomPos(), Quaternion.identity);
        }
        for (int i = 1; i <= infoWave[wave - 1, 3]; i++)
        {
            Instantiate(monsterLevel4, randomPos(), Quaternion.identity);
        }
        for (int i = 1; i <= infoWave[wave - 1, 4]; i++)
        {
            Instantiate(monsterBoss, randomPos(), Quaternion.identity);
        }
    }

    private Vector3 randomPos()
    {
        Vector2 pos = new Vector2(28,4); //33 7
        float x = Random.Range(-3.0f, 3.0f);
        float y = Random.Range(-5.0f, 5.0f);
        return new Vector3(pos.x + x, pos.y + y, 0);
    }

    public void OnCLickPanelEquipment()
    {
        if (!panel_equipment.activeInHierarchy)
        {
            panel_equipment.SetActive(true);
            panel_black.SetActive(true);
            RectTransform rt = panel_equipment.GetComponent<RectTransform>();
            // rt.position = new Vector3(0, -600, rt.position.z);
            LeanTween.move(rt, new Vector3(0f, 0f, 0f), 0.2f);
        }
        else
        {
            RectTransform rt = panel_equipment.GetComponent<RectTransform>();
            // rt.position = new Vector3(0, -600, rt.position.z);
            LeanTween.move(rt, new Vector3(0f, -600f, 0f), 0.2f).setOnComplete(()=> {
                panel_equipment.SetActive(false);
                panel_black.SetActive(false);
            });
        }
    }
}
