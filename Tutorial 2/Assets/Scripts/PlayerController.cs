using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed, jump;
    public UnityEngine.UI.Text scoreText, livesText, winText;
    private int score, currentLevel, currentCoins = 0;
    private int lives = 3;
    private bool floor;
    public Animator animator;
    public LevelData[] levels;
    public CameraController mainCamera;
    private GameObject spawn;
    public AudioSource music;
    public AudioClip winMusic;

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        ChangeLevel();
        transform.position = spawn.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y);
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y);
        }
        body.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0));
        if (Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1), new Vector2(0.9f, 0.05f), 0f, 1 << 8) != null)
        {
            floor = true;
        }
        else
        {
            floor = false;
        }
        animator.SetBool("Floor", floor);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Pickup"))
        {
            Destroy(collider.gameObject);
            score++;
            currentCoins++;
            if (currentCoins == levels[currentLevel].coins)
            {
                OpenDoor();
            }
            UpdateScore();
        }
        if (collider.CompareTag("Pit"))
        {
            lives--;
            UpdateLives();
            transform.position = spawn.transform.position;
        }
        if (collider.CompareTag("Enemy"))
        {
            lives--;
            UpdateLives();
            Destroy(collider.gameObject);
        }
        if (collider.CompareTag("Exit") && currentCoins == levels[currentLevel].coins)
        {
            if (currentLevel == levels.Length - 1)
            {
                music.Stop();
                music.clip = winMusic;
                music.loop = false;
                music.Play();
                winText.text = "You Win!\nGame created by Xavier Virt.";
                winText.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                currentLevel++;
                lives = 3;
                UpdateLives();
                currentCoins = 0;
                ChangeLevel();
                transform.position = spawn.transform.position;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Wind"))
        {
            body.AddForce(new Vector2(0, 70));
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor") && floor)
        {
            if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Jump") == 1)
            {
                animator.SetTrigger("Jump");
                floor = false;
                animator.SetBool("Floor", floor);
                body.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            }
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateLives()
    {
        livesText.text = "Lives: " + lives;
        if (lives < 1)
        {
            music.Stop();
            livesText.color = Color.red;
            winText.color = Color.red;
            winText.text = "Game over\nPress 'ESC' to quit.";
            winText.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void OpenDoor()
    {
        levels[currentLevel].exit.color = Color.white;
    }

    void ChangeLevel()
    {
        mainCamera.cameraLimitL = levels[currentLevel].cameraLimitL;
        mainCamera.cameraLimitR = levels[currentLevel].cameraLimitR;
        spawn = levels[currentLevel].spawnPoint;
    }
}
