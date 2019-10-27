using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLetters : MonoBehaviour
{
    public float delay = 3f;

    public Vector3 torque;
    public float ySpeed = -3f;

    public List<GameObject> letters = new List<GameObject>();
    List<Rigidbody> rbs = new List<Rigidbody>();

    private void Start()
    {
        for(int k = 0; k < letters.Count; k++)
        {
            rbs.Add(letters[k].GetComponent<Rigidbody>());

            for (int i = 0; i < letters[k].transform.childCount; i++)
            {
                rbs.Add(letters[k].transform.GetChild(i).GetComponent<Rigidbody>());
            }
        }

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);

        while (rbs.Count > 0)
        {
            int i = Random.Range(0, rbs.Count);

            float x = Random.Range(-3f, 3f);
            float z = Random.Range(-3f, 3f);

            rbs[i].velocity = new Vector3(x, ySpeed, z);
            rbs[i].AddTorque(torque);
            rbs.RemoveAt(i);

            yield return null;
        }
    }
}
