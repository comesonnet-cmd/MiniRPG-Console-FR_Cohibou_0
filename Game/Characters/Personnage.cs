using Game;
using Game.Elements;
using Game.Effects;
using Game.Attacks;
using System;
using System.Collections.Generic;

namespace Game.Characters
{
    public class Personnage
    {
        
        // Attributs: Stats - Attaques - Effets - Etats 
        
        private static Random rnd = new Random();

        public string Nom;
        public Element.TYPE Type;
        public int PointsDeVie;
        public List<Attaque> Attaques = new List<Attaque>();
        public int Vitesse;
        public bool PeutAgirCeTour = true;
        public Attaque AttaqueImprevue;

        // Durées pour les effets temporaires
        public int DureeImmuniteGivre = 0;
        public int DureeProtectionVeneneux = 0;
        public int DureeEstIntouchable = 0;             // Nombre de tours consécutilfs où le joueur EstAccule

        // Etat d'empoisonnement permanent jusqu'au KO
        public bool EstEmpoisonne = false;
        // Immunité 
        public bool EstImmunise = false;
        // Engluage
        public bool EstEnglue = false;                  // true : le personnage est englué, false : il ne l'est pas (le bool décrit seulement un état)
        // Protection
        public bool EstProtege = false;

        public bool EstAccule(Personnage cible)
        {
            foreach (Attaque a in Attaques)
            {
                if (Element.GetMultiplicateur(a.Type, cible.Type) > 0)
                    return false;                       // au moins une attaque efficace   
            }
            return true;                                // aucune attaque n'est efficace sur la cible
        }

        public bool EstVivant()                         // On peut aussi l'écrire "public bool EstVivant() => PointsDeVie > 0;"
        {
            if (PointsDeVie > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SubirDegats(int degats)
        {

            PointsDeVie -= degats;
            if (PointsDeVie < 0)
            {
                PointsDeVie = 0;
            }

        }
      

        // LANCER ATTAQUE
        public void LancerAttaque(Personnage cible, Attaque attaque)            // ici, cible représente l'ennemi, et "this" le joueur quand le joueur attaque
        {                                                                       // et inversement this devient l'ennemi et cible le joueur quand l'ennemi attaqe
            // 1. Protection Anti-Givre                                         // Donc this existe automatiquement dans toute méthode d’instance, sans devoir le déclarer.
            if (attaque.Type == Element.TYPE.Givre && cible.EstImmunise)         // 1. Vérifier immunité Givre AVANT les dégâts
            {
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) sur {cible.Nom} !\n");
                Console.WriteLine($"Mais le givre n'affecte pas {cible.Nom} en ce moment !\n");
                return;
            }

            // 2. Protection Anti-Vénéneux (réduction dégâts)
            double reduction = 1.0;
            if (attaque.Type == Element.TYPE.Veneneux && attaque.Nom != "Poche" && attaque.Nom != "Smog bleu" && cible.EstProtege)
            {
                reduction = 0.5;
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) sur {cible.Nom} !\n");
                Console.WriteLine($"Mais {cible.Nom} est protégé ! L'attaque vénéneuse est affaiblie\n");

            }
            else if (attaque.Type == Element.TYPE.Veneneux && attaque.Nom == "Poche" && cible.EstProtege)
            {
                reduction = 0.5;
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) sur {cible.Nom} !\n");
            }
            else if (attaque.Type == Element.TYPE.Veneneux && attaque.Nom == "Smog bleu" && cible.EstProtege)
            {
                reduction = 0.5;
                Console.WriteLine($"Mais {cible.Nom} est protégé ! L'attaque vénéneuse est affaiblie\n");
            }

            // Calcul des dégâts
            double multiplicateur = Element.GetMultiplicateur(attaque.Type, cible.Type);    // Récupère le multiplicateur de dégâts selon les types de l'attaque et de la cible
            int degatsFinaux = (int)(attaque.Puissance * multiplicateur * reduction);           // - (int) convertit les dégâts en entier / - Calcule les dégâts finaux : puissance de l'attaque * multiplicateur de type * éventuelle réduction
            cible.SubirDegats(degatsFinaux);                                                    // Applique les dégâts calculés à la cible

            if (attaque.Effet == EffectsManager.EFFECT.ImmuGivre || attaque.Effet == EffectsManager.EFFECT.ProtectVeneneux)
            {
                // Si les effets de l'attaque sont ImmuGivre ou ProtectVeneneux, on affiche la phrase ci-dessous car le joueur d'auto-cible 
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) !\n");
            }
            else if (attaque.Type == Element.TYPE.Veneneux && cible.EstProtege)
            {
                // Si l'attaque cible quelqu'un qui s'est protégé du poison, on n'affiche pas le message car affiché plus haut
                Console.WriteLine();
            }
            else if (attaque.Nom == "Smog bleu")
            {
                // Si l'attaque est Smog Bleu, on n'affiche pas le message car déjàaffiché 
                Console.WriteLine();
            }
            else
            {
                // L'attaque cible un autre personnage
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) sur {cible.Nom} !\n");
            }


            if (cible.PointsDeVie <= 0)         // Afficher KO si plus de PV
            {
                Console.WriteLine($"Cela enlève {degatsFinaux} PV au {cible.Nom}.\n");
                Console.WriteLine($"{cible.Nom} est KO ! Il va se reposer...\n");
                Console.ReadKey();                          //  Attend que l’utilisateur appuie sur une touche du clavier
            }
            else if (degatsFinaux > 0)
            {
                Console.WriteLine($"Cela enlève {degatsFinaux} PV au {cible.Nom}. Il ne lui reste plus que {cible.PointsDeVie} PV.\n");
            }
            else if (attaque.Puissance > 0 && multiplicateur == 0)          // Attaque offensive mais inefficace
            {
                Console.WriteLine($"Cela n'inflige pas de dégâts au {cible.Nom}.\n");
            }
            // Sinon (attaque utilitaire, puissance 0) → rien n'affiche (Feuille rouge par ex!)


            // Gestion des effets secondaires:
            switch (attaque.Effet)
            {
                case EffectsManager.EFFECT.Empoisonnement:
                    // le type Givré est immunisé à l'empoisonnement
                    if (cible.Type == Element.TYPE.Givre)
                    {
                        Console.WriteLine($"{cible.Nom} est de type Givré, le poison ne l'affecte pas !\n");
                    }
                    else if (cible.EstEmpoisonne)
                    {
                        cible.EstEmpoisonne = true;
                        Console.WriteLine();    // car la cible est déjà empoisonnée
                    }
                    else
                    {
                        cible.EstEmpoisonne = true;
                        Console.WriteLine($"{cible.Nom} est empoisonné, il va perdre 5 PV à chaque tour...\n");
                    }
                    break;

                case EffectsManager.EFFECT.ImmuGivre:
                    // Ne peut pas utiliser si l'effet est encore actif
                    if (EstImmunise && DureeImmuniteGivre > 0)
                    {
                        Console.WriteLine($"L'immunité givre est encore active ! Cela n'aurait aucun effet !\n");
                        break;
                    }

                    // Immunise aux attaques Givrées pendant 3 tour
                    EstImmunise = true;
                    DureeImmuniteGivre = 3;
                    Console.WriteLine($"{Nom} produit un pigment rouge protecteur qui l'immunise au givre pendant 3 tours\n");

                    break;

                case EffectsManager.EFFECT.ConversionPenombre:
                    // Convertit le type du personnage en type Pénombre jusqu'à la fin du combat
                    if (cible.Type != Element.TYPE.Penombre)
                    {
                        cible.Type = Element.TYPE.Penombre;
                        Console.WriteLine($"{cible.Nom} ne se sent pas bien...il prend le type Pénombre !\n");
                    }
                    break;

                case EffectsManager.EFFECT.Engluage:
                    // Englue la cible, elle a une chance sur deux de pouvoir attaquer à chaque tour, jusqu'à la fin du combat. 
                    if (!cible.EstEnglue) // si la cible n'est pas (déjà) engluée
                    {
                        cible.EstEnglue = true;
                        Console.WriteLine($"{cible.Nom} est englué! Comment va t-il se sortir de là?...\n");
                    }
                    break;


                case EffectsManager.EFFECT.ProtectVeneneux:
                    // Diminue de moitié la puissance des attaques vénéneuses pendant 3 tours
                    if (EstProtege && DureeProtectionVeneneux > 0)
                    {
                        Console.WriteLine($"La protection est encore active ! Cela n'aurait aucun effet !\n");
                        break;
                    }

                    EstProtege = true;
                    DureeProtectionVeneneux = 3;
                    Console.WriteLine($"{Nom} se pourvoit d'un film protecteur qui diminue de moitié les attaques vénéneuses pendant 3 tours\n");
                    break;

                case EffectsManager.EFFECT.PochetteSurprise:
                    // Tire à pile ou face, si pile = "Poche vide", n'inflige aucun dégât, si face = Attaque (selon l'attaque utilisée).
                    if (attaque.TirePileFace)
                    {

                        bool face = rnd.Next(0, 2) == 0;     // 0 = face, 1 = pile

                        if (face)
                        {

                            Console.WriteLine($"{Nom} sort une boule de smog de sa poche...Il lance smog bleu!\n");

                            if (attaque.AttaqueBonus != null)
                            {
                                // Lancer l'attaque bonus sur la cible
                                LancerAttaque(cible, attaque.AttaqueBonus);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Arghh... la poche est vide... C'est raté ! \n");           // Aucun dégât/ effet
                        }

                    }
                    break;

                case EffectsManager.EFFECT.Aucun:
                default:
                    break;
            }


        }
    }
}
