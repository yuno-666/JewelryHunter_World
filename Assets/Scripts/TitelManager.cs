using UnityEngine;
using UnityEngine.SceneManagement;

public class TitelManager : MonoBehaviour
{
    public string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // シーンを読み込むメソッド
    public void Load()
    {
        GameManager.totalScore = 0; //新しいゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }
}
