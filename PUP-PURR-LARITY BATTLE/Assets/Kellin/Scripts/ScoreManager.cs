using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    public Image m_catDogBar;
    public Image m_birdFishBar;

    [SerializeField] private float m_maxScore = 790;   // 1 Billion > 100 Milion > 10 Milion
    public float m_currentCatScore = 0;
    public float m_currentBirdScore = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            UpdateCatDogBar(20, -15);
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {
            UpdateCatDogBar(-10, 10);
        }
    }

    // Update the visual Chart seen in the office
    public void UpdateCatDogBar(int dogValue, int fishValue)
    {
        m_currentCatScore += dogValue;

        if (m_currentCatScore < 0) { m_currentCatScore = 0; }

        m_catDogBar.fillAmount = m_currentCatScore / m_maxScore;

        m_currentBirdScore += fishValue;

        if (m_currentBirdScore < 0) { m_currentBirdScore = 0; }
        m_birdFishBar.fillAmount = m_currentBirdScore / m_maxScore;
    }
}
