using System;
using System.Threading;
using MiniRPG_Console_FR_Cohibou_0.Characters;
using Game;
using Game.Effects;

namespace Game.Fight
{
    public static class TurnManager
    {
        private static Random rnd = new Random();

        public static void CheckEffects(Personnage turnPers, Personnage ennemi, ref bool shouldEndTurn)
        {
            if (EffectsManager.CheckIfEnglue(turnPers))
            {
                shouldEndTurn = true;
                return;
            }

            if (EffectsManager.CheckIfAccule(turnPers,ennemi))
            {
                shouldEndTurn = true;
                return;
            }
        }
        public static void TourJoueur(Personnage joueur, Personnage ennemi)
        {

            private bool shouldEndTurn = false;

            CheckEffects(joueur, ennemi, ref shouldEndTurn)
            {
                 if (shouldEndTurn)
                 {
                     return;
                 }
            }

            if (joueur.EstEnglue)
            {
                if (!joueur.PeutAgirCeTour)
                {
                    Console.WriteLine($"{joueur.Nom} est englué. Il ne peut plus bouger !!\n");
                    Console.ReadKey();
                    return;
                }                        // Si le joueur ne peut pas agir ce tour, on sort de Tour Joueur
                else
                {
                    Console.WriteLine($"{joueur.Nom} est englué, mais il parvient tout de même à attaquer !!\n");
                    Console.ReadKey();
                }
            }




            Console.Clear();
            Thread.Sleep(500); // pause 0.5 seconde

           



            // Affiche le menu d'ataques
            int lignes = 2;
            int colonnes = 2;

            Attaque[,] grilleAttaques = new Attaque[2, 2];

            // Ligne 0                                                                          // Objectif : mettre les deux premières attaques du joueur sur la première ligne du tableau.
            grilleAttaques[0, 0] = joueur.Attaques.Count > 0 ? joueur.Attaques[0] : null;       // On vérifie si le joueur a au moins une attaque. Si oui, on met la première attaque dans la case [0,0] sinon, on met null (case vide) pour éviter un crash.
            grilleAttaques[0, 1] = joueur.Attaques.Count > 1 ? joueur.Attaques[1] : null;

            // Ligne 1
            grilleAttaques[1, 0] = joueur.Attaques.Count > 2 ? joueur.Attaques[2] : null;
            grilleAttaques[1, 1] = joueur.Attaques.Count > 3 ? joueur.Attaques[3] : null;


            int ligne = 0;
            int colonne = 0;
            bool choisi = false;

            while (!choisi)     // tant que choisi est false (tant que l'utilisateur n'a pas choisi)
            {
                Console.Clear();                                            // efface le menu précédent uniquement
                Console.WriteLine($"{joueur.Nom}, à toi de jouer !\n");   // on ré-affiche le message de début de tour
                Console.WriteLine("\nTu vas faire quoi?\n");                  // Choisir une attaque

                for (int i = 0; i < lignes; i++)     // On parcourt les attaques du joueur
                {
                    for (int j = 0; j < colonnes; j++)
                    {
                        if (i == ligne && j == colonne)
                            Console.Write("> ");    // curseur visible                       
                        else
                            Console.Write("  ");

                        Console.Write($"{(grilleAttaques[i, j]?.Nom ?? "----").PadRight(15)}\t"); //?.Nom → si la case est null, ça ne plante pas ?? "----" → affiche un placeholder pour les cases vides
                                                                                                  // PadRight(15) Aligne toutes les attaques sur 15 caractères
                    }
                    Console.WriteLine();
                }

                // Lire la touche appuyée
                ConsoleKeyInfo key = Console.ReadKey(true);     // Après affichage, Console.ReadKey(true) attend une touche sans l’afficher (true = key interceptée).

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        ligne = (ligne - 1 + lignes) % lignes;          // parce que si on doit appuyer sur flèche du haut ça veut dire que l'on est sur la ligne du bas et donc que ligne = 1
                        break;                                          // dc ligne = (1 - 1 + 2) % 2 => 0 donc on part en haut sur la ligne 0
                    case ConsoleKey.DownArrow:                          // parce que si on doit appuyer sur flèche du bas ça veut dire que l'on est sur la ligne du haut et donc que ligne = 0
                        ligne = (ligne + 1) % lignes;                   // dc ligne = (0 + 1) % 2 =>  1 donc on part en bas sur la ligne 1
                        break;
                    case ConsoleKey.LeftArrow:
                        colonne = (colonne - 1 + colonnes) % colonnes;
                        break;
                    case ConsoleKey.RightArrow:
                        colonne = (colonne + 1) % colonnes;
                        break;
                    case ConsoleKey.Enter:
                        choisi = true;
                        break;
                }
            }

            Console.Clear();
            Thread.Sleep(500); // pause 0.5 seconde



            Attaque attaqueChoisie = grilleAttaques[ligne, colonne];            // Récupère l’attaque actuellement sélectionnée par le curseur dans la grille (ligne, colonne)

            joueur.LancerAttaque(ennemi, attaqueChoisie);                           // Le joueur exécute l’attaque choisie sur l’ennemi (calcul des dégâts, effets, etc.)




        }

        public static void TourEnnemi(Personnage ennemi, Personnage joueur)
        {
            if (ennemi.EstEnglue)
            {
                if (!ennemi.PeutAgirCeTour)
                {
                    Console.WriteLine($"{ennemi.Nom} est englué. Il ne peut plus bouger !!\n");
                    Console.ReadKey();
                    return; // attaque bloquée ici : on appelle PeutAgirCeTour(), si la méthode retourne false alors on quitte le tour et l'attaque n'ajamais lieu
                }
                else
                {
                    Console.WriteLine($"{ennemi.Nom} est englué, mais il parvient tout de même à attaquer !!\n");
                    Console.ReadKey();
                }
            }


            Console.WriteLine($"\nC'est au tour de {ennemi.Nom}:\n\n");

            Attaque attaqueChoisie;

            // Ennemi est acculé, il doit utiliser son attaque imprévue:
            if (ennemi.EstAccule(joueur))
            {
                ennemi.DureeEstIntouchable++;
            }
            else
            {
                ennemi.DureeEstIntouchable = 0;
            }

            if (ennemi.EstAccule(joueur) && ennemi.AttaqueImprevue != null && ennemi.DureeEstIntouchable >= 3) // si 1. l'ennemi n'a pas d'attaques capable de toucher le joueur 2. qu'il a une attaque imprévue 3. que la durée EstIntouchable est arrivée à 3, alors il lance son attaque imprévue
            {
                Console.WriteLine($"{ennemi.Nom} est acculé...\n");
                attaqueChoisie = ennemi.AttaqueImprevue;

            }
            else
            {
                attaqueChoisie = ennemi.Attaques[rnd.Next(ennemi.Attaques.Count)];      // sinon, il lance une attaque random de son set d'attaques
            }


            ennemi.LancerAttaque(joueur, attaqueChoisie);

            Console.ReadKey(true);


        }

       
    }
}
