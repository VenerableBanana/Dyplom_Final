using System;
using System.IO;
using System.Text.RegularExpressions;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SandboxSettingsScript : MonoBehaviour
{
    public Slider Slider;
    public InputField HeightInput;
    public InputField WidthInput;
    public InputField SeedInput;
    public InputField ChunksInput;
    public Text ErrorLabel;

    private string _file = "SandboxSettings.json";

    public void StartSandbox()
    {
        if (SaveJSONFile())
        { 
            SceneManager.LoadScene(3);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
    }

    private bool SaveJSONFile()
    {
        var fillPercentage = Slider.value;
        var height = HeightInput.text;
        var width = WidthInput.text;
        var seed = SeedInput.text;
        var chunks = ChunksInput.text;

        Regex _isNumber = new Regex(@"^\d+$");

        if (string.IsNullOrEmpty(height) ||
            string.IsNullOrEmpty(width))
        {
            ErrorLabel.text = "HEIGHT, WIDTH AND CHUNKS CAN NOT BE EMPTY";
            return false;
        }

        if (!_isNumber.IsMatch(height) ||
            !_isNumber.IsMatch(width) ||
            !_isNumber.IsMatch(chunks))
        {
            ErrorLabel.text = "HEIGHT, WIDTH AND CHUNKS MUST BE NUMERIC";
            return false;
        }

        if (Int32.Parse(chunks) > Int32.Parse(height) ||
            Int32.Parse(chunks) > Int32.Parse(width))
        {
            ErrorLabel.text = "HEIGHT AND WIDTH MUST BE GREATER THEN CHUNKS";
            return false;
        }

        SandboxData data = new SandboxData();

        data.fillPercentage = fillPercentage;
        data.height = height;
        data.width = width;
        data.seed = seed;
        data.chunks = chunks;

        var json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.dataPath + "/Resources/SandboxSettings.json", json);

        ErrorLabel.text = "";

        return true;
    }

}
