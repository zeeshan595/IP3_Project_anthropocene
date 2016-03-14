using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public TeamType team;
    public bool explode = false;

    [SerializeField]
    private bool isParticle = false;
    [SerializeField]
    private GameObject petals;
    [SerializeField]
    private GameObject redFlower;
    [SerializeField]
    private GameObject blueFlower;

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Bullet")
            return;

        Vector3 pos = collider.contacts[0].point + (collider.contacts[0].normal * 0.2f);
        Quaternion rot = Quaternion.LookRotation(collider.contacts[0].normal);

        if (collider.gameObject.tag == "Player")
        {

        }
        else
        {
            if (team == TeamType.Blue)
            {
                Instantiate(redFlower, pos, rot);
            }
            else
            {
                Instantiate(blueFlower, pos, rot);
            }
        }

        if (explode)
        {
            int len = Random.Range(10, 20);
            int counter = 0;
            for (int i = 0; i < len; i++)
            {
                Vector3 offset = Vector3.zero;
                if (i < len / 3)
                {
                    offset += new Vector3((i * 0.21f) - len / 6, -0.5f, 0);
                    if (i + 1 >= len / 3)
                        counter = 0;
                }
                else if (i > (len / 3) * 2)
                {
                    offset += new Vector3((i * 0.21f) - len / 6, 0, 0);
                    if (i + 1 >= (len / 3) * 2)
                        counter = 0;
                }
                else
                {
                    offset += new Vector3((i * 0.21f) - len / 6, 0.5f, 0);
                }
                GameObject copy = (GameObject)Instantiate(petals, pos + offset, rot);
                Vector3 force = collider.contacts[0].normal;
                force += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-0.5f, 1.0f), Random.Range(-1.0f, 1.0f));
                copy.GetComponent<Rigidbody>().AddForce(force * 100 * Random.Range(1.0f, 2.0f));
                counter++;
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Flower")
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