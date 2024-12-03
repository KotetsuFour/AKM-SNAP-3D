using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class NetworkLogic : NetworkBehaviour
{
    private bool transferredDeck;
    private bool waitingForOtherDecks;
    private NetworkVariable<DeckPackage> serverDeck = new NetworkVariable<DeckPackage>(new DeckPackage(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<DeckPackage> clientDeck0 = new NetworkVariable<DeckPackage>(new DeckPackage(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<DeckPackage> clientDeck1 = new NetworkVariable<DeckPackage>(new DeckPackage(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<TurnActions> serverActions = new NetworkVariable<TurnActions>(new TurnActions(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<TurnActions> clientActions0 = new NetworkVariable<TurnActions>(new TurnActions(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<TurnActions> clientActions1 = new NetworkVariable<TurnActions>(new TurnActions(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public List<int>[] playerActions;
    public struct DeckPackage : INetworkSerializable
    {
        public FixedString128Bytes playerId;
        public FixedString32Bytes playerName;
        public bool filled;
        public int card0;
        public int card1;
        public int card2;
        public int card3;
        public int card4;
        public int card5;
        public int card6;
        public int card7;
        public int card8;
        public int card9;
        public int card10;
        public int card11;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref playerId);
            serializer.SerializeValue(ref playerName);
            serializer.SerializeValue(ref filled);
            serializer.SerializeValue(ref card0);
            serializer.SerializeValue(ref card1);
            serializer.SerializeValue(ref card2);
            serializer.SerializeValue(ref card3);
            serializer.SerializeValue(ref card4);
            serializer.SerializeValue(ref card5);
            serializer.SerializeValue(ref card6);
            serializer.SerializeValue(ref card7);
            serializer.SerializeValue(ref card8);
            serializer.SerializeValue(ref card9);
            serializer.SerializeValue(ref card10);
            serializer.SerializeValue(ref card11);
        }

    }
    public struct TurnActions : INetworkSerializable
    {
        public int player;
        public int turn;
        public int numActions;
        public int actionType0;
        public int handPos0;
        public int laneplayed0;
        public int actionType1;
        public int handPos1;
        public int laneplayed1;
        public int actionType2;
        public int handPos2;
        public int laneplayed2;
        public int actionType3;
        public int handPos3;
        public int laneplayed3;
        public int actionType4;
        public int handPos4;
        public int laneplayed4;
        public int actionType5;
        public int handPos5;
        public int laneplayed5;
        public int actionType6;
        public int handPos6;
        public int laneplayed6;
        public int actionType7;
        public int handPos7;
        public int laneplayed7;
        public int actionType8;
        public int handPos8;
        public int laneplayed8;
        public int actionType9;
        public int handPos9;
        public int laneplayed9;
        public int actionType10;
        public int handPos10;
        public int laneplayed10;
        public int actionType11;
        public int handPos11;
        public int laneplayed11;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref player);
            serializer.SerializeValue(ref turn);
            serializer.SerializeValue(ref numActions);
            serializer.SerializeValue(ref actionType0);
            serializer.SerializeValue(ref handPos0);
            serializer.SerializeValue(ref laneplayed0);
            serializer.SerializeValue(ref actionType1);
            serializer.SerializeValue(ref handPos1);
            serializer.SerializeValue(ref laneplayed1);
            serializer.SerializeValue(ref actionType2);
            serializer.SerializeValue(ref handPos2);
            serializer.SerializeValue(ref laneplayed2);
            serializer.SerializeValue(ref actionType3);
            serializer.SerializeValue(ref handPos3);
            serializer.SerializeValue(ref laneplayed3);
            serializer.SerializeValue(ref actionType4);
            serializer.SerializeValue(ref handPos4);
            serializer.SerializeValue(ref laneplayed4);
            serializer.SerializeValue(ref actionType5);
            serializer.SerializeValue(ref handPos5);
            serializer.SerializeValue(ref laneplayed5);
            serializer.SerializeValue(ref actionType6);
            serializer.SerializeValue(ref handPos6);
            serializer.SerializeValue(ref laneplayed6);
            serializer.SerializeValue(ref actionType7);
            serializer.SerializeValue(ref handPos7);
            serializer.SerializeValue(ref laneplayed7);
            serializer.SerializeValue(ref actionType8);
            serializer.SerializeValue(ref handPos8);
            serializer.SerializeValue(ref laneplayed8);
            serializer.SerializeValue(ref actionType9);
            serializer.SerializeValue(ref handPos9);
            serializer.SerializeValue(ref laneplayed9);
            serializer.SerializeValue(ref actionType10);
            serializer.SerializeValue(ref handPos10);
            serializer.SerializeValue(ref laneplayed10);
            serializer.SerializeValue(ref actionType11);
            serializer.SerializeValue(ref handPos11);
            serializer.SerializeValue(ref laneplayed11);
        }

    }

    public override void OnNetworkSpawn()
    {
        int[] order = new int[StaticData.myDeck.Count];
        for (int q = 0; q < order.Length; q++)
        {
            order[q] = q;
        }
        for (int q = 0; q < order.Length; q++)
        {
            int replaceLoc = Random.Range(0, order.Length);
            int temp = order[q];
            order[q] = order[replaceLoc];
            order[replaceLoc] = temp;
        }

        DeckPackage packToSend = new DeckPackage();
        packToSend.playerId = StaticData.playerLoginId;
        packToSend.playerName = StaticData.playerName;
        packToSend.filled = true;
        packToSend.card0 = StaticData.myDeck[order[0]];
        packToSend.card1 = StaticData.myDeck[order[1]];
        packToSend.card2 = StaticData.myDeck[order[2]];
        packToSend.card3 = StaticData.myDeck[order[3]];
        packToSend.card4 = StaticData.myDeck[order[4]];
        packToSend.card5 = StaticData.myDeck[order[5]];
        packToSend.card6 = StaticData.myDeck[order[6]];
        packToSend.card7 = StaticData.myDeck[order[7]];
        packToSend.card8 = StaticData.myDeck[order[8]];
        packToSend.card9 = StaticData.myDeck[order[9]];
        packToSend.card10 = StaticData.myDeck[order[10]];
        packToSend.card11 = StaticData.myDeck[order[11]];

        if (IsServer)
        {
            serverDeck.Value = packToSend;
        }
        else
        {
            DeckServerRpc(packToSend);
        }
        playerActions = new List<int>[3];
        for (int q = 0; q < playerActions.Length; q++)
        {
            playerActions[q] = new List<int>();
        }
    }

    public void setActionsForTurn(List<int> myActions)
    {
        TurnActions acts = new TurnActions();
        acts.player = StaticData.player;
        acts.turn = myActions[0];
        acts.numActions = (myActions.Count - 1) / 3;
        if (acts.numActions >= 1)
        {
            acts.actionType0 = myActions[1];
            acts.handPos0 = myActions[2];
            acts.laneplayed0 = myActions[3];
        }
        if (acts.numActions >= 2)
        {
            acts.actionType1 = myActions[4];
            acts.handPos1 = myActions[5];
            acts.laneplayed1 = myActions[6];
        }
        if (acts.numActions >= 3)
        {
            acts.actionType2 = myActions[7];
            acts.handPos2 = myActions[8];
            acts.laneplayed2 = myActions[9];
        }
        if (acts.numActions >= 4)
        {
            acts.actionType3 = myActions[10];
            acts.handPos3 = myActions[11];
            acts.laneplayed3 = myActions[12];
        }
        if (acts.numActions >= 5)
        {
            acts.actionType4 = myActions[13];
            acts.handPos4 = myActions[14];
            acts.laneplayed4 = myActions[15];
        }
        if (acts.numActions >= 6)
        {
            acts.actionType5 = myActions[16];
            acts.handPos5 = myActions[17];
            acts.laneplayed5 = myActions[18];
        }
        if (acts.numActions >= 7)
        {
            acts.actionType6 = myActions[19];
            acts.handPos6 = myActions[20];
            acts.laneplayed6 = myActions[21];
        }
        if (acts.numActions >= 8)
        {
            acts.actionType7 = myActions[22];
            acts.handPos7 = myActions[23];
            acts.laneplayed7 = myActions[24];
        }
        if (acts.numActions >= 9)
        {
            acts.actionType8 = myActions[25];
            acts.handPos8 = myActions[26];
            acts.laneplayed8 = myActions[27];
        }
        if (acts.numActions >= 10)
        {
            acts.actionType9 = myActions[28];
            acts.handPos9 = myActions[29];
            acts.laneplayed9 = myActions[30];
        }
        if (acts.numActions >= 11)
        {
            acts.actionType10 = myActions[31];
            acts.handPos10 = myActions[32];
            acts.laneplayed10 = myActions[33];
        }
        if (acts.numActions >= 12)
        {
            acts.actionType11 = myActions[34];
            acts.handPos11 = myActions[35];
            acts.laneplayed11 = myActions[36];
        }

        TurnServerRpc(acts);
        waitingForOtherDecks = true;
    }

    private void Update()
    {
        if (!transferredDeck)
        {
            if (serverDeck.Value.filled
                && clientDeck0.Value.filled
                && (StaticData.numPlayers == 2 || clientDeck1.Value.filled))
            {
                StaticData.board.setDeck(0, serverDeck.Value);
                StaticData.board.setDeck(1, clientDeck0.Value);
                if (StaticData.numPlayers == 3)
                {
                    StaticData.board.setDeck(2, clientDeck1.Value);
                }
                if (IsServer)
                {
                    StaticData.player = 0;
                }
                else if (clientDeck0.Value + "" == StaticData.playerLoginId)
                {
                    StaticData.player = 1;
                }
                else if (clientDeck1.Value + "" == StaticData.playerLoginId)
                {
                    StaticData.player = 2;
                }
                StaticData.board.startGame();
                transferredDeck = true;
            }
        }
        else if (waitingForOtherDecks)
        {
            int turn = StaticData.board.turn;
            if (serverActions.Value.turn == turn
                && clientActions0.Value.turn == turn
                && (StaticData.numPlayers == 2 || clientActions1.Value.turn == turn))
            {
                interpretPlayerActions(serverActions.Value, playerActions[0]);
                interpretPlayerActions(clientActions0.Value, playerActions[1]);
                if (StaticData.numPlayers == 3)
                {
                    interpretPlayerActions(clientActions1.Value, playerActions[2]);
                }
                StaticData.board.endPlayPhase = true;
            }
        }
    }
    public void interpretPlayerActions(TurnActions acts, List<int> actionsList)
    {
        actionsList.Clear();
        actionsList.Add(acts.player);
        if (acts.numActions >= 1)
        {
            actionsList.Add(acts.actionType0);
            actionsList.Add(acts.handPos0);
            actionsList.Add(acts.laneplayed0);
        }
        if (acts.numActions >= 2)
        {
            actionsList.Add(acts.actionType1);
            actionsList.Add(acts.handPos1);
            actionsList.Add(acts.laneplayed1);
        }
        if (acts.numActions >= 3)
        {
            actionsList.Add(acts.actionType2);
            actionsList.Add(acts.handPos2);
            actionsList.Add(acts.laneplayed2);
        }
        if (acts.numActions >= 4)
        {
            actionsList.Add(acts.actionType3);
            actionsList.Add(acts.handPos3);
            actionsList.Add(acts.laneplayed3);
        }
        if (acts.numActions >= 5)
        {
            actionsList.Add(acts.actionType4);
            actionsList.Add(acts.handPos4);
            actionsList.Add(acts.laneplayed4);
        }
        if (acts.numActions >= 6)
        {
            actionsList.Add(acts.actionType5);
            actionsList.Add(acts.handPos5);
            actionsList.Add(acts.laneplayed5);
        }
        if (acts.numActions >= 7)
        {
            actionsList.Add(acts.actionType6);
            actionsList.Add(acts.handPos6);
            actionsList.Add(acts.laneplayed6);
        }
        if (acts.numActions >= 8)
        {
            actionsList.Add(acts.actionType7);
            actionsList.Add(acts.handPos7);
            actionsList.Add(acts.laneplayed7);
        }
        if (acts.numActions >= 9)
        {
            actionsList.Add(acts.actionType8);
            actionsList.Add(acts.handPos8);
            actionsList.Add(acts.laneplayed8);
        }
        if (acts.numActions >= 10)
        {
            actionsList.Add(acts.actionType9);
            actionsList.Add(acts.handPos9);
            actionsList.Add(acts.laneplayed9);
        }
        if (acts.numActions >= 11)
        {
            actionsList.Add(acts.actionType10);
            actionsList.Add(acts.handPos10);
            actionsList.Add(acts.laneplayed10);
        }
        if (acts.numActions >= 12)
        {
            actionsList.Add(acts.actionType11);
            actionsList.Add(acts.handPos11);
            actionsList.Add(acts.laneplayed11);
        }
    }

    [ServerRpc]
    public void DeckServerRpc(DeckPackage deck)
    {
        if (!clientDeck0.Value.filled)
        {
            clientDeck0.Value = deck;
        }
        else
        {
            clientDeck1.Value = deck;
        }
    }

    [ServerRpc]
    public void TurnServerRpc(TurnActions acts)
    {
        if (acts.player == 0)
        {
            serverActions.Value = acts;
        }
        else if (acts.player == 1)
        {
            clientActions0.Value = acts;
        }
        else if (acts.player == 2)
        {
            clientActions1.Value = acts;
        }
    }

}
