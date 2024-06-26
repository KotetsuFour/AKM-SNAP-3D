using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerConnector : NetworkBehaviour
{
    public NetworkVariable<int[]>[] playerActions;
    public NetworkVariable<List<int>>[] decks;
    // Start is called before the first frame update
    void Awake()
    {
        playerActions = new NetworkVariable<int[]>[StaticData.numPlayers];
        for (int q = 0; q < playerActions.Length; q++)
        {
            playerActions[q] = new NetworkVariable<int[]>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        }
        playerActions[0].Value = new int[3];

        decks = new NetworkVariable<List<int>>[StaticData.numPlayers];
        for (int q = 0; q < decks.Length; q++)
        {
            decks[q] = new NetworkVariable<List<int>>(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
