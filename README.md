# Festival-of-Lights-My-Atelier

Um cozy game / simulação criativa 2D para estudantes e jogadores casuais que gostem de jogos relaxantes.

## Ideia inicial do jogo (GDD)
O jogador administra um pequeno ateliê de lanternas de papel. Ele recebe encomendas, escolhe o formato (redondo, cilíndrico ou quadrado), personaliza com cor (vermelho, amarelo, azul, verde ou rosa) e estampa (flores, estrelas ou lua), e entrega para ganhar moedas. Meta final: concluir 10 pedidos e acumular 200 moedas para que o Festival das Luzes aconteça.

## Base inicial (scripts)
- `Assets/Scripts/LanternOrder.cs` — dados de uma encomenda (formato, cor, estampa, cliente, recompensa) e lista predefinida de pedidos.
- `Assets/Scripts/GameManager.cs` — gerenciador de estados do jogo (Menu, Tutorial, Ateliê, Personalização, Entrega, Festival Final), validação de entrega, contadores de pedidos e moedas, e checagem da meta do festival.

## Player (Gato do Ateliê)

Os sprites do player estão em `Assets/Sprites/Player/` (pastas IDLE, RUN, ATTACK 1). Cada PNG é uma sprite sheet com 8 quadros de animação em linha (96×80 px por quadro).

### Como configurar no Unity
1. Selecione todos os PNGs dentro de `Assets/Sprites/Player/`.
2. Vá no menu **Assets > Aplicar Configuração de Sprites do Player** (script `Assets/Editor/SpriteImportSettings.cs`).
   - Isso define **Sprite Mode = Multiple**, **Pixels Per Unit = 32**, filtro **Point** e slicing automático de 8 quadros (96×80).
3. Crie um GameObject chamado `Player` na cena e adicione:
   - `SpriteRenderer`
   - `Rigidbody2D` (Gravity Scale = 0)
   - `Collider2D` (BoxCollider2D ou CircleCollider2D)
   - `Animator` com um `AnimatorController` contendo as animações IDLE, RUN e ATTACK para as 4 direções
   - `PlayerController` (`Assets/Scripts/Player/PlayerController.cs`)
4. No Animator, use os parâmetros: `Horizontal` (float), `Vertical` (float), `Speed` (float), `Attack` (bool).
5. Controles: **WASD / setas** para andar, **Espaço ou clique esquerdo** para atacar.

## Integrantes do grupo
- [Raquel Aires](https://github.com/RaquelAires)
- [Tamyres Matos](https://github.com/tamyrs)
- [Matheus Cirqueira](https://github.com/bandeirahx)