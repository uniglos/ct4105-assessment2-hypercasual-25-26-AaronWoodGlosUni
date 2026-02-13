using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TowerShoot : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    Transform currentBullet = null;
    [SerializeField] float power = 0.01f;
    [SerializeField] bool spawnBulletOnStart = true;

    [SerializeField] UnityEvent onShoot;
    [SerializeField] UnityEvent<Vector3> bulletPos, aimDir;

    Vector2 dir = Vector2.zero;
    Rigidbody2D rb2D;
    Rigidbody rb;

    private void Start() {
        if (spawnBulletOnStart) {
            SpawnBullet();
        }
    }

    public void StartHold() {
        if (currentBullet != null) {
            bulletPos.Invoke(currentBullet.position);
        }
    }

    public void HoldPos(Vector2 pos) {
        aimDir.Invoke(pos);
    }

    public void EndHold() {
        currentBullet.localPosition = Vector2.zero;
    }

    public void Shoot(Vector2 dir) {
        if (dir != new Vector2(Mathf.Infinity, Mathf.Infinity)) {
            if (currentBullet != null) {
                currentBullet.parent = null;
                rb2D = currentBullet.AddComponent<Rigidbody2D>();
                if (currentBullet.TryGetComponent<InteractiveObject>(out InteractiveObject goEvent)) {
                    goEvent.Interaction.Invoke();
                }
                rb2D.position = transform.position;
                rb2D.linearVelocity = -dir * power;
                currentBullet = null;
                onShoot.Invoke();
            }
        }
    }

    public void SpawnBullet() {
        currentBullet = Instantiate(bullet,transform).transform;
        currentBullet.position = gameObject.transform.position;
    }
}
