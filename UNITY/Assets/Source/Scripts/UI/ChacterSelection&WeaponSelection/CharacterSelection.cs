using UnityEngine;
using System.Collections;

public class CharacterSelection : MonoBehaviour {

    public GameObject[] characters; // 0 Potatree, 1 other, 2 another other, 3 the last other
    public GameObject platform;
    public GameObject selectedCharacter;
    private int characterIndex = 0;
    public GameObject weaponSelectPanel;

    public float speed = 1;
    private bool isRotating = false;
    private bool isRotatingLeft = false;
    private int sign = 1;
    private float angleValue;
    private float currentRot;
    WeaponRotating weaponRotating;

    public GameObject weaponPanel;
    private bool pressDownLeft = false;
    private bool pressDownRight = false;

    void MoveLeft()
    {
        sign = -1;
        isRotatingLeft = true;
        characterIndex--;
        currentRot = angleValue - 90;
        Settings.character = (Character)characterIndex;
    }

    void MoveRight()
    {
        isRotating = true;
        sign = 1;
        characterIndex++;
        Settings.character = (Character)characterIndex;
        currentRot = angleValue + 90;
    }

	void Update () 
    {
        if (characterIndex >= characters.Length) characterIndex = 0;
        if (characterIndex < 0) characterIndex = characters.Length - 1;
        selectedCharacter = characters[characterIndex];

        if (!isRotatingLeft && !isRotating)
        {
            //Rotate right TODO change input to xbox axis
            float horizontal = InputManager.GetAxies(ControllerAxies.LeftStickX);

            if (horizontal < -0.1f)
            {
                if (!pressDownLeft)
                {
                    MoveRight();
                    pressDownLeft = true;
                }
            }
            else
                pressDownLeft = false;


            //Rotate Left TODO change input to xbox axis
            if (horizontal > 0.1f)
            {
                if (!pressDownRight)
                {
                    MoveLeft();
                    pressDownRight = true;
                }
                else
                    pressDownRight = false;
            }

            if (Input.GetKey(KeyCode.O))
            {
                selectedCharacter.transform.Rotate(new Vector3(0, 90 * Time.deltaTime, 0));
            }

            if (Input.GetKey(KeyCode.P))
            {
                selectedCharacter.transform.Rotate(new Vector3(0, -90 * Time.deltaTime, 0));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }

            //Select the character
            if (InputManager.GetButton(ControllerButtons.A)) //TODO change the logs to the appropriate characters once they are set on the Settings class
            {
                if (characterIndex == 0)
                {
                    Settings.character = Character.Potatree;
                    weaponRotating.EnableRendererButton(1);
                }
                else if (characterIndex == 1)
                {
                    Settings.character = Character.RakTheFish;
                    weaponRotating.EnableRendererButton(1);
                }
                else if (characterIndex == 2)
                    Debug.Log("another other character");
                else if (characterIndex == 3)
                    Debug.Log("Last other character");
            }
        }

        

        if(isRotatingLeft)
        {
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

    void Start()
    {
        //StartCoroutine(displayWeaponPanel());
    }

    IEnumerator displayWeaponPanel()
    {
        yield return new WaitForSeconds(1);
        weaponPanel.SetActive(true);
    }


}
