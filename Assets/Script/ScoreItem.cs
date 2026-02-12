using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public ItemDeta itemDeta;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        GetComponent <SpriteRenderer>().sprite = itemDeta.itemSprite;
    }

   
}
