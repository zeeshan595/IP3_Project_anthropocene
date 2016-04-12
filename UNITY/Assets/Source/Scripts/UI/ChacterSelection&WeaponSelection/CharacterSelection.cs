using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {

    public GameObject[] characters; // 0 Potatree, 1 other, 2 another other, 3 the last other
    public Button[] weaponButtons;
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
    bool inWeaponSelection = false;
    int currentSeleectedWeapon = 0;

    public void MoveLeft()
    {
        if (isRotatingLeft)
            return;

        characterIndex--;
        if (characterIndex >= characters.Length) characterIndex = 0;
        if (characterIndex < 0) characterIndex = characters.Length - 1;
        selectedCharacter = characters[characterIndex];
        sign = -1;
        isRotatingLeft = true;
        currentRot = angleValue - 120;
        Settings.character = (Character)characterIndex;
        ChangeCharacterName(characters[characterIndex].name);
    }

    public void MoveRight()
    {
        if (isRotating)
            return;

        characterIndex++;
        if (characterIndex >= characters.Length) characterIndex = 0;
        if (characterIndex < 0) characterIndex = characters.Length - 1;
        selectedCharacter = characters[characterIndex];
        isRotating = true;
        sign = 1;
        Settings.character = (Character)characterIndex;
        currentRot = angleValue + 120;
        ChangeCharacterName(characters[characterIndex].name);
    }

	void Update () 
    {
        if (!isRotatingLeft && !isRotating)
        {
            //Rotate right TODO change input to xbox axis
            float horizontal = InputManager.GetAxies(ControllerAxies.LeftStickX);

            if (!inWeaponSelection)
            {
                if (horizontal < -0.1f)
                {
                    if (!pressDownLeft)
                    {
                        MoveLeft();
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
                        MoveRight();
                        pressDownRight = true;
                    }
                }
                else
                    pressDownRight = false;

                if (Input.GetKeyDown(KeyCode.A))
                {
                    MoveLeft();
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    MoveRight();
                }

                if (InputManager.GetButtonDown(ControllerButtons.A)) //TODO change the logs to the appropriate characters once they are set on the Settings class
                {
                    characterSelectPanel.SetActive(false);
                    SelectButton();
                }
                selectedCharacter.transform.localRotation = Quaternion.Lerp(selectedCharacter.transform.localRotation, Quaternion.identity, Time.deltaTime * 2);
                if (InputManager.GetButtonDown(ControllerButtons.B)) //TODO change the logs to the appropriate characters once they are set on the Settings class
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
                }
            }
            else
            {
                selectedCharacter.transform.localRotation = Quaternion.Lerp(selectedCharacter.transform.localRotation, Quaternion.Euler(new Vector3(0, -50, 0)), Time.deltaTime * 2);
                if (InputManager.GetButtonDown(ControllerButtons.B)) //TODO change the logs to the appropriate characters once they are set on the Settings class
                {
                    weaponPanel.SetActive(false);
                    BackButton();
                }
                if (InputManager.GetButtonDown(ControllerButtons.A))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
                }

                if (horizontal < -0.1f)
                {
                    if (!pressDownLeft)
                    {
                        if (currentSeleectedWeapon > 0)
                        {
                            currentSeleectedWeapon--;
                            weaponButtons[currentSeleectedWeapon].onClick.Invoke();
                        }
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
                        if (currentSeleectedWeapon < weaponButtons.Length - 1)
                        {
                            currentSeleectedWeapon++;
                            weaponButtons[currentSeleectedWeapon].onClick.Invoke();
                        }
                        pressDownRight = true;
                    }
                }
                else
                    pressDownRight = false;
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
        Settings.character = Character.Potatree;
        selectedCharacter = characters[0];
    }

    public void SelectButton()
    {
        inWeaponSelection = true;
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
        inWeaponSelection = false;
        StartCoroutine(DisplayCharacterPanel());
    }

    IEnumerator DisplayCharacterPanel()
    {
        ani["Camera Zoom in"].speed = -1.0f;
        ani["Camera Zoom in"].time = ani["Camera Zoom in"].length;
        ani.Play();
        yield return new WaitForSeconds(0.75f);
        ani.Stop();
        characterSelectPanel.SetActive(true);
        ani["Camera Zoom in"].speed = 1.0f;
        ani["Camera Zoom in"].time = ani["Camera Zoom in"].length;
    }

    private void ChangeCharacterName(string characterName)
    {
        characterText.text = characterName;
    }
}
