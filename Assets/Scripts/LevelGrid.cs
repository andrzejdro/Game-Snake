using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private int width;
    private int height;
    private Snake snake;
    public int Points = 0;


    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake) //odwo³anie do GameHandler
    {
        this.snake = snake;

        SpawnFood();
    }

    private void SpawnFood() 
    {
        do
        {
          foodGridPosition = new Vector2Int(Random.Range(1, width - 3), Random.Range(1, height - 3));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1); //spawn jab³ka w innym miejscu ni¿ pozycja snake

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer)); //Tworzy nam "mijsce" w unity, ¿eby pod³o¿yc grafikê do jab³ka w typie SpriteRenderer
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)  //funkcja sprawdza pozycjê wê¿a oraz jab³ka, kiedy jest ona tama sama jab³ko siê niszczy, po czym tworzy siê kolejne. Dla potiwerdzneia dzia³ania konsola wypisuje nam komunikat "Snake ate food"
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            Debug.Log("Snake ate food");
            Points++;
            return true;
        } else
        {
            return false;
        }    
    }
}
