using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class Movement : MonoBehaviour
{
    public float thrustPower = 20.0f;          // Increased thrust power to overcome gravity
    public float maxSpeed = 15.0f;             // Maximum speed
    public float acceleration = 5.0f;          // Acceleration rate
    public float deceleration = 3.0f;          // Deceleration rate when not thrusting
    public float airBrakeDeceleration = 10.0f; // Air brake deceleration rate
    public float rotationSpeed = 500.0f;       // Rotation speed
    public float rotationSmoothing = 0.1f;     // Rotation smoothing factor
    public float boostMultiplier = 2.0f;       // Speed multiplier during boost
    public float boostDuration = 2.0f;         // Duration of boost
    public float boostCooldown = 5.0f;         // Cooldown between boosts
    public float normalGravityScale = 1.0f;    // Normal gravity scale when not thrusting
    public float reducedGravityScale = 0.2f;   // Reduced gravity scale when thrusting
    public PostProcessVolume _postProcessVolume;
    public ParticleSystem playerTrailFX;
    public ParticleSystem playerBoostFX;
    public AudioSource boostAudio;
    public Canvas boostIndicator;

    private Rigidbody2D rb;
    private bool isBoosting = false;
    private float boostEndTime = 0;
    private float boostCooldownEndTime = 0;
    private ColorGrading _cg;
    private Vector4 targetCg;
    private Vector4 originalCg;
    private bool wroteBoost = false;

    public Slider boostSlider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = deceleration;         // Add drag to simulate air resistance
        rb.gravityScale = normalGravityScale;  // Set initial gravity scale
        _postProcessVolume.profile.TryGetSettings(out _cg);
        targetCg = _cg.gamma.value;
        originalCg = _cg.gamma.value;
    }

    void Update()
    {
        //print(_cg.gamma.value);
        HandleRotation();
        HandleThrust();
        HandleAirBrake();
        HandleBoost();
        AdjustGravity();

        if (!wroteBoost && Time.time > boostCooldownEndTime)
        {
            boostIndicator.gameObject.SetActive(true);
            wroteBoost = true;
            StartCoroutine(WriteMessage(boostIndicator.transform.GetChild(0).GetComponent<TMP_Text>(), "Boost Active!"));
        }

        boostIndicator.transform.position = transform.position + Vector3.down * 1.3f;

        //boostSlider.maxValue = 1;
        boostSlider.value = Mathf.Clamp01(1 - (boostCooldownEndTime - Time.time) / boostCooldown);
        _cg.gamma.Override(Vector4.Lerp(_cg.gamma.value, targetCg, Time.deltaTime * 10f));
    }

    private IEnumerator WriteMessage(TMP_Text obj, string message)
    {
        obj.text = "";
        for (int i = 0; i < message.Length; i++)
        {
            obj.text += message[i];
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(.5f);
        obj.transform.parent.gameObject.SetActive(false);
    }

    void HandleRotation()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0;

        Vector2 direction = (worldMousePosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        float newRotation = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSmoothing);
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, newRotation, rotationSpeed * Time.deltaTime));
    }

    void HandleThrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            float appliedThrustPower = thrustPower;

            if (isBoosting)
            {
                appliedThrustPower *= boostMultiplier;
            }

            Vector2 thrustDirection = transform.up * appliedThrustPower;
            rb.AddForce(thrustDirection, ForceMode2D.Force);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed * (isBoosting ? boostMultiplier : 1f));
        }
    }

    void HandleAirBrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, airBrakeDeceleration * Time.deltaTime);
        }
    }

    void HandleBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > boostCooldownEndTime)
        {
            boostAudio.Play();

            var emission = playerBoostFX.emission;
            emission.enabled = true;
            emission = playerTrailFX.emission;
            emission.enabled = false;

            targetCg = originalCg * 4f;
            isBoosting = true;
            boostEndTime = Time.time + boostDuration;
            boostCooldownEndTime = Time.time + boostDuration + boostCooldown;
            StartCoroutine(BoostSliderEffect());
        }

        if (isBoosting && Time.time > boostEndTime)
        {
            isBoosting = false;
            var emission = playerBoostFX.emission;
            emission.enabled = false;
            targetCg = originalCg;
            wroteBoost = false;
        }
    }

    void AdjustGravity()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (!isBoosting)
            {
                var emission = playerTrailFX.emission;
                emission.enabled = true;
            }

            rb.gravityScale = reducedGravityScale;
        }
        else
        {
            var emission = playerTrailFX.emission;
            emission.enabled = false;

            rb.gravityScale = normalGravityScale;
        }
    }

    IEnumerator BoostSliderEffect()
    {
        float startTime = Time.time;
        float startValue = boostSlider.value;
        float targetValue = 0f;
        float duration = 0.5f; // Adjust the duration as needed

        while (Time.time < startTime + duration)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / duration;
            boostSlider.value = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }

        boostSlider.value = targetValue;
    }
}
