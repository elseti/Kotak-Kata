
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public Button keluarButton;
    public Button mulaiDenganGambarButton;
    public Button mulaiTanpaGambarButton;

    public AudioSource bgmSource;
    public AudioSource voiceSource;
    public AudioSource sfxSource;

    public static bool denganGambar;

    void Awake(){
        // var loadedObjects = Resources.LoadAll("");
        // foreach(var go in loadedObjects)
        // {
        //     Debug.Log(go.name);
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        Button keluarBtn = keluarButton.GetComponent<Button>();
        keluarBtn.onClick.AddListener(keluarBtnFunc);

        Button mulaiDenganGambarBtn = mulaiDenganGambarButton.GetComponent<Button>();
        mulaiDenganGambarBtn.onClick.AddListener(mulaiDenganGambarBtnFunc);

        Button mulaiTanpaGambarBtn = mulaiTanpaGambarButton.GetComponent<Button>();
        mulaiTanpaGambarBtn.onClick.AddListener(mulaiTanpaGambarBtnFunc);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // button functions
    void keluarBtnFunc(){
        Application.Quit();
    }

    void mulaiDenganGambarBtnFunc(){
        sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/click"));
        denganGambar = true;
        SceneManager.LoadScene("GameScene");
    }

    void mulaiTanpaGambarBtnFunc(){
        sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/click"));
        denganGambar = false;
        SceneManager.LoadScene("GameScene");
    }

    public void hoverButtonFunc(){
        sfxSource.PlayOneShot((AudioClip) Resources.Load<AudioClip>("sfx/hover"));
    }
}
