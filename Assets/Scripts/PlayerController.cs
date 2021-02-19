using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    //
    public int coint;
    public int HPMax;
    private int HP;
    public int EXP;
    public int ARMORMax;
    public int ARMOR;

    public int type_weapon;
    public int type_armor;

    //UI
    public Slider slideHP;
    public Slider slideEXP;
    public Slider slideARMOR;
    public Text text_BtnTask;

    //Cntroller
    public Joystick joystick;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    public float runSpeed = 40f;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector3 m_Velocity = Vector3.zero;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool isStickAttack;


    //GAME
    public GameObject currentGame;
    public GameObject wateringFlower;
    public GameObject pickingWarm;
    public GameObject cooking;

    void Start()
    {
        if (instance == null) instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        slideHP.value = 1;
        slideEXP.value = 0;
        HP = HPMax;
        ARMORMax = 6;
        ARMOR = 3;
        slideARMOR.value = (float)ARMOR / (float)ARMORMax;
        isStickAttack = false;
        text_BtnTask.text = "ATTACK";
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        moveVelocity = moveInput.normalized * runSpeed;
        if (HP <= 0)
        {
            endGame();
        }
    }

    public void endGame()
    {
        Debug.Log("endgame");
        Time.timeScale = 0;
    }

    public void takeDamage(int hp)
    {
        if (ARMOR <= 0)
        {
            HP -= hp;
            slideHP.value = (float)HP / (float)HPMax;
            LeanTween.color(gameObject, Color.red, 0.2f).setOnComplete(() => {
                LeanTween.color(gameObject, Color.white, 0.1f);
            });
        }
        else
        {
            ARMOR -= 1;
            slideARMOR.value = (float)ARMOR / (float)ARMORMax;
            LeanTween.color(gameObject, Color.red, 0.2f).setOnComplete(() => {
                LeanTween.color(gameObject, Color.white, 0.1f);
            });
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = moveVelocity;
        float speedX = Mathf.Abs(moveVelocity.x);

        animator.SetFloat("speed", speedX);

      //  rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity,ref m_Velocity, m_MovementSmoothing);
        rb.MovePosition(rb.position + targetVelocity * Time.fixedDeltaTime);
        if (targetVelocity.x > 0)
        {
            spriteRenderer.flipX = false;
            animator.SetBool("isFlip", false);
        }
        else if(targetVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
            animator.SetBool("isFlip", true);
        }
    }

    public void BtnDoTask()
    {
        if (currentGame)
        {
            currentGame.SetActive(true);
            RectTransform rectTransform = currentGame.GetComponent<RectTransform>();
            LeanTween.move(rectTransform, new Vector3(0f, 0f, 0f), 0.4f);
        }
    }

    public void BtnAttackUp()
    {
        if (text_BtnTask.text == "ATTACK")
        {
            switch (type_weapon)
            {
                case 0:
                    {
                        animator.SetBool("isBloomAttack", false);
                        break;
                    }
                case 1:
                    {
                        animator.SetBool("isKatanaAttack", false);
                        break;
                    }
                case 2:
                    {
                        animator.SetBool("isLBAttack", false);
                        break;
                    }
                default: break;
            }
        }
    }

    public void BtnAttackDowm()
    {
        if(text_BtnTask.text== "ATTACK")
        {
            switch (type_weapon)
            {
                case 0:
                    {
                        animator.SetBool("isBloomAttack", true);
                        isStickAttack = true;
                        break;
                    }
                case 1:
                    {
                        animator.SetBool("isKatanaAttack", true);
                        isStickAttack = true;
                        break;
                    }
                case 2:
                    {
                        animator.SetBool("isLBAttack", true);
                        isStickAttack = true;
                        break;
                    }
                default: break;
            }
            Debug.Log("BtnAttackDowm");
            
        }
      
    }
/*
    IEnumerator attack()
    {
        yield return new WaitForSeconds(0.4f);
        if(!animator.GetBool("isStickAttack")) isStickAttack = false;
    }
*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EXP")
        {
            LeanTween.move(collision.gameObject, transform.position, 0.2f).setOnComplete(() => {
                Destroy(collision.gameObject);
                EXP += 1;
                slideEXP.value = (float)EXP / 6f;
            });
        }
        if (collision.tag == "HP")
        {
            LeanTween.move(collision.gameObject, transform.position, 0.2f).setOnComplete(() => {
                Destroy(collision.gameObject);
                HP += 1;
                slideHP.value = (float)HP / 6f;
            });
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "watering")
        {
            Debug.Log("watering");
            currentGame = wateringFlower;
            text_BtnTask.text = "watering";
        }
        else
        if (collision.tag == "pickingworm")
        {
            Debug.Log("pickingworm");
            currentGame = pickingWarm;
            text_BtnTask.text = "pickingworm";
        }
        else
        if (collision.tag == "cooking")
        {
            Debug.Log("cooking");
            currentGame = cooking;
            text_BtnTask.text = "cooking";
        }
        /* if (collision.tag == "enemy")
         {
         }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "pickingworm" || collision.tag == "watering" || collision.tag == "cooking")
        {
            currentGame = null;
            text_BtnTask.text = "ATTACK";
        }
    }
}
