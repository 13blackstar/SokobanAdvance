using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class AudioSettings : MonoBehaviour
{
    [Header("Main Menu UI")]
    public Slider mainMenuMasterSlider; 
    public Toggle mainMenuMasterToggle; 
    public Slider mainMenuBgmSlider;
    public Slider mainMenuSfxSlider;
    public Toggle mainMenuBgmToggle;
    public Toggle mainMenuSfxToggle;

    [Header("Pause Menu UI")]
    public Slider pauseMenuMasterSlider; 
    public Toggle pauseMenuMasterToggle; 
    public Slider pauseMenuBgmSlider;
    public Slider pauseMenuSfxSlider;
    public Toggle pauseMenuBgmToggle;
    public Toggle pauseMenuSfxToggle;

    private FMOD.Studio.VCA masterVCA; 
    private FMOD.Studio.VCA bgmVCA;
    private FMOD.Studio.VCA sfxVCA;

    private float currentMasterVolume = 1f; 
    private float currentBgmVolume = 1f;
    private float currentSfxVolume = 1f;

    private bool isMasterOn = true; 
    private bool isBgmOn = true;
    private bool isSfxOn = true;

    void Start()
    {
        masterVCA = RuntimeManager.GetVCA("vca:/Master");
        bgmVCA = RuntimeManager.GetVCA("vca:/BGM");
        sfxVCA = RuntimeManager.GetVCA("vca:/SFX");

        SyncUI();

        AddAllListeners();
    }

    public void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume;
        if (isMasterOn) masterVCA.setVolume(currentMasterVolume);
        SyncUI();
    }

    public void SetBGMVolume(float volume)
    {
        currentBgmVolume = volume;
        if (isBgmOn) bgmVCA.setVolume(currentBgmVolume);
        SyncUI();
    }

    public void SetSFXVolume(float volume)
    {
        currentSfxVolume = volume;
        if (isSfxOn) sfxVCA.setVolume(currentSfxVolume);
        SyncUI();
    }

    public void ToggleMasterMute(bool isOn)
    {
        isMasterOn = isOn;
        masterVCA.setVolume(isMasterOn ? currentMasterVolume : 0f);
        SyncUI();
    }

    public void ToggleBGMMute(bool isOn)
    {
        isBgmOn = isOn;
        bgmVCA.setVolume(isBgmOn ? currentBgmVolume : 0f);
        SyncUI();
    }

    public void ToggleSFXMute(bool isOn)
    {
        isSfxOn = isOn;
        sfxVCA.setVolume(isSfxOn ? currentSfxVolume : 0f);
        SyncUI();
    }

    private void SyncUI()
    {
        RemoveAllListeners();

        if (mainMenuMasterSlider != null) mainMenuMasterSlider.value = currentMasterVolume;
        if (mainMenuMasterToggle != null) mainMenuMasterToggle.isOn = isMasterOn;
        if (mainMenuBgmSlider != null) mainMenuBgmSlider.value = currentBgmVolume;
        if (mainMenuBgmToggle != null) mainMenuBgmToggle.isOn = isBgmOn;
        if (mainMenuSfxSlider != null) mainMenuSfxSlider.value = currentSfxVolume;
        if (mainMenuSfxToggle != null) mainMenuSfxToggle.isOn = isSfxOn;

        if (pauseMenuMasterSlider != null) pauseMenuMasterSlider.value = currentMasterVolume;
        if (pauseMenuMasterToggle != null) pauseMenuMasterToggle.isOn = isMasterOn;
        if (pauseMenuBgmSlider != null) pauseMenuBgmSlider.value = currentBgmVolume;
        if (pauseMenuBgmToggle != null) pauseMenuBgmToggle.isOn = isBgmOn;
        if (pauseMenuSfxSlider != null) pauseMenuSfxSlider.value = currentSfxVolume;
        if (pauseMenuSfxToggle != null) pauseMenuSfxToggle.isOn = isSfxOn;

        AddAllListeners();
    }

    private void RemoveAllListeners()
    {
        if (mainMenuMasterSlider != null) mainMenuMasterSlider.onValueChanged.RemoveAllListeners();
        if (mainMenuMasterToggle != null) mainMenuMasterToggle.onValueChanged.RemoveAllListeners();
        if (mainMenuBgmSlider != null) mainMenuBgmSlider.onValueChanged.RemoveAllListeners();
        if (mainMenuBgmToggle != null) mainMenuBgmToggle.onValueChanged.RemoveAllListeners();
        if (mainMenuSfxSlider != null) mainMenuSfxSlider.onValueChanged.RemoveAllListeners();
        if (mainMenuSfxToggle != null) mainMenuSfxToggle.onValueChanged.RemoveAllListeners();

        if (pauseMenuMasterSlider != null) pauseMenuMasterSlider.onValueChanged.RemoveAllListeners();
        if (pauseMenuMasterToggle != null) pauseMenuMasterToggle.onValueChanged.RemoveAllListeners();
        if (pauseMenuBgmSlider != null) pauseMenuBgmSlider.onValueChanged.RemoveAllListeners();
        if (pauseMenuBgmToggle != null) pauseMenuBgmToggle.onValueChanged.RemoveAllListeners();
        if (pauseMenuSfxSlider != null) pauseMenuSfxSlider.onValueChanged.RemoveAllListeners();
        if (pauseMenuSfxToggle != null) pauseMenuSfxToggle.onValueChanged.RemoveAllListeners();
    }

    private void AddAllListeners()
    {
        if (mainMenuMasterSlider != null) mainMenuMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (mainMenuMasterToggle != null) mainMenuMasterToggle.onValueChanged.AddListener(ToggleMasterMute);
        if (mainMenuBgmSlider != null) mainMenuBgmSlider.onValueChanged.AddListener(SetBGMVolume);
        if (mainMenuBgmToggle != null) mainMenuBgmToggle.onValueChanged.AddListener(ToggleBGMMute);
        if (mainMenuSfxSlider != null) mainMenuSfxSlider.onValueChanged.AddListener(SetSFXVolume);
        if (mainMenuSfxToggle != null) mainMenuSfxToggle.onValueChanged.AddListener(ToggleSFXMute);

        if (pauseMenuMasterSlider != null) pauseMenuMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (pauseMenuMasterToggle != null) pauseMenuMasterToggle.onValueChanged.AddListener(ToggleMasterMute);
        if (pauseMenuBgmSlider != null) pauseMenuBgmSlider.onValueChanged.AddListener(SetBGMVolume);
        if (pauseMenuBgmToggle != null) pauseMenuBgmToggle.onValueChanged.AddListener(ToggleBGMMute);
        if (pauseMenuSfxSlider != null) pauseMenuSfxSlider.onValueChanged.AddListener(SetSFXVolume);
        if (pauseMenuSfxToggle != null) pauseMenuSfxToggle.onValueChanged.AddListener(ToggleSFXMute);
    }
}