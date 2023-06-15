using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax; //czas pomiedzy wykonaniem kolejnego ruchu
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI pointsText;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10); //miejsce startu Snake
        gridMoveTimerMax = 0.3f; // ruch co 0,3 sekundy (1f = 1 sekunda)
        gridMoveTimer = gridMoveTimerMax; //ciagly ruch
        gridMoveDirection = new Vector2Int(1, 0); //domyœlnie ruch snake zacznie siê w prawo po ropoczêciu gry, dziêki temu nie bêdzie sta³ w miejscu zanim gracz wska¿e Snake kierunek

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>(); //tworze liste o typie SnakeBodyPart
    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();
        pointsText.text = "Score: " + levelGrid.Points;
    }

    private void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow)) //ruch w gore
        {
            if (gridMoveDirection.y != -1) //nie mozemy poruszac sie w dol, jesli obecnie idziemy w gore, sami byœmy spowodowali kolizje z wlasnym ogonem
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }

        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) //ruch w dol
        {
            if (gridMoveDirection.y != +1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) //ruch w lewo
        {
            if (gridMoveDirection.x != +1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }

        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) //ruch w prawo
        {
            if (gridMoveDirection.x != -1)
            {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }

        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime; //czas od poprzedniego sprawdzenia ruchu gracza
        if (gridMoveTimer >= gridMoveTimerMax) //czy od ostatniego ruchu minê³o wystarczaj¹co du¿o czasu.
        {
            gridMoveTimer -= gridMoveTimerMax; //kod bêdzie siê odpala³ co 1 sekunde w celu sprawdzenia dalszego ruchu
            

            snakeMovePositionList.Insert(0, gridPosition); //dodaje bie¿¹c¹ pozycjê wê¿a na pocz¹tek jego listy ruchów

            gridPosition += gridMoveDirection; //aktualizuje pozycjê wê¿a na siatce na podstawie jego bie¿¹cego kierunku ruchu

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood) //w momêcie zjedzenia jab³ka w¹¿ roœnie
            {
                snakeBodySize++;
                CreateSnakeBodyPart();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1) //sprawdzamy liste, w momencie kiedy wielkosc snake jest wiêksza lub równa, odjemienimy 1
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            /* foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    Debug.Log("Dead", transform.position);
                }
                  
            } */



            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90); //-90 wynika z domyœlnych ustawieñ unity, dziêki wprowadzeniu tego g³owa bêdzie siê poprawnie obracaæ po skeceniu

            UpdateSnakeBodyParts();

         

            CheckColisionItself();
        }

        /* public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        } */


    }
  
    private void CheckColisionItself()
    {
        for (int i = 1; i < snakeMovePositionList.Count; i++)
        {
            if (gridPosition == snakeMovePositionList[i])
            {
                Debug.Log("Game Over");
                Time.timeScale = 0;
                gameOverText.text = "GameOver\n Your score is: " + levelGrid.Points;

                // GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Game Over");
            }
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int dir) //dziêki tej funckji g³owa Snake, bêdzie siê obracaæ w kierunku obecengo ruchu po skrêcaniu
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList() //funkcja sprawdza nam aktuln¹ pozycjê naszego ca³ego snake i j¹ nam zwraca. Jest nam to potrzebne, ¿eby nie spawnowaæ jab³ka w miejscu snake
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>()
        {
            gridPosition
        };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }


    private class SnakeBodyPart
    {
        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex) //Tworzy nam "miejsce" w unity,¿eby pod³o¿yc grafikê do cia³a snake w typie SpriteRenderer
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = snakeBodyGameObject.transform;
        }


        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //funckja wbudowana w unity 
    {
        if(collision.tag == "Wall")
        {
            Debug.Log("Game Over");
            Time.timeScale = 0;
            gameOverText.text = "GameOver\n Your score is: " + levelGrid.Points; 
        }
    }

}


