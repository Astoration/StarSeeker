using UnityEngine;
using System.Collections;
using live2d;

public class Live2DPlayer : MonoBehaviour {
    private ALive2DModel live2DModel;
    private Live2DMotion motion;
    private MotionQueueManager motionMgr;
    private LAppModelProxy modelManager;

    public TextAsset[] motionFiles;
    public TextAsset motionFile;
    private Live2DMotion[] motions = new Live2DMotion[5];

    void Start()
    {
        modelManager = GetComponent<LAppModelProxy>();
        live2DModel = modelManager.GetModel().getLive2DModel();
        motionMgr = new MotionQueueManager();
        motion = Live2DMotion.loadMotion(motionFile.bytes);
        int i = 0;
        foreach(TextAsset t in motionFiles)
        {
            motions[i] = Live2DMotion.loadMotion(motionFiles[i].bytes);
            i++;
        }
        InvokeRepeating("RandomMotion", 10, 10);
    }
    
    void RandomMotion()
    {
        int rand = Random.Range(0, 5);
        motionMgr.startMotion(motions[rand]);
    }
    void Update()
    {
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
