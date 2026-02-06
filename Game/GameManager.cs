using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MiniRPG_Console_FR_Cohibou_0.Utils;
using Game.Fight;
using Game.Effects;
using Game.Characters;
using Game.Attacks;
using Game.Elements;

namespace Game
{
    public static class GameManager
    {
        private static Random rnd = new Random();

        // 1. Initialisation des personnages:

        //A la place d'init les persos ici, je créerais un PersonnageManager (par ex) qui se chargerait de tout ce qui a à voir avec les perso. 
        public static List<Personnage> CreerTousLesPersonnages()
        {

            // Création des personnages jouables

            List<Personnage> tousLesPersonnages = new List<Personnage>();

            Personnage Cohibou = new Personnage
            {
                Nom = "Cohibou",
                Type = Element.TYPE.Automne,
                PointsDeVie = 100,
                Attaques = new List<Attaque> { AtkManager.GetAtkByName("Feuille rouge"), AtkManager.GetAtkByName("Feuille dorée"), AtkManager.GetAtkByName("Jet de pomme de pin"), AtkManager.GetAtkByName("Jet de bogue") },
                AttaqueImprevue = AtkManager.GetAtkByName("égarement"),
                Vitesse = 4

            };

            Personnage SbirePetrole = new Personnage
            {
                Nom = "Sbire pétrole",
                Type = Element.TYPE.Penombre,
                PointsDeVie = 50,
                Attaques = new List<Attaque> { AtkManager.GetAtkByName("Conversion nocturne"), AtkManager.GetAtkByName("Obscure mélasse") },
                Vitesse = 5

            };

            Personnage SbireSuintant = new Personnage
            {
                Nom = "Sbire suintant",
                Type = Element.TYPE.Veneneux,
                PointsDeVie = 50,
                Attaques = new List<Attaque> { AtkManager.GetAtkByName("Méthylène gun"), AtkManager.GetAtkByName("Poche") },
                Vitesse = 3
            };

            Personnage Grelon = new Personnage
            {
                Nom = "Grêlon",
                Type = Element.TYPE.Givre,
                PointsDeVie = 50,
                Attaques = new List<Attaque> { AtkManager.GetAtkByName("Gelée douce"), AtkManager.GetAtkByName("Martelage") },
                Vitesse = 2
            };

            tousLesPersonnages.Add(Cohibou);
            tousLesPersonnages.Add(SbireSuintant);
            tousLesPersonnages.Add(SbirePetrole);
            tousLesPersonnages.Add(Grelon);

            return tousLesPersonnages;
        }

        public static void LancerJeu()
        {
           

            // 2. Choix du personnage:
            
            // Définition de la grille pour le choix du personnage 2x2:
         
            var perso = CreerTousLesPersonnages();  // Si tu as un PersonnageManager, cette ligne pourrait être : List<Personnage> perso = PersonnageManager.GetList(); (par ex)
            Personnage[,] grillePersonnages = new Personnage[2, 2]               // tableau multidimensionnel (ici 2 lignes × 2 colonnes). 
            {
                { perso[0],         perso[1]},                                      // La première paire { Cohibou, SbireSuintant } est la ligne 0 (col 0 et col 1).
                { perso[2],         perso[3]}                                         // La seconde paire est la ligne 1.
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

                /*  grillePersonnages.Cast<Personnage>() : transforme la grille 2×2 en liste pour pouvoir utiliser Max().                                        
                    PadRight(maxLength) : ajoute des espaces après chaque nom pour que toutes les colonnes aient la même largeur.
                    .Max(p => p.Nom.Length) - Max() parcourt chaque élément de la séquence et renvoie la valeur maximale.
                                        - Ici : p => p.Nom.Length indique qu’on veut la longueur du nom de chaque personnage.
                                        - Donc, Max() va calculer le nom le plus long parmi tous les personnages. Ici: 14
                                        - On ajoute 2 caractères d’espace pour séparer les colonnes dans l’affichage: Donc maxLength = 14 + 2 = 16.
                */

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

            // 3. Création d'une liste pour y mettre les personnages non-choisis par le joueur et les définir en tant qu'ennemis:

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

            // 4. COMMENCER LA BOUCLE DE COMBAT

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
