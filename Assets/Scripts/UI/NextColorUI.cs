using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextColorUI : MonoBehaviour
{
    [SerializeField] SlingShot _slingshot; //probably convert to parent class?

    [SerializeField] private GameObject yarnBall;

    private Queue<GameObject> _colors = new Queue<GameObject>();

    private void Start()
    {
        

    }

    private void OnEnable()
    {
        _slingshot.OnNextColorChange.AddListener(UpdateNextColor);
    }

    private void OnDisable()
    {
        _slingshot.OnNextColorChange.RemoveListener(UpdateNextColor);
    }

    private void UpdateNextColor(Queue<Color> Colors)
    {
        if (_colors.Count > 2)
        {
            StartCoroutine(PlayAnimation(new LinkedList<Color>(Colors.ToArray())));
        }
        else
        {
            // Add the first three yarn balls
            foreach (Color color in Colors)
            {
                yarnBall = Instantiate(yarnBall, transform);

                yarnBall.GetComponent<RawImage>().color = color;

                _colors.Enqueue(yarnBall);
            }
        }
    }

    IEnumerator PlayAnimation(LinkedList<Color> Colors)
    {
        // Play the fade animation
        _colors.Peek().GetComponent<Animator>().Play("Fade");

        GetComponent<VerticalLayoutGroup>().enabled = false;

        InvokeRepeating("SlideBallUIDown", 0f, .1f);

        yield return new WaitForSeconds(_colors.Peek().GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length);

        GetComponent<VerticalLayoutGroup>().enabled = true;

        yarnBall = Instantiate(yarnBall, transform);

        // Add the yarn ball to the list and set its color
        yarnBall.GetComponent<RawImage>().color = Colors.Last.Value;

        _colors.Enqueue(yarnBall);

        // Removes extra yarn ball UI's that are caused when the player rapidly throws the yarn balls into the level
        while (_colors.Count > 3)
        {
            Destroy(_colors.Dequeue());
        }

        CancelInvoke("SlideBallUIDown");
    }

    public void SlideBallUIDown()
    {
        foreach (GameObject ball in _colors)
        {
            ball.transform.position += Vector3.down * 5f;
        }
    }
}