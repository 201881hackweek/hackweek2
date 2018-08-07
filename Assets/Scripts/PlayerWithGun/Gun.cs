using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public Vector3 hitPos;
    public Vector2 hitDir;
    public Camera mainCamera;
    public GameObject bullet;
    public Transform hand;
    public Transform savePosition;
    public LineRenderer line;

    public int bulletNum;
    public int maxBulletNum;
    public int resetTime;
    public int val;
    public int depth;

    public float rotation;

    public bool canShot;
    public bool canReset;

    // Use this for initialization
    void Start () {

        bulletNum = 5;
        maxBulletNum = 5;
        resetTime = 2;

        canShot = true;
        canReset = true;

        //射线
        line = gameObject.GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

        //计算瞄准位置
        hitPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        hitDir = hitPos-transform.position;
        

        //右键开始瞄准
        if (Input.GetMouseButtonDown(1)&&canShot)
            StartCoroutine("Shot");
    }
 

    IEnumerator Shot()
    {
        // 改变玩家朝向
        if (hitDir.x > 0)
        {
            Player.instance.transform.localScale = new Vector3(Player.instance.scaleX, Player.instance.transform.localScale.y, Player.instance.transform.localScale.z);
            val = 1;
        }

        if (hitDir.x <= 0)
        {
            Player.instance.transform.localScale = new Vector3(-Player.instance.scaleX, Player.instance.transform.localScale.y, Player.instance.transform.localScale.z);
            val = -1;
        }

        transform.position = hand.position;
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(0, 0, 40.5f));     //枪口默认朝向

        hitDir.Normalize();
        Player.instance.isShoting = true;               //瞄准动画
        Vector2 dir, realDir;                           //目标瞄准位置和实际瞄准位置
        float angle, realAngle, timer, bulletSpeed;     //最大范围误差角，实际误差角以及瞄准精确所需最小时间，子弹速度
        int error;                                      //最大范围误差参数 越小 误差范围越大

        timer = 1f;
        bulletSpeed = 20f;
        dir = hitDir;
        error = 5;
        depth = 50;
       
        while (true)
        {
            

            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0.1f;
            angle = timer * Mathf.PI / error;
            realAngle = Random.Range(-angle, angle);        //获取误差范围内的最终偏差角
            realDir = Quaternion.AngleAxis(realAngle* (180 / Mathf.PI), Vector3.forward) * dir;       //四元数 不懂
            realDir.Normalize();

            transform.rotation = Quaternion.identity;
            transform.Rotate(new Vector3(0, 0, 40.5f));     //枪口默认朝向
            rotation = Vector3.Angle(realDir, val * Vector3.right);

            if (Vector3.Dot(realDir,val*Vector3.up)<0)
                rotation = ( 360 - rotation);
            transform.Rotate(new Vector3(0, 0, val * rotation));

            transform.position = hand.position;

            line.positionCount = 2;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + depth * new Vector3(realDir.x, realDir.y, 0));
            

            Debug.DrawRay(transform.position, dir * bulletSpeed, Color.red);
            Debug.DrawRay(transform.position, realDir * bulletSpeed, Color.green);

            if (Input.GetMouseButtonDown(0))
            {  
                if (bulletNum <= 0)
                {
                    StartCoroutine("ResetBullet");
                    transform.position = savePosition.position;
                    Player.instance.isShoting = false;
                    line.positionCount = 0;
                    break;
                }
                //发射
                ShotIt(realDir,bulletSpeed);
                Player.instance.isShot = true;
                bulletNum--;
            }
            if(Input.GetMouseButtonUp(1))
            {
                transform.position = savePosition.position;
                Player.instance.isShoting = false;
                line.positionCount = 0;
                //结束瞄准
                break;
            }

            yield return null;
        }
    }
    
    public void ShotIt(Vector2 dir,float bulletSpeed)
    {
        bullet = Pool.instance.SetByPool();
        bullet.transform.position = transform.position;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody2D>().AddForce(bulletSpeed * dir, ForceMode2D.Impulse);
    }

    IEnumerator ResetBullet()       //重装弹
    {
        float timer = resetTime;
        canShot = false;
        while(true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                bulletNum = maxBulletNum;
                canShot = true;
                break;
            }
            yield return null;
        }
    }
}
