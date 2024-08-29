using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform menu;

    public void startTwoPlayerGame()
    {
        startGame(2);
    }
    public void startThreePlayerGame()
    {
        startGame(3);
    }
    private void startGame(int numPlayers)
    {
        try
        {
            StaticNetworking.numPlayers = numPlayers;
            StaticNetworking.createRelay(StaticData.findDeepChild(menu, "JoinCode").GetComponent<TextMeshProUGUI>());
            StaticData.findDeepChild(menu, "JoinCodeInput").gameObject.SetActive(false);
            StaticData.findDeepChild(menu, "Quit").gameObject.SetActive(true);
            switchToScreen("Lobby");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public void switchToScreen(string screenName)
    {
        for (int q = 0; q < menu.childCount; q++)
        {
            menu.GetChild(q).gameObject.SetActive(false);
        }
        StaticData.findDeepChild(menu, screenName).gameObject.SetActive(true);
    }
    public void joinGame()
    {
        string code = StaticData.findDeepChild(menu, "JoinCodeInput").GetComponent<TMP_InputField>().text;
        code = code.Trim();
        code = code.ToUpper();
        if (!string.IsNullOrEmpty(code))
        {
            try
            {
                StaticNetworking.joinRelay(code);
                StaticData.findDeepChild(menu, "JoinCodeInput").gameObject.SetActive(false);
                StaticData.findDeepChild(menu, "Quit").gameObject.SetActive(true);
                StaticData.findDeepChild(menu, "JoinCode").GetComponent<TextMeshProUGUI>().text = "Joined!";
            }
            catch (Exception ex)
            {
                StaticData.findDeepChild(menu, "JoinCode").GetComponent<TextMeshProUGUI>().text = ex.Message;
                Debug.Log(ex);
            }
        }
    }
    public void cancelGame()
    {
        StaticNetworking.cancelConnection();
        switchToScreen("MainBackground");
    }
}
