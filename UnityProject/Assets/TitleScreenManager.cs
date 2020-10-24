using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public DialogueController dialogueController;
    public float cursingPeriod = 4f;
    public float additionalRandomPeriod = 2f;
    float cursingAlarm;

    // Start is called before the first frame update
    void Start()
    {
        cursingAlarm = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown & !(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            SceneManager.LoadScene("Main");
        }

        cursingAlarm -= Time.deltaTime;
        if (cursingAlarm <= 0f)
        {
            if (dialogueController.enabled == false)
                dialogueController.enabled = true;

            dialogueController.SayACurse();

            cursingAlarm = cursingPeriod + Random.Range(0f, additionalRandomPeriod);
        }
    }
}
