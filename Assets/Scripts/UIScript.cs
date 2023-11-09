using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    //Game options and point
    private int enemyCount;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI hpText;
    public GameObject resetButton;
    public GameObject quitButton;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public int hpCount;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = 7;
        hpCount = player.GetComponent<HealthScript>().Hp;
    }

    // Update is called once per frame
    void Update()
    {
        hpCount = player.GetComponent<HealthScript>().Hp;
        SetCountText();
    }

    void SetCountText()
    {
        countText.text = "Enemies Left: " + enemyCount.ToString();
        hpText.text = "HP: " + hpCount.ToString();
        if (enemyCount <= 0)
        {
            winTextObject.SetActive(true);
            SetButtonsOn();
        }

        if (hpCount <= 0)
        {
            loseTextObject.SetActive(true);
            SetButtonsOn();
        }
    }

    public void EnemyKilled()
    {
        enemyCount--;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetButtonsOn()
    {
        resetButton.SetActive(true);
        quitButton.SetActive(true);
    }


}
