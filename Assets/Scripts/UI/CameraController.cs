using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject player;
    private Vector3 offset;
    void Start()
    {

        player = GameObject.Find("Player");
        offset = 10 * Vector3.forward + 1f * Vector3.down;//后续改
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //transform.position = player.transform.position-10*Vector3.forward;
        if (player.transform.position.x > -11.7)//后续改
            transform.position = Vector3.Lerp(transform.position, player.transform.position - offset, Time.deltaTime * 4);
        //Debug.Log(Time.deltaTime);
        //else transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }
}
