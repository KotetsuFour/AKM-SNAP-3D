using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Location location;
    public List<LaneSegment> segments;

    public List<int> players;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void revealLocation()
    {
        if (location != null)
        {
            Destroy(location);
        }
        location = Instantiate(StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)]);
        location.lane = this;
        updateLocationDisplay();
    }
    public void setLocation(Location prefab)
    {
        if (location != null)
        {
            Destroy(location);
        }
        location = Instantiate(prefab);
        updateLocationDisplay();
    }
    private void updateLocationDisplay()
    {
        //TODO
    }
    public List<CharacterCard> getAllCardsHere()
    {
        List<CharacterCard> ret = new List<CharacterCard>();
        foreach (int pl in players)
        {
            ret.AddRange(segments[pl].cardsHere);
        }
        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<int> getWinners()
    {
        int most = segments[0].calculatedPower;
        List<int> ret = new List<int>();
        ret.Add(0);
        for (int q = 1; q < segments.Count; q++)
        {
            if (segments[q].calculatedPower == most)
            {
                ret.Add(q);
            } else if (segments[q].calculatedPower > most)
            {
                ret.Clear();
                ret.Add(q);
                most = segments[q].calculatedPower;
            }
        }
        return ret;
    }
}