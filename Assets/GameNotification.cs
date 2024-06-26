using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNotification
{
    private bool disputable;
    private Nature nature;
    private Stage stage;
    private NotificationHandler cause;

    private int[] intVals;
    private CharacterCard[] characterCardVals;
    private Location[] locationVals;
    private PositionState[] positionStateVals;

    private List<Permission> permissionsQueue;
    private bool denied;

    public GameNotification(Nature nature, bool disputable, NotificationHandler cause)
    {
        this.nature = nature;
        this.disputable = disputable;
        this.stage = Stage.PERMISSION;
    }
    public void setInts(int[] vals)
    {
        intVals = vals;
    }
    public void setCards(CharacterCard[] vals)
    {
        characterCardVals = vals;
    }
    public void setLocations(Location[] vals)
    {
        locationVals = vals;
    }
    public void setPositions(PositionState[] vals)
    {
        positionStateVals = vals;
    }
    public bool allow()
    {
        if (!disputable)
        {
            return true;
        }
        permissionsQueue = new List<Permission>();
        List<NotificationHandler> handlers = StaticData.board.getAllReactors();
        foreach (NotificationHandler handler in handlers)
        {
            Permission permit = handler.allowNotification(this);
            if (!permit.permitted)
            {
                permit.notificationSubject = this;
                permissionsQueue.Add(permit);
            }
        }
        for (int q = 0; q < permissionsQueue.Count; q++)
        {
            foreach (NotificationHandler handler in handlers)
            {
                Permission permit = handler.allowPermission(permissionsQueue[q]);
                if (!permit.permitted)
                {
                    permit.permissionSubject = permissionsQueue[q];
                    permissionsQueue.Add(permit);
                }
            }
        }
        for (int q = permissionsQueue.Count - 1; q >= 0; q--)
        {
            permissionsQueue[q].deny();
        }
        return !denied;
    }
    public void act()
    {
        stage = Stage.ACTING;
        //TODO the action
    }

    public bool isDisputable()
    {
        return disputable;
    }
    public Nature getNature()
    {
        return nature;
    }
    public Stage getStage()
    {
        return stage;
    }
    public int[] getInts()
    {
        return intVals;
    }
    public CharacterCard[] getCharacterCards()
    {
        return characterCardVals;
    }
    public Location[] getLocations()
    {
        return locationVals;
    }
    public PositionState[] getPositions()
    {
        return positionStateVals;
    }
    public enum Nature
    {
        GAME_START, TURN_START, PLAY_PHASE, TURN_END, GAME_END, FINISH, STANDBY,
        REVEAL_LOCATION, REVEAL_CARD, REGISTER_MOVE, ON_REVEAL, ONGOING, OTHER_EFFECT, SETTLE_CARD,
        PERM_ALTER_POWER, TEMP_ALTER_POWER, ALTER_COST,
        CREATE_CARD, RELOCATE_CARD, CHANGE_LOCATION, ALTER_ONREVEAL, ALTER_ONGOING, TRANSFORM_CARD
    }
    public enum Stage
    {
        PERMISSION, ACTING, COMPLETED
    }

    public class Permission
    {
        public NotificationHandler actor;
        public bool permitted;
        public int effectType; //0 for general, 1 for onReveal, 2 for ongoing
        public GameNotification notificationSubject;
        public Permission permissionSubject;

        public bool denied;
        public Permission(NotificationHandler actor, bool permitted, int effectType)
        {
            this.actor = actor;
            this.permitted = permitted;
            this.effectType = effectType;
        }
        public Permission(NotificationHandler actor, bool permitted)
        {
            this.actor = actor;
            this.permitted = permitted;
        }
        public void deny()
        {
            if (!denied && !permitted)
            {
                if (notificationSubject != null)
                {
                    notificationSubject.denied = true;
                }
                else if (permissionSubject != null)
                {
                    permissionSubject.denied = true;
                }
            }
        }
    }
    public static void changePermanentPower(CharacterCard card, int amount)
    {
        card.changePermanentPower(amount);
    }
    public static void changeTemporaryPower(CharacterCard card, int amount)
    {
        card.changeTemporaryPower(amount);
    }
    public static void changeCost(CharacterCard card, int amount)
    {
        card.changeCost(amount);
    }
    public static void createCard(CharacterCard prefab, PositionState dest)
    {
        dest.cardsHere.Add(Object.Instantiate(prefab));
        dest.updateCardPositions();
    }
    public static void move(CharacterCard card, PositionState origin, PositionState dest)
    {
        origin.cardsHere.Remove(card);
        dest.cardsHere.Add(card);
        origin.updateCardPositions();
        dest.updateCardPositions();
    }
    public static void changeLocation(LaneSegment seg, Location loc)
    {
        Object.Destroy(seg.lane.location);
        seg.lane.setLocation(loc);
    }
}
