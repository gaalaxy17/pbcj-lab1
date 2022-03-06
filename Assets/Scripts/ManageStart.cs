using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

// Gerencia o início do jogo
public class ManageStart : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Carrega primeira cena
    public void StartGameWorld()
    {
        SceneManager.LoadScene("Lab1");
    }
}
