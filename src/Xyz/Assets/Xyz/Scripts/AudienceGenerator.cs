using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class AudienceGenerator : MonoBehaviour
{
    private GameObject _malePrefab;
    private GameObject _femalePrefab;
    
    // Use this for initialization
    void Start()
    {
        var amount = 5;

        var difficultyManager = FindObjectOfType<DifficultyManager>();
        if (difficultyManager != null)
        {
            amount = difficultyManager.GetAudienceMemberCount();
        }

        SpawnAudienceMembers(amount);
    }


    public void SpawnAudienceMembers(int spawnAmount)
    {
        var render = gameObject.GetComponent<Renderer>();
        if (render != null)
        {
            render.enabled = false;
        }

        _malePrefab = Resources.Load<GameObject>("Prefabs/Audience_Male");
        _femalePrefab = Resources.Load<GameObject>("Prefabs/Audience_Female");

        var position = transform.position;


        var deviation = Random.Range(-2, 3);
        spawnAmount += deviation;

        var isDeadZone = Random.Range(0, 5) == 0;
        if (isDeadZone)
        {
            spawnAmount = 2;
        }


        for (int i = 0; i < spawnAmount; i++)
        {
            var xBound = ((Random.Range(-1f, 1f) * transform.localScale.x / 2f));
            var zBound = (Random.Range(-1f, 1f) * transform.localScale.z / 2f);
            var targetVector = new Vector3(xBound, 0, zBound);

            Vector3 rotatedPosition = Quaternion.AngleAxis(transform.eulerAngles.y, transform.up) * targetVector;
            Quaternion targetRotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y + Random.Range(-45, 45f), Vector3.up);

            Instantiate(_malePrefab, position + rotatedPosition, targetRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
