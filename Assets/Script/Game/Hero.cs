using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Hero : MonoBehaviour
{
    public bool Stuck;
    Vector3 JumpResetPos;
    int Sup;
    float Super;
    public Sound sound;
    public ParticleSystem WalkDust;
    ParticleSystem.EmissionModule D;
    public ParticleSystem JumpDust;
    public ParticleSystem EatDust;
    public ParticleSystem BreathDust;
    public ParticleSystem Blood;
    public Image HookGauge;

    public int LimitFood;
    public Text FoodText;

    public UI ui;
    public GameObject HurtFire;
    public Image I;
    public GameObject MiniMap;
    [HideInInspector]
    public string Activation = null;
    GameObject ActivationObj;
    public GameObject Fire;
    public GameObject Rope;
    public Transform RideRope;
    [HideInInspector]
    public float RopePos;
    [HideInInspector]
    public float LastRopePos;
    public Text HpText;

    int CampFire;
    float AlphaTime;
    Vector2 Dir;
    GameObject curHook;
    //AnimatorClipInfo[] Animation; //Animation[0].clip.name.Contains("Name");
    Rigidbody2D r;
    Animator Ani;
    RaycastHit2D Hit;
    float Speed = 4;
    int Jump = 0;
    public int Hp = 3;
    float ChangeColorTime = 0.0f;

    bool NotPressed()
    {
        return (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow));
    }

    void NotMove()
    {
        if (r.linearVelocity.x != 0)
        {
            Vector3 vel = r.linearVelocity;
            vel.x = 0;
            r.linearVelocity = vel;
        }
    }

    public void SetIdle()
    {
        if (!Stuck || GetComponent<SpriteRenderer>().color.b > 0 || !Ani.GetBool("Hurt"))
        {
            if (HurtFire.activeInHierarchy)
                HurtFire.SetActive(false);
            if (!Fire.activeInHierarchy)
                Fire.SetActive(true);
        }
        ResetAll();
        NotMove();
        SetAni("Idle");
    }

    public void ResetAll()
    {
        ResetAni("Idle");
        ResetAni("Walk");
        ResetAni("Jump");
        ResetAni("Ride");
    }

    void ShowParticle()
    {
        if (Jump == 0 && D.rateOverDistanceMultiplier != 1.1f)
            D.rateOverDistanceMultiplier = 1.1f;
        else if (Jump != 0 && D.rateOverDistanceMultiplier != 0f)
            D.rateOverDistanceMultiplier = 0f;
    }

    private void Awake()
    {
        Stuck = false;
        Clear.ClearTime = 0;
        Sup = 0;
        Super = 0.0f;
        AlphaTime = 1f;
        CampFire = 0;
        HpText.text = Hp.ToString();
        D = WalkDust.emission;
        JumpDust.Stop();
        BreathDust.Stop();
        LimitFood = 3;
        FoodText.text = ": " + LimitFood.ToString();
        JumpResetPos = JumpDust.transform.position;
    }

    void ChangeColor()
    {
        if (Sup == 1)
        {
            ChangeColorTime += Time.deltaTime;
            if (ChangeColorTime >= 1.0f)
            {
                ChangeColorTime = 0.0f;
                Fire.SetActive(true);
                HurtFire.SetActive(false);
                Ani.SetBool("Hurt", false);
                Sup = 2;
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 0);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Hit = Physics2D.Raycast(transform.position, Vector3.up);
        Ani = GetComponent<Animator>();
        r = GetComponent<Rigidbody2D>();
        //Animation = Ani.GetCurrentAnimatorClipInfo(0);
    }

    void CampCook()
    {
        if (CampFire == 1)
        {
            I.color = new Color(I.color.r, I.color.g, I.color.b, I.color.a + AlphaTime * Time.deltaTime);
            if (I.color.a >= 0.0f && I.color.a <= 0.1f)
            {
                sound.SoundPlay(Sound.SOUND.COOK);
            }
            if (I.color.a >= 1.2f)
            {
                CampFire = 2;
                Destroy(ActivationObj);
                ActivationObj = null;
            }
        }
        if (CampFire == 2)
        {
            I.color = new Color(I.color.r, I.color.g, I.color.b, I.color.a - AlphaTime * Time.deltaTime);
            if (I.color.a <= 0)
            {
                I.gameObject.SetActive(false);
                CampFire = 0;
                SetIdle();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UI.Stop)
        {
            SetIdle();
        }
        if (!UI.Stop)
        {
            if (HookGauge.fillAmount != 0)
                HookGauge.fillAmount -= 0.01f;
            if (Door.Playing != 0 && Door.Playing != 3)
            {
                if (!Door.Open)
                {
                    if(Sup == 1)
                    {
                        if (!HurtFire.activeInHierarchy)
                            HurtFire.SetActive(true);
                        if (Fire.activeInHierarchy)
                            Fire.SetActive(false);
                    }
                    if (Sup == 2)
                    {
                        Super += Time.deltaTime;
                        if (Super >= 2.0f)
                        {
                            Super = 0.0f;
                            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
                            Sup = 0;
                        }
                    }
                    if (ui.O2.value <= 0 || ui.Hunger.value <= 0)
                    {
                        SetHp();
                    }
                    if (Hp < 1)
                    {
                        Door.Open = true;
                    }
                    MapActive();
                    if (CampFire == 0)
                    {
                        if (!I.gameObject.activeInHierarchy)
                        {
                            if (!Stuck)
                            {
                                if (Input.GetKeyDown(KeyCode.Q))
                                {
                                    Hp = 5;
                                    HpText.text = Hp.ToString();
                                }
                                if (Input.GetKeyDown(KeyCode.W))
                                {
                                    Clear.ClearTime = 2;
                                }
                                ShowParticle();
                                Move();
                                if (Jump == 2 && !GetAni("Jump"))
                                {
                                    SetAni("Jump");
                                }
                            }
                            ChangeColor();
                        }
                    }
                    else
                    {
                        CampCook();
                    }
                }
                else
                {
                    if (CampFire == 0)
                    {
                        if (!I.gameObject.activeInHierarchy)
                            I.gameObject.SetActive(true);
                        I.color = new Color(I.color.r, I.color.g, I.color.b, I.color.a + AlphaTime * Time.deltaTime);
                        if (I.color.a > 1.3f)
                        {
                            if (Hp < 1)
                            {
                                Clear.isClear = false;
                                SceneManager.LoadScene("ResultScene");
                            }
                            else
                            {
                                if (Clear.ClearTime >= 3)
                                {
                                    SceneManager.LoadScene("BeforeScene");
                                }
                                else
                                {
                                    if (!Clear.Choose)
                                    {
                                        Clear.Choose = true;
                                        SceneManager.LoadScene("GameScene");
                                    }
                                    else
                                    {
                                        Clear.ClearTime++;
                                        Hp += 1;
                                        HpText.text = Hp.ToString();
                                    }
                                    sound.SoundPlay(Sound.SOUND.FIRE_STOP);
                                    ui.ChangeMap();
                                    SetIdle();
                                    if (transform.localScale.x < 0)
                                    {
                                        Dir = new Vector2(3, -1);
                                        Vector3 Scale = transform.localScale;
                                        Scale.x *= -1;
                                        transform.localScale = Scale;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (!I.gameObject.activeInHierarchy)
                    I.gameObject.SetActive(true);
                I.color = new Color(I.color.r, I.color.g, I.color.b, I.color.a - AlphaTime * Time.deltaTime);
                if (I.color.a <= 0)
                {
                    sound.SoundPlay(Sound.SOUND.FIRE);
                    I.gameObject.SetActive(false);
                    CampFire = 0;
                    Door.Playing = 0;
                }
            }
            Vector3 CameraPos = Camera.main.transform.position;
            float x = Mathf.Lerp(CameraPos.x, transform.position.x, 3f * Time.deltaTime);
            float y = Mathf.Lerp(CameraPos.y, transform.position.y, 3f * Time.deltaTime);
            Camera.main.transform.position = new Vector3(x, y, CameraPos.z);
        }
    }

    private void FixedUpdate()
    {
        if (!UI.Stop)
        {
            if (!I.gameObject.activeInHierarchy && !Door.Open && Door.Playing == 1)
            {
                if (!Stuck)
                {
                    PhysicsMove();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!UI.Stop)
        {
            if (!Door.Open && Door.Playing == 1 && CampFire == 0)
            {
                if (collision.CompareTag("Monster"))
                {
                    SetHp();
                }
                if (collision.CompareTag("Fire"))
                {
                    sound.SoundPlay(Sound.SOUND.ON_FIRE);
                    sound.SoundPlay(Sound.SOUND.FIRE_RESET);
                    for (int i = 0; i < ui.F.Length - 1; i++)
                    {
                        ui.F_2[i].startSizeMultiplier = collision.gameObject.transform.GetChild(0).transform.GetChild(i).GetComponent<ParticleSystem>().main.startSizeMultiplier;
                    }
                    ui.F_2[ui.F.Length - 1].startSizeMultiplier = 2f;
                    ui.L.range = ui.L_2.range = 16;
                }
                if (collision.CompareTag("Camp"))
                {
                    if (LimitFood > 0)
                    {
                        ActivationObj = collision.gameObject;
                        SetIdle();
                        CampFire = 1;
                        I.gameObject.SetActive(true);
                        LimitFood--;
                        FoodText.text = ": " + LimitFood.ToString();
                    }
                }
                if (collision.CompareTag("Tank"))
                {
                    ui.O2.value += 0.33f;
                    if (ui.O2.value > 1)
                    {
                        ui.O2.value = 1;
                    }
                    BreathDust.Play();
                    sound.SoundPlay(Sound.SOUND.BREATH);
                    Destroy(collision.gameObject);
                }
                if (collision.CompareTag("Food"))
                {
                    ui.Hunger.value += 0.33f;
                    if (ui.Hunger.value > 1)
                    {
                        ui.Hunger.value = 1;
                    }
                    Hp += 1;
                    HpText.text = Hp.ToString();
                    EatDust.Play();
                    sound.SoundPlay(Sound.SOUND.EAT);
                    Destroy(collision.gameObject);
                }
                if (collision.CompareTag("RopeCol"))
                {
                    if (collision.GetComponent<Hook>().done == 3)
                    {
                        sound.SoundPlay(Sound.SOUND.TAKEHOOK);
                        HookGauge.fillAmount = 0f;
                        ResetAll();
                        SetAni("Jump");
                        r.gravityScale = 1f;
                        Jump = 2;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!UI.Stop)
        {
            if (!Door.Open && Door.Playing == 1 && CampFire == 0)
            {
                if (collision.collider.CompareTag("Spike"))
                {
                    SetHp();
                }
                if (collision.collider.CompareTag("Monster"))
                {
                    SetHp();
                }
                if (collision.collider.CompareTag("Tile"))
                {
                    sound.SoundPlay(Sound.SOUND.JUMP);
                    Jump = 0;
                    r.linearVelocity = new Vector3(r.linearVelocity.x, 0, 0);
                    SetIdle();
                    Vector3 Pos = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, JumpResetPos.z);

                    Pos = Pos - JumpResetPos;
                    float Angle = Mathf.Atan2(Pos.y, Pos.x) * Mathf.Rad2Deg - 90;

                    JumpDust.transform.position = new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y, JumpResetPos.z);

                    JumpDust.transform.rotation = Quaternion.Euler(0, 0, Angle);
                    JumpDust.Play();
                }
            }
        }
    }

    void MapActive()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            MiniMap.GetComponent<RectTransform>().localPosition = new Vector3(0, -10, 0);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MiniMap.GetComponent<RectTransform>().localPosition = new Vector3(-2007, -10, 0);
        }
    }

    void Jumpping()
    {
        Hit = Physics2D.Raycast(transform.position, Vector3.up);
        if (GetAni("Ride") && Hit.transform.gameObject.layer == 8)
        {
            Jump = 0;
            SetIdle();
            if (HurtFire.activeInHierarchy)
                HurtFire.SetActive(false);
            if (!Fire.activeInHierarchy)
                Fire.SetActive(true);
        }
        if (Hit.transform.gameObject.layer == 8 && Jump == 2)
        {
            r.linearVelocity = new Vector3(r.linearVelocity.x, 0, 0);
            Jump = 0;
            SetIdle();
        }
        if (Jump == 2 && Hit.transform.gameObject.layer == 9)
        {
            if (!GetAni("Ride") && GetAni("Jump"))
            {
                Fire.SetActive(false);
                HurtFire.SetActive(true);
                Jump = 3;
                ResetAll();
                r.linearVelocity = new Vector3(0, 0, 0);
                SetAni("Ride");
                RideRope.position = transform.position;
                transform.position = RideRope.position;
            }
        }
        if (Hit.transform.gameObject.layer == 0 && Jump == 0 && !GetAni("Jump"))
        {
            Jump = 2;
            SetAni("Jump");
        }
        if (Hit.transform.gameObject.layer == 8 && Jump == 3)
        {
            r.linearVelocity = new Vector3(r.linearVelocity.x, 0, 0);
            Jump = 0;
            SetIdle();
        }
    }

    void SetAni(string Name)
    {
        Ani.SetTrigger(Name);
    }

    void ResetAni(string Name)
    {
        Ani.ResetTrigger(Name);
    }

    bool GetAni(string Name)
    {
        return Ani.GetCurrentAnimatorStateInfo(0).IsName(Name);
    }

    public void SetHp()
    {
        if (CampFire == 0 && Door.Playing == 1)
        {
            if (Sup == 0)
            {
                if (Hp > 0)
                {
                    if (!Ani.GetBool("Hurt"))
                    {
                        Sup = 1;
                        Blood.Play();
                        Ani.SetBool("Hurt", true);
                        Fire.SetActive(false);
                        HurtFire.SetActive(true);
                        sound.SoundPlay(Sound.SOUND.HIT);
                        Hp -= 1;
                        HpText.text = Hp.ToString();
                        GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                    }
                }
            }
        }
    }

    void Move()
    {
        if (Jump != 3)
        {
            if (!GetAni("Idle") && !GetAni("Jump") && Jump == 0 && NotPressed())
            {
                NotMove();
                ResetAni("Walk");
                SetAni("Idle");
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !GetAni("Jump") && Jump == 0)
            {
                Jump = 1;
                SetAni("Jump");
            }
            if (Input.GetKeyDown(KeyCode.Z) && Jump == 0 && HookGauge.fillAmount == 0)
            {
                HookGauge.fillAmount = 1f;
                sound.SoundPlay(Sound.SOUND.ROPE);
                Vector2 destiny = new Vector2(transform.position.x, transform.position.y + 50.0f);

                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, 0);
                curHook = Instantiate(Rope, pos, Quaternion.identity);
                curHook.transform.SetParent(ui.Map[ui.MapIndex].transform);
                curHook.GetComponent<Hook>().des = destiny;
            }
            Jumpping();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            {
                r.linearVelocity = Vector3.zero;
                Jump = 1;
                Fire.SetActive(true);
                HurtFire.SetActive(false);

                SetAni("Jump");
            }
        }
        if (Hp < 1)
        {
            Clear.isClear = false;
            if (!I.gameObject.activeInHierarchy)
                I.gameObject.SetActive(true);
        }
    }

    void PhysicsMove()
    {
        if (Jump < 3)
        {
            if (r.gravityScale != 1)
                r.gravityScale = 1;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (Hit.transform.gameObject.layer == 8)
                    sound.SoundPlay(Sound.SOUND.WALK);
                if (!GetAni("Walk") && !GetAni("Jump") && Jump == 0)
                {
                    SetAni("Walk");
                }
                if (transform.localScale.x > 0)
                {
                    Dir = new Vector2(-3, -1);
                    Vector3 Scale = transform.localScale;
                    Scale.x *= -1;
                    transform.localScale = Scale;
                }
                Vector3 vel = r.linearVelocity;
                vel.x = -Speed;
                r.linearVelocity = vel;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (Hit.collider != null)
                    if (Hit.transform.gameObject.layer == 8)
                        sound.SoundPlay(Sound.SOUND.WALK);
                if (!GetAni("Walk") && !GetAni("Jump") && Jump == 0)
                {
                    SetAni("Walk");
                }
                if (transform.localScale.x < 0)
                {
                    Dir = new Vector2(3, -1);
                    Vector3 Scale = transform.localScale;
                    Scale.x *= -1;
                    transform.localScale = Scale;
                }
                Vector3 vel = r.linearVelocity;
                vel.x = Speed;
                r.linearVelocity = vel;
            }
            else
            {
                NotMove();
            }
            if (Jump == 1)
            {
                r.AddForce(Vector2.up * 200.0f);
                Jump = 2;
            }
        }
        else if (Jump == 3)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (transform.position.y < RopePos)
                {
                    Vector3 vel = r.linearVelocity;
                    vel.y = (Speed * 0.5f);
                    r.linearVelocity = vel;
                    RideRope.position = transform.position;
                }
                else
                {
                    NotMove();
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (transform.position.y > LastRopePos)
                {
                    Vector3 vel = r.linearVelocity;
                    vel.y = -(Speed * 0.5f);
                    r.linearVelocity = vel;
                    RideRope.position = transform.position;
                }
                else
                {
                    Jump = 0;
                    SetIdle();
                }
            }
            else
            {
                NotMove();
                if (r.gravityScale != 0)
                    r.gravityScale = 0;
                transform.position = RideRope.position;
            }
        }
        else if (Jump == 4)
        {
            r.AddForce(Dir, ForceMode2D.Impulse);
            Jump = 5;
        }
    }
}
