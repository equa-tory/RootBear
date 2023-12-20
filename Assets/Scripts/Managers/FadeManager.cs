using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;


    // [Header("Variables")]
    
    
    [Header("References")]
    public Animator anim;

    
    //-------------------------------------------------------------------------------------------------------------------
    
    private void Awake() {
        Instance = this;
    }   
    
    private void Start() {
    
    } 
    
    private void Update() {
    
    }
    
    //-------------------------------------------------------------------------------------------------------------------
    
    
}
