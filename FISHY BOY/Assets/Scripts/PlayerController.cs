using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 jump;
    public float jumpForce, angin;
    Rigidbody rb;
    public GameObject cam1, cam2, cam3, triggerAngin, DeathZone, winUI, jawaban1, jawaban2, jawaban3, jawaban4, jawaban5, gerbang, switchButtonOn, switchButtonoff, pauseMenu, triggerAngin2;
    public GameObject buttonJawaban1, buttonJawaban2;
    public AudioSource clickButton, deathSound, BackgroundMusic, wrongMusic, correctMusic;

    bool isGround, jawabanBenar;

    Vector3 forward, right;

    [Space(15)]
    public float checkDistance;
    public Transform GroundCheck;
    public LayerMask GroundMask;

    [Space(15)]
    public Transform PlayerMesh;

    [Space(15)]
    public bool canJump;
    public bool canMove;

    public int Health = 5;
    int score;
    public Text scoreText, healthText;
    public GameObject GameOverUI;

    public Animator papanTulisAnim, balokAnim;

    public GameObject soal1, soal2, soal3, soal4, jwb1, jwb2, jwb3, tembok, air, timer, timerUI;

    // Start is called before the first frame update
    void Start()
    {
        jump = new Vector3(0f, 2f, 0f);
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        Time.timeScale = 1;

        healthText.text = "Health: " + Health.ToString();

        GameObject wind = GameObject.FindWithTag("Baling-Baling");
        wind.GetComponent<RotateObject>().enabled = false;

        papanTulisAnim = GetComponent<Animator>();

        

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 MoveDirection = forward * verticalInput + right * horizontalInput;

        rb.velocity = new Vector3(MoveDirection.x * moveSpeed, rb.velocity.y, MoveDirection.z * moveSpeed);

        if (MoveDirection != new Vector3(0, 0, 0))
        {
            PlayerMesh.rotation = Quaternion.LookRotation(MoveDirection);
        }

    }

    private void Update()
    {
        PlayerJump();

        if (Health <= 0)
         {
            Health = 0;
            healthText.text = "Health: " + Health.ToString();
            Time.timeScale = 0;
            GameOverUI.SetActive(true);
            //BackgroundMusic.Stop();

            GameObject cam = GameObject.FindWithTag("MainCamera");
            cam.GetComponent<CameraMovement>().enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SkorManager.score = 0;
        }

        Pause();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        //if (col.gameObject.tag == "Obstacle")
        //{
        //    Time.timeScale = 0;
        //}

        if (col.gameObject.tag == "Camera Trigger1")
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
            cam3.SetActive(false);
            moveSpeed = 0.5f;
        }

        if (col.gameObject.tag == "Camera Trigger2")
        {
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(true);
        }

        if (col.gameObject.tag == "Switch Angin")
        {
            triggerAngin.SetActive(true);

            GameObject wind = GameObject.FindWithTag("Baling-Baling");
            wind.GetComponent<RotateObject>().enabled = true;

            switchButtonOn.SetActive(true);
            switchButtonoff.SetActive(false);

            clickButton.Play();
        }

        if (col.gameObject.tag == "Switch Angin 2")
        {
            triggerAngin2.SetActive(true);

            //GameObject wind = GameObject.FindWithTag("Baling-Baling");
            //wind.GetComponent<RotateObject>().enabled = true;

            clickButton.Play();
        }

        if (col.gameObject.tag == "WinZone")
        {
            Destroy(DeathZone);
        }
        if (col.gameObject.tag == "Win")
        {
            Time.timeScale = 0;
            winUI.SetActive(true);

            GameObject cam = GameObject.FindWithTag("MainCamera");
            cam.GetComponent<CameraMovement>().enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            BackgroundMusic.Stop();
        }

        if (col.gameObject.tag == "Jawaban1")
        {
            jawaban1.SetActive(true);
            jawabanBenar = true;
            Debug.Log("Benar");
            Destroy(gerbang);
            papanTulisAnim.Play("PapanTulisJatoh");
            Destroy(jawaban2);
            correctMusic.Play();
            Destroy(buttonJawaban1);
            Health += 2;
            healthText.text = "Health: " + Health.ToString();

        }
        if(col.gameObject.tag == "Jawaban2")
        {
            jawaban2.SetActive(true);
            jawabanBenar = false;
            Invoke("matikanObject", 2);
            Debug.Log("Salah");
            Health--;
            healthText.text = "Health: " + Health.ToString();
            wrongMusic.Play();
        }

        if (col.gameObject.tag == "Jawaban3")
        {
            jawaban3.SetActive(true);
            jawabanBenar = true;
            Debug.Log("Benar");
            balokAnim.Play("BalokNaik");
            Destroy(jawaban4);
            Destroy(jawaban5);
            correctMusic.Play();
            Destroy(buttonJawaban2);
            Health += 2;
            healthText.text = "Health: " + Health.ToString();
        }

        if (col.gameObject.tag == "Jawaban4")
        {
            jawaban4.SetActive(true);
            jawaban3.SetActive(false);
            jawaban5.SetActive(false);
            jawabanBenar = false;
            Debug.Log("Salah");
            Health--;
            healthText.text = "Health: " + Health.ToString();
            wrongMusic.Play();
        }

        if (col.gameObject.tag == "Jawaban5")
        {
            jawaban5.SetActive(true);
            jawabanBenar = false;
            jawaban4.SetActive(false);
            jawaban3.SetActive(false);
            Debug.Log("Salah");
            Health--;
            healthText.text = "Health: " + Health.ToString();
            wrongMusic.Play();
        }

        if (col.gameObject.tag == "Timer")
        {
            timer.SetActive(true);
            timerUI.SetActive(true);
        }

        
        

        if (col.gameObject.tag == "DeathZone" || col.gameObject.tag == "Obstacle")
        {
            deathSound.Play();
            Health--;
            healthText.text = "Health: " + Health.ToString();

            if (Health <= 0)
            {
                Health = 0;
                healthText.text = "Health: " + Health.ToString();
                Time.timeScale = 0;
                GameOverUI.SetActive(true);
                BackgroundMusic.Stop();

                GameObject cam = GameObject.FindWithTag("MainCamera");
                cam.GetComponent<CameraMovement>().enabled = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SkorManager.score = 0;

            }
        }

        if (col.gameObject.tag == "Camera Trigger1")
        {
                    cam2.SetActive(true);
                    cam1.SetActive(false);
                    moveSpeed += 1;
        }

        if (col.gameObject.tag == "Benar")
        {
            soal1.SetActive(false);
            jwb1.SetActive(false);
            correctMusic.Play();

            Invoke("TampilSoal2", 2);

        }

        if (col.gameObject.tag == "Benar2")
        {
            soal2.SetActive(false);
            jwb2.SetActive(false);
            correctMusic.Play();

            Invoke("TampilSoal3", 2);
        }

        if (col.gameObject.tag == "Benar3")
        {
            air.SetActive(true);
            tembok.SetActive(false);
            DestroySoal3();
            correctMusic.Play();
            Destroy(timer);
            timerUI.SetActive(false);

   

        }

        if (col.gameObject.tag == "Salah1")
        {
            wrongMusic.Play();
            Health -= 1;
            healthText.text = "Health: " + Health.ToString();
        }

            isGround = true;
    }

    //private void OnTriggerExit(Collider col)
    //{
    //    if (col.gameObject.tag == "Camera Trigger1")
    //    {
    //        cam1.enabled = true;
    //        cam2.enabled = false;
    //    }

    //    if (col.gameObject.tag == "Camera Trigger2")
    //    {
    //        cam2.enabled = true;
    //        cam3.enabled = false;
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Angin")
        {
            rb.AddForce(new Vector3(0, angin, 0), ForceMode.Impulse);
        }
         
        if (other.gameObject.tag == "Angin 2")
        {
            rb.AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
        }
    }

    void matikanObject()
    {
        jawaban2.SetActive(false);
        jawaban4.SetActive(false);
        jawaban5.SetActive(false);

        
    }

    void Pause()
    {
       
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;

                GameObject cam = GameObject.FindWithTag("MainCamera");
                cam.GetComponent<CameraMovement>().enabled = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;


            }
        
    }

   

    void TampilSoal2()
    {
        soal2.SetActive(true);
        jwb2.SetActive(true);
    }

    void TampilSoal3()
    {
        soal3.SetActive(true);
        jwb3.SetActive(true);
    }

    void DestroySoal3()
    {
        soal3.SetActive(false);
        jwb3.SetActive(false);
    }


}
