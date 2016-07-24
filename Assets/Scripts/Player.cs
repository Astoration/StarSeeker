using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public const float Speed = 60;
    public Image RaderArea;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            if (Input.GetMouseButton(0))
            {
                CameraRotate(Camera.main.ScreenToViewportPoint(Input.mousePosition));
            }
            foreach(Touch t in Input.touches)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(t.position);
                CameraRotate(pos);
            }
        }
	}

    void CameraRotate(Vector3 pos)
    {
        if (pos.y > 0.2)
        {
            if (pos.x < 0.1)
            {
                RaderArea.transform.Rotate(Vector3.forward, Time.deltaTime * Speed);
                this.transform.Rotate(Vector3.down, Time.deltaTime * Speed);
            }
            if (0.9 < pos.x)
            {
                RaderArea.transform.Rotate(Vector3.back, Time.deltaTime * Speed);
                this.transform.Rotate(Vector3.up, Time.deltaTime * Speed);
            }
        }
    }
}
