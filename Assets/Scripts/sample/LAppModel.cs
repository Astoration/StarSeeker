using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using live2d.framework;
using live2d;


public class LAppModel :L2DBaseModel
{
    private LAppModelProxy parent;
    private LAppView view;

    
    private String modelHomeDir;
    private ModelSetting modelSetting = null;	

	private Matrix4x4 matrixHitArea; 
    

    
    private AudioSource asVoice;

    System.Random rand = new System.Random();

    private Bounds bounds;




    public LAppModel(LAppModelProxy p)
    {
        if (isInitialized()) return;
        parent = p;
		
		if (parent.GetComponent<AudioSource>() != null)
		{
			asVoice = parent.gameObject.GetComponent<AudioSource>();
            asVoice.playOnAwake = false;
        }
        else
        {
            if (LAppDefine.DEBUG_LOG)
            {
                Debug.Log("Live2D : AudioSource Component is NULL !");
            }
        }

        bounds = parent.GetComponent<MeshFilter>().sharedMesh.bounds;

        view = new LAppView(this, parent.transform);
        view.StartAccel();


        //if (LAppDefine.DEBUG_LOG) mainMotionManager.setMotionDebugMode(true);
    }


    public void LoadFromStreamingAssets(String dir,String filename)
    {
        if (LAppDefine.DEBUG_LOG) Debug.Log(dir + filename);
        modelHomeDir = dir;
		var data = Live2DFramework.getPlatformManager().loadString(modelHomeDir + filename);
        Init(data);
    }
   

    
    public void Init(String modelJson)
    {
        updating = true;
        initialized = false;

        modelSetting = new ModelSettingJson(modelJson);

        if (LAppDefine.DEBUG_LOG) Debug.Log("Start to load model");

        // Live2D Model
        if (modelSetting.GetModelFile() != null)
        {
            loadModelData(modelHomeDir + modelSetting.GetModelFile());

            var len = modelSetting.GetTextureNum();
            for (int i = 0; i < len; i++)
            {
                loadTexture(i, modelHomeDir + modelSetting.GetTextureFile(i));
            }
        }
       
      

        
        for (int i = 0; i < modelSetting.GetInitParamNum(); i++)
        {
            string id = modelSetting.GetInitParamID(i);
            float value = modelSetting.GetInitParamValue(i);
            live2DModel.setParamFloat(id, value);
        }

        for (int i = 0; i < modelSetting.GetInitPartsVisibleNum(); i++)
        {
            string id = modelSetting.GetInitPartsVisibleID(i);
            float value = modelSetting.GetInitPartsVisibleValue(i);
            live2DModel.setPartsOpacity(id, value);
        }

        

        view.SetupView(
            live2DModel.getCanvasWidth(),
            live2DModel.getCanvasHeight());

        updating = false;
        initialized = true;
    }


    
    public void Update()
    {
        if ( ! isInitialized() || isUpdating())
        {
            return;
        }


        view.Update(Input.acceleration);
        if (live2DModel == null)
        {
            if (LAppDefine.DEBUG_LOG) Debug.Log("Can not update there is no model data");
            return;
        }

        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }
        
        
        live2DModel.update();
    }


    public void Draw()
    {
        Matrix4x4 planeLocalToWorld = parent.transform.localToWorldMatrix;

        
        Matrix4x4 rotateModelOnToPlane = Matrix4x4.identity;
        rotateModelOnToPlane.SetTRS(Vector3.zero, Quaternion.Euler(90, 0, 0), Vector3.one);

        Matrix4x4 scale2x2ToPlane = Matrix4x4.identity;
        
        Vector3 scale = new Vector3(bounds.size.x / 2.0f, -1, bounds.size.z / 2.0f);
        scale2x2ToPlane.SetTRS(Vector3.zero, Quaternion.identity, scale);

        
        Matrix4x4 modelMatrix4x4 = Matrix4x4.identity;
        float[] matrix = modelMatrix.getArray();
        for (int i = 0; i < 16; i++)
        {
            modelMatrix4x4[i] = matrix[i];
        }

        Matrix4x4 modelCanvasToWorld = planeLocalToWorld * scale2x2ToPlane * rotateModelOnToPlane * modelMatrix4x4;

        GetLive2DModelUnity().setMatrix(modelCanvasToWorld);

        live2DModel.draw();

		matrixHitArea = modelCanvasToWorld;
    }


    
    public void DrawHitArea()
    {
        
        int len = modelSetting.GetHitAreasNum();
        for (int i = 0; i < len; i++)
        {
            string drawID = modelSetting.GetHitAreaID(i);
            float left = 0;
            float right = 0;
            float top = 0;
            float bottom = 0;

            if (!getSimpleRect(drawID, out left, out right, out top, out bottom))
            {
                continue;
            }

			HitAreaUtil.DrawRect(matrixHitArea,left, right, top, bottom);
        }
    }

    public Live2DModelUnity GetLive2DModelUnity()
    {
        return (Live2DModelUnity)live2DModel;
    }


    public Bounds GetBounds()
    {
        return bounds;
    }

    internal void TouchesBegan(Vector3 inputPos)
    {
        view.TouchesBegan(inputPos);
    }

    internal void TouchesMoved(Vector3 inputPos)
    {
        view.TouchesMoved(inputPos);
    }

    internal void TouchesEnded(Vector3 inputPos)
    {
        view.TouchesEnded(inputPos);
    }
}
