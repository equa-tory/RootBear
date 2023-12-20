using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foam : MonoBehaviour
{
    Renderer rend;
    public Wobble part;
    public float adder;
    private float off;
    public float maxFoam = .4f;
    public float minFoam = .1f;


    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {

        if(part) 
        {
            rend.material.SetFloat("_WobbleX", part.wobbleAmountX);
            rend.material.SetFloat("_WobbleZ", part.wobbleAmountZ);
            if(part.fi>=1) rend.material.SetFloat("_Fill", part.fi);
            else rend.material.SetFloat("_Fill", part.fi+off);

            if(part.act)
            {
                if(off<maxFoam) off+=adder*Time.deltaTime/1.5f;
            }
            else
            {
                if(off>minFoam) off-=Time.deltaTime/5f;
            }

            return;
        }

    }

}
