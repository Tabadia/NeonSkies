using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public GameObject player;
    public GameObject mainEnemy;  // The enemy prefab to spawn
    public TextMeshProUGUI tutorialText;  // Reference to the TextMeshPro text object

    private Movement playerMovement;
    private Shooting playerShooting;
    private Rigidbody2D playerRb;
    private int tutorialStep = 0;

    void Start()
    {
        playerMovement = player.GetComponent<Movement>();
        playerShooting = player.GetComponent<Shooting>();
        playerRb = player.GetComponent<Rigidbody2D>();

        // Initial state: enable rotation, disable thrust and shooting, make the player kinematic
        playerMovement.enabled = true;
        playerMovement.thrustPower = 0;  
        playerShooting.enabled = false;
        playerRb.isKinematic = true; 
        StartCoroutine(TutorialSequence());
    }

    private bool isEscPressed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscPressed = true;
            StartCoroutine(CheckEscHold());
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            isEscPressed = false;
        }
    }

    private IEnumerator CheckEscHold()
    {
        yield return new WaitForSeconds(2f);

        if (isEscPressed)
        {
            SceneManager.LoadScene("Main");
        }
    }

    IEnumerator TutorialSequence()
    {
        // Step 1: Rotate direction with the mouse
        tutorialText.text = "Rotate your direction with the mouse.";
        float initialRotation = player.transform.eulerAngles.z;
        float totalRotation = 0f;

        while (totalRotation < 180f)
        {
            float currentRotation = player.transform.eulerAngles.z;
            float rotationDelta = Mathf.DeltaAngle(initialRotation, currentRotation);
            totalRotation += Mathf.Abs(rotationDelta);
            initialRotation = currentRotation;
            yield return null;
        }

        // Enable thrust but keep the player kinematic
        playerMovement.thrustPower = 20.0f;
        yield return new WaitForSeconds(.5f);  

        // Step 2: Thrust with W
        tutorialText.text = "Press W to thrust.";
        yield return new WaitUntil(() => Input.GetKey(KeyCode.W));

        // Make the player dynamic
        playerRb.isKinematic = false;
        yield return new WaitForSeconds(2);  

        // Step 3: Brake with Space
        tutorialText.text = "Press Space to brake.";
        yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
        yield return new WaitForSeconds(1);  

        // Step 4: Boost with Shift
        tutorialText.text = "Press Left Shift to boost.";
        yield return new WaitUntil(() => Input.GetKey(KeyCode.LeftShift));
        yield return new WaitForSeconds(1);  

        // Step 5: Shoot with Left Click, spawn an enemy
        tutorialText.text = "Press Left Click to shoot.";
        playerShooting.enabled = true;
        yield return new WaitUntil(() => Input.GetButtonDown("Fire1"));

        // Spawn the first enemy
        yield return new WaitForSeconds(1);  
        tutorialText.text = "Destroy the enemy.";
        Instantiate(mainEnemy, new Vector3(player.transform.position.x + 10, player.transform.position.y, 0), Quaternion.identity);
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);  

        // Step 6: Use homing missiles with Right Click
        tutorialText.text = "Press Right Click to use homing missiles.";
        yield return new WaitUntil(() => Input.GetButtonDown("Fire2"));

        yield return new WaitForSeconds(1);  
        tutorialText.text = "Destroy the enemies.";
        // Spawn three enemies for the final test
        for (int i = 0; i < 3; i++)
        {
            Instantiate(mainEnemy, new Vector3(player.transform.position.x + 10, player.transform.position.y + (i * 4), 0), Quaternion.identity);
            yield return new WaitForSeconds(.25f);  
        }
        
        // Wait until all enemies are destroyed
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);

        // Tutorial complete, transition to the next scene
        tutorialText.text = "Good Luck!";
        yield return new WaitForSeconds(2); 
        SceneManager.LoadScene("Main");
    }
}
