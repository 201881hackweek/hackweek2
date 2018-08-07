using UnityEngine;

public class Jumped : MonoBehaviour {

    public GameObject parent;
    LayerMask l1, l2;
	// Use this for initialization
	void Start () {
        l1 = LayerMask.GetMask("People");
        l2 = LayerMask.GetMask("Jumped");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Monster")
            Physics2D.IgnoreLayerCollision(l1, l2);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Monster")
            Physics2D.IgnoreLayerCollision(l1, l2);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Monster")
            return;
    }
}
