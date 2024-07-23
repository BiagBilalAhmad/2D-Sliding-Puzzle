using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    

    public GameObject selectedScreen;
    public Image slectedImg,gameActualImg;
    public ImageData imageData;
    public Ease easeType;

    [Header("Panels")]
    public RectTransform LoginPannel;
    public RectTransform ModesPannel;
    public RectTransform LevelPannel;
    public RectTransform inGamePannel;
    public RectTransform pausePannel;
    public RectTransform settingsPannel;
    public RectTransform gameOverPannel;
    public RectTransform storePannel;
    public RectTransform leaderBoardPannel;

    public Image gameOverImg;

    [Header("Current Panel")]
    public RectTransform currentPanel;

    [Header("GamePlay")]
    public GameObject gameBoardBg;
    public GameObject GameBoard;
    public ParticleSystem GameBoardParticles;
    public GameObject gameManager;
    GameObject gameMan;
    public Timer timer;

    [Space]
    public GameObject powerUp;
    public TMP_Text powerUpText;
    public bool hasPowerUp = false;

    [Space]
    public TMP_Text InGameCoinCount;

    [Space]
    public GameObject LargeImageGO;
    public Image InGameLargeImage;

    [Header("Shop")]
    public int coinCount;
    public int powerupCount;

    // Start is called before the first frame update
    void Start()
    {
        coinCount = PlayerPrefs.GetInt("Coins", 0);
        powerupCount = PlayerPrefs.GetInt("Powerups", 0);

        SoundManager.instance.PlayPannelSound();
        LoginPannel.DOAnchorPosX(0, .6f).SetEase(easeType).SetUpdate(true);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
           Destroy(instance);
        }
    }

    public void StartGameBtn()
    {
        SoundManager.instance.PlayPannelSound();

        currentPanel = ModesPannel;

        LoginPannel.DOAnchorPosX(-471, .25f).SetEase(easeType).SetUpdate(true).OnComplete(() =>
            ModesPannel.DOAnchorPosX(0, .25f).SetEase(easeType).SetUpdate(true)
        );
    }

    public void OpenLevelsPanel(string mode)
    {
        SoundManager.instance.PlayPannelSound();

        switch (mode)
        {
            case "Easy":
                imageData.rows = 5;
                imageData.cols = 3;
                break;

            case "Medium":
                imageData.rows = 6;
                imageData.cols = 4;
                break;

            case "Hard":
                imageData.rows = 7;
                imageData.cols = 5;
                break;

            default:
                imageData.rows = 5;
                imageData.cols = 3;
                break;
        }

        currentPanel = LevelPannel;

        ModesPannel.DOAnchorPosX(-471, .25f).SetEase(easeType).SetUpdate(true).OnComplete(() =>
            LevelPannel.DOAnchorPosX(0, .25f).SetEase(easeType).SetUpdate(true)
        );
    }

    public void CloseLevelsPanel()
    {
        SoundManager.instance.PlayPannelSound();

        currentPanel = ModesPannel;

        LevelPannel.DOAnchorPosX(-471, .25f).SetEase(easeType).SetUpdate(true).OnComplete(() =>
            ModesPannel.DOAnchorPosX(0, .25f).SetEase(easeType).SetUpdate(true)
        );
    }

    public void EnableSelectedScreen()
    {
       SoundManager.instance.PlayPannelPopSound();

        selectedScreen.SetActive(true);
        selectedScreen.transform.DOScale(1f, .3f).SetEase(easeType).SetUpdate(true);
        slectedImg.sprite = imageData.sprite;
    }

    public void CloseSlectedScreen()
    {
        SoundManager.instance.PlayPannelCloseSound();

        selectedScreen.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).SetUpdate(true).OnComplete(() => selectedScreen.SetActive(false)
        );
    }

    public void SelectTypeLevel()
    {
        gameMan = Instantiate(gameManager, Vector3.zero, Quaternion.identity);
        InGameCoinCount.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        gameActualImg.sprite = imageData.sprite;
        InGameLargeImage.sprite = imageData.sprite;
        gameOverImg.sprite = imageData.sprite;
        SoundManager.instance.PlayPannelCloseSound();
        selectedScreen.transform.DOScale(0f, .2f).SetEase(easeType).SetUpdate(true).SetUpdate(true).OnComplete(() => {
            selectedScreen.SetActive(true);
            LevelPannel.DOAnchorPosX(-471, .2f).SetEase(easeType).SetUpdate(true).SetUpdate(true).OnComplete(() => {
                SoundManager.instance.PlayPannelSound();
                gameBoardBg.transform.DOMoveX(0f, .2f).SetEase(easeType).SetUpdate(true);
                inGamePannel.DOAnchorPosX(0, .25f).SetEase(easeType).SetUpdate(true).OnComplete(() => {
                    timer.ResetTimer();
                    timer.StartTimer();
                    if (hasPowerUp)
                    {
                        powerUpText.text = powerupCount.ToString();
                        powerUp.SetActive(true);
                        powerUp.GetComponent<Button>().onClick.AddListener(() => { 
                            gameMan.GetComponent<GameManager>().SetUsePowerUp();
                            SoundManager.instance.PlayPannelCloseSound();
                        });
                    }
                    else
                        powerUp.SetActive(false);
                    SoundManager.instance.PlayPannelPopSound();
                    GameBoard.transform.DOScale(1.98476f, .2f).SetEase(easeType).SetUpdate(true);
                });
            });
        });
    }

    public void EnlargeImage()
    {
        SoundManager.instance.PlayPannelPopSound();

        LargeImageGO.SetActive(true);
        LargeImageGO.transform.DOScale(1f, .3f).SetEase(easeType).SetUpdate(true);
    }

    public void MinimizeLargeImage()
    {
        SoundManager.instance.PlayPannelCloseSound();

        LargeImageGO.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).SetUpdate(true).OnComplete(() => selectedScreen.SetActive(false)
        );
    }

    public void PauseGame()
    {
        SoundManager.instance.PlayPannelPopSound();

        pausePannel.gameObject.SetActive( true );
        Time.timeScale = 0f;
        pausePannel.transform.DOScale(1f, .3f).SetEase(easeType).SetUpdate(true);
    }

    public void Resume()
    {
        SoundManager.instance.PlayPannelCloseSound();
        pausePannel.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).
            OnComplete(()=> {
               // SoundManager.instance.PlayPannelPopSound();
                pausePannel.gameObject.SetActive(false);
                Time.timeScale = 1f;
            });
    }

    public void settings()
    {
        SoundManager.instance.PlayPannelCloseSound();

        pausePannel.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).OnComplete(() => {
            SoundManager.instance.PlayPannelPopSound();
            pausePannel.gameObject.SetActive(false);
            settingsPannel.gameObject.SetActive(true);
            settingsPannel.DOScale(1f,.3f).SetEase(easeType).SetUpdate(true);
            //sTime.timeScale = 0f;
        });
    }

    public void CloseSettings()
    {
        SoundManager.instance.PlayPannelCloseSound();

        settingsPannel.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).OnComplete(() => {
            SoundManager.instance.PlayPannelPopSound();

            pausePannel.gameObject.SetActive(true);
            settingsPannel.gameObject.SetActive(false);
            pausePannel.DOScale(1f, .3f).SetEase(easeType).SetUpdate(true);
            //sTime.timeScale = 0f;
        });
    }

    public void ShowGameOver()
    {
        timer.ResetTimer();
        timer.StopTimer();

        gameOverPannel.transform.DOScale(1, .3f).SetEase(easeType).SetUpdate(true).OnComplete(() =>
            GameBoardParticles.Play()
        );
    }

    public void GameBackButton()
    {
        timer.ResetTimer();
        timer.StopTimer();
        SoundManager.instance.PlayPannelCloseSound();
        GameBoard.transform.DOScale(0, .2f).SetEase(easeType).SetUpdate(true).OnComplete(() => {
            foreach (Transform t in GameBoard.transform)
            {
                Destroy(t.gameObject);
            }
            Destroy(gameMan);
            gameOverPannel.transform.DOScale(0, .3f).SetEase(easeType).SetUpdate(true);
            gameBoardBg.transform.DOMoveX(7.8888f, .2f).SetEase(easeType).SetUpdate(true);
            SoundManager.instance.PlayPannelSound();
            GameBoard.SetActive(false);
            inGamePannel.DOAnchorPosX(471, .3f).SetEase(easeType).SetUpdate(true).OnComplete(()=> {
                SoundManager.instance.PlayPannelSound();
                LevelPannel.DOAnchorPosX(0, .2f).SetEase(easeType).SetUpdate(true);
            });
        });
    }

    public void NewGame()
    {
        GameBoard.gameObject.SetActive(true);
    }

    public void ShowStore()
    {
        SoundManager.instance.PlayPannelPopSound();

        storePannel.gameObject.SetActive(true);

        storePannel.transform.DOScale(1f, .3f).SetEase(easeType).SetUpdate(true);
    }

    public void CloseStore()
    {
        SoundManager.instance.PlayPannelCloseSound();
        storePannel.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).OnComplete(()=> 
            storePannel.gameObject.SetActive(true)
        );
    }

    public void ShowLeaderBoard()
    {
        SoundManager.instance.PlayPannelSound();
        currentPanel.DOAnchorPosX(471, .3f).SetEase(easeType).SetUpdate(true).OnComplete(() => {
            SoundManager.instance.PlayPannelSound();
            leaderBoardPannel.DOAnchorPosX(0f, .3f).SetEase(easeType).SetUpdate(true);
        });
    }

    public void CloseLeaderBoard()
    {
        SoundManager.instance.PlayPannelSound();
        leaderBoardPannel.DOAnchorPosX(471, .3f).SetEase(easeType).SetUpdate(true).OnComplete(() => {
            SoundManager.instance.PlayPannelSound();
            currentPanel.DOAnchorPosX(0f, .3f).SetEase(easeType).SetUpdate(true);
        });
    }
}
