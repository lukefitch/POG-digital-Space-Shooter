using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Destroy the game object after a default time of 2.8 seconds
        Destroy(this.gameObject, 2.8f);
    }
}
