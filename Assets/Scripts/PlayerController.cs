using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    delegate void TurnDelegate();    //fonksiyon tutucu pointer gibi düsun  platforma gore fonksiyon secimi icin touch mu space mi
    TurnDelegate turnDelegate;

    public float moveSpeed = 2;
    bool lookingRight = true;
    GameManager gameManager;
    Animator anim;
    public Transform rayOrigin; //oyuncuya ışının merkezini verdik
    public ParticleSystem effect; //efffect

    public Text scoreTxt, hScoreTxt;  //scorlar
    public int Score { get; private set; }
    public int HScore { get; private set; }
    // Start is called before the first frame update
    void Start()
    {

        #region PLATFORM FOR TURNING
            #if UNITY_EDITOR
                    turnDelegate = TurnPlayerUsingKeyboard;
            #endif
            #if UNITY_ANDROID
                    turnDelegate = TurnPlayerUsingTouch;
            #endif
        #endregion

        gameManager = GameObject.FindObjectOfType<GameManager>(); //tipi game manger olan nesneyi al
        anim = gameObject.GetComponent<Animator>();//animasyona başlaması için tip aldık

        LoadHighScore(); // high scoru yaz
    }

    private void LoadHighScore()
    {
        HScore = PlayerPrefs.GetInt("highscore");
        hScoreTxt.text = HScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameStarted) return; //oyun başlamamışsa hareket etme animasyon çalıştırma

        anim.SetTrigger("gameStarted"); //oyun başlayinca hareket et

        moveSpeed *= 1.001f;
        Debug.Log(moveSpeed);
        //transform.position += transform.forward*Time.deltaTime*moveSpeed;
        transform.Translate(new Vector3(0, 0, 1) * moveSpeed * Time.deltaTime);

        turnDelegate();  //platform

        CheckFalling(); //düşme kontrolu

    }

    private void TurnPlayerUsingKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Turn();
    }

    private void TurnPlayerUsingTouch()
    {
        if (Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began) //parmaz dokunma fazzı 
            Turn();
    }

    float elapsedTime = 0; //gecen sure baslangıc 0
    float freq = 1f / 5f; // 5 saniyede 1 kere

    private void CheckFalling()
    {
        if ((elapsedTime += Time.deltaTime) > freq)  //gecen sure frekansdan buyukse
        {
            if (!Physics.Raycast(rayOrigin.position, new Vector3(0, -1, 0)))  //ışın gönderiliyor altında bi nesne varmı diye yoksa ölcek
            {
                anim.SetTrigger("falling"); //animasyon degisti
                gameManager.RestartGame(); //oyun yeniden basladı
                elapsedTime = 0;
            }
        }
    }

    private void Turn()
    {
        if (lookingRight)
        {
            transform.Rotate(new Vector3(0, 1, 0), -90);
        }
        else
        {
            transform.Rotate(new Vector3(0, 1, 0), 90);
        }
        lookingRight = !lookingRight;
    }

    private void OnTriggerEnter(Collider other)  //sekere carpma anı
    {
        if (other.tag.Equals("candy"))
        {
            MakeScore();
            CreateEffect();
            Destroy(other.gameObject); //sekere carpınca yok et
        }
    }

    private void OnCollisionExit(Collision collision) //bloklar carpışma esnasında yok et
    {
        Destroy(collision.gameObject, 2f);
    }
    private void CreateEffect()
    {
        var vfx = Instantiate(effect, transform);
        Destroy(vfx, 1f);
    }

    private void MakeScore()
    {
        Score++;
        scoreTxt.text = Score.ToString();
        if (Score > HScore)
        {
            HScore = Score;
            hScoreTxt.text = HScore.ToString();
            PlayerPrefs.SetInt("highscore", HScore); //high score kaydet
        }
    }
}
