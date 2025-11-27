using UnityEngine;
using UnityEngine.UI;

public class PlayerBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostMultiplier = 5f;      // How much faster the player moves
    public float boostDuration = 2f;        // Boost lasts this long
    public float cooldownDuration = 4f;     // Cooldown after boost ends

    [Header("UI")]
    public Image iconImage;                  // Icon shows while boosting
    public Image cooldownFillImage;          // Optional cooldown fill

    private float boostTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isBoosting = false;
    private bool isOnCooldown = false;

    private void Update()
    {
        // Count boost time
        if (isBoosting)
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= boostDuration)
            {
                isBoosting = false;
                isOnCooldown = true;
                boostTimer = 0f;
                cooldownTimer = cooldownDuration;
            }
        }

        // Count cooldown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                cooldownTimer = 0f;
                isOnCooldown = false;
            }
        }

        // Update UI
        if (iconImage != null)
            iconImage.enabled = isBoosting;

        if (cooldownFillImage != null)
            cooldownFillImage.fillAmount = isOnCooldown && cooldownDuration > 0f
                ? cooldownTimer / cooldownDuration
                : 0f;
    }

    // Called from PlayerMovement when boost key is pressed
    public void StartBoost()
    {
        if (!isBoosting && !isOnCooldown)
        {
            isBoosting = true;
            boostTimer = 0f;
        }
    }

    public float CurrentMultiplier() => isBoosting ? boostMultiplier : 1f;
}
