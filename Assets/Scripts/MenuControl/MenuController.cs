using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]

public class MenuController : MonoBehaviour {
    Animator anim;
    public GameObject TeamPanel;
    public GameObject CreditsPanel;
    public GameObject AssetsPanel;
    public GameObject MainPanel;
    private PostProcessVolume effect;
    private GameObject crosshair;
    public GameObject OptionsPanel;
    public AudioMixer audioMixer;
    public bool EnableResume = true;

    public GameObject pauseFirstButton, creditsFirstButton, optionsFirstButton, teamMembersFirstButton, assetsUsedFirstButton, startFirstButton;

    void Awake() {
        effect = GameObject.Find("Main Camera").GetComponent<PostProcessVolume>();
        crosshair = GameObject.Find("Crosshair");
        anim = GetComponent<Animator>();
        if (!EnableResume) {
            transform.Find("Main/MainPanel/Resume").gameObject.SetActive(false);
            transform.Find("Main/MainPanel/StartGame").gameObject.SetActive(true);
        }
    }

    void Update() {
        if (!Input.GetButton("Fire4"))
        {
            if (!Input.GetKeyUp(KeyCode.Escape) || !EnableResume)
                return;
        }
        UpdateState(!transform.parent.GetComponent<CanvasGroup>().interactable);
    }

    private void UpdateState(bool enabled) {
        transform.parent.GetComponent<CanvasGroup>().interactable = enabled;
        transform.parent.GetComponent<CanvasGroup>().alpha =
            transform.GetComponent<CanvasGroup>().alpha = enabled ? 1 : 0;
        HidePanels();
        MainPanel.SetActive(enabled);
        Time.timeScale = enabled ? 0f : 1f;
        Cursor.visible = enabled;
        AudioListener.pause = enabled;
        Cursor.lockState = enabled ? CursorLockMode.None : CursorLockMode.Locked;
        effect.enabled = enabled;
        crosshair.SetActive(!enabled);

        //set the selector on first option of menu for pause
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void HidePanels() {
        CreditsPanel.SetActive(false);
        TeamPanel.SetActive(false);
        AssetsPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        MainPanel.SetActive(false);
    }

    public void SendEscape() {
        UpdateState(false);
    }

    public void OpenOptions() {
        HidePanels();
        OptionsPanel.SetActive(true);
        //set the selector on first option of menu for pause
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void OpenCredits() {
        HidePanels();
        CreditsPanel.SetActive(true);
        //set the selector on first option of menu for pause
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }

    public void OpenTeam() {
        HidePanels();
        TeamPanel.SetActive(true);
        //set the selector on first option of menu for pause
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(teamMembersFirstButton);
    }

    public void OpenAssets() {
        HidePanels();
        AssetsPanel.SetActive(true);
        //set the selector on first option of menu for pause
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(assetsUsedFirstButton);
    }


    public void OpenMainMenu() {
        HidePanels();
        MainPanel.SetActive(true);
        //set the selector on first option of menu for pause
        EventSystem.current.SetSelectedGameObject(null);
        if (!EnableResume)
        {
            EventSystem.current.SetSelectedGameObject(startFirstButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene("Level1");
    }

    public void BackToMainMenu() {
        HidePanels();
        MainPanel.SetActive(true);
        //set the selector on first option of menu for pause
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StartGame() {
        LevelState.Instance.PlaySound("menu_option", gameObject);
        SceneManager.LoadScene("Level1");
    }

    public void SetMouseSpeed(float speed) {
        LevelState.Instance.MouseSpeed = speed;
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
    }
}
