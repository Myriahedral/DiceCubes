using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    private Dictionary<string, int> faceNeighbours = new Dictionary<string, int>();
    public int currentFace;

    public Vector2Int coord;

    [Header("Animation")]
    [SerializeField] private GameObject mesh;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float rotationTime;
    [SerializeField] public Animator anim;

    [Header("Faces")]
    [SerializeField] private Sprite[] faceSpr = new Sprite[6];
    [SerializeField] private SpriteRenderer[] faceRend = new SpriteRenderer[6];

    [Header("Patterns")]
    public bool isRoot;
    public List<Dice> otherDice = new List<Dice>();
    public Dice root;

    public int GetOppositeFace(int faceIndex)
    {
        return 5 - faceIndex;
    }

    public override bool Equals(System.Object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Person return false.
        Dice d = obj as Dice;
        if ((System.Object)d == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (coord == d.coord);
    }

    private void InitDice()
    {
        List<int> faces = new List<int>();
        faces.Add(0);
        faces.Add(1);
        faces.Add(2);
        faces.Add(3);
        faces.Add(4);
        faces.Add(5);

        //Top and opposite
        int t = faces[Random.Range(0, faces.Count)];
        currentFace = t;
        faceRend[0].sprite = faceSpr[t];
        faces.Remove(t);
        int o = 5 - t;
        faceRend[1].sprite = faceSpr[o];
        faces.Remove(o);

        //Up and Down
        int u = faces[Random.Range(0, faces.Count)];
        faceNeighbours[Direction.Up.ToString()] = u;
        faceRend[2].sprite = faceSpr[u];
        faces.Remove(u);
        int d = 5 - u;
        faceNeighbours[Direction.Down.ToString()] = d;
        faceRend[3].sprite = faceSpr[d];
        faces.Remove(d);

        //Up and Down
        int l = faces[Random.Range(0, faces.Count)];
        faceNeighbours[Direction.Left.ToString()] = l;
        faceRend[4].sprite = faceSpr[l];
        faces.Remove(l);
        int r = 5 - l;
        faceNeighbours[Direction.Right.ToString()] = r;
        faceRend[5].sprite = faceSpr[r];
        faces.Remove(r);

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
        print(currentFace);
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
