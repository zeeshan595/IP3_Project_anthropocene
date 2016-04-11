using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public GameObject characterTextObject;
    private Text characterText;
    public GameObject characterSelectPanel;
    Animation ani;

    void MoveLeft()
    {
        characterIndex--;
        if (characterIndex >= characters.Length) characterIndex = 0;
        if (characterIndex < 0) characterIndex = characters.Length - 1;
        selectedCharacter = characters[characterIndex];
        sign = -1;
        isRotatingLeft = true;
        currentRot = angleValue - 90;
        Settings.character = (Character)characterIndex;
        ChangeCharacterName(characters[characterIndex].name);
    }

    void MoveRight()
    {
        characterIndex++;
        if (characterIndex >= characters.Length) characterIndex = 0;
        if (characterIndex < 0) characterIndex = characters.Length - 1;
        selectedCharacter = characters[characterIndex];
        isRotating = true;
        sign = 1;
        Settings.character = (Character)characterIndex;
        currentRot = angleValue + 90;
        ChangeCharacterName(characters[characterIndex].name);
    }

	void Update () 
    {
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
                    Settings.character = Character.Rak;
                    weaponRotating.EnableRendererButton(1);
                }
                else if (characterIndex == 2)
                {
                    Settings.character = Character.Fishy;
                    weaponRotating.EnableRendererButton(1);
                }
                else if (characterIndex == 3)
                {
                    Settings.character = Character.JackieChan;
                    weaponRotating.EnableRendererButton(1);
                }
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
        characterText = characterTextObject.GetComponent<Text>();
        ani = GetComponent<Animation>();
    }

    public void SelectButton()
    {
        StartCoroutine(DisplayWeaponPanel());
    }

    IEnumerator DisplayWeaponPanel()
    {
        ani.Rewind();
        ani.Play();
        yield return new WaitForSeconds(0.75f);
        weaponPanel.SetActive(true);
    }

    public void BackButton()
    {
        StartCoroutine(DisplayCharacterPanel());
    }

    IEnumerator DisplayCharacterPanel()
    {
        ani["Camera Zoom in"].speed = -1.0f;
        ani["Camera Zoom in"].time = ani["Camera Zoom in"].length;
        ani.Play();
        yield return new WaitForSeconds(0.75f);
        characterSelectPanel.SetActive(true);
        ani["Camera Zoom in"].speed = 1.0f;
        ani["Camera Zoom in"].time = ani["Camera Zoom in"].length;
    }

    private void ChangeCharacterName(string characterName)
    {
        characterText.text = characterName;
    }

}
