using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextColorUI : MonoBehaviour
{
    [SerializeField] SlingShot _slingshot; //probably convert to parent class?

    [SerializeField] GameObject yarnBall;

    private LinkedList<GameObject> _colors = new LinkedList<GameObject>();

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

                _colors.AddLast(yarnBall);
            }
        }
    }

    IEnumerator PlayAnimation(LinkedList<Color> Colors)
    {
        // Play the fade animation
        _colors.First.Value.GetComponent<Animator>().Play("Fade");

        yield return new WaitForSeconds(_colors.First.Value.GetComponent<Animator>().runtimeAnimatorController.animationClips[1].length);

        yarnBall = Instantiate(yarnBall, transform);

        // Add the yarn ball to the list and set its color
        yarnBall.GetComponent<RawImage>().color = Colors.Last.Value;

        _colors.AddLast(yarnBall);

        // Removes extra yarn ball UI's that are caused when the player rapidly throws the yarn balls into the level
        while (_colors.Count > 3)
        {
            Destroy(_colors.First.Value);
            _colors.RemoveFirst();
        }
    }
}