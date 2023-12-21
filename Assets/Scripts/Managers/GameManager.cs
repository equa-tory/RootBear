using System.Collections;
using System.Collections.Generic;
using Dan.Main;
using UnityEngine;
using Dan.Main;

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

    public bool inMenu;
    public Animator menuAnim;
    public TMPro.TMP_InputField nickInput;

    public GameObject lbGO;
    public LeaderboardItem lbItem;
    public Transform lbContent;

    public Clock clock;
    private bool brk;

    private string publiclLeaderboardKey = "8f554816f8f5d8cafef3913cadfa50289586fac9551ffe201f89b4f369f2f297"; 
    
    
    //-------------------------------------------------------------------------------------------------------------------
    
    private void Awake() {
        Instance = this;
    }   
    
    private void Start() {
        NextBear(0);

        string nickname = "";
        if(PlayerPrefs.HasKey("Nickname")) nickname = PlayerPrefs.GetString("Nickname");
        else {
            nickname = "Boi#" + Random.Range(0,10000).ToString("0000");
            SetNickName(nickname);
        }
        nickInput.text = nickname;
    } 
    
    private void Update() {

        if(clock.timer<=0) 
        {
            if(!brk)
            {
                brk = true;
                string nn = PlayerPrefs.GetString("Nickname");
                SetLeaderboardEntry(nn,currentSalary);
                inMenu = true;
                menuAnim.SetBool("Show",inMenu);
                Time.timeScale = 0;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(lbGO.active)
            {
                B_Back();
                return;
            }
            if(clock.timer > 0){
                inMenu = !inMenu;
                menuAnim.SetBool("Show",inMenu);
                if(inMenu) Time.timeScale = 0;
                else Time.timeScale = 1;
            }
        }

        if(currentBear) currentBear.position = Vector3.Lerp(currentBear.position, bearPos.position, bearSpeed*Time.deltaTime);
    }
    
    //-------------------------------------------------------------------------------------------------------------------
    
    public void B_Continue()
    {
        if(clock.timer<=0) 
        {
            B_Restart();
            return;
        }
        inMenu = false;
        menuAnim.SetBool("Show",inMenu);
        Time.timeScale = 1;
    }
    public void B_Restart()
    {
        FadeManager.Instance.anim.SetBool("FadeIn", true);
        Time.timeScale = 1;
        Invoke(nameof(Restart),.8f);
    }
    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Debug.Log("Restart");
    }
    public void B_Leaderboard()
    {
        GetLeaderboard();
    }
    public void B_Back()
    {
        lbGO.SetActive(false);
    }
    public void B_Exit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void GetLeaderboard()
    {
        lbGO.SetActive(true);
        LeaderboardCreator.GetLeaderboard(publiclLeaderboardKey, ((msg) => {
            for(int i = 0; i < msg.Length; i++)
            {
                Instantiate(lbItem,lbContent).Init(msg[i].Username, msg[i].Score.ToString());
            }
        }));
    }
    public void SetLeaderboardEntry(string _n, int _s)
    {
        LeaderboardCreator.UploadNewEntry(publiclLeaderboardKey, _n,
            _s, ((msg) => {
                // GetLeaderboard();
        }));
    }

    public void SetNickName(string _name)
    {
        PlayerPrefs.SetString("Nickname",_name);
    }

    public void NextBear(int _salary)
    {
        
        currentSalary += _salary;
        salaryText.text = "$" + currentSalary.ToString();

        if(_salary > 0) CoinSound();

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

    public void CoinSound()
    {
        coinSource.clip = coinClips[Random.Range(0,coinClips.Count-1)];
        coinSource.Play();
    }

    

}
