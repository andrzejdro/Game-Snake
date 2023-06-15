using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameAssets : MonoBehaviour
{
    public static GameAssets i; //instance "i" b�dzie kr�cej

    private void Awake()
    {
        i = this; //Dzi�ki temu b�d� m�g� si� odwo�ywa� "public Sprite" do "i" jak np. snakeHeadSprite 
    }
            
    public Sprite snakeHeadSprite;
    public Sprite snakeBodySprite;
    public Sprite foodSprite; //LevelGrid
}
