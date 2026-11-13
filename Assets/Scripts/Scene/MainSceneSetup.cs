using UnityEngine;

/// <summary>
/// Configura automaticamente a cena principal do jogo "Festival of Lights: My Atelier".
///
/// O que este script faz ao iniciar o jogo:
///   1. Configura a Main Camera (ortográfica, posição e tamanho).
///   2. Cria o Player (se não existir um na cena) com SpriteRenderer, Rigidbody2D, Collider,
///      Animator e o PlayerController.
///   3. Cria um cenário simples: chão, paredes e alguns objetos decorativos do ateliê.
///
/// Como usar:
///   - Crie um GameObject vazio na cena chamado "SceneSetup".
///   - Adicione este componente nele.
///   - Opcional: arraste um Prefab do Player no campo "Player Prefab" (se não tiver, um é criado).
/// </summary>
public class MainSceneSetup : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Prefab do Player (opcional). Se vazio, um Player é criado automaticamente.")]
    public GameObject playerPrefab;

    [Header("Câmera")]
    public float cameraSize = 5f;
    public Vector3 cameraPosition = new Vector3(0f, 0f, -10f);

    [Header("Cenário")]
    public float floorY = -2f;
    public float wallWidth = 1f;
    public float wallHeight = 4f;
    public Color wallColor = new Color(0.6f, 0.45f, 0.35f);
    public Color floorColor = new Color(0.35f, 0.25f, 0.2f);

    private void Awake()
    {
        SetupCamera();
        SetupFloorAndWalls();
        SetupPlayer();
        DecorateAtelier();
    }

    private void SetupCamera()
    {
        Camera main = Camera.main;
        if (main == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.AddComponent<Camera>();
            main = camObj.GetComponent<Camera>();
            Debug.Log("[MainSceneSetup] Main Camera criada automaticamente.");
        }

        main.orthographic = true;
        main.orthographicSize = cameraSize;
        main.transform.position = cameraPosition;
        main.backgroundColor = new Color(0.2f, 0.25f, 0.35f); // azul noite suave
    }

    private void SetupFloorAndWalls()
    {
        // Chão
        GameObject floor = CreateSpriteObject("Chão", new Vector3(0f, floorY, 0f), new Vector3(20f, 1f, 1f), floorColor);
        floor.layer = LayerMask.NameToLayer("Ground");

        // Parede esquerda
        CreateSpriteObject("Parede Esquerda", new Vector3(-10f, floorY + wallHeight / 2f, 0f),
            new Vector3(wallWidth, wallHeight, 1f), wallColor);

        // Parede direita
        CreateSpriteObject("Parede Direita", new Vector3(10f, floorY + wallHeight / 2f, 0f),
            new Vector3(wallWidth, wallHeight, 1f), wallColor);

        // Parede cima
        CreateSpriteObject("Parede Cima", new Vector3(0f, floorY + wallHeight, 0f),
            new Vector3(20f + wallWidth * 2f, wallWidth, 1f), wallColor);
    }

    private GameObject CreateSpriteObject(string name, Vector3 position, Vector3 scale, Color color)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        obj.transform.localScale = scale;

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.color = color;

        var col = obj.AddComponent<BoxCollider2D>();

        return obj;
    }

    private void SetupPlayer()
    {
        GameObject existingPlayer = GameObject.FindGameObjectWithTag("Player");
        if (existingPlayer != null)
        {
            Debug.Log("[MainSceneSetup] Player já existe na cena.");
            return;
        }

        GameObject player;
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.name = "Player";
        }
        else
        {
            player = new GameObject("Player");
            player.transform.position = new Vector3(0f, floorY + 1f, 0f);

            var sr = player.AddComponent<SpriteRenderer>();
            sr.color = Color.white;

            var rb = player.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            player.AddComponent<BoxCollider2D>();

            var anim = player.AddComponent<Animator>();
            // Nota: atribua um Animator Controller no Inspector para as animações funcionarem.
        }

        // Garante que tenha o PlayerController
        if (player.GetComponent<PlayerController>() == null)
        {
            player.AddComponent<PlayerController>();
        }

        player.tag = "Player";
        Debug.Log("[MainSceneSetup] Player criado na cena.");
    }

    private void DecorateAtelier()
    {
        // Mesa do ateliê
        CreateSpriteObject("Mesa", new Vector3(-3f, floorY + 0.6f, 0f),
            new Vector3(2f, 0.4f, 1f), new Color(0.4f, 0.3f, 0.25f));

        // Prateleira
        CreateSpriteObject("Prateleira", new Vector3(3f, floorY + 1.2f, 0f),
            new Vector3(2.5f, 0.3f, 1f), new Color(0.5f, 0.4f, 0.35f));

        // Janela / luz de fundo simples (SpriteRenderer amarelo para simular luz, sem depender do pacote 2D/URP)
        GameObject luz = new GameObject("Luz do Ateliê");
        luz.transform.position = new Vector3(0f, 2f, 0f);
        var luzSr = luz.AddComponent<SpriteRenderer>();
        luzSr.color = new Color(1f, 0.95f, 0.8f);
        luzSr.drawMode = SpriteDrawMode.Simple;
        luzSr.size = new Vector2(4f, 4f);
        luzSr.sortingOrder = -1; // fica atrás do player
    }
}
