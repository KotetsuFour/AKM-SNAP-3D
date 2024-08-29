using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkLogic : NetworkBehaviour
{
    private bool transferredDeck;
    private NetworkVariable<DeckPackage> deck = new NetworkVariable<DeckPackage>(new DeckPackage(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public struct DeckPackage
    {
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
        for (int q = 0; q < order.Length; q++)
        {
            order[q] = StaticData.myDeck[order[q]];
        }

        DeckPackage packToSend = new DeckPackage();
        packToSend.card0 = order[0];
        packToSend.card1 = order[1];
        packToSend.card2 = order[2];
        packToSend.card3 = order[3];
        packToSend.card4 = order[4];
        packToSend.card5 = order[5];
        packToSend.card6 = order[6];
        packToSend.card7 = order[7];
        packToSend.card8 = order[8];
        packToSend.card9 = order[9];
        packToSend.card10 = order[10];
        packToSend.card11 = order[11];

        deck.Value = packToSend;
    }

    private void Update()
    {
        if (!transferredDeck && !IsOwner && deck.Value.card0 != deck.Value.card1)
        {
            DeckPackage pack = deck.Value;
            StaticData.board.setDeck(1, pack);
            transferredDeck = true;
        }
    }


}
