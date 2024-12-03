using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public abstract class CharacterCard : NotificationHandler
{
    public Sprite faceImage;
    public string characterName;
    public int baseCost;
    public int temporaryAlterCost;
    public int basePower;
    public int permanentAlterPower;
    public int temporaryAlterPower;
    public string abilityDescription;
    public string story;
    public List<Attribute> attributes;
    public Series series;
    public int id;
    public int myPlayer;
    public PositionState positionState;
    public int turnPlayed;
    public bool revealed;
    public float ongoingMultiplier;
    public bool canMove;
    public int getPower()
    {
        return getPermanentPower() + temporaryAlterPower;
    }
    public int getPermanentPower()
    {
        return basePower + permanentAlterPower;
    }
    public int getCost()
    {
        return baseCost + temporaryAlterCost;
    }
    public void changePermanentPower(int amount)
    {
        permanentAlterPower += amount;
    }
    public void changeTemporaryPower(int amount)
    {
        temporaryAlterPower += amount;
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
        return note.getNature() == GameNotification.Nature.ON_REVEAL
            && note.getCharacterCards()[0] == this;
    }
    public bool isMyOngoing(GameNotification note)
    {
        return note.getNature() == GameNotification.Nature.ONGOING
            && note.getCharacterCards()[0] == this;
    }
    public bool isOnMySideOfTheField(CharacterCard card)
    {
        return card.positionState is LaneSegment && ((LaneSegment)card.positionState).lane.segments.IndexOf((LaneSegment)card.positionState) == myPlayer;
    }
    public enum Series
    {
        KOTETSU_CLASSIC, KOTETSU_EXPANDED, BROTHER, SISTER, KOTETSU_VILLAIN_CLASSIC,
        KOTETSU_VILLAIN_EXPANDED, BROTHER_VILLAIN, SISTER_VILLAIN, SPECIAL, MOSSONITE
    }
    public enum Attribute
    {
        FIRE, WATER, EARTH, AIR, LIGHTNING, ICE, LIFE, LEGENDARY_WEAPON, SPIRIT, DREAM_DIMENSION,
        HIGH_DIMENSION, INNATE_MAGIC, LEARNED_MAGIC, ZISHIAN, POUND, SUPERKNIGHT, BEGGAR_ALLIANCE,
        COMICS_ACADEMY
    }

    public void setToUnrevealedPosition()
    {
        transform.rotation = Quaternion.Euler(180, 0, 0);
    }
    public void setToRevealedPosition()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void updatePowerAndCostDisplay()
    {
        if (positionState == null || positionState is Hand)
        {
            StaticData.findDeepChild(transform, "CostCircle").gameObject.SetActive(true);
            StaticData.findDeepChild(transform, "PowerCircle").gameObject.SetActive(true);
        }
        else if (positionState is LaneSegment)
        {
            StaticData.findDeepChild(transform, "CostCircle").gameObject.SetActive(false);
            StaticData.findDeepChild(transform, "PowerCircle").gameObject.SetActive(true);
        }
        else
        {
            StaticData.findDeepChild(transform, "CostCircle").gameObject.SetActive(true);
            StaticData.findDeepChild(transform, "PowerCircle").gameObject.SetActive(true);
        }
        TextMeshProUGUI costDisplay = StaticData.findDeepChild(transform, "Cost").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI powerDisplay = StaticData.findDeepChild(transform, "Power").GetComponent<TextMeshProUGUI>();
        costDisplay.text = "" + getCost();
        powerDisplay.text = "" + getPower();

        if (getCost() > baseCost)
        {
            costDisplay.color = Color.red;
        }
        else if (getCost() < baseCost)
        {
            costDisplay.color = Color.green;
        }
        else
        {
            costDisplay.color = Color.white;
        }

        if (getPower() > basePower)
        {
            powerDisplay.color = Color.green;
        }
        else if (getPower() < basePower)
        {
            powerDisplay.color = Color.red;
        }
        else
        {
            powerDisplay.color = Color.white;
        }
    }
}
