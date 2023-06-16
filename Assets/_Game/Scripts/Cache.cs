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
    private static Dictionary<Collider, Player> m_Player = new Dictionary<Collider, Player>();
    public static Player GetPlayer(Collider key)
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

}

