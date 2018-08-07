using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager1 : MonoBehaviour {

    public GameObject[] monsters;
    public List<GameObject> nowMonsters;
    public List<Transform> positions;
    public Transform monsterParent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void CreatMonster(int index)
    {
        GameObject monster = monsters[index];
        GameObject nowMonster = Instantiate(monster, monsterParent);
        nowMonsters.Add(nowMonster);
        nowMonster.transform.position = positions[index].position;
        nowMonster.SetActive(true);
    }
}
