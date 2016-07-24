using UnityEngine;
using System.Collections;
using live2d;

public class Live2DObject : MonoBehaviour {
    private ALive2DModel live2DModel;
    private Live2DMotion motion;
    private MotionQueueManager motionMgr;
    private LAppModelProxy modelManager;
    private int HP = 100;
    public TextAsset motionFile;
    public GameObject target;

    public TextAsset AttackMotion;
    private Live2DMotion AtkMotion;
    private Coroutine attack;
    private int Damage;

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            motionMgr.startMotion(AtkMotion);
            if(!StatusManager.instance.DealHP(Damage))
                break;
        }
    }
    void Start()
    {
        modelManager = GetComponent<LAppModelProxy>();
        live2DModel = modelManager.GetModel().getLive2DModel();
        motionMgr = new MotionQueueManager();
        motion = Live2DMotion.loadMotion(motionFile.bytes);

        target = GameObject.FindGameObjectWithTag("Player");
        AtkMotion = Live2DMotion.loadMotion(AttackMotion.bytes);
        attack = StartCoroutine(Attack());

        int Stage = PlayerPrefs.GetInt("stage", 1);
        Damage = 20 + (5*Stage);
        HP = 100 + (10*Stage);
    }

    public void DealDamage(int Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            StopCoroutine(attack);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 myPos = transform.position;
        Vector3 relativePos = targetPos-  myPos;
        transform.rotation = Quaternion.LookRotation(relativePos);
        transform.Rotate(new Vector3(-75, 180, 0));
        Vector3 pos = transform.position;
        pos.y = 81;
        this.transform.position = pos;
        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }

        if (motionMgr.isFinished())
        {
            motionMgr.startMotion(motion);
        }

        motionMgr.updateParam(live2DModel);

        live2DModel.update();
    }


    void OnRenderObject()
    {
        if (live2DModel == null) return;
        live2DModel.draw();
    }
}
