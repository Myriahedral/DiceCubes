using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    private Dictionary<string, int> faceNeighbours = new Dictionary<string, int>();
    private int currentFace;

    [Header("Animation")]
    public AnimationCurve rotationCurve;
    public float rotationTime;

    public int GetOppositeFace(int faceIndex)
    {
        return 5 - faceIndex;
    }

    private void InitDice()
    {
        currentFace = 0;
        faceNeighbours[Direction.Up.ToString()] = 4;
        faceNeighbours[Direction.Down.ToString()] = 1;
        faceNeighbours[Direction.Left.ToString()] = 2;
        faceNeighbours[Direction.Right.ToString()] = 3;
    }

    private void Start()
    {
        InitDice();
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateDice(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotateDice(Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateDice(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateDice(Direction.Right);
        }


    }*/

    public void RotateDice(Direction direction)
    {
        //Update currentFace and neighbours
        int previousFace = currentFace;
        

        switch (direction)
        {
            case Direction.Down:
                currentFace = faceNeighbours[Direction.Up.ToString()];
                faceNeighbours[Direction.Down.ToString()] = previousFace;
                faceNeighbours[Direction.Up.ToString()] = GetOppositeFace(previousFace);
                break;

            case Direction.Up:
                currentFace = faceNeighbours[Direction.Down.ToString()];
                faceNeighbours[Direction.Up.ToString()] = previousFace;
                faceNeighbours[Direction.Down.ToString()] = GetOppositeFace(previousFace);
                break;

            case Direction.Left:
                currentFace = faceNeighbours[Direction.Right.ToString()];
                faceNeighbours[Direction.Left.ToString()] = previousFace;
                faceNeighbours[Direction.Right.ToString()] = GetOppositeFace(previousFace);
                break;

            case Direction.Right:
                currentFace = faceNeighbours[Direction.Left.ToString()];
                faceNeighbours[Direction.Right.ToString()] = previousFace;
                faceNeighbours[Direction.Left.ToString()] = GetOppositeFace(previousFace);
                break;
        }

        print(currentFace+1);

        StartCoroutine(RotationCoroutine(direction));
    }

    private IEnumerator RotationCoroutine(Direction direction)
    {
        Quaternion originRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.identity;

        if (direction == Direction.Up || direction == Direction.Down)
        {
            targetRotation = Quaternion.Euler(new Vector3(-90 * Mathf.Sign((float)direction), 0, 0)) * transform.rotation;
        }
        else
        {
            targetRotation = Quaternion.Euler(new Vector3(0, 0, -90 * Mathf.Sign((float)direction))) * transform.rotation;
        }

        float t = 0;

        while (t < rotationTime)
        {
            transform.rotation = Quaternion.Slerp(originRotation, targetRotation, rotationCurve.Evaluate(t*1/rotationTime));
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        
    }
}
