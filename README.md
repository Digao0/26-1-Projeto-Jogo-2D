# RogueKnight

Jogo 2D top-down de wave defense desenvolvido em Unity 6. O jogador percorre 3 fases temáticas eliminando ondas de inimigos antes de avançar.

## Controles

| Ação | Tecla |
|---|---|
| Mover | WASD |
| Atacar | Setas direcionais (4 direções) |
| Reiniciar (Game Over) | R |

## Fases

| Fase | Cena | Inimigos |
|---|---|---|
| 1 | Floresta | Slime, Orc, RiderOrc, ArmoredOrc, EliteOrc |
| 2 | Castelo | Soldier, ArmoredAxeman, Lancer, Werewolf, KnightTemplar |
| 3 | Caverna | Skeleton, ArmoredSkeleton, Werebear, Greatsword, Archer |

---

## Estatísticas dos Inimigos

> Jogador de referência: 100 HP · 20 dano base (40 com power-up) · speed 5

### Floresta

| Inimigo | HP | Dano | Speed | Knockback recebido | Cooldown ataque | Habilidade especial |
|---|---|---|---|---|---|---|
| **Slime** | 30 | 8 | 1.5 | 7 (alto) | 1.2 s | — |
| **Orc** | 60 | 12 | 2.0 | 5 (normal) | 1.0 s | — |
| **RiderOrc** | 50 | 12 | 3.2 | 4 (normal) | 1.0 s | **Pique:** a cada 6 s dash a speed 7 por 1 s |
| **ArmoredOrc** | 110 | 16 | 1.5 | 2 (baixo) | 0.9 s | — |
| **EliteOrc** | 160 | 20 | 2.2 | 3 (baixo) | 0.85 s | — |

### Castelo

| Inimigo | HP | Dano | Speed | Knockback recebido | Cooldown ataque | Habilidade especial |
|---|---|---|---|---|---|---|
| **Soldier** | 80 | 15 | 2.3 | 5 (normal) | 1.0 s | — |
| **ArmoredAxeman** | 130 | 24 | 1.8 | 2.5 (baixo) | 0.8 s | — |
| **Lancer** | 70 | 20 | 2.8 | 4 (normal) | 1.2 s | — |
| **Werewolf** | 90 | 22 | 3.0 | 4 (normal) | 0.75 s | **Sprint:** a cada 5 s dash a speed 8 por 1.2 s |
| **KnightTemplar** | 220 | 22 | 1.8 | 1.5 (mínimo) | 0.8 s | — |

### Caverna

| Inimigo | HP | Dano | Speed | Knockback recebido | Cooldown ataque | Habilidade especial |
|---|---|---|---|---|---|---|
| **Skeleton** | 55 | 16 | 2.8 | 6 (alto) | 0.8 s | — |
| **ArmoredSkeleton** | 140 | 20 | 2.0 | 2 (baixo) | 0.9 s | — |
| **Werebear** | 270 | 30 | 1.8 | 1.5 (mínimo) | 0.7 s | — |
| **Greatsword** | 170 | 38 | 1.6 | 4 (normal) | 1.5 s | — |
| **Archer** | 60 | 20 | 2.0 | 6 (alto) | 2.0 s | — |

---

## Waves por fase

### Floresta (SampleScene)

| Wave | Slime | Orc | RiderOrc | ArmoredOrc | EliteOrc | Total |
|---|---|---|---|---|---|---|
| 1 | 4 | 2 | — | — | — | 6 |
| 2 | 2 | 2 | 1 | — | — | 5 |
| 3 | — | 2 | 1 | 2 | — | 5 |
| 4 | — | 2 | 2 | 2 | 1 | 7 |

### Castelo (CastleScene)

| Wave | Soldier | Lancer | ArmoredAxeman | Werewolf | KnightTemplar | Total |
|---|---|---|---|---|---|---|
| 1 | 3 | 2 | — | — | — | 5 |
| 2 | 2 | 2 | — | 1 | — | 5 |
| 3 | 2 | 1 | 2 | — | — | 5 |
| 4 | 2 | 2 | 2 | 1 | 1 | 8 |

### Caverna (CaveScene)

| Wave | Skeleton | Archer | ArmoredSkeleton | Greatsword | Werebear | Total |
|---|---|---|---|---|---|---|
| 1 | 3 | 2 | — | — | — | 5 |
| 2 | 2 | 2 | 2 | — | — | 6 |
| 3 | 2 | 1 | 2 | 1 | — | 6 |
| 4 | 2 | 2 | 2 | 1 | 1 | 8 |

---

## Power-ups

| Item | Efeito |
|---|---|
| **HealthPickup** (maçã) | +30 HP |
| **DamagePowerUp** | 2× dano por 10 s |

## Perigos ambientais

| Perigo | Efeito |
|---|---|
| **Espinhos** | 10 dano/s + slowdown (velocidade × 0.4 por 0.5 s) |
