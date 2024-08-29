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
        this.cause = cause;
    }
    public NotificationHandler getCause()
    {
        return cause;
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
    public void allow()
    {
        if (!disputable)
        {
            stage = Stage.ANIMATING;
        }
        permissionsQueue = new List<Permission>();
        List<NotificationHandler> handlers = StaticData.board.getAllPermissionNeeded();
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
        if (!denied)
        {
            stage = Stage.ANIMATING;
        }
        stage = Stage.DENIED;
    }
    public void animate()
    {
        if (getCause() == null || getCause().animate(this))
        {
            stage = Stage.ACTING;
        }
    }
    public void act()
    {
        if (getNature() == Nature.ALTER_COST)
        {
            CharacterCard subject = getCharacterCards()[0];
            subject.temporaryAlterCost += getInts()[0];
        }
        else if (getNature() == Nature.ALTER_ONGOING)
        {
            CharacterCard subject = getCharacterCards()[0];
            subject.ongoingMultiplier *= getInts()[0];
        }
        else if (getNature() == Nature.CHANGE_LOCATION)
        {
            Location old = getLocations()[0];
            Lane lane = old.lane;
            Object.Destroy(old);
            Location newLoc = Object.Instantiate(getLocations()[1]);
            getLocations()[1] = newLoc;
            lane.setLocation(newLoc);

            StaticData.board.calculateScores();
        }
        else if (getNature() == Nature.CREATE_CARD)
        {
            if (!getPositions()[0].isFull())
            {
                CharacterCard subject = Object.Instantiate(getCharacterCards()[0]);
                getCharacterCards()[0] = subject;
                getPositions()[0].addCard(subject);
                subject.myPlayer = getInts()[0];

                StaticData.board.calculateScores();
            }
        }
        else if (getNature() == Nature.FINALIZE_PLAY_PHASE)
        {
            //nothing
        }
        else if (getNature() == Nature.FINISH)
        {
            //nothing
        }
        else if (getNature() == Nature.GAME_END)
        {
            //nothing
        }
        else if (getNature() == Nature.GAME_START)
        {
            //nothing
        }
        else if (getNature() == Nature.ONGOING)
        {
            //nothing
        }
        else if (getNature() == Nature.ON_REVEAL)
        {
            //nothing
        }
        else if (getNature() == Nature.LOCATION_EFFECT)
        {
            //nothing
        }
        else if (getNature() == Nature.PERM_ALTER_POWER)
        {
            CharacterCard subject = getCharacterCards()[0];
            subject.changePermanentPower(getInts()[0]);

            subject.updatePowerAndCostDisplay();
            StaticData.board.calculateScores();
        }
        else if (getNature() == Nature.PLAY_CARD)
        {
            CharacterCard subject = getCharacterCards()[0];
            subject.setToUnrevealedPosition();
        }
        else if (getNature() == Nature.PLAY_PHASE)
        {
            //nothing
            if (!StaticData.board.endPlayPhase)
            {
                return;
            }
            StaticData.board.endPlayPhase = false;
        }
        else if (getNature() == Nature.REGISTER_MOVE)
        {
            StaticData.board.calculateScores();
        }
        else if (getNature() == Nature.RELOCATE_CARD)
        {
            CharacterCard subject = getCharacterCards()[0];
            PositionState from = getPositions()[0];
            PositionState to = getPositions()[1];
            if (!(from.isEmpty() || to.isFull()))
            {
                from.removeCard(subject);
                to.addCard(subject);
                subject.myPlayer = to.myPlayer;
            }

            StaticData.board.calculateScores();
        }
        else if (getNature() == Nature.REVEAL_CARD)
        {
            CharacterCard subject = getCharacterCards()[0];
            subject.setToRevealedPosition();

            StaticData.board.calculateScores();
        }
        else if (getNature() == Nature.SHUFFLE)
        {
            ((Deck)getPositions()[0]).shuffle();
        }
        else if (getNature() == Nature.STANDBY)
        {
            return;
        }
        else if (getNature() == Nature.TEMP_ALTER_POWER)
        {
            CharacterCard subject = getCharacterCards()[0];
            subject.changeTemporaryPower(getInts()[0]);

            subject.updatePowerAndCostDisplay();
            StaticData.board.calculateScores();
        }
        else if (getNature() == Nature.TRANSFORM_CARD)
        {
            CharacterCard old = getCharacterCards()[0];
            CharacterCard newCard = Object.Instantiate(getCharacterCards()[1]);
            newCard.myPlayer = old.myPlayer;
            getCharacterCards()[1] = newCard;
            old.positionState.replaceCard(old, newCard);
            Object.Destroy(old);

            StaticData.board.calculateScores();
        }
        else if (getNature() == Nature.TURN_END)
        {
            //nothing
        }
        else if (getNature() == Nature.TURN_START)
        {
            //nothing
        }
        stage = Stage.COMPLETED;
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
        GAME_START, TURN_START, PLAY_PHASE, TURN_END, GAME_END, FINISH, STANDBY, PLAY_CARD, FINALIZE_PLAY_PHASE, SHUFFLE,
        REVEAL_CARD, REGISTER_MOVE, ON_REVEAL, ONGOING, LOCATION_EFFECT,
        PERM_ALTER_POWER, TEMP_ALTER_POWER, ALTER_COST,
        CREATE_CARD, RELOCATE_CARD, CHANGE_LOCATION, ALTER_ONGOING, TRANSFORM_CARD
    }
    public enum Stage
    {
        PERMISSION, ANIMATING, ACTING, COMPLETED, DENIED
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
    public static void createCard(CharacterCard prefab, PositionState dest)
    {
        dest.cardsHere.Add(Object.Instantiate(prefab));
        dest.updateCardPositions();
    }
    public static void move(CharacterCard card, PositionState origin, PositionState dest)
    {
        origin.removeCard(card);
        dest.addCard(card);
    }
    public static void changeLocation(LaneSegment seg, Location loc)
    {
        Object.Destroy(seg.lane.location);
        seg.lane.setLocation(loc);
    }
}
