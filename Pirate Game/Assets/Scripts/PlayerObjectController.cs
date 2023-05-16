using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerObjectController : NetworkBehaviour
{
    //Player data
    [SyncVar] public int ConnectionID;
    [SyncVar] public int PlayerIdNumber;
    [SyncVar] public ulong PlayerSteamID;

    private CustomNetworkManger manager;

    private CustomNetworkManger Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManger.singleton as CustomNetworkManger;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnStartAuthority()
    {
        gameObject.name = "LocalGamePlayer";
    }
    public override void OnStartClient()
    {
        Manager.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Manager.GamePlayers.Remove(this);
    }

    public void CanStartGame(string SceneName)
    {
        if(isOwned)
        {
            CmdCanStartGame(SceneName);
        }
    }

    [Command]
    public void CmdCanStartGame(string SceneName)
    {
        manager.StartGame(SceneName);
    }
}
