using UnityEngine;

public class JetFrameController : MonoBehaviour
{
    public Sprite[] frames; // Array of sprites representing the jet at different angles
    public int maxFrame;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float rot = transform.eulerAngles.z; 
        int frameIndex = PoseMatcher(rot);
        spriteRenderer.sprite = frames[frameIndex];
    }

    int PoseMatcher(float rot)
    {
        // Adjust the rotation angle to match Unity's coordinate system
        rot -= 90;
        if (rot < 0) rot += 360; // Ensure the rotation is within the range [0, 360]

        float index = 0;
        if (rot > 180) rot -= 360; // Convert to range [-180, 180]

        if (rot > 0)
        {
            index = maxFrame - Mathf.Abs(RangeLerp(rot, 180, 0, maxFrame / 2, -maxFrame / 2));
        }
        else
        {
            index = Mathf.Abs(RangeLerp(rot, -180, 0, maxFrame / 2, -maxFrame / 2));
        }


        //Debug.Log(transform.localEulerAngles);
        if (transform.localEulerAngles.z > 0 && transform.localEulerAngles.z < 180 )
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        return Mathf.RoundToInt(index);
    }

    float RangeLerp(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
