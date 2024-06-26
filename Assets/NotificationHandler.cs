using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameNotification.Permission allowNotification(GameNotification note)
    {
        return new GameNotification.Permission(this, true, -1);
    }
    public GameNotification.Permission allowPermission(GameNotification.Permission note)
    {
        return new GameNotification.Permission(this, true, -1);
    }
    public List<GameNotification> getResponse(GameNotification note)
    {
        return null;
    }
    public bool allowPlaceCard(CharacterCard card, LaneSegment dest)
    {
        return true;
    }
    public bool allowMoveCard(CharacterCard card, LaneSegment origin, LaneSegment dest)
    {
        return true;
    }

}
