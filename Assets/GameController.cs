using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject selectedScreen;
    public Image slectedImg,gameActualImg;
    public ImageData imageData;
    public Ease easeType;

    public RectTransform LoginPannel;
    public RectTransform LevelPannel;
    public RectTransform inGamePannel;
    public RectTransform pausePannel;
    public RectTransform settingsPannel;
    public RectTransform gameOverPannel;
    public RectTransform storePannel;
    public RectTransform leaderBoardPannel;
    public GameObject gameBoardBg;
    public GameObject GameBoard;
    public ParticleSystem GameBoardParticles;
    public GameObject gameManager;
    GameObject gameMan;
    public Timer timer;
    // Start is called before the first frame update
    void Start()
    {
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

        LoginPannel.DOAnchorPosX(-471, .25f).SetEase(easeType).SetUpdate(true).OnComplete(() =>

        LevelPannel.DOAnchorPosX(0, .25f).SetEase(easeType).SetUpdate(true)

        ); ;
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

        selectedScreen.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).SetUpdate(true).
            OnComplete(() =>
        selectedScreen.SetActive(true)

            );
    }
    public void SelectTypeLevel(int index)
    {
        
        imageData.size = index;
        gameMan = Instantiate(gameManager, Vector3.zero, Quaternion.identity);
        gameActualImg.sprite = imageData.sprite;
        SoundManager.instance.PlayPannelCloseSound();
        selectedScreen.transform.DOScale(0f, .2f).SetEase(easeType).SetUpdate(true).SetUpdate(true).
            OnComplete(() =>
            {
                selectedScreen.SetActive(true);

                LevelPannel.DOAnchorPosX(-471, .2f).SetEase(easeType).SetUpdate(true).SetUpdate(true).OnComplete(() =>
                {

                    SoundManager.instance.PlayPannelSound();
                    gameBoardBg.transform.DOMoveX(0f, .2f).SetEase(easeType).SetUpdate(true);
                    inGamePannel.DOAnchorPosX(0, .25f).SetEase(easeType).SetUpdate(true).OnComplete(() =>
                    {
                        timer.ResetTimer();
                        timer.StartTimer();
                        SoundManager.instance.PlayPannelPopSound();
                        GameBoard.transform.DOScale(1.98476f, .2f).SetEase(easeType).SetUpdate(true);
                    }
                        );
                }


                    ) ;
            }

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
            OnComplete(()=>

            {
               // SoundManager.instance.PlayPannelPopSound();

                pausePannel.gameObject.SetActive(false);
                Time.timeScale = 1f;

            }

            );

    }

    public void settings()
    {

        SoundManager.instance.PlayPannelCloseSound();

        pausePannel.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).
                    OnComplete(() =>

                    {
                        SoundManager.instance.PlayPannelPopSound();
                        pausePannel.gameObject.SetActive(false);
                        settingsPannel.gameObject.SetActive(true);
                        settingsPannel.DOScale(1f,.3f).SetEase(easeType).SetUpdate(true);
                        //sTime.timeScale = 0f;

                    }

                    );
    }
    public void CloseSettings()
    {
        SoundManager.instance.PlayPannelCloseSound();

        settingsPannel.transform.DOScale(0f, .3f).SetEase(easeType).SetUpdate(true).
                   OnComplete(() =>

                   {
                       SoundManager.instance.PlayPannelPopSound();

                       pausePannel.gameObject.SetActive(true);
                       settingsPannel.gameObject.SetActive(false);
                       pausePannel.DOScale(1f, .3f).SetEase(easeType).SetUpdate(true);
                       //sTime.timeScale = 0f;

                   }

                   );
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
        GameBoard.transform.DOScale(0, .2f).SetEase(easeType).SetUpdate(true).OnComplete(() =>
        {
            foreach (Transform t in GameBoard.transform)
            {
                Destroy(t.gameObject);
            }
            Destroy(gameMan);
            gameBoardBg.transform.DOMoveX(7.8888f, .2f).SetEase(easeType).SetUpdate(true);
            SoundManager.instance.PlayPannelSound();
            GameBoard.SetActive(false);
            inGamePannel.DOAnchorPosX(471, .3f).SetEase(easeType).SetUpdate(true).OnComplete(()=>


            {
                SoundManager.instance.PlayPannelSound();

                LevelPannel.DOAnchorPosX(0, .2f).SetEase(easeType).SetUpdate(true);
            }
            );
            
        }


        );
    }
    public void NewGame()
    {
        GameBoard.gameObject.SetActive( true );
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
        LevelPannel.DOAnchorPosX(471, .3f).SetEase(easeType).SetUpdate(true).OnComplete(() =>

        {
            SoundManager.instance.PlayPannelSound();
            leaderBoardPannel.DOAnchorPosX(0f, .3f).SetEase(easeType).SetUpdate(true);
        }
        

        );
    }
    public void CloseLeaderBoard()
    {
        SoundManager.instance.PlayPannelSound();
        leaderBoardPannel.DOAnchorPosX(471, .3f).SetEase(easeType).SetUpdate(true).OnComplete(() =>
        {
            SoundManager.instance.PlayPannelSound();
            LevelPannel.DOAnchorPosX(0f, .3f).SetEase(easeType).SetUpdate(true);
        }
       

        );
    }
}
