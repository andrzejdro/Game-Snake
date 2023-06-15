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

    public void Setup(Snake snake) //odwo�anie do GameHandler
    {
        this.snake = snake;

        SpawnFood();
    }

    private void SpawnFood() 
    {
        do
        {
          foodGridPosition = new Vector2Int(Random.Range(1, width - 3), Random.Range(1, height - 3));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1); //spawn jab�ka w innym miejscu ni� pozycja snake

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer)); //Tworzy nam "mijsce" w unity, �eby pod�o�yc grafik� do jab�ka w typie SpriteRenderer
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)  //funkcja sprawdza pozycj� w�a oraz jab�ka, kiedy jest ona tama sama jab�ko si� niszczy, po czym tworzy si� kolejne. Dla potiwerdzneia dzia�ania konsola wypisuje nam komunikat "Snake ate food"
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
