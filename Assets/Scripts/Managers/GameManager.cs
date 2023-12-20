using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;

    
    [Header("Variables")]
    public float bearSpeed = 1f;
    public int currentSalary;
    private float spawnCd = 1f;
    private float spawnTimer;

    
    [Header("References")]
    public Camera cam;
    public Animator despAnim;
    public ParticleSystem despPart;
    public Bear bear;
    public Transform bearPos;
    public Transform currentBear;
    public TMPro.TMP_Text salaryText;

    public AudioSource waterSource;
    public AudioSource bellSource;
    public AudioSource coinSource;
    public List<AudioClip> coinClips = new List<AudioClip>();
    
    
    //-------------------------------------------------------------------------------------------------------------------
    
    private void Awake() {
        Instance = this;
    }   
    
    private void Start() {
        NextBear(0);
    } 
    
    private void Update() {
        if(currentBear) currentBear.position = Vector3.Lerp(currentBear.position, bearPos.position, bearSpeed*Time.deltaTime);
    }
    
    //-------------------------------------------------------------------------------------------------------------------
    
    public void NextBear(int _salary)
    {
        
        currentSalary += _salary;
        salaryText.text = "$" + currentSalary.ToString();

        despAnim.SetBool("On",false);
        despPart.Stop();

        if(currentBear)
        {
            currentBear.GetComponent<Bear>().cup.speed = 0.01f;
            spawnTimer = spawnCd;
            Invoke(nameof(WalkAway),spawnTimer);
        }
        else
        {
            Invoke(nameof(SpawnBear),spawnTimer);
        }

        
    }

    private void WalkAway()
    {
        coinSource.clip = coinClips[Random.Range(0,coinClips.Count-1)];
        coinSource.Play();
        bearPos.position = new Vector3(-10,0,-2);
        Invoke(nameof(SpawnBear),spawnTimer);
    }

    private void SpawnBear()
    {
        bellSource.Play();
        if(currentBear)Destroy(currentBear.gameObject);
        bearPos.position = new Vector3(0,0,-2);
        Bear _b = Instantiate(bear, new Vector3(10,0,-2),Quaternion.identity);
        _b.Init(cam,despAnim,despPart,waterSource);
        currentBear = _b.transform;
    }

    public void Pay(int money)
    {
        currentSalary -= money;
        salaryText.text = "$" + currentSalary.ToString();
    }

    

}
