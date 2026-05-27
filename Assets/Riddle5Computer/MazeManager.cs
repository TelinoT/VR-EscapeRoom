using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform snake;
    public RectTransform exitZone;
    public GameObject winScreen; 

    [Header("Maze Texture Settings")]
    public RawImage mazeDisplay;
    public Texture2D mazeTexture;
    public Color pathColor = Color.black; 
    public float colorTolerance = 0.2f; 

    [Header("Movement Settings")]
    public float stepSize = 10f; 
    public float winDistance = 25f;

    private bool isWon = false;

    public GameObject PoweredOnStuff;

    public GameObject finalSequence;

    void Start()
    {
        // Ensure the win screen is hidden when the game starts
        if (winScreen != null) winScreen.SetActive(false);
    }

    public void MoveSnake(Vector2 direction)
    {
        if (isWon) return;

        if (PoweredOnStuff != null && !PoweredOnStuff.activeInHierarchy) return;

        // Calculate the next step
        Vector2 targetPosition = snake.anchoredPosition + (direction * stepSize);

        // Check if that step hits a neon wall
        if (IsPositionWalkable(targetPosition))
        {
            snake.anchoredPosition = targetPosition;
            CheckWinCondition();
        }
    }

    private bool IsPositionWalkable(Vector2 uiPosition)
    {
        Rect mazeRect = mazeDisplay.rectTransform.rect;
        
        // Convert UI coordinates to image pixel coordinates
        float normalizedX = (uiPosition.x - mazeRect.x) / mazeRect.width;
        float normalizedY = (uiPosition.y - mazeRect.y) / mazeRect.height;

        int pixelX = Mathf.RoundToInt(normalizedX * mazeTexture.width);
        int pixelY = Mathf.RoundToInt(normalizedY * mazeTexture.height);

        // Block movement if trying to walk off the edge of the monitor
        if (pixelX < 0 || pixelX >= mazeTexture.width || pixelY < 0 || pixelY >= mazeTexture.height)
            return false;

        // Read the color of the pixel
        Color pixelColor = mazeTexture.GetPixel(pixelX, pixelY);

        // Compare the pixel color to your allowed path color
        float rDiff = Mathf.Abs(pixelColor.r - pathColor.r);
        float gDiff = Mathf.Abs(pixelColor.g - pathColor.g);
        float bDiff = Mathf.Abs(pixelColor.b - pathColor.b);

        return (rDiff < colorTolerance && gDiff < colorTolerance && bDiff < colorTolerance);
    }

    private void CheckWinCondition()
    {
        float distance = Vector2.Distance(snake.anchoredPosition, exitZone.anchoredPosition);
        
        if (distance <= winDistance)
        {
            isWon = true;
            winScreen.SetActive(true); // Trigger your custom graphic!
            PoweredOnStuff.SetActive(false);
            if (BombMachineManager.Instance != null)
            {
                BombMachineManager.Instance.ComputerRiddleDone();
                BombMachineManager.Instance.riddlesUp();
            }
            finalSequence.SetActive(true);
        }
    }
}