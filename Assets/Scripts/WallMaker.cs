
using UnityEngine;

public class WallMaker : MonoBehaviour
{

    public Transform lastWall;
    public GameObject wallPref; //klonlanacak objenin prifabı
    Vector3 lastPos; //son blogun konumu
    Camera cam;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        lastPos = lastWall.position;
        player = FindObjectOfType<PlayerController>();
        cam = Camera.main; //camera
        InvokeRepeating("CreateWalls",1,0.1f);
    }

    // Update is called once per frame


    private void CreateWalls()
    {
        float distance = Vector3.Distance(lastPos, player.transform.position); ; //mesafe cameranın
        if (distance > cam.orthographicSize * 2) return;

        Vector3 newPos = Vector3.zero; // newpos son konum
        int rand = Random.Range(0, 11);
        if (rand <= 5)
        {
            newPos = new Vector3(lastPos.x - 0.707f, lastPos.y, lastPos.z + 0.707f);
        }
        else
        {
            newPos = new Vector3(lastPos.x + 0.707f, lastPos.y, lastPos.z + 0.707f);
        }

        GameObject newBlock = Instantiate(wallPref, newPos, Quaternion.Euler(0, 45, 0), transform);
        newBlock.transform.GetChild(0).gameObject.SetActive(rand % 3 == 2); // rasgale seker olusturma
        lastPos = newBlock.transform.position; //last posun konumu son eklenen bloga verildi.
        
    }
}
