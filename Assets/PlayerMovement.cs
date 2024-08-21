using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float velocity;
    public float speed;
    public float maxSpeed;
    [SerializeField]private Rigidbody rb;
    [SerializeField]private GameObject cameraAnchor;
    [Range(1.0001f, 1.002f)]
    public float velocityFalloff;
    private float CRRCFTimer = 0f;
    [SerializeField] private Camera cam;
    [SerializeField] private float senitivity;

    private Vector3 movementInput;
    private Vector2 mouseInput;
    private float xRot;
    public float cameraRotRecoverySpeed;
    float rotX, rotZ;
    [Range(0.05f, 0.5f)]
    public float cameraRotRecoveryCheckFrequency;

    public float energy;
    public float maxEnergy;
    [Range(0.00f, 0.90f)]
    public float energyRegainRate;
    public float secondsUntilEnergyRegen;
    private float energyRegenTimer = 0f;
    private bool canEnergyRegen = true;
    private bool canDash = true;
    public float dashCooldown = 1.0f;
    private float dashCT = 0f;
    [Range(0.00f, 0.50f)]
    public float dashEnergyConsumption;
    public float dashForce;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Timers();

        // Clamp first
        Mathf.Clamp(rotX, -180f, 180f);
        Mathf.Clamp(rotZ, -180f, 180f);
        transform.localRotation = Quaternion.Euler(rotX, transform.localRotation.y, rotZ);

        // Recovery
        rotX = Mathf.Lerp(rotX, 0, cameraRotRecoverySpeed * Time.deltaTime);
        rotZ = Mathf.Lerp(rotZ, 0, cameraRotRecoverySpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(rotX, transform.localRotation.y, rotZ);

        // Energy
        if (energy < maxEnergy && canEnergyRegen)
        {
            energy += (maxEnergy * energyRegainRate) * Time.deltaTime;
        }
        energy = Mathf.Clamp(energy, 0, maxEnergy);

        MovePlayer();
        SpecialMovement();
        MoveCamera();
    }

    void Timers()
    {
        //CCRF
        if (CRRCFTimer >= cameraRotRecoveryCheckFrequency)
        {
            rotX = transform.localRotation.x;
            rotZ = transform.localRotation.z;
            CRRCFTimer = 0f;
        }
        else
        {
            CRRCFTimer += Time.deltaTime;
        }

        // Energy Regen
        if (!canEnergyRegen)
        {
            energyRegenTimer += Time.deltaTime;
            if (energyRegenTimer >= secondsUntilEnergyRegen)
            {
                canEnergyRegen = true;
                energyRegenTimer = 0f;
            }
        }

        // Energy Regen
        if (!canDash)
        {
            dashCT += Time.deltaTime;
            if (dashCT >= dashCooldown)
            {
                canDash = true;
                dashCT = 0f;
            }
        }
    }

    void SpecialMovement()
    {
        if (energy >= maxEnergy * dashEnergyConsumption && canDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                canDash = false;
                dashCT = 0f;
                canEnergyRegen = false;
                energyRegenTimer = 0f;
                energy -= (maxEnergy * dashEnergyConsumption);
                rb.AddForce(cameraAnchor.transform.forward * movementInput.y * dashForce + cameraAnchor.transform.right * movementInput.x * dashForce, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canDash = false;
                dashCT = 0f;
                canEnergyRegen = false;
                energyRegenTimer = 0f;
                energy -= (maxEnergy * dashEnergyConsumption);
                rb.AddForce(transform.up * dashForce, ForceMode.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                canDash = false;
                dashCT = 0f;
                canEnergyRegen = false;
                energyRegenTimer = 0f;
                energy -= (maxEnergy * dashEnergyConsumption);
                rb.AddForce(transform.up * -dashForce, ForceMode.Impulse);
            }
        }
    }

    void MovePlayer()
    {
        Vector3 velocity = (rb.velocity * velocityFalloff) + (((cam.transform.forward * movementInput.y * speed) + (cam.transform.right * movementInput.x * speed)) * Time.deltaTime);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        rb.velocity = velocity;
    }

    void MoveCamera()
    {
        xRot -= mouseInput.y * senitivity;
        xRot = Mathf.Clamp(xRot, -90, 90);

        cameraAnchor.transform.Rotate(0f, mouseInput.x * senitivity, 0f);
        cam.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        
    }
}
