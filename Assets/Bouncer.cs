using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    private Rigidbody rb;
    public float bounceCheckRange;
    public float testRange;
    public LayerMask wall;
    bool canMove = true;
    bool firstPush = false;
    public float bounceSpeedMinMultiplier;
    public float bounceSpeedMaxMultiplier;
    public float axisSpeedResetRange = 0.5f;
    public float axisJumpMinMultiplier;
    public float axisJumpMaxMultiplier;
    public float deadAxisRange = 1f;
    [Range(0.00f, 100.00f)]
    public float axisJumpChance = 85.00f;
    [Range(0.00f, 100.00f)]
    public float axisResetChance = 17.00f;
    private int xDir, yDir, zDir;
    bool readyToMove = false;
    bool firstPushDone = false;
    public float minMagnitude;
    public float velocityBoostMultiplier;
    private Vector3 lastVelocity;
    bool xAxisResetable = true;
    bool yAxisResetable = true;
    bool zAxisResetable = true;
    bool canBoostX = true;
    bool canBoostY = true;
    bool canBoostZ = true;
    private bool left, right, up, down, forward, backward = false;
    private bool addScore = true;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = FindAnyObjectByType<GameManager>();
    }

    private void Update()
    {
        if (!firstPush)
        {
            firstPush = true;
            decideDir();
        }
        if(readyToMove && !firstPushDone && canMove)
        {
            firstPushDone = true;
            rb.velocity = new Vector3(xDir, yDir, zDir) * speed;
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            if (rb.velocity.x < 0) rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, rb.velocity.z);
            else rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);
        }
        if (Mathf.Abs(rb.velocity.y) > maxSpeed)
        {
            if (rb.velocity.y < 0) rb.velocity = new Vector3(rb.velocity.x, -maxSpeed, rb.velocity.z);
            else rb.velocity = new Vector3(rb.velocity.x, maxSpeed, rb.velocity.z);
        }
        if (Mathf.Abs(rb.velocity.z) > maxSpeed)
        {
            if (rb.velocity.z < 0) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxSpeed);
            else rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
        }
        if (rb.velocity.magnitude < minMagnitude)
        {
            rb.velocity *= velocityBoostMultiplier;
        }
    }
    private void FixedUpdate()
    {
        BounceDetection();
        if (rb.velocity.sqrMagnitude < lastVelocity.sqrMagnitude)
        {
            lastVelocity = rb.velocity;
        }
    }

    void BounceDetection()
    {
        CheckRepeats();
        if (Physics.Raycast(transform.position, transform.right, bounceCheckRange, wall) || Physics.Raycast(transform.position, -transform.right, bounceCheckRange, wall))
        {
            xAxisResetable = false; canBoostX = false;
            if (Physics.Raycast(transform.position, transform.right, bounceCheckRange, wall))
            {
                rb.velocity = new Vector3(-1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.y, rb.velocity.z);
                if (addScore && !right)
                {
                    right = true;
                    Bounce();
                    addScore = false;
                }
            }
            else
            {
                rb.velocity = new Vector3(1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.y, rb.velocity.z);
                if (addScore && !left)
                {
                    left = true;
                    Bounce();
                    addScore = false;
                }
            }
        }
        if (Physics.Raycast(transform.position, transform.up, bounceCheckRange, wall) || Physics.Raycast(transform.position, -transform.up, bounceCheckRange, wall))
        {
            yAxisResetable = false; canBoostY = false;
            if (Physics.Raycast(transform.position, transform.up, bounceCheckRange, wall))
            {
                rb.velocity = new Vector3(rb.velocity.x, -1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.z);
                if (addScore && !up)
                {
                    up = true;
                    Bounce();
                    addScore = false;
                }
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, 1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.z);
                if (addScore && !down)
                {
                    down = true;
                    Bounce();
                    addScore = false;
                }
            }
        }
        if (Physics.Raycast(transform.position, transform.forward, bounceCheckRange, wall) || Physics.Raycast(transform.position, -transform.forward, bounceCheckRange, wall))
        {
            zAxisResetable = false; canBoostZ = false;
            if (Physics.Raycast(transform.position, transform.forward, bounceCheckRange, wall))
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier));
                if (addScore && !forward)
                {
                    forward = true;
                    Bounce();
                    addScore = false;
                }
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier));
                if (addScore && !backward)
                {
                    backward = true;
                    Bounce();
                    addScore = false;
                }
            }
        }
        addScore = true;
    }

    void CheckRepeats()
    {
        if (right) right = Physics.Raycast(transform.position, transform.right, out RaycastHit hit, bounceCheckRange, wall);
        if (left) left = Physics.Raycast(transform.position, -transform.right, out RaycastHit hit2, bounceCheckRange, wall);
        if (up) up = Physics.Raycast(transform.position, transform.up, out RaycastHit hit3, bounceCheckRange, wall);
        if (down) down = Physics.Raycast(transform.position, -transform.up, out RaycastHit hit4, bounceCheckRange, wall);
        if (forward) forward = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit5, bounceCheckRange, wall);
        if (backward) backward = Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit6, bounceCheckRange, wall);
    }

    private void decideDir()
    {
        xDir = Random.Range(-1, 1);
        yDir = Random.Range(-1, 1);
        zDir = Random.Range(-1, 1);
        if (xDir == 0 && yDir == 0 && zDir == 0)
        {
            decideDir();
        }
        else readyToMove = true;
    }

    void Bounce()
    {
        if (rb.velocity.x <= deadAxisRange && rb.velocity.x >= -deadAxisRange && Random.Range(0.00f, 100.00f) >= 100.00f - axisJumpChance && canBoostX)
        {
            xAxisResetable = false;
            if (Random.Range(0, 1) == 1)
            {
                rb.velocity = new Vector3(-1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.y, rb.velocity.z);
            }
            else rb.velocity = new Vector3(1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.y, rb.velocity.z);
        }
        if (rb.velocity.y <= deadAxisRange && rb.velocity.y >= -deadAxisRange && Random.Range(0.00f, 100.00f) >= 100.00f - axisJumpChance && canBoostY)
        {
            yAxisResetable = false;
            if (Random.Range(0, 1) == 1)
            {
                rb.velocity = new Vector3(rb.velocity.x, -1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.z);
            }
            else rb.velocity = new Vector3(rb.velocity.x, 1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier), rb.velocity.z);
        }
        if (rb.velocity.z <= deadAxisRange && rb.velocity.z >= -deadAxisRange && Random.Range(0.00f, 100.00f) >= 100.00f - axisJumpChance && canBoostZ)
        {
            zAxisResetable = false;
            if (Random.Range(0, 1) == 1)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier));
            }
            else rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 1 * speed * Random.Range(axisJumpMinMultiplier, axisJumpMaxMultiplier));
        }
        if (Mathf.Abs(rb.velocity.x) > 1 && Random.Range(0.00f, 100.00f) >= 100.00f - axisResetChance && xAxisResetable)
        {
            rb.velocity = new Vector3(Random.Range(-axisSpeedResetRange, axisSpeedResetRange), rb.velocity.y, rb.velocity.z); yAxisResetable = false; zAxisResetable = false;
        }
        if (Mathf.Abs(rb.velocity.y) > 1 && Random.Range(0.00f, 100.00f) >= 100.00f - axisResetChance && yAxisResetable) zAxisResetable = false;
        {
            rb.velocity = new Vector3(rb.velocity.x, Random.Range(-axisSpeedResetRange, axisSpeedResetRange), rb.velocity.z);
        }
        if (Mathf.Abs(rb.velocity.z) > 1 && Random.Range(0.00f, 100.00f) >= 100.00f - axisResetChance && zAxisResetable)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, Random.Range(-axisSpeedResetRange, axisSpeedResetRange));
        }

        if (addScore)
        {
            gm.score += 1;
        }

        xAxisResetable = true;
        yAxisResetable = true;
        zAxisResetable = true;
        canBoostX = true;
        canBoostY = true;
        canBoostZ = true;
    }
}
