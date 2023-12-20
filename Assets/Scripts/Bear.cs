using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Bear : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private Wobble wobble;
    public ObjTransform cup;


    private void Start() {
        Sprite _s = sprites[Random.Range(0, sprites.Count)]; 
        sr.sprite = _s;
    }

    public void Init(Camera cam, Animator anim, ParticleSystem dps, AudioSource waterSource)
    {
        sr.GetComponent<Billboard>().target = cam.transform;
        wobble.anim = anim;
        wobble.factorDec = Random.Range(.5f,2f);
        wobble.dps = dps;
        wobble.waterSource = waterSource;
    }

}