using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("Delay between crash and level restart in seconds")] [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("SHIP DESTROYED");
        Invoke("ReloadScene", loadDelay);
        gameObject.GetComponent<Movement>().enabled = false;
        explosionEffect.Play();
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    void ReloadScene()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
