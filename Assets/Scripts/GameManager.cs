using System;
using System.Collections.Generic;
using UnityEngine;
using FestivalOfLights;

/// <summary>
/// GameManager - Base inicial do jogo "Festival of Lights: My Atelier" (baseada no GDD).
///
/// Cozy game / simulação criativa 2D onde o jogador administra um ateliê de lanternas de papel:
/// recebe encomendas, escolhe formato, cor e estampa, e entrega para ganhar moedas.
/// Meta final: 10 pedidos concluídos e 200 moedas acumuladas para o Festival das Luzes.
///
/// Estados do jogo (GDD seção 6):
/// Menu -> Tutorial -> Ateliê -> Personalização -> Validação/Entrega -> Festival Final
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Metas do Festival (GDD seção 3.5 / 7)")]
    [Tooltip("Quantidade de pedidos concluídos necessária para desbloquear o festival.")]
    public int targetOrders = 10;

    [Tooltip("Quantidade de moedas acumuladas necessária para desbloquear o festival.")]
    public int targetCoins = 200;

    [Header("Estado atual")]
    public GameState currentState = GameState.Menu;

    public int coins = 0;
    public int ordersCompleted = 0;

    public LanternOrder currentOrder { get; private set; }
    public int currentOrderIndex = -1;

    // Personalização em andamento
    public LanternFormat selectedFormat;
    public LanternColor selectedColor;
    public LanternPattern selectedPattern;

    // Callbacks para a UI se inscrever (observador simples, sem depender de prefabs ainda)
    public Action<GameState> onStateChanged;
    public Action<int, int> onProgressChanged;   // (moedas, pedidos concluídos)
    public Action<LanternOrder> onNewOrder;
    public Action<bool, int> onDeliveryResult;   // (acerto, moedas ganhas)
    public Action onFestivalUnlocked;

    private int orderCursor = 0; // índice no OrderDatabase

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("[Festival of Lights] Ateliê aberto! Bem-vindo ao jogo de lanternas.");
        ResetProgress();
    }

    /// <summary>
    /// Reinicia o progresso (usado no início e para testes).
    /// </summary>
    public void ResetProgress()
    {
        coins = 0;
        ordersCompleted = 0;
        currentOrderIndex = -1;
        orderCursor = 0;
        selectedFormat = LanternFormat.Round;
        selectedColor = LanternColor.Red;
        selectedPattern = LanternPattern.Flowers;
        currentState = GameState.Menu;
        onStateChanged?.Invoke(currentState);
        onProgressChanged?.Invoke(coins, ordersCompleted);
        Debug.Log("[Festival of Lights] Progresso reiniciado. Estado: Menu");
    }

    /// <summary>
    /// Inicia uma nova partida a partir do menu (tutorial breve).
    /// </summary>
    public void StartGame()
    {
        currentState = GameState.Tutorial;
        onStateChanged?.Invoke(currentState);
        Debug.Log("[Festival of Lights] Tutorial: atenda uma encomenda passo a passo.");
    }

    /// <summary>
    /// Avança do tutorial para o ateliê com o primeiro pedido.
    /// </summary>
    public void EnterAtelier()
    {
        currentState = GameState.Atelier;
        onStateChanged?.Invoke(currentState);
        GiveNextOrder();
    }

    /// <summary>
    /// Entrega o próximo pedido da lista predefinida (GDD 3.1: um pedido por vez).
    /// </summary>
    public void GiveNextOrder()
    {
        if (orderCursor >= OrderDatabase.Orders.Count)
        {
            // Não há mais pedidos na base; em um jogo completo, geraríamos mais.
            Debug.Log("[Festival of Lights] Sem mais pedidos na base de dados.");
            return;
        }

        currentOrder = OrderDatabase.Orders[orderCursor];
        currentOrderIndex = orderCursor;
        orderCursor++;

        // Redefine personalização para o padrão ao iniciar um novo pedido
        selectedFormat = LanternFormat.Round;
        selectedColor = LanternColor.Red;
        selectedPattern = LanternPattern.Flowers;

        currentState = GameState.Atelier;
        onStateChanged?.Invoke(currentState);
        onNewOrder?.Invoke(currentOrder);

        Debug.Log($"[Festival of Lights] Novo pedido: {currentOrder}");
    }

    /// <summary>
    /// Entra na tela de personalização da lanterna (GDD 3.2 / 3.3).
    /// </summary>
    public void EnterCustomization()
    {
        if (currentOrder == null) return;
        currentState = GameState.Customization;
        onStateChanged?.Invoke(currentState);
        Debug.Log("[Festival of Lights] Personalizando lanterna...");
    }

    /// <summary>
    /// Seleciona o formato da lanterna (redondo, cilíndrico ou quadrado).
    /// </summary>
    public void SelectFormat(LanternFormat format)
    {
        selectedFormat = format;
        Debug.Log($"[Festival of Lights] Formato selecionado: {format}");
    }

    /// <summary>
    /// Seleciona a cor da lanterna (vermelho, amarelo, azul, verde ou rosa).
    /// </summary>
    public void SelectColor(LanternColor color)
    {
        selectedColor = color;
        Debug.Log($"[Festival of Lights] Cor selecionada: {color}");
    }

    /// <summary>
    /// Seleciona a estampa da lanterna (flores, estrelas ou lua).
    /// </summary>
    public void SelectPattern(LanternPattern pattern)
    {
        selectedPattern = pattern;
        Debug.Log($"[Festival of Lights] Estampa selecionada: {pattern}");
    }

    /// <summary>
    /// Valida e entrega o pedido (GDD 3.4).
    /// Acerto -> encomenda concluída + moedas. Erro -> pode corrigir sem perder moedas.
    /// </summary>
    public void DeliverOrder()
    {
        if (currentOrder == null) return;

        bool correct =
            selectedFormat == currentOrder.format &&
            selectedColor == currentOrder.color &&
            selectedPattern == currentOrder.pattern;

        if (correct)
        {
            ordersCompleted++;
            coins += currentOrder.rewardCoins;
            currentState = GameState.Delivery;
            onStateChanged?.Invoke(currentState);
            onDeliveryResult?.Invoke(true, currentOrder.rewardCoins);
            onProgressChanged?.Invoke(coins, ordersCompleted);

            Debug.Log($"[Festival of Lights] Pedido correto! +{currentOrder.rewardCoins} moedas. " +
                      $"Progresso: {ordersCompleted}/{targetOrders} pedidos, {coins}/{targetCoins} moedas.");

            CheckFestivalUnlocked();
        }
        else
        {
            currentState = GameState.Delivery;
            onStateChanged?.Invoke(currentState);
            onDeliveryResult?.Invoke(false, 0);
            Debug.Log("[Festival of Lights] Pedido incorreto. Corrija formato, cor ou estampa e tente novamente.");
        }
    }

    /// <summary>
    /// Volta do resultado da entrega para o ateliê (próximo pedido ou correção).
    /// </summary>
    public void ReturnToAtelier()
    {
        currentState = GameState.Atelier;
        onStateChanged?.Invoke(currentState);

        if (ordersCompleted < targetOrders)
        {
            GiveNextOrder();
        }
    }

    private void CheckFestivalUnlocked()
    {
        if (ordersCompleted >= targetOrders && coins >= targetCoins)
        {
            currentState = GameState.FestivalFinal;
            onStateChanged?.Invoke(currentState);
            onFestivalUnlocked?.Invoke();
            Debug.Log("🎉 Festival das Luzes desbloqueado! O ateliê ilumina a vila. 🎉");
        }
    }

    /// <summary>
    /// Exibe o status atual do ateliê no console.
    /// </summary>
    public void ShowStatus()
    {
        Debug.Log("===== Ateliê de Lanternas =====");
        Debug.Log($"Estado: {currentState}");
        Debug.Log($"Pedidos concluídos: {ordersCompleted}/{targetOrders}");
        Debug.Log($"Moedas: {coins}/{targetCoins}");
        if (currentOrder != null)
            Debug.Log($"Pedido atual: {currentOrder}");
        Debug.Log("===============================");
    }
}

/// <summary>
/// Estados possíveis do jogo (GDD seção 6).
/// </summary>
public enum GameState
{
    Menu,
    Tutorial,
    Atelier,
    Customization,
    Delivery,
    FestivalFinal
}
