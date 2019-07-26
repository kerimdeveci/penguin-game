using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    Player player;
    bool startFadeOut;
    bool startFadeIn;
    float fadeScreenAlpha;
    bool fadeOutInProgress;
    bool fadeInInProgress;
    GameObject fadeScreen;
    GameObject pauseScreen;
    float fadeTime = 1f;
    float fadeAlphaLimit = 1f;
    bool isGameOver = false;
    List<Tuple<string, string>> optionsPause;
    int optionsPauseIndex;
    GameObject optionsPauseCursor;
    PlayerRanking rankaroo;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        startFadeIn = true;
        startFadeOut = false;
        fadeInInProgress = true;
        fadeOutInProgress = false;
        fadeScreenAlpha = 1;
        fadeScreen = transform.Find("FadeScreen").gameObject;
        fadeScreen.GetComponent<CanvasGroup>().alpha = fadeScreenAlpha;
        pauseScreen = transform.Find("PauseScreen").gameObject;
        pauseScreen.SetActive(false);

        optionsPause = new List<Tuple<string, string>>();
        optionsPause.Add(Tuple.Create("Resume", "ResumeGame"));
        optionsPause.Add(Tuple.Create("Main Menu", "GoMenu"));
        optionsPauseIndex = 0;
        optionsPauseCursor = transform.Find("PauseScreen/Options/Cursor").gameObject;

    }

    public void DoGameComplete()
    {
        Debug.Log("DoGameComplete");
        SaveRanking();
        ListRanking();
    }

    // Update is called once per frame
    void Update()
    {
        if (startFadeIn)
        {
            if (!fadeInInProgress)
            {
            }
            else
            {
                fadeScreenAlpha -= 0.02f;
                fadeScreen.GetComponent<CanvasGroup>().alpha = fadeScreenAlpha;
                if (fadeScreenAlpha <= 0)
                {
                    fadeInInProgress = false;
                }
            }
        }

        if (startFadeOut)
        {
            if (!fadeOutInProgress)
            {
            }
            else
            {
                fadeScreenAlpha += 0.02f;
                fadeScreen.GetComponent<CanvasGroup>().alpha = fadeScreenAlpha;
                if (fadeScreenAlpha >= fadeAlphaLimit)
                {
                    fadeOutInProgress = false;
                    if (isGameOver)
                    {
                        Debug.Log("U R DEAD");
                        ReloadScene();
                    }
                }
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Home" || sceneName == "Monster" || sceneName == "Boss")
            {
                if(pauseScreen.activeInHierarchy)
                {
                    Time.timeScale = 1;
                    pauseScreen.SetActive(false);
                }
                else
                {
                    Time.timeScale = 0;
                    pauseScreen.SetActive(true);
                }
            }
        }

        if (optionsPauseCursor.activeInHierarchy && Input.GetButtonDown("Submit"))
        {
            Debug.Log(optionsPause[optionsPauseIndex]);
        }

        if (pauseScreen.activeInHierarchy && !Mathf.Approximately(Input.GetAxisRaw("Vertical"), 0))
        {
            float vertical = Input.GetAxisRaw("Vertical");
            if (vertical > 0 && optionsPauseIndex > 0)
            {
                optionsPauseIndex--;
                optionsPauseCursor.transform.localPosition = new Vector3(optionsPauseCursor.transform.localPosition.x, optionsPauseCursor.transform.localPosition.y + 60);
            }

            if (vertical < 0 && optionsPauseIndex < optionsPause.Count - 1)
            {
                optionsPauseIndex++;
                optionsPauseCursor.transform.localPosition = new Vector3(optionsPauseCursor.transform.localPosition.x, optionsPauseCursor.transform.localPosition.y - 60);
            }
        }
    }

    public void DoFade()
    {
        startFadeOut = true;
        fadeOutInProgress = true;
    }

    public bool IsFadingOut()
    {
        return fadeOutInProgress;
    }

    public float GetFadeTime()
    {
        return fadeTime;
    }

    public void DoGameOver()
    {
        DoFade();
        isGameOver = true;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void GoLeaderboards()
    {
        SceneManager.LoadScene("Ranking");
    }

    public void GoEnterName()
    {
        SceneManager.LoadScene("Enter Name");
    }

    public void StartGame()
    {
        SaveName();
        SceneManager.LoadScene("Home");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public bool IsPaused()
    {
        return pauseScreen.activeInHierarchy;
    }

    void LoadRanking()
    {
        if (File.Exists(Application.persistentDataPath + "/playerRanking.txt"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerRanking.txt", FileMode.Open);
            PlayerRanking playerData = (PlayerRanking)binaryFormatter.Deserialize(file);
            file.Close();

            rankaroo = playerData;
        }
        else
        {
            rankaroo = new PlayerRanking();
            rankaroo.names = new List<string>();
            rankaroo.weaponNames = new List<string>();
            rankaroo.weaponAttacks = new List<int>();
            rankaroo.timeCompletes = new List<float>();
        }
    }

    public void SaveRanking()
    {
        LoadRanking();
        rankaroo.test = "hahahahaha";
        rankaroo.names.Add(player.Name);
        rankaroo.weaponNames.Add(player.Weapon.Name + " " + player.Weapon.Modifier);
        rankaroo.weaponAttacks.Add(player.Weapon.Attack);
        rankaroo.timeCompletes.Add(Time.time);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerRanking.txt", FileMode.OpenOrCreate);

        PlayerRanking playerData = rankaroo;

        binaryFormatter.Serialize(file, playerData);
        file.Close();
    }

    public void ListRanking()
    {
        int length = rankaroo.timeCompletes.Count;

        for (int i = 0; i < length; i++)
        {
            Debug.Log("------------------");
            Debug.Log(rankaroo.names[i]);
            Debug.Log(rankaroo.weaponNames[i]);
            Debug.Log(rankaroo.weaponAttacks[i]);
            Debug.Log(rankaroo.timeCompletes[i]);
        }
    }

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/currentPlayer.txt", FileMode.OpenOrCreate);

        PlayerData playerData = new PlayerData();
        playerData.name = player.Name;
        playerData.money = player.Coins;
        playerData.progress = player.Progress;

        playerData.weaponId = player.Weapon.ID;
        playerData.weaponAttack = player.Weapon.Attack;
        playerData.weaponModifierChance = player.Weapon.ModifierChance;
        playerData.weaponModifier = player.Weapon.Modifier;

        binaryFormatter.Serialize(file, playerData);
        file.Close();
    }

    public void SaveName()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/currentPlayer.txt", FileMode.OpenOrCreate);

        InputField inputField = GameObject.FindGameObjectWithTag("InputName").GetComponent<InputField>();

        PlayerData playerData = new PlayerData();
        playerData.name = inputField.text;
        playerData.money = 69;
        playerData.progress = 1;

        playerData.weaponId = 3;
        playerData.weaponAttack = 10;

        binaryFormatter.Serialize(file, playerData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/currentPlayer.txt"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/currentPlayer.txt", FileMode.Open);
            PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(file);
            file.Close();

            player.UpdateCoins(playerData.money);
            player.Name = playerData.name;
            player.Progress = playerData.progress;

            player.Weapon.ID = playerData.weaponId;
            player.Weapon.Attack = playerData.weaponAttack;
            player.Weapon.ModifierChance = playerData.weaponModifierChance;
            player.Weapon.Modifier = playerData.weaponModifier;

        }
    }
}

[Serializable]
class PlayerData
{
    public string name;
    public int money;

    public int progress;
    public int weaponId;
    public int weaponAttack;
    public float weaponModifierChance;
    public string weaponModifier;
}

[Serializable]
class PlayerRanking
{
    public string test;
    public List<string> names;
    public List<string> weaponNames;
    public List<int> weaponAttacks;
    public List<float> timeCompletes;
}