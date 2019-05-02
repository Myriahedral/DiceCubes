using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("DestroySelf", 3f);
	}

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
