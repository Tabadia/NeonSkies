using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipExplosionFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.GetComponent<AudioSource>().Play();
        Destroy(gameObject, 1.5f);
        transform.GetChild(0).GetComponent<ParticleSystem>().Emit(15);
        transform.GetChild(1).GetComponent<ParticleSystem>().Emit(15);
        transform.GetChild(2).GetComponent<ParticleSystem>().Emit(25);
    }

}
