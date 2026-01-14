using System;

namespace MiniRPG_Console_FR_Cohibou_0.Utils
{
    public static class ConsoleHelpers
    {
        public static bool ChoixOuiNon(string question)      // bool → La fonction renvoie true si le joueur choisit Oui, false si le joueur choisit Non.
        {                                                    // string question → Tu passes le texte affiché au-dessus du menu (“Es-tu sûr ?”, etc.).
            int choix = 0;

            while (true)                                     // Boucle d'affichage continue : La boucle tourne jusqu’à ce que l’utilisateur appuie sur Entrée. On redessine l’écran à chaque déplacement.

            {
                // On efface l’écran et on affiche la phrase, puis une ligne vide:

                Console.Clear();
                Console.WriteLine(question);
                Console.WriteLine();

                // L'opérateur ternaire ?:  si choix == 0 --> afficher "> oui"  si choix == 1 --> afficher "> non"

                Console.Write(choix == 0 ? "> Oui" : "  Oui");
                Console.Write("    ");
                Console.Write(choix == 1 ? "> Non" : "  Non");
                Console.WriteLine();

                // Lecture de la touche pressée:

                ConsoleKeyInfo key = Console.ReadKey(true);     // Attend qu’une touche soit pressée. true = ne pas afficher la touche à l'écran (lecture silencieuse)

                // Gestion des flèches:

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        choix = (choix - 1 + 2) % 2;    // ---> // choix - 1 -> si on est sur oui (0) -> (0-1) ça fait -1  / si on est sur non (1) -> (1-1) ça fait 0
                        break;                                  // Pourquoi +2? Parce qu'on veut s'assurer que le résultat est toujours positif -> si on est sur oui (0) -> (0-1) ça fait -1 +2 = 1
                    case ConsoleKey.RightArrow:                 // -> si on est sur non (1) -> (1-1) ça fait 0 + 2 = 2 -> on obtient 2 temporairement, mais on va corriger ça avec la dernière étape: Le modulo → boucle entre 0 et 1 -> Le modulo 2 réduit le résultat à 0 ou 1 seulement.
                        choix = (choix + 1) % 2;                // En résumé: choix (si on est sur 0 -> Oui et qu'on appuie)  = ( (0) - 1 + 2) % 2 donc : (0 - 1 + 2 = 1 % 2) = On arrive sur 1 ("Non") 
                        break;                                                   // (si on est sur 1 -> Non et qu'on appuie)  = ( (1) - 1 + 2) % 2 donc : (1 - 1 + 2 = 2 % 2) = On arrive sur 0 ("Oui")

                    // Validation:

                    case ConsoleKey.Enter:
                        return choix == 0;              // TRUE si Oui, FALSE si Non
                }
            }
        }
    }
}
