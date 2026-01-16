using System;
using MiniRPG_Console_FR_Cohibou_0.Characters;

namespace MiniRPG_Console_FR_Cohibou_0.Game
{
    public static class EffectsManager
    {
        private static Random rnd = new Random();

        public static void AppliquerEffets(Personnage p)
        {
            // Empoisonnement permanent

            if (p.EstEmpoisonne)
            {
                Console.WriteLine();
                Console.WriteLine($"{p.Nom} est infecté. Il souffre. Cela lui enlève 5 PV !\n");
                p.SubirDegats(5);
            }

            // Engluage : une chance sur deux d'agir ou de ne rien faire

            if (p.EstEnglue)
            {
                bool para = rnd.Next(2) == 0;       // Génère soit 0, soit 1 (pile ou face) Si le résultat est 0 → para = true = paralysé Si le résultat est 1 → para = false = peut bouger
                p.PeutAgirCeTour = !para;           // On Définie que peutAgirCeTour est true quand para est false
            }
            else
            {
                p.PeutAgirCeTour = true;
            }

            // Diminution des durées

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
