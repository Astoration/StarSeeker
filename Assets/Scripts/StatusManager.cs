using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatusManager : MonoBehaviour {
    public static StatusManager instance;
    public Image HPMask;
    public Image MPMask;
    private int Atk = 0;
    private int Def = 0;
    private int Element = 0;
    private float HPhealPerSecond=0;
    private float MPhealPerSceond=0;
    private const float BarLength = 238f;
    public GameObject HitEffect;
    void Awake()
    {
        if (!instance)
            instance = this;
    }
    private float HP=0;
    private float MAX_HP;
    private float MP=0;
    private float MAX_MP;
    private Coroutine hitCoroutine;
    public bool isHit = false;
    IEnumerator Hit()
    {
        isHit = true;
        HitEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        HitEffect.SetActive(false);
        isHit = false;
        StopCoroutine(hitCoroutine);
    }
    void ApplyBar()
    {
        HPMask.rectTransform.sizeDelta= new Vector2(BarLength * (HP / MAX_HP),HPMask.rectTransform.sizeDelta.y);
        MPMask.rectTransform.sizeDelta = new Vector2(BarLength * (MP / MAX_MP),HPMask.rectTransform.sizeDelta.y);
    }

    public void HealHP(float HealAmount)
    {
        HP += HealAmount;
        if (MAX_HP < HP)
            HP = MAX_HP;
        ApplyBar();
    }
    public void HealMP(float HealAmount)
    {
        MP += HealAmount;
        if (MAX_MP < MP)
            MP = MAX_MP;
        ApplyBar();
    }

    public bool DealHP(float DealAmount)
    {
        if(!isHit)
            hitCoroutine=StartCoroutine(Hit());
        HP -= DealAmount;
        if (HP <= 0)
        {
            HP = 0;
            ApplyBar();
            SceneManager.LoadScene("MainScene");
            return false;
        }
        ApplyBar();
        return true;
    }
    public bool DealMP(float DealAmount)
    {
        if (MP < DealAmount)
        {
            return false;
        }
        MP -= DealAmount;
        ApplyBar();
        return true;
    }
    // Use this for initialization
    void Start () {
        MAX_HP = 150;
        MAX_MP = 100;
        HP = MAX_HP;
        MP = MAX_MP;
        HPhealPerSecond = MAX_HP / 100;
        MPhealPerSceond = MAX_MP / 100;
	}
	
	// Update is called once per frame
	void Update () {
        HealHP(HPhealPerSecond*Time.deltaTime);
        HealMP(MPhealPerSceond*Time.deltaTime);
	}
}
