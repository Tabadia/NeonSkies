using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public GameObject planeExplosionFX;
    private TMP_Text score;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void Die()
    {
        score.text = (int.Parse(score.text) + 50).ToString("D6");
        Instantiate(planeExplosionFX, transform.position, Quaternion.identity);
        // frame, intensity
        Camera.main.GetComponent<CameraFollow>().ScreenShake(15f, 1f);

        Destroy(gameObject);
    }
}
