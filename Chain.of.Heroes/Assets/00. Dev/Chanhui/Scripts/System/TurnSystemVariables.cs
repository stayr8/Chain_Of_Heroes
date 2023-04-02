using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class TurnSystem : MonoBehaviour
{
    public bool m_IsPlayerTurn = true;
    //public int m_allPlayerPoints = 0;
    //public int m_allEnemyPoints = 0;

    void Initialize()
    {
        Property.IsPlayerTurn = m_IsPlayerTurn;
        //Property.AllPlayerPoint = m_allPlayerPoints;
        //Property.AllEnemyPoint = m_allEnemyPoints;
    }
}