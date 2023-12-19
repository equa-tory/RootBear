using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerController>()) Debug.Log("!");
    }
}
