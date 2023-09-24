using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text Text;
    public Image fpsBG;

    private Dictionary<int, string> CachedNumberStrings = new();
    private int[] _frameRateSamples;
    public int _cacheNumbersAmount = 950;
    private int _averageFromAmount = 65;
    private int _averageCounter = 0;
    private int _currentAveraged;
    public string _text = string.Empty;

    public static FPSCounter Instance;

    void Awake()
    {
        Instance = this;
        fpsBG.enabled = false;
        Text.text = "";
        // Cache strings and create array
        for (int i = 0; i < _cacheNumbersAmount; i++)
        {
            CachedNumberStrings[i] = i.ToString();
        }
        _frameRateSamples = new int[_averageFromAmount];
    }

    private void Start()
    {
        if (!SettingsManager.showFPS)
        {
            fpsBG.enabled = false; Text.text = "";
            this.enabled = false;
        }
    }
    private void OnEnable()
    {
        InvokeRepeating(nameof(DoFPS), 0.1f, 0.1f);
    }

    private void OnDisable()
    {
        Text.text = "";
        fpsBG.enabled = false;
    }
    float average = 0f;
    private void Update()
    {

        _frameRateSamples[_averageCounter] = (int)Math.Round(1f / Time.unscaledDeltaTime);

        // Average
        {
            average = 0f;

            for (int i = 0; i < _frameRateSamples.Length; i++)
            {
                average += _frameRateSamples[i];
            }

            _currentAveraged = (int)Math.Round(average / _averageFromAmount);
            _averageCounter = (_averageCounter + 1) % _averageFromAmount;
        }
        _text = _currentAveraged < _cacheNumbersAmount && _currentAveraged > 0
                ? CachedNumberStrings[_currentAveraged]
                : _currentAveraged < 0
                    ? "< 0"
                    : _currentAveraged > _cacheNumbersAmount
                        ? $"> {_cacheNumbersAmount}"
                        : "-1"; ;
    }
    void DoFPS()
    {
        if (!SettingsManager.showFPS) { fpsBG.enabled = false; Text.text = ""; this.enabled = false; return;  }
        if (Time.timeScale == 0f) return;
        Text.text = $"{_text} fps";
        fpsBG.enabled = true;
    }
}