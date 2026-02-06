using System;
using Game.Attacks;
using Game.Characters;


namespace Game.Effects
{
    public static class EffectsManager
    {
        private static Random rnd = new Random();
        public enum EFFECT
        {
            Aucun,
            Empoisonnement,
            ConversionPenombre,
            Engluage,
            PochetteSurprise,
            ImmuGivre,
            ProtectVeneneux,
           
        }

        public static bool CheckIfEnglue(Personnage p)
        {
            // Engluage : une chance sur deux d'agir ou de ne rien faire

            if (p.EstEnglue && !p.PeutAgirCeTour)
            {
                bool para = rnd.Next(2) == 0;       // Génère soit 0, soit 1 (pile ou face) Si le résultat est 0 → para = true = paralysé Si le résultat est 1 → para = false = peut bouger
                p.PeutAgirCeTour = !para;           // On Définie que peutAgirCeTour est true quand para est false
                Console.WriteLine($"{p.Nom} est englué. Il ne peut plus bouger !!\n");
                Console.ReadKey();
                                    // attaque bloquée ici : on appelle PeutAgirCeTour(), si la méthode retourne false alors on quitte le tour et l'attaque n'a jamais lieu 
                return true;
            }
            else
            {
                p.PeutAgirCeTour = true;
                    Console.WriteLine($"{p.Nom} est englué, mais il parvient tout de même à attaquer !!\n");
                    Console.ReadKey();
            }

            return false;
           
        }

        public static bool CheckIfAccule(Personnage joueur, Personnage ennemi)
        {
            if (joueur.EstAccule(ennemi))           // Si le joueur est acculé par l'ennemi , on initialise la durée EstIntouchable
            {
                joueur.DureeEstIntouchable++;
            }
            else                                    // Sinon, la durée EstIntouchable est égale à 0
            {
                joueur.DureeEstIntouchable = 0;
            }

            if (joueur.EstAccule(ennemi) && joueur.AttaqueImprevue != null && joueur.DureeEstIntouchable >= 3)      // Si 1. le joueur est acculé par l'ennemi 2. il a une attaque imprévue 3. que la durée EstIntouchable est arrivée à 3 (tours), on lance l'attaque imprévue
            {
                Console.Clear();
                Console.WriteLine($"{joueur.Nom} n'a plus d'autre choix...\n");
                Console.WriteLine();
                Console.WriteLine($"> {joueur.AttaqueImprevue.Nom}");
                Console.WriteLine();

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }       // Tant que l’utilisateur n’appuie pas sur Entrée, on ne fait absolument rien et on attend.

                joueur.LancerAttaque(ennemi, joueur.AttaqueImprevue);
                return true;
            }

            
            // Ennemi est acculé, il doit utiliser son attaque imprévue:

            Attaque attaqueChoisie;

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
                return true;
            }
            
            return false;
        }
        
        public static void AppliquerEffets(Personnage p)
        {
            // Empoisonnement permanent
           
               static void ManagePoison(Personnage p)
               {
                    if (p.EstEmpoisonne)
                         {
                             Console.WriteLine();
                             Console.WriteLine($"{p.Nom} est infecté. Il souffre. Cela lui enlève 5 PV !\n");
                             p.SubirDegats(5);
                         }       

               }
              
               static void ManageImmunGivre(Personnage p)
               {
                    // Diminution de la durée d'immunité au givre
                if (p.DureeImmuniteGivre > 0)
                {
                    p.DureeImmuniteGivre--;         // On diminue la durée de 1.

                    if (p.DureeImmuniteGivre > 0)
                    {
                        Console.WriteLine($"{p.Nom} est toujours protégé du givre. {p.DureeImmuniteGivre} tour(s) restant(s)\n");
                    }
                    else  // p.DureeImmuniteGivre == 0
                    {
                        p.EstImmunise = false;
                        Console.WriteLine($"{p.Nom} n'est plus immunisé contre le givre !\n");
                    }
                }
               }

               static void ManageProtectVénéneux(Personnage p)
               {
                if (p.DureeProtectionVeneneux > 0)
                {
                    p.DureeProtectionVeneneux--;


                    if (p.DureeProtectionVeneneux > 0)
                    {
                        Console.WriteLine($"{p.Nom} est toujours protégé des attaques vénéneuses. {p.DureeProtectionVeneneux} tour(s) restant(s)\n");
                    }
                    else // if p.DureeProtectionVeneneux == 0
                    {
                        p.EstProtege = false;
                        Console.WriteLine($"{p.Nom} n'est plus protégé contre les attaques vénéneuses !\n");
                    }
                }
            }

           
        }
    }
}
