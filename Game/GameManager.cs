using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MiniRPG_Console_FR_Cohibou_0.Characters;
using MiniRPG_Console_FR_Cohibou_0.Utils;
using Game.Fight;
using Game.Effects;

namespace Game
{
    public static class GameManager
    {
        private static Random rnd = new Random();



        public static void LancerJeu()
        {
            // Étape 1 : créer les attaques 

            // 1) Attaques de Cohibou
            Attaque feuilleRouge = new Attaque
            {
                Nom = "Feuille rouge",
                Type = Element.TYPE.Automne,
                Puissance = 0,
                Effet = EffetType.ImmuGivre         // Produit un pigment rouge protecteur qui immunise contre le givre pendant 3 tours         
            };
            Attaque feuilleDoree = new Attaque
            {
                Nom = "Feuille dorée",
                Type = TypeElement.Automne,
                Puissance = 0,
                Effet = EffetType.ProtectVeneneux       // Diminue de moitié la puissance des attaques vénéneuses pendant 3 tours            
            };
            Attaque jetPommeDePin = new Attaque { Nom = "Jet de pomme de pin", Type = TypeElement.Automne, Puissance = 5, Effet = EffetType.Aucun };
            Attaque jetDeBogue = new Attaque { Nom = "Jet de bogue", Type = TypeElement.Automne, Puissance = 15, Effet = EffetType.Aucun };


            // 2) Attaques du Grelon
            Attaque geleeDouce = new Attaque { Nom = "Gelée douce", Type = TypeElement.Givre, Puissance = 5, Effet = EffetType.Aucun };
            Attaque martelage = new Attaque { Nom = "Martelage", Type = TypeElement.Penombre, Puissance = 5, Effet = EffetType.Aucun };

            //3) Attaques du Sbire suintant
            Attaque methyleneGun = new Attaque
            {
                Nom = "Méthylène gun",
                Type = TypeElement.Veneneux,
                Puissance = 5,
                Effet = EffetType.Empoisonnement        // Applique "Empoisonnement", inflige 5 dégâts à chaque tour jusqu'au KO          
            };

            Attaque smogBleu = new Attaque
            {
                Nom = "Smog bleu",
                Type = TypeElement.Veneneux,
                Puissance = 10,
                Effet = EffetType.Aucun
            };

            Attaque poche = new Attaque
            {
                Nom = "Poche",
                Type = TypeElement.Veneneux,
                Puissance = 0,                          // pas de dégâts directs
                Effet = EffetType.PochetteSurprise,     // Tire à pile ou face, si pile = "Poche vide", n'inflige aucun dégât, si face = "Smog bleu", enlève 10 PV.
                TirePileFace = true,
                AttaqueBonus = smogBleu         // attaque à déclencher si Face
            };

            // 4) Attaques du Sbire Pétrole
            Attaque conversionNocturne = new Attaque
            {
                Nom = "Conversion nocturne",
                Type = TypeElement.Penombre,
                Puissance = 5,
                Effet = EffetType.ConversionPenombre    // Convertit la cible en type Pénombre
            };
            Attaque obscureMelasse = new Attaque
            {
                Nom = "Obscure mélasse",
                Type = TypeElement.Penombre,
                Puissance = 5,
                Effet = EffetType.Engluage              // Englue la cible dans une mélasse noire, elle a 1 chance sur 2 d'attaquer (pile ou face) jusqu'à la fin du combat.
            };

            //ATTAQUES IMPREVUES:


            // Attaque imprévue de Cohibou:
            Attaque ImpCohibou = new Attaque
            {
                Nom = "égarement",
                Type = TypeElement.Penombre,
                Puissance = 15,
                Effet = EffetType.Aucun
            };



            //  Étape 1 : créer les personnages jouables

            List<Personnage> tousLesPersonnages = new List<Personnage>();

            Personnage Cohibou = new Personnage
            {
                Nom = "Cohibou",
                Type = TypeElement.Automne,
                PointsDeVie = 100,
                Attaques = new List<Attaque> { feuilleRouge, feuilleDoree, jetPommeDePin, jetDeBogue },
                AttaqueImprevue = ImpCohibou,
                Vitesse = 4

            };

            Personnage SbirePetrole = new Personnage
            {
                Nom = "Sbire pétrole",
                Type = TypeElement.Penombre,
                PointsDeVie = 50,
                Attaques = new List<Attaque> { conversionNocturne, obscureMelasse },
                Vitesse = 5

            };

            Personnage SbireSuintant = new Personnage
            {
                Nom = "Sbire suintant",
                Type = TypeElement.Veneneux,
                PointsDeVie = 50,
                Attaques = new List<Attaque> { methyleneGun, poche },
                Vitesse = 3
            };

            Personnage Grelon = new Personnage
            {
                Nom = "Grêlon",
                Type = TypeElement.Givre,
                PointsDeVie = 50,
                Attaques = new List<Attaque> { geleeDouce, martelage },
                Vitesse = 2
            };

            tousLesPersonnages.Add(Cohibou);
            tousLesPersonnages.Add(SbireSuintant);
            tousLesPersonnages.Add(SbirePetrole);
            tousLesPersonnages.Add(Grelon);

            // Définir la grille pour le choix du personnages 2x2:

            Personnage[,] grillePersonnages = new Personnage[2, 2]               // tableau multidimensionnel (ici 2 lignes × 2 colonnes). 
            {
            { Cohibou,         SbireSuintant},                                      // La première paire { Cohibou, SbireSuintant } est la ligne 0 (col 0 et col 1).
            { SbirePetrole,    Grelon}                                         // La seconde paire est la ligne 1.
            };                                                                  // Accès : grillePersonnages[i, j] où i = ligne (0..1), j = colonne (0..1).

            int ligne = 0;                                                      // ligne et colonne sont les indices actuels du curseur (commencent sur le premier élément : Cohibou).
            int colonne = 0;
            bool choisi = false;                                                // choisi indique si l’utilisateur a appuyé sur Entrée pour valider.

            // Boucle principale d’affichage / gestion des touches:

            while (!choisi)
            {
                Console.Clear();                                                // Console.Clear() efface l’écran pour redessiner la grille proprement à chaque itération (donc le curseur "se déplace").
                Console.WriteLine("Qui es-tu?\n");


                // Calculer la largeur maximale des noms + 2 espaces
                int maxLength = grillePersonnages.Cast<Personnage>().Max(p => p.Nom.Length) + 2;

                // grillePersonnages.Cast<Personnage>() : transforme la grille 2×2 en liste pour pouvoir utiliser Max().                                        
                // PadRight(maxLength) : ajoute des espaces après chaque nom pour que toutes les colonnes aient la même largeur.
                //.Max(p => p.Nom.Length) - Max() parcourt chaque élément de la séquence et renvoie la valeur maximale.
                //                        - Ici : p => p.Nom.Length indique qu’on veut la longueur du nom de chaque personnage.
                //                        - Donc, Max() va calculer le nom le plus long parmi tous les personnages. Ici: 14
                //                        - On ajoute 2 caractères d’espace pour séparer les colonnes dans l’affichage: Donc maxLength = 14 + 2 = 16.


                // Afficher la grille avec le curseur                                               
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (i == ligne && j == colonne)                         //  → on affiche "> " devant le nom pour montrer la sélection actuelle.
                            Console.Write("> ");  // CURSEUR
                        else
                            Console.Write("  ");                                // Sinon on met deux espaces " " pour aligner.

                        Console.Write($"{grillePersonnages[i, j].Nom.PadRight(maxLength)} ");       // Affiche le nom du personnage à cette case.
                    }
                    Console.WriteLine();
                }

                // Lire la touche appuyée
                ConsoleKeyInfo key = Console.ReadKey(true);                     // Après affichage, Console.ReadKey(true) attend une touche sans l’afficher (true = key interceptée).

                switch (key.Key)                                                // switch (key.Key) regarde quelle touche a été pressée 
                {
                    case ConsoleKey.UpArrow:                                    // UpArrow et DownArrow modifient ligne.
                        ligne = (ligne - 1 + 2) % 2;                            // (ligne - 1 + 2) % 2 garantit que le résultat reste entre 0 et 1 :
                        break;                                                  // si ligne = 0 → (0 - 1 + 2) % 2 = 1 (on passe à la ligne du bas),  si ligne = 1 → (1 - 1 + 2) % 2 = 0, Le + 2 évite les négatifs avant le %. Pour une grille n x m on utiliserait (index + delta + n) % n.
                    case ConsoleKey.DownArrow:
                        ligne = (ligne + 1) % 2;
                        break;
                    case ConsoleKey.LeftArrow:                                  // LeftArrow et RightArrow modifient colonne.
                        colonne = (colonne - 1 + 2) % 2;
                        break;
                    case ConsoleKey.RightArrow:
                        colonne = (colonne + 1) % 2;
                        break;
                    case ConsoleKey.Enter:                                      // Enter fixe choisi = true et fait sortir de la boucle (le choix est validé).
                        choisi = true; // choix validé
                        break;
                }
            }

            // Récupérer le personnage choisi
            Personnage joueur = grillePersonnages[ligne, colonne];
            Console.WriteLine($"\n {joueur.Nom} ?");

            bool confirmation = false;                  // Cette variable va recevoir la réponse du joueur (Oui → true, Non → false).

            switch (joueur.Nom)                         // switch (joueur.Nom) : on regarde la valeur de joueur.Nom (une chaîne) et on exécute le case correspondant.
            {
                case "Cohibou":                         // Pour chaque case on appelle la fonction ChoixOuiNon(string question) en lui passant le texte à afficher.
                    confirmation = ConsoleHelpers.ChoixOuiNon("Cohibou? C'est bien toi, n'est-ce pas?");   // ChoixOuiNon(...) affiche un mini-menu Oui/Non (avec curseur) et retourne un bool : true si le joueur choisit Oui, false si Non
                    break;                                                                  // Le résultat retourné est stocké dans confirmation.
                case "Sbire pétrole":
                    confirmation = ConsoleHelpers.ChoixOuiNon("Happeur de décombres, confondeur des méandres... Es-tu bien sûr ?");
                    break;
                case "Sbire suintant":
                    confirmation = ConsoleHelpers.ChoixOuiNon("Ce sbire est vénéneux, tu es sûr de toi ?");
                    break;
                case "Grêlon":
                    confirmation = ConsoleHelpers.ChoixOuiNon("Brr, il fait frisqué ici. C'est toi?");
                    break;                                                                 // Si aucun case ne matche (improbable ici), confirmation reste false (valeur initiale).
            }

            Console.Clear();
            Thread.Sleep(500); // pause 0.5 seconde

            if (!confirmation)                                                             // if (!confirmation) signifie si la variable confirmation est fausse (! = NON logique). Autrement dit : si le joueur a répondu NON.
            {
                Console.WriteLine("\n Ah! Tu m'as fait peur mon gars! Alors, qui es-tu VRAIMENT ?");
                Thread.Sleep(1500);                     // Thread.Sleep(1500); → pause 1,5 seconde (1500 ms) pour laisser le message s’afficher. (Nécessite using System.Threading; si tu veux éviter les warnings.)

                return;



            }

            // On crée une liste pour mettre les personnages non-choisis par le joueur dedans et les définir en tant qu'ennemis

            List<Personnage> ennemis = new List<Personnage>();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (grillePersonnages[i, j] != joueur)
                        ennemis.Add(grillePersonnages[i, j]);
                }
            }

            Personnage ennemi = ennemis[rnd.Next(ennemis.Count)];           // ennemi random parmi les ennemis
            Console.WriteLine($"\n\nOh! Un ennemi apparaît!\n\nMais..? C'est {ennemi.Nom} ?!\n\nLe bougre vous attaque !");
            Console.ReadLine();

            // COMMENCER LA BOUCLE DE COMBAT

            bool joueurCommence = joueur.Vitesse >= ennemi.Vitesse;     // ordre de tour

            // Boucle de combat
            while (joueur.EstVivant() && ennemi.EstVivant())
            {
                Console.Clear();


                if (joueurCommence)
                {
                    TurnManager.TourJoueur(joueur, ennemi);
                    if (ennemi.EstVivant())
                        TurnManager.TourEnnemi(ennemi, joueur);
                }
                else if (!joueurCommence)
                {
                    TurnManager.TourEnnemi(ennemi, joueur);
                    if (joueur.EstVivant())
                        TurnManager.TourJoueur(joueur, ennemi);
                }

                // Effets  
                EffectsManager.AppliquerEffets(joueur);
                EffectsManager.AppliquerEffets(ennemi);

                if (!joueur.EstVivant() || !ennemi.EstVivant())
                    break;


                Console.WriteLine("\n Fin du tour");
                Console.ReadKey();
            }
        }
    }
}
