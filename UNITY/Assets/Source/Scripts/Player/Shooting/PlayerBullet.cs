using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour
{
    public TeamType team;
    public int explode = 0;

    [SerializeField]
    private bool isParticle = false;
    [SerializeField]
    private GameObject petals;
    [SerializeField]
    private GameObject redFlower;
    [SerializeField]
    private GameObject blueFlower;

    private bool touchedFlower = false;
    private Transform flowerParent;

    private void Start()
    {
        flowerParent = GameObject.Find("Flowers").transform;
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Bullet")
            return;

        Vector3 pos = collider.contacts[0].point + (collider.contacts[0].normal * 0.2f);
        Quaternion rot = Quaternion.LookRotation(collider.contacts[0].normal);

        if (collider.gameObject.tag == "Player")
        {

        }
        else if (!touchedFlower)
        {
            if (team == TeamType.Blue)
            {
                GameObject obj = (GameObject)Instantiate(blueFlower, pos, rot);
                obj.transform.SetParent(flowerParent);
                GameManager.flowers.Add(obj);
            }
            else
            {
                GameObject obj = (GameObject)Instantiate(redFlower, pos, rot);
                obj.transform.SetParent(flowerParent);
                GameManager.flowers.Add(obj);
            }
        }

        if (explode > 0)
        {
            int len = explode;
            int counter = 0;
            for (int i = 0; i < len; i++)
            {
                Vector3 offset = collider.contacts[0].normal;
                GameObject copy = (GameObject)Instantiate(petals, pos + offset, rot);
                copy.GetComponent<PlayerBullet>().team = team;
                Vector3 force = collider.contacts[0].normal;
                force = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-10.0f, 10.0f));
                copy.GetComponent<Rigidbody>().AddForce(force * 100);
                counter++;
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Flower")
        {
            touchedFlower = true;
            Replace(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Flower")
        {
            touchedFlower = true;
            Replace(other.gameObject);
        }
    }

    private void Replace(GameObject other)
    {
        if (other.GetComponent<PlayerFlower>().team != team)
        {
            GameManager.flowers.Remove(other);
            Destroy(other);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        if (isParticle)
        {
            transform.LookAt(Camera.main.gameObject.transform.position);
        }
    }
}