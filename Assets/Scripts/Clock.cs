using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

    [Header("Variables")]
    public List<Material> mats;
    
    [Header("References")]
    public MeshRenderer rend;
    
    //-------------------------------------------------------------------------------------------------------------------
    
    private void Start() {
        int m1 = Random.Range(0,mats.Count-1);
        rend.materials[0] = mats[m1];
        mats.RemoveAt(m1);
        int m2 = Random.Range(0,mats.Count-1);
        rend.materials[1] = mats[m2];
    } 
    
    //-------------------------------------------------------------------------------------------------------------------
    
    

}
