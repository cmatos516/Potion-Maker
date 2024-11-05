using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    float[] rotations = { 0, 90, 180, 270 };
    public float[] correctRotation;
    [SerializeField] bool isPlaced = false;
    int PossibleRots = 1;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Corrected line to find the GameManager instance

        PossibleRots = correctRotation.Length;
        int rand = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);

        if (PossibleRots > 0)
        {
            if (transform.eulerAngles.z == correctRotation[0] || (PossibleRots > 1 && transform.eulerAngles.z == correctRotation[1]))
            {
                isPlaced = true;
            }
        }
    }

    private void OnMouseDown()
    {
        transform.Rotate(new Vector3(0, 0, 90));

        if (PossibleRots > 1)
        {
            if ((transform.eulerAngles.z == correctRotation[0] || (PossibleRots > 1 && transform.eulerAngles.z == correctRotation[1])) && !isPlaced)
            {
                isPlaced = true;
                gameManager.correctMove();
            }
            else if (isPlaced)
            {
                isPlaced = false;
                gameManager.wrongMove();
            }
            else if (PossibleRots > 1 && transform.eulerAngles.z == correctRotation[1] && !isPlaced)
            {
                isPlaced = true;
                gameManager.correctMove();
            }
            else if (isPlaced)
            {
                isPlaced = false;
                gameManager.wrongMove();
            }
        }
    }
}