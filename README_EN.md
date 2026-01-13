# Mini Combat System in C#

A solo project developed outside of school to experiment with game logic and type interactions.

---

## Project Overview

This mini combat system was developed in **C#** using **Visual Studio**.  
It allows players to:

- Choose from **4 characters**, each with a unique type.  
- Engage in battles where **types interact**, with strengths, weaknesses, resistances, and immunities.  
- Use attacks that may produce **additional effects**, adding strategy to battles.  

 The code works but could still be optimized.  
This project mainly served as practice for **object-oriented programming**, **type interactions**, and **combat logic**.

---

## Type Interaction Table

| Attacker \ Defender | Autumn | Penumbra | Venomous | Frost |
|--------------------|-------|----------|----------|-------|
| **Autumn**         | 1×    | 2×       | 0×       | 1×    |
| **Penumbra**       | 0×    | 2×       | 2×       | 0×    |
| **Venomous**       | 2×    | 1×       | 0.5×     | 0.5×  |
| **Frost**          | 2×    | 1×       | 2×       | 1×    |

**Legend:**

- `2×` : super effective  
- `1×` : normal damage  
- `0.5×` : resistance (damage reduced by 50%)  
- `0×` : immunity (no damage)  

---

## List of Attacks and Effects

### 1) Cohibou (Autumn Type)

| Attack               | Power | Effect           | Description                                                      |
|---------------------|-------|-----------------|------------------------------------------------------------------|
| Red Leaf            | 0     | Frost Immunity   | Makes the user immune to Frost attacks for 3 turns.             |
| Golden Leaf         | 0     | Venom Protection | Reduces damage from Venomous attacks by 50% for 3 turns.        |
| Pinecone Throw      | 5     | None             | Basic offensive attack.                                          |
| Spiny Shell Throw   | 15    | None             | Strong offensive attack.                                         |
| Surprise Attack: Mislead | 15 | None            | Used if Cohibou is cornered after 3 turns without an effective attack. |

---

### 2) Oil Minion (Penumbra Type)

| Attack               | Power | Effect           | Description                                                      |
|---------------------|-------|-----------------|------------------------------------------------------------------|
| Nocturnal Conversion | 5     | Penumbra Conversion | Changes the target's type to Penumbra until the end of the battle. |
| Dark Slime           | 5     | Sticky           | Glues the target: 50% chance the target cannot act each turn.   |

---

### 3) Oozing Minion (Venomous Type)

| Attack       | Power | Effect  | Description                                                      |
|-------------|-------|---------|------------------------------------------------------------------|
| Methylene Gun | 5     | Poison  | Inflicts 5 HP damage each turn until KO.                         |
| Blue Smog    | 10    | None    | Standard offensive attack.                                        |
| Pocket       | 0     | Surprise | Coin toss: heads = does nothing; tails = launches Blue Smog (10 HP). |

---

### 4) Hail (Frost Type)

| Attack       | Power | Effect | Description                     |
|-------------|-------|--------|---------------------------------|
| Soft Gel    | 5     | None   | Standard offensive attack.      |
| Hammering   | 5     | None   | Standard offensive attack.      |

---

## Legend of Special Effects

- **Frost Immunity**: Immune to Frost attacks for 3 turns.  
- **Venom Protection**: Reduces damage from Venomous attacks by 50% for 3 turns.  
- **Poison**: Deals 5 HP damage each turn until KO.  
- **Penumbra Conversion**: Changes the target’s type to Penumbra.  
- **Sticky**: 50% chance the target cannot act each turn.  
- **Surprise**: Coin toss to trigger a bonus attack or fail.  

---

## How to Play

1. Choose a character from the 4 available.  
2. Select an attack from the **2x2 attack grid**.  
3. Observe **type-based damage multipliers** applied automatically.  
4. Take into account **special effects** (poison, immunity, sticky, etc.) to plan strategy.  
5. Win by combining strategy and knowledge of each type’s strengths and weaknesses.  

---
