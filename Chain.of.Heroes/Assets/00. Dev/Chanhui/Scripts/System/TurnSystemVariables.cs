using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class TurnSystem : MonoBehaviour
{
    public bool m_IsPlayerTurn = true;

    void Initialize()
    {
        Property.IsPlayerTurn = m_IsPlayerTurn;
        
    }
}