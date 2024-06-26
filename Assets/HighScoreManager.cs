using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;

    private List<HighScoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTransformList;


    public void Start()
    {
        entryContainer = transform.Find("HighScoreContainer");
        entryTemplate = entryContainer.Find("ScoreTemplate");

        entryTemplate.gameObject.SetActive(false);

        //highScoreEntryList = new List<HighScoreEntry>()
        //{
        //    new HighScoreEntry{ name = "asdf", score = 223},
        //    new HighScoreEntry{ name = "asdf", score = 123},
        //    new HighScoreEntry{ name = "asdf", score = 423},
        //    new HighScoreEntry{ name = "asdf", score = 723},
        //    new HighScoreEntry{ name = "asdf", score = 523},
        //    new HighScoreEntry{ name = "asdf", score = 623},
        //    new HighScoreEntry{ name = "asdf", score = 123},
        //    new HighScoreEntry{ name = "asdf", score = 3323},
        //    new HighScoreEntry{ name = "asdf", score = 1223},
        //    new HighScoreEntry{ name = "asdf", score = 923},
        //};

        //AddHighScoreEntry("Hahahha", 10000);

        LoadHighScores();

        //SaveHighScores();
    }

    public void LoadHighScores()
    {
        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
        Debug.Log("HighScores Loaded!");

        for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highScores.highScoreEntryList.Count; j++)
            {
                if (highScores.highScoreEntryList[j].score > highScores.highScoreEntryList[i].score)
                {
                    HighScoreEntry tmp = highScores.highScoreEntryList[i];

                    highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                    highScores.highScoreEntryList[j] = tmp;
                }
            }
        }

        highScoreEntryTransformList = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScores.highScoreEntryList)
        {
            CreateHighScoreEntry(highScoreEntry, entryContainer, highScoreEntryTransformList);
        }
    }

    public void AddHighScoreEntry(string name, int score)
    {
        HighScoreEntry highScoreEntry = new HighScoreEntry { name = name, score = score };

        string jsonString = PlayerPrefs.GetString("HighScoreTable");
        Debug.Log(jsonString);
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        highScores.highScoreEntryList.Add(highScoreEntry);

        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("HighScoreTable", json);
        Debug.Log("HighScores Saved!");
    }

    private void SaveHighScores()
    {
        HighScores highScores = new HighScores { highScoreEntryList = highScoreEntryList };

        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("HighScoreTable", json);
        Debug.Log("HighScores Saved!");
    }

    private void CreateHighScoreEntry(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformsList)
    {
        float templteHeight = 183f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templteHeight * transformsList.Count);
        entryTransform.gameObject.SetActive(true);

        //int rank = transformsList.Count + 1;

        //entryTransform.Find("RankTMP").GetComponent<TMP_Text>().text = rank.ToString();

        string name = highScoreEntry.name;

        entryTransform.Find("NameTMP").GetComponent<TMP_Text>().text = name;

        int score = highScoreEntry.score;
        entryTransform.Find("CoinsSection").Find("ScoreTMP").GetComponent<TMP_Text>().text = score.ToString();

        //entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

        transformsList.Add(entryTransform);
    }

    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    [System.Serializable]
    private class HighScoreEntry
    {
        public string name;
        public int score;
    }
}
