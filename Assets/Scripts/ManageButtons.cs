using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageButtons : MonoBehaviour
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
            this.message.GetComponent<Text>().color = Color.green;
            this.message.GetComponent<Text>().text = "U WIN";
        }
        else{
            this.message.GetComponent<Text>().color = Color.red; 
            this.message.GetComponent<Text>().text = "BANIDO";
        }
    }

    public void StartGameWorld()
    {
        SceneManager.LoadScene("Lab1");
    }
}
