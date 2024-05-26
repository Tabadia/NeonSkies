using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<ParticleSystem>().Emit(1);
        Destroy(gameObject, 1f);
    }
}
