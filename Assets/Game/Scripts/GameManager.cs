using FMODUnity;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public GameObject[] levelPrefabs;
    private int currentLevelIndex = 0;
    private GameObject currentLevelInstance;

    [Header("Win Condition")]
    private int totalTargets = 0;
    public int filledTargets = 0;

    [Header("Menu UI Settings")]
    public GameObject mainMenuPanel;    
    public GameObject levelSelectPanel; 
    public GameObject inGameUIPanel;
    public GameObject mainMenuSettingsPanel;

    [Header("In-Game UI Settings")]
    public GameObject winPanel;
    public GameObject pausePanel;

    [Header("Audio Settings")]
    public EventReference bgmEvent;
    public EventReference sfxWin;
    private FMOD.Studio.EventInstance bgmInstance; 

    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (!bgmEvent.IsNull)
        {
            bgmInstance = RuntimeManager.CreateInstance(bgmEvent);
            bgmInstance.start();
        }
        ShowMainMenu();
    }

    private void Update()
    {
        if (inGameUIPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.R)) RestartLevel();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }
    }
    private void OnDestroy()
    {
        if (bgmInstance.isValid())
        {
            bgmInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            bgmInstance.release();
        }
    }
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        inGameUIPanel.SetActive(false);
        winPanel.SetActive(false);
        pausePanel.SetActive(false);

        if (currentLevelInstance != null) Destroy(currentLevelInstance);

        if (mainMenuSettingsPanel != null) mainMenuSettingsPanel.SetActive(false);

        if (currentLevelInstance != null) Destroy(currentLevelInstance);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ShowLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void GoToMenu()
    {
        ShowMainMenu();
    }
    public void OpenMainMenuSettings()
    {
        mainMenuSettingsPanel.SetActive(true);
    }

    public void CloseMainMenuSettings()
    {
        mainMenuSettingsPanel.SetActive(false);
    }
    public void PauseGame()
    {
        if (winPanel.activeSelf) return;
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadLevel(int index)
    {
        CancelInvoke(nameof(CheckWinCondition));
        Time.timeScale = 1f;
        isPaused = false;

        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        inGameUIPanel.SetActive(true);

        winPanel.SetActive(false);
        pausePanel.SetActive(false);

        if (currentLevelInstance != null) Destroy(currentLevelInstance);

        totalTargets = 0;
        filledTargets = 0;

        if (index < levelPrefabs.Length)
        {
            currentLevelInstance = Instantiate(levelPrefabs[index], Vector3.zero, Quaternion.identity);
            currentLevelIndex = index;
        }
        else
        {
            Debug.Log("ĐÃ PHÁ ĐẢO TOÀN BỘ GAME!");
            ShowMainMenu();
        }
    }

    public void RestartLevel() { LoadLevel(currentLevelIndex); }
    public void NextLevel() { LoadLevel(currentLevelIndex + 1); }

    public void RegisterTarget() { totalTargets++; }
    public void TargetFilled()
    {
        filledTargets++;
        CancelInvoke(nameof(CheckWinCondition));
        Invoke(nameof(CheckWinCondition), 0.25f);
    }
    public void TargetEmptied() { filledTargets--; }

    private void CheckWinCondition()
    {
        if (filledTargets >= totalTargets && totalTargets > 0)
        {
            winPanel.SetActive(true);

            if (!sfxWin.IsNull)
            {
                RuntimeManager.PlayOneShot(sfxWin);
            }
        }
    }
}   