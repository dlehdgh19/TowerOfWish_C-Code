using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacterController : MonoBehaviour
{
    [SerializeField]
    public static float speed = 4.0f;
    [SerializeField]
    private float jumpForce = 8.0f;

    [SerializeField]
    private AudioSource[] Audio;

    [SerializeField]
    private GameObject[] WalkRunAudio;

    [SerializeField]
    private GameObject[] DeadAndTextAudio;

    public Text DeadText;
    public SpriteRenderer BlackScreenSR;

    public static bool Resetbool;
    public Transform MainCameraTransform;
    

    /*  오디오 리스트
      
        Audio[0] = Slash1
        Audio[1] = Slash2
        Audio[2] = Jump
        Audio[3] = Dash
        Audio[4] = Slash3
    */



    private Rigidbody2D rigid2D;         // 캐릭터 좌우반전, 이동, 점프 한번 가능, 콤보공격, 지면충돌처리 선언
    private SpriteRenderer rend;
    private float x;
    private Animator animator;
    private bool runbool = false;
    private bool runChangebool = false;
    private bool slash1bool = false;
    private bool WhenAttackbool = false;
    private int AttackClickInt = 0;
    private bool Attackbool = true;
    public static bool isgrounded;
    private bool dashbool = false;
    private bool CantDashbool = false;
    public static bool Dashingbool = false;
    public static float dashcooldown = 3f;
    public static float dashspeed = 9;
    private float dashTime;
    public static float DashingTime = 0;
    public static bool canjumptwicebool = false;

    public static float healtime;
    private float Healcooldowntime = 30;
    private bool Healbool;
    private bool CannotHealYetbool;
    private bool SpawnHealNPCbool;

    public SpriteRenderer Slash1Rend;   // 공격1,2,3 충돌처리 active or deactive
    public SpriteRenderer Slash2Rend;
    public SpriteRenderer Slash3Rend;
    public GameObject Slash1;
    public GameObject Slash2;
    public GameObject Slash3;

    public static bool cameraStopbool = false;

    public static bool CharacterCantMovebool = false;

    public GameObject HealNPCPrefab;

    public static bool Hurtbool1;
    public static bool Hurtbool2;
    public static bool Hurtbool3;
    public static bool Hurtbool4;
    public static bool Hurtbool5;
    public static bool Hurtbool6;
    public static bool Hurtbool7;
    private float hurtTime;

    private bool Deadbool;
    public static float DeadTime;
    public static Vector3 RevivePosition;
    // Start is called before the first frame update
    private void Awake()
    {
        
        rend = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();

        Slash1.transform.localPosition = new Vector3(0.3232f, -0.0418f, 0);
        Slash2.transform.localPosition = new Vector3(0.3468f, -0.0337f, 0);
        Slash3.transform.localPosition = new Vector3(0.287f, 0.0342f, 0);

        WalkRunAudio[0].SetActive(false);
        WalkRunAudio[1].SetActive(false);

        healtime = 0;
        Healbool = false;
        CannotHealYetbool = false;

        Hurtbool1 = false;
        Hurtbool2 = false;
        Hurtbool3 = false;
        Hurtbool4 = false;
        Hurtbool5 = false;
        hurtTime = 0;

        Slash1.SetActive(false);
        Slash2.SetActive(false);
        Slash3.SetActive(false);
        DeadTime = 0;

        DeadAndTextAudio[0].SetActive(false);
        DeadAndTextAudio[1].SetActive(false);

        BlackScreenSR.color = new Color(0,0, 0, 0);
        Resetbool = false;

        Deadbool = false;
    }

    private void Start()
    {
        
    }

    
    // Update is called once per frame
    private void Update()
    {
        Physics2D.IgnoreLayerCollision(7, 18);
        Physics2D.IgnoreLayerCollision(8, 18);
        Physics2D.IgnoreLayerCollision(9, 18);

        Dead();

        if (CharacterCantMovebool == true && CharacterUI_HP_SkillCoolDown.Deadbool == false)
        {
            x = 0;
            animator.SetBool("RunBool", false);
            animator.SetBool("WalkBool", false);
            WalkRunAudio[0].SetActive(false);
            WalkRunAudio[1].SetActive(false);
            Audio[0].Stop();
            Audio[1].Stop();
            Audio[2].Stop();
            Audio[3].Stop();
            animator.SetInteger("JumpInt", 0);
            animator.SetInteger("DashInt", 0);
            rigid2D.gravityScale = 3.5f;
        }

        if(x == 0)
        {
            rigid2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }


        if (CharacterCantMovebool == false)
        {
            DeadTime = 0;

            if (rigid2D.velocity.y > 3f && isgrounded == false)
            {
                animator.SetInteger("JumpInt", 1);
            }
            if (rigid2D.velocity.y >= -3f && rigid2D.velocity.y <= 3f && isgrounded == false)
            {
                animator.SetInteger("JumpInt", 2);
            }
            if (rigid2D.velocity.y < -3f && isgrounded == false)
            {
                animator.SetInteger("JumpInt", 3);
            }

            if (rigid2D.velocity.y < -15f)
            {
                rigid2D.velocity = new Vector2(rigid2D.velocity.x, -15f);
            }
            if (WhenAttackbool == false)
            {
                x = Input.GetAxisRaw("Horizontal");
                rigid2D.velocity = new Vector2(x * speed, rigid2D.velocity.y);
                if (Input.GetKeyDown(KeyCode.C) && isgrounded == true)
                {
                    WalkRunAudio[0].SetActive(false);
                    WalkRunAudio[1].SetActive(false);
                    rigid2D.velocity = Vector2.up * jumpForce;
                    Audio[2].Play();
                }
                else if (canjumptwicebool == true && Input.GetKeyDown(KeyCode.C) && isgrounded == false)
                {
                    WalkRunAudio[0].SetActive(false);
                    WalkRunAudio[1].SetActive(false);
                    rigid2D.velocity = Vector2.up * (jumpForce * 0.9f);
                    Audio[2].Play();
                    canjumptwicebool = false;
                }
                if (Input.GetKeyDown(KeyCode.D) && CantDashbool == false && x != 0)
                {
                    Dash();
                    Audio[3].Play();
                }
            }

            if (isgrounded == true && PlayButtonManager.nPCProgressInt >= 3 && CannotHealYetbool == false && Healbool == false) // 힐 가능 타이밍 #######################
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Healbool = true;
                    
                }
            }

            if (Healbool == true)
            {
                healtime += Time.deltaTime;
                if (healtime > 0 && healtime < Healcooldowntime)
                {
                    CannotHealYetbool = true;
                    if (SpawnHealNPCbool == false)
                    {
                        Instantiate(HealNPCPrefab, new Vector3(transform.position.x, transform.position.y + 0.18f, transform.position.z), Quaternion.identity);
                        SpawnHealNPCbool = true;
                    }
                    
                }
                if (healtime >= Healcooldowntime)
                {
                    healtime = 0;
                    CannotHealYetbool = false;
                    SpawnHealNPCbool = false;
                    Healbool = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Combo();
            }

            Hurt();


            if (x < 0 && WhenAttackbool == false)
            {
                rend.flipX = true;
                Slash1.transform.localPosition = new Vector3(-0.3232f, -0.0418f, 0);
                Slash2.transform.localPosition = new Vector3(-0.3468f, -0.0337f, 0);
                Slash3.transform.localPosition = new Vector3(-0.287f, 0.0342f, 0);
                if (runbool == true && isgrounded == true)
                {
                    if (dashTime > 0 && dashTime < 0.6f)
                    {
                        WalkRunAudio[1].SetActive(false);
                        WalkRunAudio[0].SetActive(false);
                    }
                    if (dashTime == 0 || dashTime >= 0.6f)
                    {
                        WalkRunAudio[1].SetActive(true);
                        WalkRunAudio[0].SetActive(false);
                    }
                    speed = 7;
                    animator.SetBool("RunBool", true);
                    animator.SetBool("WalkBool", false);
                }
                if (runbool == false && isgrounded == true)
                {
                    if (dashTime > 0 && dashTime < 0.6f)
                    {
                        WalkRunAudio[1].SetActive(false);
                        WalkRunAudio[0].SetActive(false);
                    }
                    if (dashTime == 0 || dashTime >= 0.6f)
                    {
                        WalkRunAudio[0].SetActive(true);
                        WalkRunAudio[1].SetActive(false);
                    }
                    speed = 4f;
                    animator.SetBool("WalkBool", true);
                    animator.SetBool("RunBool", false);
                }
                if (isgrounded == false)
                {
                    WalkRunAudio[0].SetActive(false);
                    WalkRunAudio[1].SetActive(false);
                    animator.SetBool("WalkBool", false);
                    animator.SetBool("RunBool", false);
                    if (rigid2D.velocity.y > 2f)
                    {
                        animator.SetInteger("JumpInt", 1);
                    }
                    if (rigid2D.velocity.y >= -2f && rigid2D.velocity.y <= 2f)
                    {
                        animator.SetInteger("JumpInt", 2);
                    }
                    if (rigid2D.velocity.y < -2f)
                    {
                        animator.SetInteger("JumpInt", 3);
                    }
                    if (runbool == true)
                    {
                        speed = 7;
                    }
                    if (runbool == false)
                    {
                        speed = 4f;
                    }
                }

            }
            if (x > 0 && WhenAttackbool == false)
            {
                rend.flipX = false;
                Slash1.transform.localPosition = new Vector3(0.3232f, -0.0418f, 0);
                Slash2.transform.localPosition = new Vector3(0.3468f, -0.0337f, 0);
                Slash3.transform.localPosition = new Vector3(0.287f, 0.0342f, 0);
                if (runbool == true && isgrounded == true)
                {
                    if (dashTime > 0 && dashTime < 0.6f)
                    {
                        WalkRunAudio[1].SetActive(false);
                        WalkRunAudio[0].SetActive(false);
                    }
                    if (dashTime == 0 || dashTime >= 0.6f)
                    {
                        WalkRunAudio[1].SetActive(true);
                        WalkRunAudio[0].SetActive(false);
                    }
                    speed = 7;
                    animator.SetBool("RunBool", true);
                    animator.SetBool("WalkBool", false);
                }
                if (runbool == false && isgrounded == true)
                {
                    if (dashTime > 0 && dashTime < 0.6f)
                    {
                        WalkRunAudio[1].SetActive(false);
                        WalkRunAudio[0].SetActive(false);
                    }
                    if (dashTime == 0 || dashTime >= 0.6f)
                    {
                        WalkRunAudio[0].SetActive(true);
                        WalkRunAudio[1].SetActive(false);
                    }
                    speed = 4f;
                    animator.SetBool("WalkBool", true);
                    animator.SetBool("RunBool", false);
                }
                if (isgrounded == false || (canjumptwicebool == false && isgrounded == false))
                {
                    WalkRunAudio[0].SetActive(false);
                    WalkRunAudio[1].SetActive(false);
                    animator.SetBool("WalkBool", false);
                    animator.SetBool("RunBool", false);
                    if (rigid2D.velocity.y > 2f)
                    {
                        animator.SetInteger("JumpInt", 1);
                    }
                    if (rigid2D.velocity.y >= -2f && rigid2D.velocity.y <= 2f)
                    {
                        animator.SetInteger("JumpInt", 2);
                    }
                    if (rigid2D.velocity.y < -2f)
                    {
                        animator.SetInteger("JumpInt", 3);
                    }
                    if (runbool == true)
                    {
                        speed = 7;
                    }
                    if (runbool == false)
                    {
                        speed = 4f;
                    }
                }

            }
            if (x == 0 && WhenAttackbool == false)
            {
                WalkRunAudio[0].SetActive(false);
                WalkRunAudio[1].SetActive(false);
                speed = 0;
                animator.SetBool("WalkBool", false);
                animator.SetBool("RunBool", false);
                if (isgrounded == false || (canjumptwicebool == false && isgrounded == false))
                {

                    if (rigid2D.velocity.y > 2f)
                    {
                        animator.SetInteger("JumpInt", 1);
                    }
                    if (rigid2D.velocity.y >= -2f && rigid2D.velocity.y <= 2f)
                    {
                        animator.SetInteger("JumpInt", 2);
                    }
                    if (rigid2D.velocity.y < -2f)
                    {
                        animator.SetInteger("JumpInt", 3);
                    }
                }
            }

            if (dashbool == true) // 대쉬 쿨다임 동안 대쉬 재사용 불가
            {

                dashTime += Time.deltaTime;
                DashingTime += Time.deltaTime;

                if (DashingTime > 0 && DashingTime < dashcooldown)
                {
                    CantDashbool = true;
                }
                if (DashingTime > dashcooldown)
                {
                    CantDashbool = false;
                    dashTime = 0;
                    DashingTime = 0;
                    dashbool = false;
                }
                if (dashTime > 0 && dashTime < 0.6f)
                {

                    animator.SetInteger("DashInt", 1);
                    rigid2D.gravityScale = 0;
                    rigid2D.velocity = new Vector2(x * dashspeed, 0);
                }
                if (dashTime >= 0.6f)
                {
                    animator.SetInteger("DashInt", 0);
                    rigid2D.gravityScale = 3.5f;

                }
            }

            if(animator.GetInteger("DashInt") == 1)
            {
                Dashingbool = true;
            }
            if (animator.GetInteger("DashInt") == 0)
            {
                Dashingbool = false;
            }


            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                runbool = !runbool;
            }

        }
    }


    void Combo()    // 첫번째 공격 선언
    {
        if(Attackbool)
        {
            WalkRunAudio[0].SetActive(false);
            WalkRunAudio[1].SetActive(false);
            AttackClickInt++;
        }

        if(AttackClickInt == 1)
        {
            WhenAttackbool = true;
            animator.SetInteger("SlashInt", 1);
            animator.SetInteger("JumpInt", 0);
            Slash1.SetActive(true);
            Audio[0].Play();
        }
    }

    public void Verify_combo()   // 두번째, 세번째 공격 콤보 선언
    {
        Attackbool = false;
        animator.SetInteger("JumpInt", 0);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash1") && AttackClickInt == 1)
        {
            
            Slash1.SetActive(false);
            WhenAttackbool = false;
            animator.SetInteger("SlashInt", 0);
            Attackbool = true;
            AttackClickInt = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash1") && AttackClickInt >= 2)
        {
            Slash2.SetActive(true);
            Slash1.SetActive(false);
            animator.SetInteger("SlashInt", 2);
            Attackbool = true;
            Audio[4].Play();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash2") && AttackClickInt == 2)
        {
            
            Slash2.SetActive(false);
            WhenAttackbool = false;
            animator.SetInteger("SlashInt", 0);
            Attackbool = true;
            AttackClickInt = 0;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash2") && AttackClickInt >= 3)
        {
            Slash3.SetActive(true);
            Slash2.SetActive(false);
            animator.SetInteger("SlashInt", 3);
            Attackbool = true;
            Audio[1].Play();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash3"))
        {
            Slash3.SetActive(false);
            WhenAttackbool = false;
            animator.SetInteger("SlashInt", 0);
            Attackbool = true;
            AttackClickInt = 0;
        }
    }

    public void Dash()
    {
        dashbool = true;
    }

    public void Hurt()
    {
        if ((Hurtbool1 == true || Hurtbool2 == true || Hurtbool3 == true || Hurtbool4 == true || Hurtbool5 == true || Hurtbool6 == true || Hurtbool7 == true) && CharacterUI_HP_SkillCoolDown.HealthValue > 0)
        {
            hurtTime += Time.deltaTime;
            if (hurtTime > 0 && hurtTime < 0.1f)
            {
                rend.color = new Color(1,0,0,1);
            }
            if (hurtTime > 0.1f && hurtTime < 1.5f)
            {
                rend.color = new Color(1, 1, 1, 0.6f);
            }
            if (hurtTime > 1.5f)
            {
                rend.color = new Color(1, 1, 1, 1f);
                Hurtbool1 = false;
                Hurtbool2 = false;
                Hurtbool3 = false;
                Hurtbool4 = false;
                Hurtbool5 = false;
                Hurtbool6 = false;
                Hurtbool7 = false;
                hurtTime = 0;
                CharacterUI_HP_SkillCoolDown.Hurtingbool = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)    // 지면 닿음 시 점프가능
    {
        if(collision.collider.gameObject.CompareTag("Ground"))
        {
            canjumptwicebool = false;
            isgrounded = true;
            animator.SetInteger("JumpInt", 0);
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)    // 지면 닿음 시 점프가능
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            canjumptwicebool = false;
            isgrounded = true;
            animator.SetInteger("JumpInt", 0);
        }

    }

    private void OnCollisionExit2D(Collision2D collision)    // 지면 떨어짐 시 점프 불가능
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            
            canjumptwicebool = true;
            isgrounded = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "CameraStopCollider")
        {
            cameraStopbool = true;
        }
        if (other.tag == "Attack1")
        {
            Hurtbool1 = true;
        }
        if (other.tag == "Attack2")
        {
            Hurtbool2 = true;
        }
        if (other.tag == "Attack3")
        {
            Hurtbool3 = true;
        }
        if (other.tag == "Attack4")
        {
            Hurtbool4 = true;
        }
        if (other.tag == "Attack5")
        {
            Hurtbool5 = true;
        }
        if (other.tag == "Attack6")
        {
            Hurtbool6 = true;
        }
        if (other.tag == "Attack7")
        {
            Hurtbool7 = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "CameraStopCollider")
        {
            cameraStopbool = true;
        }
        if (other.tag == "Attack1")
        {
            Hurtbool1 = true;
        }
        if (other.tag == "Attack2")
        {
            Hurtbool2 = true;
        }
        if (other.tag == "Attack3")
        {
            Hurtbool3 = true;
        }
        if (other.tag == "Attack4")
        {
            Hurtbool4 = true;
        }
        if (other.tag == "Attack5")
        {
            Hurtbool5 = true;
        }
        if (other.tag == "Attack6")
        {
            Hurtbool6 = true;
        }
        if (other.tag == "Attack7")
        {
            Hurtbool7 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "CameraStopCollider")
        {
            cameraStopbool = false;
            CameraFollow.CameraControlbool = false;
        }

    }

    public void Dead()
    {
        if(CharacterUI_HP_SkillCoolDown.Deadbool == true)
        {
            Deadbool = true;
        }
        if(Deadbool == true)
        {
            WhenAttackbool = false;
            animator.SetInteger("SlashInt", 0);
            Attackbool = true;
            AttackClickInt = 0;
            DeadTime += Time.deltaTime;
            Slash1.SetActive(false);
            Slash2.SetActive(false);
            Slash3.SetActive(false);
            PlayButtonManager.CantPressPausebool = true;
            CharacterCantMovebool = true;
            rend.color = new Color(1, 1, 1, 1f);
            rigid2D.gravityScale = 3.5f;
            WalkRunAudio[0].SetActive(false);
            WalkRunAudio[1].SetActive(false);
            Audio[0].Stop();
            Audio[1].Stop();
            Audio[2].Stop();
            Audio[3].Stop();
            Audio[4].Stop();
            DeadAndTextAudio[0].SetActive(true);
            if (DeadTime > 0 && DeadTime < 0.5f)
            {
                animator.SetBool("DeadBool", true);
                animator.SetBool("RunBool", false);
                animator.SetBool("WalkBool", false);
                animator.SetInteger("JumpInt", 0);
                animator.SetInteger("DashInt", 0);
                
            }
            
            if (DeadTime > 3 && DeadTime < 6)
            {
                DeadAndTextAudio[1].SetActive(true);
                DeadText.color = new Color(1, 0, 0, (DeadTime - 3) * 0.333f);
            }
            if (DeadTime > 7 && DeadTime < 9)
            {
                BlackScreenSR.color = new Color(0, 0, 0, (DeadTime - 7) * 0.5f);
            }
            if (DeadTime > 9 && DeadTime < 9.2f)
            {
                CameraFollow.CameraControlbool = true;
                BlackScreenSR.color = new Color(0, 0, 0, 1);

                if (PlayerPrefs.HasKey("RevivePosition_X"))
                {
                    RevivePosition = new Vector3(PlayerPrefs.GetFloat("RevivePosition_X"), PlayerPrefs.GetFloat("RevivePosition_Y"), PlayerPrefs.GetFloat("RevivePosition_Z"));
                    PlayPortalManager.MapInt = ReviveStatue.ReviveMapInt;
                    this.transform.position = RevivePosition;
                    Resetbool = true;
                    CharacterUI_HP_SkillCoolDown.Revivebool = true;
                    CharacterUI_HP_SkillCoolDown.Deadbool = false;
                    MainCameraTransform.position = RevivePosition;
                }
                else if (!PlayerPrefs.HasKey("RevivePosition_X"))
                {
                    this.transform.position = new Vector3(320.11f, 97.76f, 0);
                    PlayPortalManager.MapInt = 12;
                    Resetbool = true;
                    CharacterUI_HP_SkillCoolDown.Revivebool = true;
                    CharacterUI_HP_SkillCoolDown.Deadbool = false;
                    MainCameraTransform.position = new Vector3(320.11f, 97.76f, -10);
                }

            }
            if (DeadTime > 9.2f && DeadTime < 9.3f)
            {
                CameraFollow.CameraControlbool = false;
                animator.SetBool("DeadBool", false);
                

            }
            if (DeadTime > 11f && DeadTime < 13f)
            {
                DeadText.color = new Color(1, 0, 0, 0);
                BlackScreenSR.color = new Color(0, 0, 0, (13 - DeadTime) * 0.5f);
            }
            if (DeadTime > 13 && DeadTime < 13.5f)
            {
                CharacterUI_HP_SkillCoolDown.Revivebool = false;
                CharacterUI_HP_SkillCoolDown.Hurtingbool = false;
                CharacterCantMovebool = false;
                DeadAndTextAudio[0].SetActive(false);
                DeadAndTextAudio[1].SetActive(false);
                Resetbool = false;
                PlayPortalManager.BGMPlayOncebool = false;
                PlaceTextManager.revealbool = true;
                PlaceTextManager.revealTime = 0;
                PlaceTextManager.AudioPlayOncebool = false;
                BlackScreenSR.color = new Color(0, 0, 0, 0);
                Hurtbool1 = false;
                Hurtbool2 = false;
                Hurtbool3 = false;
                Hurtbool4 = false;
                Hurtbool5 = false;
                Hurtbool6 = false;
                Hurtbool7 = false;
                PlayButtonManager.CantPressPausebool = false;
                Deadbool = false;
            }

            if(DeadTime > 13.5f)
            {
                Deadbool = false;
                DeadTime = 0;
            }
        }
    }
}
