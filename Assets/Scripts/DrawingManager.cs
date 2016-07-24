using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawingManager : MonoBehaviour {
    public Texture2D baseTexture;
    private Vector2 oldPos=Vector2.zero;
    private Vector2 newPos = Vector2.zero;
    public Texture2D[] MagicTexture;
    private Vector2 TouchStart = Vector2.zero;
    private Vector2 TouchEnd = Vector2.zero;
    public GameObject effectOne, effectTwo, character;
    public Image Aim;
    public GameObject canvas;
    private Ray ray;
    float OperationDensity(float x) {return 3 * Mathf.Pow(x * (x + 2) - (1 / 55), 3);}

    void Start () {
        ClearImage();

    }
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(new Vector2(Screen.width/2-(Screen.height/2), Screen.height/2 - (Screen.height / 2)), new Vector2(Screen.height,Screen.height)), baseTexture);
    }

    public void ClearImage()
    {
        ClearImage(baseTexture);
    }
    public float ImageCheck()
    {
        return (CompareImage(baseTexture, MagicTexture[0]) * (Mathf.Clamp01(OperationDensity(CheckDensity(baseTexture, MagicTexture[0]))))) * 100;
    }

    private void ClearImage(Texture2D Texture) {
        for(int i = 0; i < 600; i++)
        {
            for(int j = 0; j < 600; j++)
            {
                Texture.SetPixel(i, j, Color.clear);
            }
        }
        Texture.Apply();
    }
    private float CompareImage(Texture2D Texture, Texture2D TargetTexture)
    {
        int maxCount = 0;
        int compareCount = 0;
        for(int i = 0; i < 600; i++)
        {
            for(int j= 0; j < 600; j++)
            {
                if(Texture.GetPixel(i, j).a != 0)
                {
                    maxCount += 1;
                    if (TargetTexture.GetPixel(i, j).a != 0)
                    {
                        compareCount += 1;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        return ((float)compareCount / (float)maxCount);
    }

    public void Attack()
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
        bool check = false;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject.tag.Equals("Live2D"))
            {
                check = true;
                if (80 < ImageCheck())
                {
                    hit.collider.gameObject.GetComponent<Live2DObject>().DealDamage(100);
                    GameObject eff = Instantiate(effectTwo, Vector2.zero, Quaternion.identity) as GameObject;
                    eff.transform.SetParent(canvas.transform, false);
                    ClearImage();

                }
                else
                {
                    hit.collider.gameObject.GetComponent<Live2DObject>().DealDamage(20);
                    GameObject eff = Instantiate(effectOne, Vector2.zero, Quaternion.identity) as GameObject;
                    eff.transform.SetParent(canvas.transform, false);
                    ClearImage();

                }
            }
            else {
                ClearImage();
            }
        }
        if (check == false)
        {
            GameObject eff = Instantiate(effectOne, Vector2.zero, Quaternion.identity) as GameObject;
            eff.transform.SetParent(canvas.transform, false);
            ClearImage();
        }
    }
    private float CheckDensity(Texture2D Texture, Texture2D TargetTexture)
    {
        int myDensity = 0;
        int targetDensity = 0;
        for(int i = 0; i < 600; i++)
        {
            for(int j=0; j< 600; j++)
            {
                if (Texture.GetPixel(i, j).a != 0)
                {
                    myDensity += 1;
                }
                if(TargetTexture.GetPixel(i,j).a != 0)
                {
                    targetDensity += 1;
                }
            }
        }
        return ((float)myDensity) / ((float)targetDensity);

    }
    void Update () {
        ray = Camera.main.ScreenPointToRay(Aim.rectTransform.position);

        if (Input.GetMouseButtonDown(0))
        {
            TouchStart = Input.mousePosition;
        }else if (Input.GetMouseButtonUp(0))
        {
            TouchEnd = Input.mousePosition;
            if (Vector2.Distance(TouchStart, TouchEnd) < 10&&TouchEnd.x<680&&TouchEnd.y>60)
            {
                //Aim.rectTransform.position = Input.mousePosition;
                //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            pos.x -= Screen.width / 2 - (Screen.height / 2);
            float RatioX = pos.x / Screen.height;
            float RatioY = pos.y / Screen.height;
            pos.x = RatioX * baseTexture.width;
            pos.y = RatioY * baseTexture.height;
            oldPos = newPos;
            newPos = pos;
            if (0 < pos.x && pos.x < baseTexture.width&&(Vector2.Distance(TouchStart, Input.mousePosition) > 10))
            {
                if (oldPos == Vector2.zero) oldPos = newPos;
                Vector2[] vectorList = new Vector2[102];
                vectorList[0] = oldPos;
                vectorList[101] = newPos;
                float deltas = 0.0f;
                for (int i = 1; i < 101; i++)
                {
                    deltas += 0.01f;
                    vectorList[i] = new Vector2(Mathf.Lerp(oldPos.x, newPos.x, deltas), Mathf.Lerp(oldPos.y, newPos.y, deltas));
                }
                foreach (Vector2 vec in vectorList)
                {
                    Vector2[] brush = BrushWidth((int)vec.x, (int)vec.y, 2);
                    foreach (Vector2 brushPoint in brush)
                    {
                        baseTexture.SetPixel((int)brushPoint.x, (int)brushPoint.y, Color.white);
                    }
                }
                baseTexture.Apply();
            }
            else
            {
                oldPos = Vector2.zero;
                newPos = Vector2.zero;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            oldPos = Vector2.zero;
            newPos = Vector2.zero;
        }
    }
    private Vector2[] BrushWidth(int x, int y, int width)
    {
        int index=0;
        Vector2[] Bwidth = new Vector2[(2*width+1)* (2 * width + 1)];
        for(int i = y - width; i <= y + width; i++)
        {
             for(int j = x - width;j<=x+width; j++)
            {
                Bwidth[index++] = new Vector2(j, i);
            }
        }
        return Bwidth;
    }

}
