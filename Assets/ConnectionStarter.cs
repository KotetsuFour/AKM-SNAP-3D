using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ConnectionStarter : MonoBehaviour
{
    [SerializeField] private GameObject playerConnectorPrefab;
    private void Start()
    {
        GetComponent<NetworkManager>().AddNetworkPrefab(playerConnectorPrefab);
        DontDestroyOnLoad(gameObject);
    }
    public void startAsHost(int numPlayers)
    {
        GetComponent<NetworkManager>().StartHost();
        StaticData.numPlayers = numPlayers;
        StaticData.player = 0;
    }
    public void startAsClient()
    {
        GetComponent<NetworkManager>().StartClient();
        GameObject found = GameObject.Find("Player Connector");
        if (found == null)
        {
            found = GameObject.Find("Player Connector(Clone)");
        }
        int idx = 0;
        while (found.GetComponent<PlayerConnector>().playerActions[idx] != null)
        {
            idx++;
        }
        StaticData.player = idx;
        found.GetComponent<PlayerConnector>().playerActions[StaticData.player].Value = new int[3];
    }
}
