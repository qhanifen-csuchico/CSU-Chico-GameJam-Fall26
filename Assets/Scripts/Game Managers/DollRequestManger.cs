 using UnityEngine;
using Unity.UI;
using TMPro;

public class DollRequestManger : MonoBehaviour
{
    static DollRequestManger Instance;
    public TMP_Text textField;

    class DollRequest
    {
        string message;
        DollPart.DollPartDescriptor requestedDescriptor;


    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateNewRequest()
    {

    }

}
