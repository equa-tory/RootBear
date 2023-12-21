using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardItem : MonoBehaviour
{

    [Header("References")]
    public TMP_Text nicknameText;
    public TMP_Text scoreText;

    
    //-------------------------------------------------------------------------------------------------------------------
    
    private void Awake() {
    
    }   
    
    private void Start() {
    
    } 
    
    private void Update() {
    
    }
    
    //-------------------------------------------------------------------------------------------------------------------
    
    public void Init(string _n, string _s)
    {
        nicknameText.text = _n;
        scoreText.text = "$" + _s;
    }



}