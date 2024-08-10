using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public abstract class CharacterCard : NotificationHandler
{
    public string characterName;
    public int baseCost;
    public int permanentAlterCost;
    public int temporaryAlterCost;
    public int basePower;
    public int permanentAlterPower;
    public int temporaryAlterPower;
    public string abilityDescription;
    public string story;
    public List<Attribute> attributes;
    public Series series;
    public int id;
    public int turnToDraw;
    public int myPlayer;
    public PositionState positionState;
    public int turnPlayed;
    public bool revealed;
    public int getPower(Board b)
    {
        return getPermanentPower() + temporaryAlterPower;
    }
    public int getPower()
    {
        return getPermanentPower() + temporaryAlterPower;
    }
    public int getPermanentPower()
    {
        return basePower + permanentAlterPower;
    }
    public int getCost(Board b, Lane lane)
    {
        return baseCost + permanentAlterCost + temporaryAlterCost;
    }
    public int getCost(Board b)
    {
        return baseCost + permanentAlterCost + temporaryAlterCost;
    }
    public int getCost()
    {
        return baseCost + permanentAlterCost + temporaryAlterCost;
    }
    public void changePermanentPower(int amount)
    {
        permanentAlterPower += amount;
    }
    public void changeTemporaryPower(int amount)
    {
        temporaryAlterPower += amount;
    }
    public void changeCost(int amount)
    {
        permanentAlterCost += amount;
    }
    public void setBasePower(int p)
    {
        basePower = p;
    }
    public void setBaseCost(int c)
    {
        baseCost = c;
    }
    public void resetTemporary()
    {
        temporaryAlterCost = 0;
        temporaryAlterPower = 0;
    }
    public bool isMyOnReveal(GameNotification note)
    {
        return note.getNature() == GameNotification.Nature.REVEAL_CARD
            && note.getCharacterCards()[0] == this;
    }
    public enum Series
    {
        KOTETSU_CLASSIC, KOTETSU_EXPANDED, BROTHER, SISTER, KOTETSU_VILLAIN_CLASSIC,
        KOTETSU_VILLAIN_EXPANDED, BROTHER_VILLAIN, SISTER_VILLAIN
    }
    public enum Attribute
    {
        FIRE, WATER, EARTH, AIR, LIGHTNING, ICE, LIFE, LEGENDARY_WEAPON, SPIRIT, DREAM_DIMENSION,
        HIGH_DIMENSION, INNATE_MAGIC, LEARNED_MAGIC, ZISHIAN, POUND, SUPERKNIGHT, BEGGAR_ALLIANCE,
        COMICS_ACADEMY
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "CardGame")
        {
            Board board = GameObject.Find("Board").GetComponent<Board>();
            StaticData.findDeepChild(transform, "ShowCost").GetComponent<TextMeshProUGUI>()
                .text = "" + getCost(board);
            StaticData.findDeepChild(transform, "ShowPower").GetComponent<TextMeshProUGUI>()
                .text = "" + getPower(board);
        }
        else
        {
            StaticData.findDeepChild(transform, "ShowCost").GetComponent<TextMeshProUGUI>()
                .text = "" + getCost();
            StaticData.findDeepChild(transform, "ShowPower").GetComponent<TextMeshProUGUI>()
                .text = "" + getPower();
        }
    }
}
