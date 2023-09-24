using UnityEngine;
using System.Collections.Generic;


public class Cache
{

    private static Dictionary<float, WaitForSeconds> m_WFS = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWFS(float key)
    {
        if(!m_WFS.ContainsKey(key))
        {
            m_WFS[key] = new WaitForSeconds(key);
        }

        return m_WFS[key];
    }

    //------------------------------------------------------------------------------------------------------------
    //Cache player
    private static Dictionary<Collider2D, Player> m_Player = new Dictionary<Collider2D, Player>();
    public static Player GetPlayer(Collider2D key)
    {
        if (!m_Player.ContainsKey(key))
        {
            Player player = key.GetComponent<Player>();

            if (player != null)
            {
                m_Player.Add(key, player);
            }
            else
            {
                return null;
            }
        }
        return m_Player[key];
    }

    private static Dictionary<Collider2D, Player_Golden> m_PlayerOnFinalMap = new Dictionary<Collider2D, Player_Golden>();
    public static Player_Golden GetPlayerOnFinalMap(Collider2D key)
    {
        if (!m_PlayerOnFinalMap.ContainsKey(key))
        {
            Player_Golden player = key.GetComponent<Player_Golden>();

            if (player != null)
            {
                m_PlayerOnFinalMap.Add(key, player);
            }
            else
            {
                return null;
            }
        }
        return m_PlayerOnFinalMap[key];
    }

}

