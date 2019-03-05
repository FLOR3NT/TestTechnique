using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

    int turn = 0;
    int remainingCard = 16;
    public float timer = 0;
    public bool waitForTurn = false;
    public bool gameLunched = true;

    Text timerGO;
    GameObject EndTextGO;

    private void Awake()
    {
        if (GameManager.gameManager == null)
        {
            GameManager.gameManager = this;
        }
        else
        {
            if (GameManager.gameManager != this)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (gameLunched)
        {
            if (!waitForTurn)
            {
                turn++;
                StartCoroutine(CoroutTurn());
                waitForTurn = true;
            }
            timerGO.text = Mathf.FloorToInt(timer / 60f).ToString("00") + ":" + Mathf.FloorToInt(timer % 60f).ToString("00");
            timer += Time.deltaTime;
            if (remainingCard <= 0)
            {
                gameLunched = false;
                EndGame();
            }
        }
    }

    IEnumerator CoroutTurn()
    {
        Card outFirstCard = null;
        yield return new WaitUntil(() => TestForClick(out outFirstCard));
        outFirstCard.CheckCard();
        Card outSecondCard = null;
        yield return new WaitUntil(() => TestForClick(out outSecondCard));
        outSecondCard.CheckCard();
        if (outFirstCard.name == outSecondCard.name)
        {
            outFirstCard.ValideCard();
            outSecondCard.ValideCard();
            remainingCard -= 2;
        }
        else
        {
            yield return new WaitForSeconds(1);
            outFirstCard.ResetCard();
            outSecondCard.ResetCard();
        }
        waitForTurn = false;
    }

    private bool TestForClick(out Card outCard)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.GetComponent<Card>() != null && hit.collider.GetComponent<Card>().CanCheck())
                {
                    outCard = hit.collider.GetComponent<Card>();
                    return true;
                }
            }
        }
        outCard = null;
        return false;
    }

    public void LunchLevel()
    {
        StartCoroutine(CoroutLunchLevel());
    }

    IEnumerator CoroutLunchLevel()
    {
        SceneManager.LoadScene(1);
        yield return new WaitUntil(() => SceneManager.GetSceneByBuildIndex(1).isLoaded);
        StartGame();
    }

    void StartGame()
    {
        EndTextGO = GameObject.FindGameObjectWithTag("EndText");
        EndTextGO.SetActive(false);
        timer = 0;
        timerGO = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        remainingCard = 16;
        gameLunched = true;
    }

    void EndGame()
    {
        EndTextGO.SetActive(true);
    }
}
