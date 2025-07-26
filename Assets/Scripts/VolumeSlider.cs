using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
	[SerializeField] private Slider volumeSlider;

	private void Start()
	{
		volumeSlider.onValueChanged.AddListener(SetVolume);

		float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = volume;
		SetVolume(volume);
	}

    public void SetVolume(float sliderValue)
    {
        AudioListener.volume = sliderValue;
		PlayerPrefs.SetFloat("Volume", sliderValue);
	}
}
