using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D m_collider;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Player player = GameManager.Instance.Player;
        Vector3 playerPos = player.transform.position;
        Vector3 newPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - newPos.x);
        float diffY = Mathf.Abs(playerPos.y - newPos.y);

        Vector3 direction = player.InputVec;
        float dirX = direction.x < 0 ? -1 : 1;
        float dirY = direction.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                if(m_collider.enabled)
                {
                    transform.Translate(direction * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
                }
                break;
        }
    }
}
