using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    private Dictionary<string, int> faceNeighbours = new Dictionary<string, int>();
    private int currentFace;

    [Header("Animation")]
    [SerializeField] private GameObject mesh;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float rotationTime;
    [SerializeField] public Animator anim;

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

        anim.Play("Idle", 0, Random.Range(0f, 100f));
    }

    private void Start()
    {
        InitDice();
    }

    public void RotateDice(Direction direction)
    {
        //Update currentFace and neighbours
        int previousFace = currentFace;
        

        switch (direction)
        {
            case Direction.Down:
                currentFace = faceNeighbours[Direction.Down.ToString()];
                faceNeighbours[Direction.Up.ToString()] = previousFace;
                faceNeighbours[Direction.Down.ToString()] = GetOppositeFace(previousFace);
                break;

            case Direction.Up:
                currentFace = faceNeighbours[Direction.Up.ToString()];
                faceNeighbours[Direction.Down.ToString()] = previousFace;
                faceNeighbours[Direction.Up.ToString()] = GetOppositeFace(previousFace);
                break;

            case Direction.Left:
                currentFace = faceNeighbours[Direction.Left.ToString()];
                faceNeighbours[Direction.Right.ToString()] = previousFace;
                faceNeighbours[Direction.Left.ToString()] = GetOppositeFace(previousFace);
                break;

            case Direction.Right:
                currentFace = faceNeighbours[Direction.Right.ToString()];
                faceNeighbours[Direction.Left.ToString()] = previousFace;
                faceNeighbours[Direction.Right.ToString()] = GetOppositeFace(previousFace);
                break;
        }

        StartCoroutine(RotationCoroutine(direction));
    }

    private IEnumerator RotationCoroutine(Direction direction)
    {
        Quaternion originRotation = mesh.transform.rotation;
        Quaternion targetRotation = Quaternion.identity;
        
        if (direction == Direction.Up || direction == Direction.Down)
        {
            targetRotation = Quaternion.Euler(new Vector3(90 * Mathf.Sign((float)direction), 0, 0)) * mesh.transform.localRotation;
        }
        else
        {
            targetRotation = Quaternion.Euler(new Vector3(0, 0, 90 * Mathf.Sign((float)direction))) * mesh.transform.localRotation;
        }

        anim.Play("TinyWobble");

        float t = 0;

        while (t < rotationTime)
        {
            mesh.transform.localRotation = Quaternion.Slerp(originRotation, targetRotation, rotationCurve.Evaluate(t*1/rotationTime));
            t += Time.deltaTime;
            yield return null;
        }

        mesh.transform.localRotation = targetRotation;
        
    }

    public void BigWobble()
    {
        anim.Play("BigWobble");
    }
}
