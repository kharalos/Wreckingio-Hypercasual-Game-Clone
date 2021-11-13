using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject drop,gameOver;
    [SerializeField]
    private Text debugText;
    [SerializeField]
    private TextMeshProUGUI gameOverText;
    [SerializeField]
    private PlayerController pc;
    [SerializeField]
    private Arena arena;
    private int phase = 0;
    private int enemyNumber = 0;
    private bool startCounter = false;
    // Start is called before the first frame update
    void Start()
    {
        phase = 1;
        InvokeRepeating("SpawnDrop", 0.0f, 15f);
        InvokeRepeating("NextPhase", 10.0f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = pc.GetSwipeValues();
        if (startCounter && enemyNumber == 0) YouWon();
    }
    public void EnemyNumberChange(bool spawn)
    {
        startCounter = true;
        if (spawn) enemyNumber++;
        else enemyNumber--;
    }
    private void SpawnDrop()
    {
        Instantiate(drop, new Vector3(Random.Range(-30,30),30, Random.Range(-30, 30)), drop.transform.rotation);
    }
    private void NextPhase()
    {
        phase++;
        FindObjectOfType<Arena>().MakeItFall(phase);
    }
    private void YouWon()
    {
        startCounter = false;
        FindObjectOfType<AnimatorController>().Won();
        gameOver.SetActive(true);
        gameOverText.text = "You won.";
        Debug.Log("You won.");
    }
    public void YouLose()
    {
        foreach (AnimatorController ac in FindObjectsOfType<AnimatorController>())
        {
            ac.Won();
        }
        gameOver.SetActive(true);
        gameOverText.text = "You lost.";
        Debug.Log("You lost.");
    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
