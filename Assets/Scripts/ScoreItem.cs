using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public ItemDeta itemdeta;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        GetComponent <SpriteRenderer>().sprite = itemdeta.itemSprite;
    }

   
}
