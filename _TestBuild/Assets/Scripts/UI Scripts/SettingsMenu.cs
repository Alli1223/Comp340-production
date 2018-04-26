using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public Toggle fullscreenToggle;
	public Dropdown resolutionDropdown;
	public Dropdown textureDropdown;
	public Dropdown antialiasingDropdown;
	public Dropdown vSyncDropdown;
	public Slider musicVolume;
	public Slider effectVolume;
	public Slider fovSlider;
	public Button applyOptions;
	public Button quitApp;

	public AudioSource musicSource;
	public AudioSource effectSource;
	public Resolution[] resolutions;
	public GameSettings gameSettings;
	public Camera mainCamera;


	void OnEnable()
	{
		gameSettings = new GameSettings();
		fullscreenToggle.onValueChanged.AddListener (delegate {OnFullscreenToggle();});
		resolutionDropdown.onValueChanged.AddListener (delegate {OnResolutionChange ();});	
		textureDropdown.onValueChanged.AddListener (delegate {OnTextureChange();});	
		antialiasingDropdown.onValueChanged.AddListener (delegate {OnAntialiasingChange();});	
		vSyncDropdown.onValueChanged.AddListener (delegate {OnVSyncChange();});	
		musicVolume.onValueChanged.AddListener (delegate {OnMusicChange ();});
		fovSlider.onValueChanged.AddListener (delegate {OnFOVChange ();});
		applyOptions.onClick.AddListener (delegate {OnApply ();});
		quitApp.onClick.AddListener (delegate {Quit ();});

		resolutions = Screen.resolutions;


		foreach (Resolution resolution in resolutions) {
			resolutionDropdown.options.Add (new Dropdown.OptionData (resolution.ToString ()));

			Debug.Log(resolution.width.ToString() + " x " + resolution.height.ToString() + " ( " + resolution.refreshRate.ToString()
				+ " ) ");
		}
	}

	public void OnFullscreenToggle()
	{
		gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
	}

	public void OnResolutionChange()
	{
		Screen.SetResolution (resolutions [resolutionDropdown.value].width, resolutions [resolutionDropdown.value].height, Screen.fullScreen);
	}

	public void OnTextureChange()
	{
		QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureDropdown.value;
	}

	public void OnAntialiasingChange()
	{
		QualitySettings.antiAliasing = gameSettings.antialiasing = antialiasingDropdown.value;
	}

	public void OnVSyncChange()
	{		
		QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
	}

	public void OnFOVChange()
	{
		mainCamera.fieldOfView = gameSettings.fOV = fovSlider.value;
	}

	public void OnMusicChange()
	{
		gameSettings.musicVolume = musicSource.volume = musicVolume.value;
	}
	public void OnEffectChange()
	{
		gameSettings.musicVolume = effectSource.volume = musicVolume.value;
	}
	public void OnApply()
	{
		SaveSettings ();
	}

	public void SaveSettings()
	{
		string jsonData = JsonUtility.ToJson (gameSettings, true);
		File.WriteAllText(Application.persistentDataPath + "/Gamesettings.json", jsonData);
	}
	public void LoadSettings()
	{
		gameSettings = JsonUtility.FromJson<GameSettings> (Application.persistentDataPath + "/Gamesettings.json");
		musicVolume.value = gameSettings.musicVolume;
		resolutionDropdown.value = gameSettings.resolutionIndex;
		fullscreenToggle.isOn = gameSettings.fullscreen;
		textureDropdown.value = gameSettings.textureQuality;
		antialiasingDropdown.value = gameSettings.antialiasing;
		vSyncDropdown.value = gameSettings.vSync;
	}

	public void Quit()
	{
		if (UnityEditor.EditorApplication.isPlaying = false) {
		} else {
			Application.Quit ();
		}
	}
}
