using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleRender : MonoBehaviour {
    private ParticleSystem particleSystem;
	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
	}
	void Update () {
	}
}
