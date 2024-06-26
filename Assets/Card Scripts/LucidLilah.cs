using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucidLilah : CharacterCard
{
    [SerializeField] private CharacterCard bloodyLilah;

    void Start()
    {

    }
    public new string onReveal(Board b)
    {
        string ret = "";
        if (turnPlayed != 0)
        {
            return null;
        }

        int num = Random.Range(0, 7);
        ret += $"Player {myPlayer}'s {characterName} activated ability #{num + 1}.\n";
        if (num == 0)
        {
            abilityDescription = "Ability #1: Swap the Cost and Power of enemy cards in this lane.";
            Lane lane = b.getMyLane(this);
            for (int q = 0; q < lane.segments.Count; q++)
            {
                if (q == myPlayer)
                {
                    continue;
                }
                for (int w = 0; w < lane.segments[q].Count; w++)
                {
                    if (lane.segments[q][w].revealed)
                    {
                        CharacterCard swapper = lane.segments[q][w];
                        int temp = swapper.basePower;
                        swapper.setBasePower(swapper.baseCost);
                        swapper.setBaseCost(temp);
                        ret += $"Player {q}'s {swapper.characterName} swaps its Power and Cost.\n";
                    }
                }
            }
        }
        else if (num == 1)
        {
            abilityDescription = "Ability #2: Your cards here gain 1 Power.";
            Lane lane = b.getMyLane(this);
            for (int q = 0; q < lane.segments[myPlayer].Count; q++)
            {
                if (lane.segments[myPlayer][q].revealed)
                {
                    lane.segments[myPlayer][q].changePermanentPower(1);
                    ret += $"Their {lane.segments[myPlayer][q].characterName} gains 1 Power.\n";
                }
            }
        }
        else if (num == 2)
        {
            abilityDescription = "Ability #3: This card's base Power becomes 20.";
            setBasePower(20);
            ret += $"Its base Power is 20.\n";
        }
        else if (num == 3)
        {
            abilityDescription = "Ability #4: Change this location.";
            Lane lane = b.getMyLane(this);
            lane.revealLocation();
            ret += $"This location is now {lane.location.locationName}.\n";
        }
        else if (num == 4)
        {
            abilityDescription = "Ability #5: Move to a random lane.";
            Lane lane = b.getMyLane(this);
            int laneToMoveTo = Random.Range(0, b.lanes.Length);
            while (b.lanes[laneToMoveTo] != lane && (!b.lanes[laneToMoveTo].players.Contains(myPlayer) || !b.lanes[laneToMoveTo].canPlayHere()))
            {
                laneToMoveTo = (laneToMoveTo + 1) % b.lanes.Length;
            }
            lane.segments[myPlayer].Remove(this);
            b.addToLane(b.lanes[laneToMoveTo], this, myPlayer);
            ret += "It moves to a random lane.\n";
        }
        else if (num == 5)
        {
            abilityDescription = "Ability #6: Summon Bloody Lilah.";
            Lane lane = b.getMyLane(this);
            if (lane.canPlayHere())
            {
                CharacterCard antagonist = Instantiate(bloodyLilah);
                antagonist.myPlayer = myPlayer;
                b.addToLane(lane, antagonist, myPlayer);
                string blReveal = antagonist.onReveal(b);
                ret += $"Summoned {antagonist.characterName}.\n" + blReveal;
            }
        }
        else if (num == 6)
        {
            abilityDescription = "Ability #7: Swap the Cost and Power of your cards here.";
            Lane lane = b.getMyLane(this);
            foreach (CharacterCard card in lane.segments[myPlayer])
            {
                if (card.revealed)
                {
                    int temp = card.basePower;
                    card.setBasePower(card.baseCost);
                    card.setBaseCost(temp);
                    ret += $"Their {card.characterName} swaps its Power and Cost.\n";
                }
            }
        }
        return ret;
    }
}
