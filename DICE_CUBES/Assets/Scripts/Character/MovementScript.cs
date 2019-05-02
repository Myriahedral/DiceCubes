using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpingTime;
    [SerializeField] private AnimationClip bWobble;
    [SerializeField] private GameObject ripplePart;
    private bool canJump = false;

    private void Update()
    {
        if (!canJump) { return; }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Direction.Down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Direction.Left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Direction.Right);
            }

    }

    public void SetCanJump(bool value)
    {
        canJump = value;
    }

    private void Move(Direction direction)
    {
        //Find target position
        Vector2Int currentGridPosition = Grid.instance.GetClosestCoordinates(transform.position);
        Vector2Int targetCoordinate = Grid.instance.GetNeighbour(currentGridPosition.x, currentGridPosition.y, direction);
        Dice targetDice = Grid.instance.GetDiceFromCoordinates(targetCoordinate.x, targetCoordinate.y);

        switch (direction)
        {
            case Direction.Down:
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;

            case Direction.Up:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case Direction.Left:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign((float)direction), 0));
                break;

            case Direction.Right:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign((float)direction), 0));
                break;
        }


        if (targetDice != null)
        {
            //Start Movement
            StartCoroutine(Jump(targetDice));

            GameManager.instance.Moved();

            //Turn dice
            Dice leftDice = Grid.instance.GetDiceFromCoordinates(currentGridPosition.x, currentGridPosition.y);
            leftDice.RotateDice(direction);
        }

        
    }

    private IEnumerator Jump(Dice targetDice)
    {
        canJump = false;
        Vector3 originPosition = transform.position;
        Vector3 targetPosition = targetDice.transform.position;


        float t = 0;

        while (t < jumpingTime)
        {
            float a = t / jumpingTime;
            float x = Mathf.Lerp(originPosition.x, targetPosition.x, a);
            float y = originPosition.y + jumpCurve.Evaluate(a);
            float z = Mathf.Lerp(originPosition.z, targetPosition.z, a);
            transform.position = new Vector3(x, y, z);
            t += Time.deltaTime; 
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, originPosition.y, targetPosition.z);

        targetDice.BigWobble();
        Instantiate(ripplePart, targetDice.transform.position, Quaternion.identity);
        transform.parent = targetDice.anim.gameObject.transform;
        yield return new WaitForSecondsRealtime(bWobble.length);
        transform.parent = null;

        canJump = true;
    }
}
