using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropIndicator : MonoBehaviour
{

    [SerializeField] YarnDropper dropper;
    [SerializeField] LineRenderer indicatorLine;
    [SerializeField, Range(3, 100)] int lineSteps = 3;

    [Header("TESTING ONLY")]
    [SerializeField, Range(0.01f, 2)] float recalcuationRate = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (!dropper) dropper = GetComponent<YarnDropper>();
        if (!indicatorLine) indicatorLine = GetComponent<LineRenderer>();

        StartCoroutine(RecalculateEveryNSecond(recalcuationRate));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator RecalculateEveryNSecond(float seconds = 1)
    {
        while (true)
        {

            RecalculateLine(
                transform.position,
                transform.position + Vector3.down * dropper.DropperHeight
            );
            yield return new WaitForSeconds(seconds);
        }
    }

    public void RecalculateLine(Vector3 start, Vector3 end)
    {
        indicatorLine.positionCount = lineSteps;
        if (indicatorLine)
        {
            for (int i = 0; i < lineSteps; i++)
            {
                Vector3 position = Vector3.Lerp(start, end, (float)i / (lineSteps - 1));
                indicatorLine.SetPosition(i, position);
            }
        }
    }
}
