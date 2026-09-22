using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private RectTransform rhythmArea;
    [SerializeField] private RectTransform battlePlayer;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private float spawnInterval = 1f;

    private float _timer;
    private bool _canSpawn;

    private void OnEnable()
    {
        _timer = spawnInterval;
    }

    private void Update()
    {
        if (!_canSpawn) return;
        
        _timer -= Time.deltaTime;

        if (_timer > 0f) return;

        SpawnNote();
        _timer = spawnInterval;
    }

    public void StartSpawning()
    {
        _timer = spawnInterval;
        _canSpawn = true;
    }

    public void StopSpawning()
    {
        _canSpawn = false;

        for (int i = rhythmArea.childCount - 1; i >= 0; i--)
        {
            Transform child = rhythmArea.GetChild(i);
            
            if (child.GetComponent<NoteMover>() != null) Destroy(child.gameObject);
        }
    }

    private void SpawnNote()
    {
        int lane = Random.Range(0, 5);
        GameObject note = Instantiate(notePrefab, rhythmArea);
        
        note.GetComponent<NoteMover>().Initialize(lane, battlePlayer, battleManager);
    }
}
