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

    private bool touchedFlower = false;

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
                Instantiate(blueFlower, pos, rot);
            }
            else
            {
                Instantiate(redFlower, pos, rot);
            }

            GameManager.singleton.CmdCreateFlower(pos, rot, team);
        }

        if (explode)
        {
            int len = Random.Range(10, 20);
            int counter = 0;
            for (int i = 0; i < len; i++)
            {
                Vector3 offset = collider.contacts[0].normal;
                GameObject copy = (GameObject)Instantiate(petals, pos + offset, rot);
                copy.GetComponent<PlayerBullet>().team = team;
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
        {
            PlayerFlower plFlower = other.GetComponent<PlayerFlower>();
            if (plFlower.team != team && !touchedFlower)
            {
                int f = -1;

                for (int i = 0; i < GameManager.singleton.flowers.Count; i++)
                {
                    if (GameManager.singleton.flowers[f] == plFlower)
                    {
                        f = i;
                        break;
                    }
                }

                if (f != -1)
                   GameManager.singleton.CmdFlowerChange(f, team);
            }
            touchedFlower = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Flower")
        {
            PlayerFlower plFlower = other.GetComponent<PlayerFlower>();
            if (plFlower.team != team && !touchedFlower)
            {
                int f = -1;

                for (int i = 0; i < GameManager.singleton.flowers.Count; i++)
                {
                    if (GameManager.singleton.flowers[f] == plFlower)
                    {
                        f = i;
                        break;
                    }
                }

                if (f != -1)
                    GameManager.singleton.CmdFlowerChange(f, team);
            }
            touchedFlower = true;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isParticle)
        {
            transform.LookAt(Camera.main.gameObject.transform.position);
        }
    }
}