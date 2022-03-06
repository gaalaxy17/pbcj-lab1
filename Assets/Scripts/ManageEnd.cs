using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageEnd : MonoBehaviour
{
    public GameObject message;
    public GameObject lastHiddenWord; // Palavra selecionada

    // Start is called before the first frame update
    void Start()
    {
        this.message = GameObject.Find("message");
        this.lastHiddenWord = GameObject.Find("lastHiddenWord");
        
        this.lastHiddenWord.GetComponent<Text>().text = PlayerPrefs.GetString("lastHiddenWord");
        
        PlayerPrefs.SetInt("score", 0);

        this.HandleStatusCondition(PlayerPrefs.GetString("status"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleStatusCondition(string statusValue)
    {
        if(statusValue == "victory"){
            GameObject.Find("congratulationsSound").GetComponent<AudioSource>().Play();
            GameObject.Find("successMusic").GetComponent<AudioSource>().Play();
            this.message.GetComponent<Text>().color = Color.green;
            this.message.GetComponent<Text>().text = "YOu WIN";
        }
        else{
            GameObject.Find("defeatMusic").GetComponent<AudioSource>().Play();
            this.message.GetComponent<Text>().color = Color.red; 
            this.message.GetComponent<Text>().text = "YOU LOOSE";
        }
    }

    public void StartGameWorld()
    {
        SceneManager.LoadScene("Lab1");
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("Lab1_endCredits");
    }
}
