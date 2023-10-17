
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO;
using TMPro;


public class CSVReader : MonoBehaviour
{

    public Button choiceBtn1;
    public Button choiceBtn2;
    public Button choiceBtn3;

    public Button voiceBtn;

    public Button return1Btn;
    public Button return2Btn;

    public Image ansImage;
    public Image borderImage;

    public GameObject losingScreen;
    public GameObject winningScreen;

    public VideoPlayer videoPlayer;

    public AudioSource bgmSource;
    public AudioSource voiceSource;
    public AudioSource sfxSource;

    public static float voiceVolume = 2.5f;

    public static List<string> answers = new List<string>();
    public static List<string> images = new List<string>();
    public static List<string> voices = new List<string>();
    public static List<string> randoms = new List<string>(); //random choices

    private static string currAnswer;

    // round no. and lose stats
    public static int numRound = 0;
    public static int maxRound = 5;
    public static int life = 3;
    public static int maxLife = 3;

    // public static string currPath = System.IO.Directory.GetCurrentDirectory(); 
    

    // Start is called before the first frame update
    void Start()
    {

        Button voiceButton = voiceBtn.GetComponent<Button>();
        voiceButton.onClick.AddListener(voiceButtonFunc);

        Button choiceButton1 = choiceBtn1.GetComponent<Button>();
        choiceButton1.onClick.AddListener( delegate {choiceButtonFunc(1); });

        Button choiceButton2 = choiceBtn2.GetComponent<Button>();
        choiceButton2.onClick.AddListener( delegate {choiceButtonFunc(2); });

        Button choiceButton3 = choiceBtn3.GetComponent<Button>();
        choiceButton3.onClick.AddListener( delegate {choiceButtonFunc(3); });

        Button return1 = return1Btn.GetComponent<Button>();
        return1.onClick.AddListener(goToMainMenu);

        Button return2 = return2Btn.GetComponent<Button>();
        return2.onClick.AddListener(goToMainMenu);

        readTextFile();
        setStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void readTextFile(){
        TextAsset txt = (TextAsset) Resources.Load("test");
        string content = txt.text;
        string[] lines = content.Split("\n");
        // string[] lines = File.ReadAllLines("Assets/Resources/test.txt");
        
        // print(

        try{
            foreach (string line in lines){
                answers.Add(line);
                images.Add("images/" + line); // + ".jpg"
                voices.Add("voices/" + line);
                randoms.Add(line);
            }

        }

        catch{
            print("Something wrong with the text file.");
        }
    
    }

    void setStage(){
        var randInt = Random.Range(0, answers.Count);
        var randInt1 = Random.Range(0, randoms.Count); // to use for random choices
        var randInt2 = Random.Range(0, randoms.Count);

        while(randInt1 == randInt || randInt2 == randInt){
            randInt1 = Random.Range(0, randoms.Count);
            randInt2 = Random.Range(0, randoms.Count);
        }

        var randPlace = Random.Range(0, 3); // placement of choices - note it is exclusive max


        // print(randInt);
        // print(randInt1);
        // print(randInt2);
        // print(randPlace);

        // show answer and image and voice
        string answer = answers[randInt];
        string image = images[randInt];
        string voice = voices[randInt];

        currAnswer = answer;

        // show choices
        switch(randPlace){
            case 0:
                choiceBtn1.GetComponentInChildren<TextMeshProUGUI>().text = answer;
                choiceBtn2.GetComponentInChildren<TextMeshProUGUI>().text = randoms[randInt1];
                choiceBtn3.GetComponentInChildren<TextMeshProUGUI>().text = randoms[randInt2];
                break;

            case 1:
                choiceBtn2.GetComponentInChildren<TextMeshProUGUI>().text = answer;
                choiceBtn1.GetComponentInChildren<TextMeshProUGUI>().text = randoms[randInt1];
                choiceBtn3.GetComponentInChildren<TextMeshProUGUI>().text = randoms[randInt2];
                break;

            case 2:
                choiceBtn3.GetComponentInChildren<TextMeshProUGUI>().text = answer;
                choiceBtn1.GetComponentInChildren<TextMeshProUGUI>().text = randoms[randInt1];
                choiceBtn2.GetComponentInChildren<TextMeshProUGUI>().text = randoms[randInt2];
                break;
        }
        // can do more but lazy

        // show image if need to.
        if(MainMenuScript.denganGambar){
            try{
                Sprite spri = Resources.Load<Sprite>(""+images[randInt]) as Sprite;
                ansImage.GetComponent<Image>().sprite = spri;
            }
            catch{
                print("Something went wrong with image placement");
            }
        }
        else{
            ansImage.enabled = false;
            borderImage.enabled = false;
        }
        

        // audio
        voiceSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("" + voices[randInt]), voiceVolume);
        

    }

    void voiceButtonFunc(){
        voiceSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("voices/"+currAnswer), voiceVolume);
    }

    void choiceButtonFunc(int num){
        switch(num){
            case 1:
                if(choiceBtn1.GetComponentInChildren<TextMeshProUGUI>().text == currAnswer){
                    correctAnswerFunc();
                }
                else{
                    wrongAnswerFunc();
                }
                break;

            case 2:
                if(choiceBtn2.GetComponentInChildren<TextMeshProUGUI>().text == currAnswer){
                    correctAnswerFunc();
                }
                else{
                    wrongAnswerFunc();
                }
                break;

            case 3:
                if(choiceBtn3.GetComponentInChildren<TextMeshProUGUI>().text == currAnswer){
                    correctAnswerFunc();
                }
                else{
                    wrongAnswerFunc();
                }
                break;
            
        }
    }

    void correctAnswerFunc(){
        numRound++;
        updateMana();
        if(numRound > maxRound){
            winningScreenFunc();
        }
        else{
            sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/congrats bell"));
            setStage();
        }
    }

    void wrongAnswerFunc(){
        life--;
        sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/wrong1"));
        updateLife();
    }

    void updateLife(){
        int temp = life + 1;
        GameObject heart = GameObject.Find("Canvas/life" + temp);
        heart.SetActive(false);
        if(life == 0){
            losingScreenFunc();
        }
    }

    void updateMana(){
        // image.color = new Color(image.color.r, image.color.g, image.color.b, 1)
        // int temp = numRound + 1;
        if(numRound <= maxRound){
            GameObject mana = GameObject.Find("Canvas/mana" + numRound);
            Image manaImg= mana.GetComponent<Image>();
            manaImg.sprite = Resources.Load<Sprite>("icon/mana");
            manaImg.color = new Color(manaImg.color.r, manaImg.color.g, manaImg.color.b, 1);
        }
    }

    void losingScreenFunc(){
        bgmSource.Stop();
        sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/wrong2"));
        losingScreen.SetActive(true);
        // StartCoroutine(goToMainMenu(3f));
        
        // StartCoroutine(DoFade());
    }

    void winningScreenFunc(){
        bgmSource.Stop();
        sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/victory1"));
        voiceSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/applause"));
        winningScreen.SetActive(true);
        // videoPlayer.SetDirectAudioMute(0, false);
        // videoPlayer.SetDirectAudioVolume(0, 1);
        videoPlayer.Play();
        // StartCoroutine(goToMainMenu());
    }

    

    // public IEnumerator goToMainMenu(float time)
    // {
    //     //Print the time of when the function is first called.
    //     Debug.Log("Started Coroutine at timestamp : " + Time.time);

    //     //yield on a new YieldInstruction that waits for 5 seconds.
    //     yield return new WaitForSeconds(time);

    //     //After we have waited 5 seconds print the time again.
    //     Debug.Log("Finished Coroutine at timestamp : " + Time.time);

        

    // }

    void goToMainMenu(){
        sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/click"));

        // Reset
        numRound = 0;
        life = 3;
        ansImage.enabled = true;
        borderImage.enabled = false;

        SceneManager.LoadScene("MainMenu");
    }

    /*
    public IEnumerator DoFade()
    {
        CanvasGroup canvasGroup = FindObjectOfType<CanvasGroup>();
        Camera camera = FindObjectOfType<Camera>();
        canvasGroup.interactable = false;
        Color startcolor = camera.backgroundColor;
        Color endcolor = new Color32(41, 57, 73, 255);
        float t = 0.0f;
        float duration = 3.0f;
        while (canvasGroup.alpha > 0)
        {
            camera.backgroundColor = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        yield return null;
        camera.backgroundColor = new Color32(41, 57, 73, 255);
        SceneManager.LoadScene("MainMenu");
    }
    */
}
 