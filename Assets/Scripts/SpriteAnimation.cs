using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SpriteAnimation : MonoBehaviour {
    public Sprite[] sprites;
    public float animationSpeed;
    private Image image;
    public IEnumerator Animate()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            image.sprite = sprites[i];
            yield return new WaitForSeconds(animationSpeed);
        }
        Destroy(this.gameObject);
    }
    void Start () {
        image = this.GetComponent<Image>();
        StartCoroutine(Animate());
	}
	void Update () {
	
	}
}
