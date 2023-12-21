using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

    [Header("Variables")]
    public List<Material> mats;
    public float startTime;
    public float timer;
    
    [Header("References")]
    public MeshRenderer rend;
    public TMPro.TMP_Text clockText;
    public AudioSource source;
    
    //-------------------------------------------------------------------------------------------------------------------
    
    private void Start() {

        StartCoroutine(Tick());

        timer = startTime;

        int m1 = Random.Range(0,mats.Count-1);
        rend.materials[0] = mats[m1];
        mats.RemoveAt(m1);
        int m2 = Random.Range(0,mats.Count-1);
        rend.materials[1] = mats[m2];
    } 

    private void Update() {
        
        if(timer>0)
        {
            timer-=Time.deltaTime;
            clockText.text = Mathf.Round(timer).ToString();
        }



    }
    
    //-------------------------------------------------------------------------------------------------------------------
    
    IEnumerator Tick()
    {
        while(true){
            source.Stop();
            source.Play();
            yield return new WaitForSeconds(1);
        }
    }
    

}
