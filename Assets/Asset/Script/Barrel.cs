using UnityEngine;
using System.Collections;
public class Barrel : MonoBehaviour
{
    public float hiddenSpeedMultiplier = 1.5f; // Peningkatan kecepatan saat bersembunyi
    public float additionalSpeedDuration = 1.5f; // Durasi tambahan kecepatan setelah keluar dari barel (detik)
    public float cooldownDuration = 5f; // Durasi cooldown agar dapat masuk lagi (detik)
    private bool isCooldown = false; // Status cooldown

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isCooldown)
        {
            PlayerHide playerHide = other.GetComponent<PlayerHide>();
            if (playerHide != null)
            {
                playerHide.isHidden = true;
                playerHide.SetPlayerVisibility(false);

                // Mengaktifkan tambahan kecepatan saat bersembunyi
                other.GetComponent<Topdown>().moveSpeed *= hiddenSpeedMultiplier;
                StartCoroutine(DisableSpeedBoostAfterDuration(other.GetComponent<Topdown>()));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHide playerHide = other.GetComponent<PlayerHide>();
            if (playerHide != null)
            {
                playerHide.isHidden = false;
                playerHide.SetPlayerVisibility(true);

                // Mulai cooldown
                StartCoroutine(CooldownCoroutine());
            }
        }
    }

    private IEnumerator DisableSpeedBoostAfterDuration(Topdown playerMovement)
    {
        yield return new WaitForSeconds(additionalSpeedDuration);
        playerMovement.moveSpeed /= hiddenSpeedMultiplier;
    }

    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldown = false;
    }
}
