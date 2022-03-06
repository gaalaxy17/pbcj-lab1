using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Gerencia a tela de créditos
public class ManageCredits : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
    }

    // Retorna para a cena de início
    public void Menu()
    {
        SceneManager.LoadScene("Lab1_start");
    }

}
