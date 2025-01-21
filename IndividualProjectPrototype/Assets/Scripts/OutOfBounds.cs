using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    [SerializeField] private TrackPlayerPosition trackPlayer;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.tag == "Player" ||other.gameObject.tag == "Player")
        {
            trackPlayer.gameover = true;
        }
    }
}
