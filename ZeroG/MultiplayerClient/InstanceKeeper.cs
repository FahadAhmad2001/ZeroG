using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteNetLib;

namespace ZeroG.MultiplayerClient
{
    public static class InstanceKeeper
    {
        public static GameClient gClient;
        public static Main mainClient;
        public static AccountMenu accountMenu;
        public static void SetGameClient(GameClient gameClient)
        {
            gClient = gameClient;
        }
        public static void SetAccountMenu(AccountMenu menu)
        {
            accountMenu = menu;
        }
        public static AccountMenu GetAccountMenu()
        {
            return accountMenu;
        }
        public static GameClient GetGameClient()
        {
            return gClient;
        }
        public static void SetMainClient(Main mClient)
        {
            mainClient = mClient;
        }
        public static Main GetMainClient()
        {
            return mainClient;
        }
    }
}
