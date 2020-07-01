using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    static BackgroundSound instance;
    // Start is called before the first frame update
    void Start()
    {
        if (!instance)  //muzik yoksa instance yi this(muzik objesi) yap
        {
            instance = this;
        }
        else if (instance != this) //muzik ilk nesne degilse farklı yeniden yaratılıyosa sil
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject); //muzigi tekrar baslatma
    }


}
