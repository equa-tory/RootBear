using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float adder;
    public float startOff;
    public float off;

    Renderer rend;
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;
    Vector3 angularVelocity;
    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    public float wobbleAmountX;
    public float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;


    public bool act = false;
    public bool brk = false;
    public float fi = -1f;
    public float factor;
    public float factorDec = 3f;
    public Vector3 vara;
    public Animator anim;
    public ParticleSystem dps;

    public int attemps = 3;
    public float reqFi = .5f;
    public float reqHelp = .1f;
    public float reqCoolHelp = 0.025f;

    public ParticleSystem ps;

    public float payCd = .5f;
    private float payTimer;

    public AudioSource waterSource;


    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        off = startOff;
    }
    private void Update()
    {

        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        velocity += vara;
        angularVelocity = transform.rotation.eulerAngles - lastRot;
        // Debug.Log(velocity);


        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;

        if(factor>0)
        {
            fi += factor*Time.deltaTime;
            rend.material.SetFloat("_Fill", fi);
        }
        if(act)
        {
            
            payTimer+=Time.deltaTime;
            if(payTimer>=payCd) 
            {
                GameManager.Instance.Pay(1);
                payTimer=0f;
            }

            if(factor<1)factor+=Time.deltaTime;
            else factor =1;

            // float rX = Random.Range(-.1f,.1f);
            // float rZ = Random.Range(-.1f,.1f);
            float rX = Random.Range(-10f,10f);
            float rZ = Random.Range(-10f,10f);
            vara += new Vector3(rX,0,rZ);
        }
        else
        {
            if(factor>0)factor-=factorDec*Time.deltaTime;
            else {
                dps.Stop();
                factor = 0;
            }
        }
        if(vara.x>0)vara.x-=Time.deltaTime*40;
            else vara.x = 0;
            if(vara.z>0)vara.z-=Time.deltaTime*40;
            else vara.z = 0;
        if (fi >= 1)
        {
            fi = 1;
            

            if(!brk)
            {
                brk = true;
                act = false;

                ps.gameObject.SetActive(true);
                
                waterSource.Stop();
                GameManager.Instance.NextBear(Random.Range(1,3));
            }
        }
        // if (Input.GetKeyDown(KeyCode.T)) // Debug
        // {
        //     fi=-1;
        //     rend.material.SetFloat("_Fill", fi);
        // }

        if (Input.GetKeyDown(KeyCode.Space) && !brk)
        {
            act = !act;
            anim.SetBool("On",act);

            if(act){
                waterSource.volume = Random.Range(.75f,1f);
                waterSource.pitch = Random.Range(0.8f,1.5f);
                waterSource.Play();

                dps.Play();
                attemps--;
                
            }
            else
            {
                waterSource.Stop();

                if(attemps<=0)
                {

                    GameManager.Instance.NextBear(0);
                    brk = true;
                    dps.Stop();
                    act = false;
                } 
                // dps.Stop();
                int salary = 0;

                if(fi>reqFi+reqHelp) // Lose
                {
                    salary = Random.Range(1,3);
                    GameManager.Instance.NextBear(salary);
                }
                else if(fi>=reqFi+reqCoolHelp) // 3/3
                {
                    salary = Random.Range(3,6);
                    GameManager.Instance.NextBear(salary);
                }
                else if(fi>=reqFi-reqCoolHelp) // 2/3
                {
                    salary = Random.Range(6,15);
                    GameManager.Instance.NextBear(salary);
                }
                else if(fi>=reqFi-reqHelp) // 1/3
                {
                    salary = Random.Range(3,6);
                    GameManager.Instance.NextBear(salary);
                }
            }
        }
    }



}