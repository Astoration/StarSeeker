using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {
    public GameObject[] Enemy;
    private Vector3 point;
	// Use this for initialization
	void Start () {
	    point = new Vector3(-262.4f, 80.9f, -97.6f);
        StartCoroutine(Spawn());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Spawn()
    {
        while (true)
        {
            int rand = Random.Range(0, Enemy.Length);
            int randAngle = Random.Range(0, 360);
            Vector3 position = point - new Vector3(Mathf.Cos(randAngle) * 30, 80, Mathf.Sin(randAngle) * 30);
            Instantiate(Enemy[rand], position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(6.0f, 7.5f));
        }
    }
}
