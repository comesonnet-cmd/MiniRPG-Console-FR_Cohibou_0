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

        public static bool CheckIfEnglue(Personnage turnPers)
        {
            // Engluage : une chance sur deux d'agir ou de ne rien faire

           
            bool para = rnd.Next(2) == 0;       // Génère soit 0, soit 1 (pile ou face) Si le résultat est 0 → para = true = paralysé Si le résultat est 1 → para = false = peut bouger
                
            if (para)
            {
                Console.WriteLine($"{turnPers.Nom} est englué. Il ne peut plus bouger !!\n");
              
            }                                                   
            else
            {              
                Console.WriteLine($"{turnPers.Nom} est englué, mais il parvient tout de même à attaquer !!\n");
                   
            }

            Console.ReadKey();
            return para;            // retourne true si paralisé, false s'il peut agir
           
        }

      
        public static bool CheckIfAccule(Personnage turnPers, Personnage ennemi)
        {
            if (turnPers.EstAccule(ennemi))           // Si le personnage dont c'est le tour est acculé par l'ennemi, on initialise la durée EstIntouchable
            {
                turnPers.DureeEstIntouchable++;
            }
            else                                    // Sinon, la durée EstIntouchable est égale à 0
            {
                turnPers.DureeEstIntouchable = 0;
            }

            if (turnPers.EstAccule(ennemi) && turnPers.AttaqueImprevue != null && turnPers.DureeEstIntouchable >= 3)      // Si 1. le joueur est acculé par l'ennemi 2. il a une attaque imprévue 3. que la durée EstIntouchable est arrivée à 3 (tours), on lance l'attaque imprévue
            {
                Console.Clear();
                Console.WriteLine($"{turnPers.Nom} n'a plus d'autre choix...\n");
                Console.WriteLine();
                Console.WriteLine($"> {turnPers.AttaqueImprevue.Nom}");
                Console.WriteLine();

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }       // Tant que l’utilisateur n’appuie pas sur Entrée, on ne fait absolument rien et on attend.

                turnPers.LancerAttaque(ennemi, turnPers.AttaqueImprevue);
                return true;
            }
            
            return false;
        }
        
        public static bool CheckIfTjrsEnglue(Personnage turnPers)
        {

            // Engluage Persiste: une chance sur cinq de sortir de l'engluage:

            bool stillSticky = rnd.Next(5) != 0;        // Génère un chiffre en 0 et 4. Si le résultat est 0 → stillSticky = false = le personnage sort de l'engluage. Si le résultat est autre → stillSticky = true = le personnage reste englué!

            if(stillSticky)
            {
                Console.WriteLine($"{turnPers.Nom} tente de s'extirper de la mélasse... En vain!\n");
            }
            else
            {
                Console.WriteLine($"{turnPers.Nom} tente de s'extirper de la mélasse... Avec succès!\n");
            }

            Console.ReadKey();
            return stillSticky;
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
