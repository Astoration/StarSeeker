using UnityEngine;
using System.Collections;


public class DrawingManager : MonoBehaviour {
    public Texture2D baseTexture;
    private Vector2 oldPos=Vector2.zero;
    private Vector2 newPos = Vector2.zero;
    public Texture2D[] MagicTexture;

    void Start () {

     }
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(new Vector2(Screen.width/2-(Screen.height/2), Screen.height/2 - (Screen.height / 2)), new Vector2(Screen.height,Screen.height)), baseTexture);
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
        return ((float)compareCount / (float)maxCount)*100;
    }
    void Update () {
        if (Input.GetKey(KeyCode.Tab))
        {
            ClearImage(baseTexture);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log(CompareImage(baseTexture,MagicTexture[0])+"%");
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
            if (oldPos == Vector2.zero) oldPos = newPos;
            Vector2[] vectorList = new Vector2[102];
            vectorList[0] = oldPos;
            vectorList[101] = newPos;
            float deltas = 0.0f;
            for(int i = 1; i < 101; i++)
            {
                deltas += 0.01f;
                vectorList[i] = new Vector2(Mathf.Lerp(oldPos.x,newPos.x,deltas),Mathf.Lerp(oldPos.y,newPos.y,deltas));
            }
            foreach (Vector2 vec in vectorList)
            {
                Vector2[] brush = BrushWidth((int)vec.x, (int)vec.y, 4);
                foreach (Vector2 brushPoint in brush)
                {
                    baseTexture.SetPixel((int)brushPoint.x, (int)brushPoint.y, Color.white);
                }
            }
            baseTexture.Apply();
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
