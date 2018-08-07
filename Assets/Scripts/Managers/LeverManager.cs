using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour {

    public int lever;
    public static LeverManager instance;

	// Use this for initialization
	void Start () {

        if (instance == null)
            instance = null;
        else if (instance != this)
            Destroy(this);

        Init();
	}
    public void Init()
    {
        lever = 1;
    }
	
	// Update is called once per frame
	void Update () {
		


	}
    public void LevelUp()
    {
        lever++;
    }
}
