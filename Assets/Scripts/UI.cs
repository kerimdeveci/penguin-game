﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
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

    List<Tuple<string, string>> optionsMenu;
    int optionsMenuIndex;
    GameObject optionsMenuCursor;

    PlayerRanking rankaroo;
    public bool gameIsEnding = false;

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

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Menu")
        {
            optionsMenu = new List<Tuple<string, string>>();
            optionsMenu.Add(Tuple.Create("Play", "GoEnterName"));
            optionsMenu.Add(Tuple.Create("LeaderBoards", "GoLeaderboards"));
            optionsMenuIndex = 0;
            optionsMenuCursor = GameObject.FindGameObjectWithTag("MenuOptions").transform.Find("Cursor").gameObject;
        }

        if (sceneName == "Menu" || sceneName == "Enter Name" || sceneName == "Ranking" || sceneName == "RankingAfterGame")
        {
            transform.Find("UIEnhancement").gameObject.SetActive(false);
            transform.Find("HealthSlider").gameObject.SetActive(false);
        }
    }

    public void DoGameComplete()
    {
        Debug.Log("DoGameComplete");
        SaveRanking();
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

        string sceneName = SceneManager.GetActiveScene().name;
        if (Input.GetButtonDown("Cancel"))
        {
            sceneName = SceneManager.GetActiveScene().name;
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

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(optionsPause[optionsPauseIndex].Item2);
            theMethod.Invoke(this, null);
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

        if ((sceneName == "Ranking" || sceneName == "RankingAfterGame") && Input.GetButtonDown("Submit"))
        {
            GoMenu();
        }

        if (sceneName == "Menu" && Input.GetButtonDown("Submit"))
        {
            Debug.Log(optionsMenu[optionsMenuIndex]);

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(optionsMenu[optionsMenuIndex].Item2);
            theMethod.Invoke(this, null);
        }

        if (sceneName == "Enter Name" && Input.GetButtonDown("Submit"))
        {
            InputField inputField = GameObject.FindGameObjectWithTag("InputName").GetComponent<InputField>();
            if (!String.IsNullOrEmpty(inputField.text))
            {
                StartGame();
            }
        }

        if (sceneName == "Menu" && !Mathf.Approximately(Input.GetAxisRaw("Vertical"), 0))
        {
            float vertical = Input.GetAxisRaw("Vertical");
            if (vertical > 0 && optionsMenuIndex > 0)
            {
                optionsMenuIndex--;
                optionsMenuCursor.transform.localPosition = new Vector3(optionsMenuCursor.transform.localPosition.x, optionsMenuCursor.transform.localPosition.y + 60);
            }

            if (vertical < 0 && optionsMenuIndex < optionsMenu.Count - 1)
            {
                optionsMenuIndex++;
                optionsMenuCursor.transform.localPosition = new Vector3(optionsMenuCursor.transform.localPosition.x, optionsMenuCursor.transform.localPosition.y - 60);
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

    public void GoLeaderboardsAfterGame()
    {
        SceneManager.LoadScene("RankingAfterGame");
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

    public void DisplayRanking()
    {
        Debug.Log("DisplayRanking");

        LoadRanking();

        ListRanking();

        List<RankingOrder> rankingOrder = new List<RankingOrder>();
        for (int i = 0; i < rankaroo.names.Count; i++)
        {
            rankingOrder.Add(new RankingOrder(i, rankaroo.weaponAttacks[i]));
        }
        List<RankingOrder> sortedRankingOrder = rankingOrder.OrderByDescending(o=>o.weaponAttack).ToList();

        int numberToGet = 5;
        if (sortedRankingOrder.Count < 5)
        {
            numberToGet = sortedRankingOrder.Count;
        }

        List<RankingOrder> topFive = sortedRankingOrder.GetRange(0, numberToGet);
        Debug.Log(topFive.Count);

        Transform rankings = GameObject.FindGameObjectWithTag("Ranking").transform;

        for (int i = 0; i < 5; i++)
        {
            if (numberToGet > i)
            {
                Transform rankObject = rankings.Find("RankingItem" + (i + 1)).transform;

                RankingOrder order = topFive[i];

                rankObject.Find("RankPlayerName").GetComponent<Text>().text = rankaroo.names[order.index];
                rankObject.Find("RankTime").GetComponent<Text>().text = FormatTime(rankaroo.timeCompletes[order.index]);
                rankObject.Find("RankWeapon").GetComponent<Text>().text = rankaroo.weaponNames[order.index];
                rankObject.Find("RankAttack").GetComponent<Text>().text = rankaroo.weaponAttacks[order.index].ToString() + " ATT";
                rankObject.Find("Image").GetComponent<RawImage>().enabled = true;
            }
        }

        DisplayRecent();
    }

    void DisplayRecent()
    {
        Debug.Log("DisplayRecent");


        if (File.Exists(Application.persistentDataPath + "/currentPlayer.txt"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/currentPlayer.txt", FileMode.Open);
            PlayerData playerData = (PlayerData)binaryFormatter.Deserialize(file);
            file.Close();
            
            if (!String.IsNullOrEmpty(playerData.weaponModifier))
            {
                Transform rankObject = GameObject.FindGameObjectWithTag("RankingRecent").transform;
                rankObject.Find("RankPlayerName").GetComponent<Text>().text = playerData.name;
                rankObject.Find("RankTime").GetComponent<Text>().text = "";
                rankObject.Find("RankWeapon").GetComponent<Text>().text = "Hakapik of " + playerData.weaponModifier;
                rankObject.Find("RankAttack").GetComponent<Text>().text = playerData.weaponAttack.ToString() + " ATT";
                rankObject.Find("Image").GetComponent<RawImage>().enabled = true;
            }
        }
    }

    string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(100 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public void ListRanking()
    {
        int length = rankaroo.timeCompletes.Count;

        Debug.Log("=====================");
        for (int i = 0; i < length; i++)
        {
            Debug.Log(rankaroo.names[i]);
            Debug.Log(rankaroo.weaponNames[i]);
            Debug.Log(rankaroo.weaponAttacks[i]);
            Debug.Log(rankaroo.timeCompletes[i]);
            Debug.Log("------------------");
        }
        Debug.Log("=====================");
    }

    public void SaveRanking()
    {
        Debug.Log("SaveRanking");
        LoadRanking();

        //int length = rankaroo.timeCompletes.Count;
        //bool worthSaving = false;
        //if (length >= 3)
        //{
        //    for (int i = 0; i < length; i++)
        //    {
        //        Debug.Log(rankaroo.weaponAttacks[i]);
        //        if (rankaroo.weaponAttacks[i] < player.Weapon.Attack)
        //        {
        //            worthSaving = true;
        //        }
        //    }
        //}
        //else
        //{
        //    worthSaving = true;
        //}

        Debug.Log("Yeah we save this");
        rankaroo.test = "";
        rankaroo.names.Add(player.Name);
        rankaroo.weaponNames.Add(player.Weapon.Name + " of " + player.Weapon.Modifier);
        rankaroo.weaponAttacks.Add(player.Weapon.Attack);
        rankaroo.timeCompletes.Add(Time.time - player.TimeStart);

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerRanking.txt", FileMode.OpenOrCreate);

        PlayerRanking playerData = rankaroo;
            
        binaryFormatter.Serialize(file, playerData);
        file.Close();
        

    }

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/currentPlayer.txt", FileMode.OpenOrCreate);

        PlayerData playerData = new PlayerData();
        playerData.name = player.Name;
        playerData.money = player.Coins;
        playerData.progress = player.Progress;
        playerData.timeStart = player.TimeStart;

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

        string name = inputField.text;
        if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
        {
            name = "Anonymous";
        }
        if (name.Length > 25)
        {
            name = name.Substring(0, 25);
        }
        Regex rgx = new Regex("[^a-zA-Z0-9 -/?/!]");
        name = rgx.Replace(name, "");

        PlayerData playerData = new PlayerData();
        playerData.name = name;
        playerData.money = 0;
        playerData.progress = 0;
        playerData.timeStart = Time.time;

        playerData.weaponId = 3;
        playerData.weaponAttack = 0;

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

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.UpdateCoins(playerData.money);
            player.Name = playerData.name;
            player.Progress = playerData.progress;
            player.TimeStart = playerData.timeStart;

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
    public float timeStart;
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

class RankingOrder
{
    public int index;
    public int weaponAttack;

    public RankingOrder(int index, int weaponAttack)
    {
        this.index = index;
        this.weaponAttack = weaponAttack;
    }
}