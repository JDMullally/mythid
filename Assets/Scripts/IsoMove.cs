using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class IsoGridMove : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float _speed = 150f;

    [Header("Tile size in Unity units (2:1 isometric)")]
    public float tileWidth = 1f;
    public float tileHeight = 0.5f;

    [Header("Movement")]
    public float moveSpeed = 4f;

    private InputSystem_Actions input;
    private Vector2 moveInput;

    private Vector2 gridPos = Vector2.zero;
    private Vector3 originWorld;

    private void Awake()
    {
        input = new InputSystem_Actions();

        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        originWorld = transform.position;

        // transform.position = GridToWorld(gridPos);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    //private void Update()
    //{
    //    if (moveInput.sqrMagnitude < 0.001f)
    //        return;

    //    Vector2 isoDelta = InputToIsoGrid(moveInput);
    //    if (moveInput.x != 0f)
    //        sr.flipX = moveInput.x < 0f;
    //    gridPos += isoDelta * moveSpeed * Time.deltaTime;

    //    transform.position = GridToWorld(gridPos);
    //}

    private void FixedUpdate()
    {
        Vector2 grid_input = InputToIsoGrid(moveInput);
        if (moveInput.x != 0f)
            sr.flipX = moveInput.x < 0f;

        rb.linearVelocity = moveInput.normalized * _speed * Time.fixedDeltaTime;

    }


    private Vector2 InputToIsoGrid(Vector2 input)
    {
        return new Vector2(
            input.x + input.y,
            input.y - input.x
        );
    }

    private Vector3 GridToWorld(Vector2 gp)
    {
        float halfW = tileWidth * 0.5f;
        float halfH = tileHeight * 0.5f;

        float x = (gp.x - gp.y) * halfW;
        float y = (gp.x + gp.y) * halfH;

        return originWorld + new Vector3(x, y, 0f);
    }
}
