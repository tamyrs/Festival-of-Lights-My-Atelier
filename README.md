# Festival-of-Lights-My-Atelier

Um cozy game / simulação criativa 2D para estudantes e jogadores casuais que gostem de jogos relaxantes.

## Ideia inicial do jogo (GDD)
O jogador administra um pequeno ateliê de lanternas de papel. Ele recebe encomendas, escolhe o formato (redondo, cilíndrico ou quadrado), personaliza com cor (vermelho, amarelo, azul, verde ou rosa) e estampa (flores, estrelas ou lua), e entrega para ganhar moedas. Meta final: concluir 10 pedidos e acumular 200 moedas para que o Festival das Luzes aconteça.

## Base inicial (scripts)
- `Assets/Scripts/LanternOrder.cs` — dados de uma encomenda (formato, cor, estampa, cliente, recompensa) e lista predefinida de pedidos.
- `Assets/Scripts/GameManager.cs` — gerenciador de estados do jogo (Menu, Tutorial, Ateliê, Personalização, Entrega, Festival Final), validação de entrega, contadores de pedidos e moedas, e checagem da meta do festival.

## Player (Gato do Ateliê) e Cenário Principal

Os sprites do player estão em `Assets/Sprites/Player/` (pastas IDLE, RUN, ATTACK 1). Cada PNG é uma sprite sheet com 8 quadros de animação em linha (96×80 px por quadro).

### 1. Configurar os sprites (obrigatório)
1. Selecione todos os PNGs dentro de `Assets/Sprites/Player/`.
2. Vá no menu **Assets > Aplicar Configuração de Sprites do Player** (script `Assets/Editor/SpriteImportSettings.cs`).
   - Isso define **Sprite Mode = Multiple**, **Pixels Per Unit = 32**, filtro **Point** e slicing automático de 8 quadros (96×80).

### 2. Cenário principal (automático)
O script `Assets/Scripts/Scene/MainSceneSetup.cs` monta a cena sozinho ao iniciar o jogo:

- Cria a **Main Camera** ortográfica na posição correta.
- Cria o **Player** automaticamente se não houver nenhum na cena (ou você pode arrastar um Prefab do Player no campo "Player Prefab" do componente).
- Cria **chão, paredes, mesa, prateleira e luz** do ateliê.

**Como usar:**
1. Na sua cena principal, crie um GameObject vazio chamado `SceneSetup`.
2. Adicione o componente `MainSceneSetup` nele.
3. (Opcional) Se quiser usar um Prefab próprio do Player, arraste-o no campo **Player Prefab** do `MainSceneSetup`.
4. Aperte **Play** — o cenário e o player aparecem automaticamente.

### 3. Animações do Player (você precisa fazer no Unity Editor)
Como eu não consigo abrir o Unity Editor, essa parte fica para vocês:

1. Crie um **Animator Controller** (ex: `PlayerAnimatorController`).
2. Crie animações para as 4 direções usando os slices das sprite sheets:
   - **IDLE:** idle_down, idle_left, idle_right, idle_up
   - **RUN:** run_down, run_left, run_right, run_up
   - **ATTACK 1:** attack1_down, attack1_left, attack1_right, attack1_up
3. No Animator, crie parâmetros:
   - `Horizontal` (float)
   - `Vertical` (float)
   - `Speed` (float)
   - `Attack` (bool)
4. Crie transições entre Idle ↔ Run e entre qualquer estado → Attack (quando `Attack` = true).
5. No GameObject do Player, adicione:
   - `SpriteRenderer`
   - `Rigidbody2D` (**Gravity Scale = 0**)
   - `BoxCollider2D` ou `CircleCollider2D`
   - `Animator` (arraste o PlayerAnimatorController)
   - `PlayerController` (já está no projeto)

### Controles
- **WASD / setas:** andar
- **Espaço ou clique esquerdo:** atacar

## Integrantes do grupo
- [Raquel Aires](https://github.com/RaquelAires)
- [Tamyres Matos](https://github.com/tamyrs)
- [Matheus Cirqueira](https://github.com/bandeirahx)