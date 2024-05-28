using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;
    public Slider healthBar;
    public AudioSource hurtAudio;
    public PostProcessVolume _postProcessVolume;
    public Color damageVignetteColor;
    private TMP_Text score;

    private ColorGrading _cg;

    void Start()
    {
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
        StartCoroutine(IncreaseScore());
        _postProcessVolume.profile.TryGetSettings(out _cg);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(AnimateVignette());

        hurtAudio.Play();
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator AnimateVignette()
    {
        Vector4 originalColor = _cg.gamma;
        _cg.gamma.Override(originalColor + new Vector4(2, 0, 0, 0));
        for (float i = 0; i < 1; i += .01f)
        {
            yield return new WaitForEndOfFrame();
            _cg.gamma.Override(Vector4.Lerp(_cg.gamma, originalColor, i));
        }
        _cg.gamma.Override(originalColor);
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
