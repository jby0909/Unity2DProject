using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHp = 50;
    private float playerHp;
    public float PlayerHp 
    {
        get
        {
            return playerHp;
        }
        set 
        {
            if(value <= 0)
            {
                playerHp = 0;
            }
            else if(value > maxHp)
            {
                playerHp = maxHp;
            }
            else
            {
                playerHp = value;
            }
        } 
    }
    
   
}
