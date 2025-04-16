using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int maxHp = 50;
    private int playerHp;
    public int PlayerHp 
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

    public int MaxHp { get { return maxHp; } }
    
   
}
