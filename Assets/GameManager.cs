using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
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

    private HttpClient httpClient = new HttpClient();

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

    void InitLetters()
    {
        for (int i = 0; i < this.letters.Count; i++)
        {
            Vector3 newPos = new Vector3(screenCenter.transform.position.x + ((i - this.letters.Count / 2.0f) * 80), screenCenter.transform.position.y, screenCenter.transform.position.z);
            GameObject letter = (GameObject)Instantiate(this.letter, newPos, Quaternion.identity);
            letter.name = this.letters[i].id;
            letter.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    void InitGame()
    {
        Vector3 newPos = new Vector3(screenCenter.transform.position.x, screenCenter.transform.position.y + 100, screenCenter.transform.position.z);
        GameObject bgm = (GameObject)Instantiate(this.backgroundMusic, newPos, Quaternion.identity);
        bgm.name = "backgroundMusic";
        bgm.transform.SetParent(GameObject.Find("Canvas").transform);
        GameObject.Find("backgroundMusic").GetComponent<AudioSource>().enabled = true;

        HttpResponseMessage resp = this.httpClient.GetAsync("https://random-word-form.herokuapp.com/random/animal").GetAwaiter().GetResult();
        string JSONToParse = "{\"words\":" + resp.Content.ReadAsStringAsync().GetAwaiter().GetResult() + "}";

        Response respSer = JsonUtility.FromJson<Response>(JSONToParse);

        this.word = respSer.words[0];
        this.word = word.ToUpper();

        List<char> hiddenLetters = new List<char>(this.word.ToCharArray());
        for (int i = 0; i < hiddenLetters.Count; i++)
        {
            Letter l = new Letter();
            l.id = $"letter_{i + 1}";
            l.content = hiddenLetters[i];
            l.isHidden = true;

            this.letters.Add(l);
        }
    }

    void InputLetter()
    {
        if (Input.anyKeyDown)
        {
            char input = Input.inputString.ToCharArray()[0];
            int asciInput = System.Convert.ToInt32(input);
            //if(asciInput >= 97 && asciInput <= 122)
            //{
            bool found = false;
                this.letters.ForEach(l =>
                {
                    if (l.content.ToString().ToUpper().Equals(input.ToString().ToUpper()))
                    {
                        l.isHidden = false;
                        if(asciInput == 32)
                        {
                            GameObject.Find(l.id).GetComponent<Text>().text = "_";
                        }
                        else
                        {
                            GameObject.Find(l.id).GetComponent<Text>().text = input.ToString().ToUpper();
                        }
                        found = true;
                    }
                });
            if (!found)
            {
                if (!this.wrongLetters.Contains(input))
                {
                    this.wrongLetters.Add(input);
                }
                
            }
            //}
            CheckGameEnd();
            CheckWrongLetter();
        }
    }

    void CheckGameEnd()
    {
        int count = this.letters.Count;
        int matched = 0;

        this.letters.ForEach(l =>
        {
            if (!l.isHidden) matched++;
        });

        if (matched.Equals(count))
        {
            
            if (!GameObject.Find("successMusic"))
            {
                Vector3 newPos = new Vector3(screenCenter.transform.position.x, screenCenter.transform.position.y + 100, screenCenter.transform.position.z);
                GameObject sm = (GameObject)Instantiate(this.successMessage, newPos, Quaternion.identity);
                sm.name = "successMessage";
                sm.transform.SetParent(GameObject.Find("Canvas").transform);

                GameObject smusic = (GameObject)Instantiate(this.successMusic, newPos, Quaternion.identity);
                smusic.name = "successMusic";
                smusic.transform.SetParent(GameObject.Find("Canvas").transform);

                GameObject.Find("backgroundMusic").GetComponent<AudioSource>().enabled = false;
            }
        }
    }

    void CheckWrongLetter()
    {
        for (int i = 0; i < this.wrongLetters.Count; i++)
        {
            Vector3 newPos = new Vector3(screenCenter.transform.position.x - 500 + (i*80), screenCenter.transform.position.y + 200, screenCenter.transform.position.z);
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
