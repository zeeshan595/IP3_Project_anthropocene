using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour {

    public GameObject[] characters;
    private int characterIndex = 0;
    public GameObject platform;
    public GameObject selectedCharacter;
    public float speed = 1;
    public float rotationTarget;
    public bool isRotating = false;
    public bool isRotatingLeft = false;
    private int sign = 1;
    public float angleValue;

    public float currentRot;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (characterIndex >= characters.Length) characterIndex = 0;
        if (characterIndex < 0) characterIndex = characters.Length - 1;
        selectedCharacter = characters[characterIndex];

        if(!isRotatingLeft && !isRotating)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isRotating = true;
                sign = 1;
                characterIndex++;
                currentRot = angleValue + 90;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                sign = -1;
                isRotatingLeft = true;
                characterIndex--;
                currentRot = angleValue - 90;
            }

            if (Input.GetKey(KeyCode.O))
            {
                selectedCharacter.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
            }

            if (Input.GetKey(KeyCode.P))
            {
                selectedCharacter.transform.Rotate(new Vector3(0, -90 * Time.deltaTime, 0));
            }
        }

        

        if(isRotatingLeft)
        {
            //if (angleValue == -360) 
            //{
            //    platform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            //    angleValue = 0;
            //}
            if (angleValue > currentRot)
            {
                platform.transform.Rotate(new Vector3(0, 90 * Time.deltaTime * speed * sign, 0));
                angleValue += 90 * Time.deltaTime * speed * sign;
            }
            if (angleValue < currentRot)
            {
                platform.transform.rotation = Quaternion.Euler(new Vector3(0, currentRot, 0));
                angleValue = currentRot;
                isRotatingLeft = false;
            }
        }

        if(isRotating)
        {
            //if (angleValue == 360) 
            //{
            //    platform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            //    angleValue = 0;
            //}
            if (angleValue < currentRot)
            {
                platform.transform.Rotate(new Vector3(0, 90 * Time.deltaTime * speed * sign, 0));
                angleValue += 90 * Time.deltaTime * speed * sign;
            }
            if (angleValue > currentRot)
            {
                platform.transform.rotation = Quaternion.Euler(new Vector3(0, currentRot, 0));
                angleValue = currentRot;
                isRotating = false;
            }

        }


	}




}
