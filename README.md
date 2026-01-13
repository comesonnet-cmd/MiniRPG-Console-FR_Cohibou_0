# MiniRPG-Console-FR_Cohibou_0

Mini système de combat en C#
Projet solo réalisé hors cadre scolaire pour expérimenter la logique de jeu et les interactions entre types.
Aperçu du projet
Mini système de combat développé en C# via Visual Studio.
Choix entre 4 personnages, chacun avec un type unique.
Chaque type possède ses forces, faiblesses, résistances et immunités.
Certaines attaques peuvent produire des effets supplémentaires, ajoutant de la stratégie au combat.
 ! Le code fonctionne mais peut encore être optimisé. Ce projet m’a surtout permis de tester la programmation orientée objet, la gestion des interactions entre types et la logique de combat.

## Tableau des interactions de types

| Attaquant \ Défenseur | Automne | Pénombre | Vénéneux | Givré |
|----------------------|---------|----------|----------|-------|
| **Automne**          | 1×      | 2×       | 0×       | 1×    |
| **Pénombre**        | 0×      | 2×       | 2×       | 0×    |
| **Vénéneux**        | 2×      | 1×       | 0.5×     | 0.5×  |
| **Givré**           | 2×      | 1×       | 2×       | 1×    |

Légende :
2× : super efficace
1× : dégâts normaux
0.5× : résistance (dégâts réduits de 50%)
0× : immunité (aucun dégât)

Liste des attaques et effets
1) Cohibou (Type Automne)
Attaque
Puissance
Effet
Description
Feuille rouge
0
Immunité Givre
Immunise le lanceur contre le givre pendant 3 tours.
Feuille dorée
0
Protection Vénéneux
Réduit de moitié les dégâts des attaques vénéneuses pendant 3 tours.
Jet de pomme de pin
5
Aucun
Attaque offensive simple.
Jet de bogue
15
Aucun
Attaque offensive puissante.
Attaque imprévue : Égarement
15
Aucun
Utilisée si Cohibou est acculé après 3 tours sans attaque efficace.

2) Sbire Pétrole (Type Pénombre)
Attaque
Puissance
Effet
Description
Conversion nocturne
5
Conversion Pénombre
Change le type de la cible en Pénombre jusqu’à la fin du combat.
Obscure mélasse
5
Engluage
Englue la cible : 50% de chance de ne pas pouvoir attaquer à chaque tour.

3) Sbire Suintant (Type Vénéneux)
Attaque
Puissance
Effet
Description
Méthylène gun
5
Empoisonnement
Inflige 5 PV de dégâts à chaque tour jusqu’au KO.
Smog bleu
10
Aucun
Attaque offensive classique.
Poche
0
Pochette surprise
Tire à pile ou face : si pile, rien ne se passe ; si face, lance Smog bleu (10 PV).

4) Grêlon (Type Givre)
Attaque
Puissance
Effet
Description
Gelée douce
5
Aucun
Attaque offensive classique.
Martelage
5
Aucun
Attaque offensive classique.

Légende des effets spéciaux :
Immunité Givre : Le personnage ne subit pas de dégâts givre pendant 3 tours.
Protection Vénéneux : Réduit de moitié les dégâts des attaques vénéneuses pendant 3 tours.
Empoisonnement : Inflige 5 PV de dégâts à chaque tour jusqu’au KO.
Conversion Pénombre : Change le type de la cible en Pénombre.
Engluage : 50% de chance pour la cible de ne pas pouvoir agir à chaque tour.
Pochette surprise : Tire à pile ou face pour lancer une attaque bonus ou rater.
Comment jouer
1. Choisir un personnage parmi les 4 disponibles.
2. Sélectionner une attaque dans la grille 2x2 affichée.
3. Observer les multiplicateurs de dégâts appliqués automatiquement selon le type de l’adversaire.
4. Les effets spéciaux influencent la stratégie (empoisonnement, immunité, engluage, etc.).
5. Gagner en combinant stratégie et connaissance des forces/faiblesses de chaque type.
