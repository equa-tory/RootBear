using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    [Header("Variables")]
    public const string audioName = "ost";
    private int index = 1;


    [Header("References")]
    public AudioSource source;
    public AudioClip audioClip;
    public string soundPath;

    
    //-------------------------------------------------------------------------------------------------------------------
    
    private void Awake() {
        
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        soundPath = "file://" + Application.persistentDataPath + "/Music/";
        Debug.Log(soundPath + audioName + index + ".mp3");
        StartCoroutine(LoadAudio(audioName + index + ".mp3"));
    }   
    
    private void Start() {
    
    } 
    
    private void Update() {
    
        if(source.clip && source.time >= source.clip.length)
        {
            index++;
            if(index>5) index = 1;

            StartCoroutine(LoadAudio(audioName + index + ".mp3"));
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            source.time = source.clip.length - 5;
        }

    }
    
    //-------------------------------------------------------------------------------------------------------------------
    
    private IEnumerator LoadAudio(string _audioName)
    {
        WWW request = GetAudioFromFile(soundPath, _audioName);
        yield return request;

        audioClip = request.GetAudioClip();
        audioClip.name = audioName;

        PlayAduioFile();
    }

    private void PlayAduioFile()
    {
        source.clip = audioClip;
        source.Play();
        // source.loop = true;
    }

    private WWW GetAudioFromFile(string path, string filename)
    {
        string audioToLoad = string.Format(path + "{0}", filename);
        WWW request = new WWW(audioToLoad);
        return request;
    }

    
}
