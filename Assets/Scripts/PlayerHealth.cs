using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public Slider healthBar;
    public AudioSource hurtAudio;
    private TMP_Text score;

    void Start()
    {
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
        StartCoroutine(IncreaseScore());
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        hurtAudio.Play();
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    IEnumerator IncreaseScore(){
        while(true){
            score.text = (int.Parse(score.text) + 1).ToString("D6");
            yield return new WaitForSeconds(.25f);
        }
    }
}
