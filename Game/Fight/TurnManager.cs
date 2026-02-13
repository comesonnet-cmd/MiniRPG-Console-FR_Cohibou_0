using System;
using System.Threading;
using Game;
using Game.Effects;
using Game.Characters;
using Game.Attacks;
using System.Reflection.Metadata.Ecma335;

namespace Game.Fight
{
    public static class TurnManager
    {
        private static Random rnd = new Random();

        
        public static void TourJoueur(Personnage joueur, Personnage ennemi)
        {

            EstLibere(ennemi);

            if (joueur.EstVivant() && ennemi.EstVivant() && !PeutJouerCeTour(joueur, ennemi)) return;   
            
                                                                  
                                             
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
                        if (grilleAttaques[ligne, colonne] != null)
                        choisi = true;
                        break;
                }
            }

            Console.Clear();
            Thread.Sleep(500); // pause 0.5 seconde



            Attaque attaqueChoisie = grilleAttaques[ligne, colonne];            // Récupère l’attaque actuellement sélectionnée par le curseur dans la grille (ligne, colonne)

            joueur.LancerAttaque(ennemi, attaqueChoisie);                           // Le joueur exécute l’attaque choisie sur l’ennemi (calcul des dégâts, effets, etc.)

            

            EstLibere(joueur);


        }

        public static void TourEnnemi(Personnage ennemi, Personnage joueur)
        {


            EstLibere(joueur);
            if (ennemi.EstVivant() && joueur.EstVivant() && !PeutJouerCeTour(ennemi, joueur)) return;

            Console.WriteLine($"\nC'est au tour de {ennemi.Nom}:\n\n");

            // évite le crash si l'ennemi n'a pas d'attaque
            
            if (ennemi.Attaques == null || ennemi.Attaques.Count == 0)
            {
                return;
            }

          

            Attaque attaqueChoisie = ennemi.Attaques[rnd.Next(ennemi.Attaques.Count)];      // sinon, il lance une attaque random de son set d'attaques
            
            // évite le crash si l'attaque choisie est null
            if (attaqueChoisie == null)
            {
                return;
            }

            ennemi.LancerAttaque(joueur, attaqueChoisie);

           


            Console.ReadKey(true);

            EstLibere(ennemi);


        }

        // Vérifie tous les effets qui pourraient empêcher le joueur dont c'est le tour de jouer. S'il ne peut pas jouer => false; si toutes les vérifications sont faites, on retourne true => il peut jouer
        public static bool PeutJouerCeTour(Personnage turnPers, Personnage ennemi)
        {
            if (turnPers.EstEnglue)
            {
                if (EffectsManager.CheckIfEnglue(turnPers))
                {
                    return false;
                }
            }

            if (turnPers.EstAccule(ennemi))
            {
                if (EffectsManager.CheckIfAccule(turnPers, ennemi))
                {
                    return false;
                }
            }


            return true;
        }

        // On vérifie si les effets appliqués sur le personnage dont c'est le tour demeurent à la fin du tour:
        public static bool EstLibere(Personnage turnPers)
        {
            if (turnPers.EstEnglue)     // si le personnage est englué
            {
                bool TjrsEnglue = EffectsManager.CheckIfTjrsEnglue(turnPers);      // on vérifie s'il reste englué (1 chance sur 5)
                
                if (!TjrsEnglue)             // s'il n'est plus englué
                {
                   turnPers.EstEnglue = false;  // on met  jour son état => il n'est plus englué
                }

                return !TjrsEnglue;            // on retourne true s'il est libéré, false s'il reste englué
            }
          
            return true;            // si le personnage n'était pas englué au départ, il est libre

        }
        
    }
}
