using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public Toggle fullscreenToggle;
	public Dropdown resolutionDropdown;
	public Slider musicVolume;
	public Button applyOptions;

	public AudioSource musicSource;
	public Resolution[] resolutions;
	public GameSettings gameSettings;

	void OnEnable()
	{
		gameSettings = new GameSettings();

		fullscreenToggle.onValueChanged.AddListener (delegate {OnFullscreenToggle();});
		resolutionDropdown.onValueChanged.AddListener (delegate {OnResolutionChange ();});	
		musicVolume.onValueChanged.AddListener (delegate {OnMusicChange ();});
		applyOptions.onClick.AddListener (delegate {OnApply ();});

		resolutions = Screen.resolutions;
		foreach (Resolution resolution in resolutions) {
			resolutionDropdown.options.Add (new Dropdown.OptionData (resolution.ToString ()));
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


	public void OnMusicChange()
	{
		gameSettings.musicVolume = musicSource.volume = musicVolume.value;
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
	}

	public void Quit()
	{
		if (UnityEditor.EditorApplication.isPlaying = false) {
		} else {
			Application.Quit ();
		}
	}
}
