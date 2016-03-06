using UnityEngine;
using System.Collections;

public class WeaponRotating : MonoBehaviour {

    public float rotationSpeed = 1;
    MeshRenderer renderer;
    public GameObject canvas;

	// Use this for initialization
	void Start () {
        renderer = this.GetComponent<MeshRenderer>();
        StartCoroutine(EnableRenderer(1));
	}
	
	// Update is called once per frame
	void Update () {
        if(renderer.enabled == true)
            this.transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime * rotationSpeed));
	}

    public void EnableRendererButton(float time)
    {
        StartCoroutine(EnableRenderer(time));
    }

    IEnumerator EnableRenderer(float time)
    {
        yield return new WaitForSeconds(time);
        renderer.enabled = true;
        canvas.SetActive(true);
    }

}
