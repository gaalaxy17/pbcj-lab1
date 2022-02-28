using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Response
{
    public string[] words;
}

public class GameManager : MonoBehaviour
{
    public GameObject letter;
    public GameObject successMessage;
    public GameObject successMusic;
    public GameObject screenCenter;
    public GameObject backgroundMusic;
    public GameObject score;
    public GameObject tries;

    private GameObject scoreDisplay;
    private GameObject triesDisplay;

    private const int maxTries = 10;
    private int triesCount = 0;
    private int scoreCount = 0;
    private List<char> usedLetters = new List<char>();

    private string word;
    List<Letter> letters = new List<Letter>();
    List<char> wrongLetters = new List<char>();

    // Start is called before the first frame update
    void Start()
    {
        screenCenter = GameObject.Find("screenCenter");
        InitGame();
        InitLetters();
    }

    // Update is called once per frame
    void Update()
    {
        InputLetter();
    }

    // Instancia os espa�os (interroga��es na tela) para a palavra
    void InitLetters()
    {
        List<char> hiddenLetters = new List<char>(this.word.ToCharArray());
        for (int i = 0; i < hiddenLetters.Count; i++)
        {
            Letter l = new Letter();
            l.id = $"letter_{i + 1}";
            l.content = hiddenLetters[i];
            l.isHidden = true;
            
            Vector3 newPos = new Vector3(screenCenter.transform.position.x + ((i - hiddenLetters.Count / 2.0f) * 80), screenCenter.transform.position.y, screenCenter.transform.position.z);
            GameObject letter = (GameObject)Instantiate(this.letter, newPos, Quaternion.identity);
            letter.name = l.id;
            letter.transform.SetParent(GameObject.Find("Canvas").transform);

            this.letters.Add(l);
        }
    }

    // Atualiza pontua��o e tentativas
    void updateScoreAndTries()
    {
        scoreDisplay.GetComponent<Text>().text = $"Score {scoreCount}";
        triesDisplay.GetComponent<Text>().text = $"{triesCount} | {maxTries}";
    }

    // Instancia objetos iniciais para funcionamento do jogo
    void InitGame()
    {
        Vector3 newPos = new Vector3(screenCenter.transform.position.x, screenCenter.transform.position.y + 100, screenCenter.transform.position.z);
        GameObject bgm = (GameObject)Instantiate(this.backgroundMusic, newPos, Quaternion.identity);
        bgm.name = "backgroundMusic";
        bgm.transform.SetParent(GameObject.Find("Canvas").transform);

        createScoreAndTries();

        pickRandomWord();
    }

    // Escolhe palavra aleat�ria do arquivo words.txt
    private void pickRandomWord()
    {
        GameObject.Find("backgroundMusic").GetComponent<AudioSource>().enabled = true;
        StreamReader sr = new StreamReader("Assets/words.txt");
        string allWords = sr.ReadToEnd();
        this.word = allWords.Split(' ')[Random.Range(0, allWords.Split(' ').Length)];
        this.word = word.ToUpper();
    }

    // Instancia objetos de texto para tentativas e pontua��o
    private void createScoreAndTries()
    {
        float canvasHeight = GameObject.Find("Canvas").GetComponent<RectTransform>().rect.height;
        float canvasWidth = GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width;
        Vector3 topLeft = new Vector3(0, canvasHeight);
        Vector3 topright = new Vector3(canvasWidth, canvasHeight);
        scoreDisplay = (GameObject)Instantiate(this.score, topLeft, Quaternion.identity);
        scoreDisplay.transform.SetParent(GameObject.Find("Canvas").transform);
        triesDisplay = (GameObject)Instantiate(this.tries, topright, Quaternion.identity);
        triesDisplay.transform.SetParent(GameObject.Find("Canvas").transform);
        updateScoreAndTries();
    }

    // Trata input de letra
    void InputLetter()
    {
        if (Input.anyKeyDown && Input.inputString != "")
        {
            char input = Input.inputString.ToCharArray()[0];
            if (usedLetters.Contains(input)) return;
            usedLetters.Add(input);
            int asciInput = System.Convert.ToInt32(input);
            if (asciInput >= 97 && asciInput <= 122)
            {
                bool found = false;
                this.letters.ForEach(l =>
                {
                    if (l.content.ToString().ToUpper().Equals(input.ToString().ToUpper()))
                    {
                        l.isHidden = false;
                        GameObject.Find(l.id).GetComponent<Text>().text = input.ToString().ToUpper();
                        found = true;
                        scoreCount++;
                    }
                });
                if (!found && !this.wrongLetters.Contains(input))
                {
                    this.wrongLetters.Add(input);
                    triesCount++;
                }
            }
            CheckGameEnd();
            ShowWrongLetter();
            updateScoreAndTries();
        }
    }

    // Verifica se o jogo acabou
    void CheckGameEnd()
    {

        if (scoreCount == this.letters.Count)
        {
            PlayerPrefs.SetString("lastHiddenWord", this.word);
            PlayerPrefs.SetString("status", "victory");
            SceneManager.LoadScene("Lab1_end");
        }
        else if (triesCount >= maxTries)
        {
            PlayerPrefs.SetString("lastHiddenWord", this.word);
            PlayerPrefs.SetString("status", "defeat");
            SceneManager.LoadScene("Lab1_end");
        }
    }

    // Exibe letra errada na tela
    void ShowWrongLetter()
    {
        for (int i = 0; i < this.wrongLetters.Count; i++)
        {
            Vector3 newPos = new Vector3(screenCenter.transform.position.x - 500 + (i * 80), screenCenter.transform.position.y + 200, screenCenter.transform.position.z);
            string name = $"wrongLetter_{this.wrongLetters[i]}";

            if (!GameObject.Find(name))
            {
                GameObject sm = (GameObject)Instantiate(this.letter, newPos, Quaternion.identity);
                sm.name = name;
                sm.transform.SetParent(GameObject.Find("Canvas").transform);
                sm.GetComponent<Text>().text = this.wrongLetters[i].ToString().ToUpper();
                sm.GetComponent<Text>().color = Color.red;
            }
        }

    }

}
