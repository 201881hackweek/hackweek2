using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour {

    public float life;
    public float allLife;
    public float preLife;
    public float lifeDamage;
    public int lifeNum;
    public GameObject[] lifes;
	// Use this for initialization
	void Start () {
        
        allLife = 7;
        preLife = allLife;
        lifeNum =(int) allLife;
    }
	
	// Update is called once per frame
	void Update () {

        life = Player.instance.life;
        if(preLife-life>=1)
        {
            lifes[lifeNum - 1].SetActive(false);
            lifeNum--;
            preLife--;
        }
    }
}
