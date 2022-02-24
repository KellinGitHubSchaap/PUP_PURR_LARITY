using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [SerializeField] private float m_maxScore = 100;   // 1 Billion > 100 Milion > 10 Milion
    public float m_currentCatScore = 0;
    public float m_currentBirdScore = 0;

    private void Start()
    {
        UpdateCatDogBar(0,0);
    }

    // Update the visual Chart seen in the office
    public void UpdateCatDogBar(int dogValue, int fishValue)
    {
        m_currentCatScore += dogValue;

        if (m_currentCatScore < 0) { m_currentCatScore = 0; }
        else if (m_currentCatScore >= m_maxScore) { SceneManager.LoadScene("PlayerWin"); }
        m_catDogBar.fillAmount = m_currentCatScore / m_maxScore;

        m_currentBirdScore += fishValue;

        if (m_currentBirdScore < 0) { m_currentBirdScore = 0; }
        else if (m_currentBirdScore >= m_maxScore) { SceneManager.LoadScene("PlayerLose"); }
        m_birdFishBar.fillAmount = m_currentBirdScore / m_maxScore;
    }
}
