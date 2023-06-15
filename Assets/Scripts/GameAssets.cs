using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameAssets : MonoBehaviour
{
    public static GameAssets i; //instance "i" bêdzie krócej

    private void Awake()
    {
        i = this; //Dziêki temu bêdê móg³ siê odwo³ywaæ "public Sprite" do "i" jak np. snakeHeadSprite 
    }
            
    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;
    public Sprite foodSprite; //LevelGrid
}
