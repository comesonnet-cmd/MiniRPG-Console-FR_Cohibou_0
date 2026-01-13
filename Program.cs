using System;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Threading;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

class Program
{

    static Random rnd = new Random();

    enum TypeElement                        // On définit les 4 types élémentaires : Automne, Penombre, Veneneux, Givre.
                                            // Ça permet de gérer facilement le type d’une attaque ou d’un personnage.
    {
        Automne,
        Penombre,
        Veneneux,
        Givre
    }

    enum EffetType
    {
        Aucun,
        Empoisonnement,
        ConversionPenombre,
        Engluage,
        PochetteSurprise,
        ImmuGivre,
        ProtectVeneneux,
        raté,
        Torpeur
    }
    static double GetMultiplicateur(TypeElement attaquant, TypeElement defenseur)                   // Crée un tableau 2D qui contient les multiplicateurs de dégâts selon le type qui attaque et celui qui défend.
    {                                                                                               // Exemple : si une attaque Automne touche un ennemi Pénombre → multiplicateur = 2 (super efficace).
                                                                                                    // La fonction retourne automatiquement le multiplicateur correspondant pour n’importe quelle combinaison d’attaquant/défenseur.
        double[,] multiplicateurs = new double[4, 4];   // crée un tableau 4x4

        multiplicateurs[0, 0] = 1;                      // Automne vs Automne = normal
        multiplicateurs[0, 1] = 2;                      // Automne vs Pénombre = super efficace
        multiplicateurs[0, 2] = 0;                      // Automne vs Vénéneux = aucun dégât
        multiplicateurs[0, 3] = 1;                      // Automne vs Givré = normal

        multiplicateurs[1, 0] = 0;                      // Pénombre vs Automne = aucun dégât
        multiplicateurs[1, 1] = 2;                      // Pénombre vs Pénombre = super efficace
        multiplicateurs[1, 2] = 2;                      // Pénombre vs Vénéneux = super efficace
        multiplicateurs[1, 3] = 0;                      // Pénombre vs Givré = aucun dégât

        multiplicateurs[2, 0] = 2;                      // Vénéneux vs Automne = super efficace
        multiplicateurs[2, 1] = 1;                      // Vénéneux vs Pénombre = normal
        multiplicateurs[2, 2] = 0.5;                    // Vénéneux vs Vénéneux = résitance de 50%
        multiplicateurs[2, 3] = 0.5;                    // Vénéneux vs Givré = résitance de 50%

        multiplicateurs[3, 0] = 2;                      // Givré vs Automne = super efficace
        multiplicateurs[3, 1] = 1;                      // Givré vs Pénombre = normal
        multiplicateurs[3, 2] = 2;                      // Givré vs Vénéneux = super efficace
        multiplicateurs[3, 3] = 1;                      // Givré vs Givré = normal

        return multiplicateurs[(int)attaquant, (int)defenseur];
    }

    static bool ChoixOuiNon(string question)    // bool → La fonction renvoie true si le joueur choisit Oui, false si le joueur choisit Non.
    {                                           // string question → Tu passes le texte affiché au-dessus du menu (“Es-tu sûr ?”, etc.).
        int choix = 0; // 0 = Oui, 1 = Non

        while (true)                            // Boucle d'affichage continue : La boucle tourne jusqu’à ce que l’utilisateur appuie sur Entrée. On redessine l’écran à chaque déplacement.

        {
            // On efface l’écran et on affiche la phrase, puis une ligne vide:

            Console.Clear();
            Console.WriteLine(question);
            Console.WriteLine();

            // L'opérateur ternaire ?:  si choix == 0 --> afficher "> oui"  si choix == 1 --> afficher "> non"
            Console.Write(choix == 0 ? "> Oui" : "  Oui");      // Opérateur ?: comme if/else --> condition ? valeur_si_vrai : valeur_si_faux
            Console.Write("    ");
            Console.Write(choix == 1 ? "> Non" : "  Non");
            Console.WriteLine();

            // Lecture de la touche pressée:

            ConsoleKeyInfo key = Console.ReadKey(true);     // Attend qu’une touche soit pressée. true = ne pas afficher la touche à l'écran (lecture silencieuse)

            // Gestion des flèches:

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    choix = (choix - 1 + 2) % 2; // ---> // choix - 1 -> si on est sur oui (0) -> (0-1) ça fait -1  / si on est sur non (1) -> (1-1) ça fait 0
                    break;                               // Pourquoi +2? Parce qu'on veut s'assurer que le résultat est toujours positif -> si on est sur oui (0) -> (0-1) ça fait -1 +2 = 1
                                                         // -> si on est sur non (1) -> (1-1) ça fait 0 + 2 = 2 -> on obtient 2 temporairement, mais on va corriger ça avec la dernière étape: Le modulo → boucle entre 0 et 1 -> Le modulo 2 réduit le résultat à 0 ou 1 seulement.
                case ConsoleKey.RightArrow:              // En résumé: choix (si on est sur 0 -> Oui et qu'on appuie)  = ( (0) - 1 + 2) % 2 donc : (0 - 1 + 2 = 1 % 2) = On arrive sur 1 ("Non") 
                    choix = (choix + 1) % 2;                              // (si on est sur 1 -> Non et qu'on appuie)  = ( (1) - 1 + 2) % 2 donc : (1 - 1 + 2 = 2 % 2) = On arrive sur 0 ("Oui")
                    break;

                // Validation:
                case ConsoleKey.Enter:
                    return choix == 0;  // TRUE si Oui, FALSE si Non
            }
        }
    }
    static void AppliquerEffets(Personnage p)
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
            bool para = rnd.Next(2) == 0;           // Génère soit 0, soit 1 (pile ou face) Si le résultat est 0 → para = true = paralysé Si le résultat est 1 → para = false = peut bouger
            p.PeutAgirCeTour = !para;               // On Définie que peutAgirCeTour est true quand para est false 
       
        }
        else
        {
            p.PeutAgirCeTour = true;
        }

        // Diminution des durées
        if (p.DureeImmuniteGivre > 0)
        {
            
            p.DureeImmuniteGivre--;         // On diminue la durée de 1.
            
            if(p.DureeImmuniteGivre > 0)
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

    static void TourJoueur(Personnage joueur, Personnage ennemi)
    {
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

        if (joueur.EstAccule(ennemi))
        {
            joueur.DureeEstIntouchable++;
        }
        else
        {
            joueur.DureeEstIntouchable = 0;
        }

        if (joueur.EstAccule(ennemi) && joueur.AttaqueImprevue != null && joueur.DureeEstIntouchable >= 3)
        {
            Console.Clear();
            Console.WriteLine($"{joueur.Nom} n'a plus d'autre choix...\n");
            Console.WriteLine();
            Console.WriteLine($"> {joueur.AttaqueImprevue.Nom}");
            Console.WriteLine();

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            joueur.LancerAttaque(ennemi, joueur.AttaqueImprevue);
            return;
        }       
       


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

        
       
        Attaque attaqueChoisie = grilleAttaques[ligne, colonne];

        joueur.LancerAttaque(ennemi, attaqueChoisie); 
       
    }

    static void TourEnnemi(Personnage ennemi, Personnage joueur)
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

        if (ennemi.EstAccule(joueur) && ennemi.AttaqueImprevue != null &&  ennemi.DureeEstIntouchable >= 3) // si l'ennemi n'a pas d'attaques capable de toucher le joueur
        {
             Console.WriteLine($"{ennemi.Nom} est acculé...\n");
             attaqueChoisie = ennemi.AttaqueImprevue;
             
        }
        else
        {
            attaqueChoisie = ennemi.Attaques[rnd.Next(ennemi.Attaques.Count)];
        }        


        ennemi.LancerAttaque(joueur, attaqueChoisie);

        Console.ReadKey(true);

    }
    class Attaque
    {
        public string Nom;
        public TypeElement Type;
        public int Puissance;

        public EffetType Effet = EffetType.Aucun;  // Par défaut : aucun effet
        // Tire à pile ou face
        public bool TirePileFace = false;     // true si l'effet doit tirer à pile ou face 
        public Attaque AttaqueBonus = null;   // attaque à lancer si Face
        public Attaque AttaqueImprevue;
    }

    class Personnage
    {
        public string Nom;
        public TypeElement Type;
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
        public bool EstEnglue = false;      // true : le personnage est englué, false : il ne l'est pas (le bool décrit seulement un état)
        // Protection
        public bool EstProtege = false;

        public bool EstAccule(Personnage cible)
        {
            foreach (Attaque a in Attaques)
            {
                if (Program.GetMultiplicateur(a.Type, cible.Type) > 0)
                
                    return false; // au moins une attaque efficace                
            }
            return true; // aucune attaque n'est efficace sur la cible
        }

        public bool EstVivant()
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
            {
                PointsDeVie -= degats;
                if (PointsDeVie < 0)
                {
                    PointsDeVie = 0;
                }

            }
        }

       
        public void LancerAttaque(Personnage cible, Attaque attaque)            // ici, cible représente l'ennemi, et "this" le joueur quand le joueur attaque
        {                                                                       // et inversement this devient l'ennemi et cible le joueur quand l'ennemi attaqe
                                                                                // Donc this existe automatiquement dans toute méthode d’instance, sans devoir le déclarer.
            // 1. Proection Anti- Givre                                         // 1. Vérifier immunité Givre AVANT les dégâts
            if (attaque.Type == TypeElement.Givre && cible.EstImmunise)
            {
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) sur {cible.Nom} !\n");
                Console.WriteLine($"Mais le givre n'affecte pas {cible.Nom} en ce moment !\n");
               
                return;
            }

            // 2. Protection anti-Vénéneux (réduction dégâts)
            double reduction = 1.0;
            if (attaque.Type == TypeElement.Veneneux && attaque.Nom != "Poche" && attaque.Nom != "Smog bleu" && cible.EstProtege)
            {
                reduction = 0.5;
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) sur {cible.Nom} !\n");
                Console.WriteLine($"Mais {cible.Nom} est protégé ! L'attaque vénéneuse est affaiblie\n");
                
            }
            else if (attaque.Type == TypeElement.Veneneux && attaque.Nom == "Poche" && cible.EstProtege)
            {
                reduction = 0.5;
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) sur {cible.Nom} !\n");
            }
            else if (attaque.Type == TypeElement.Veneneux && attaque.Nom == "Smog bleu" && cible.EstProtege)
            {
                reduction = 0.5;
                Console.WriteLine($"Mais {cible.Nom} est protégé ! L'attaque vénéneuse est affaiblie\n");
            }

           
                
            
            // Calcul des dégâts
            double multiplicateur = Program.GetMultiplicateur(attaque.Type, cible.Type);
            int degatsFinaux = (int)(attaque.Puissance * multiplicateur * reduction);

            // Appliquer les dégâts 
            cible.SubirDegats(degatsFinaux);

            if (attaque.Effet == EffetType.ImmuGivre || attaque.Effet == EffetType.ProtectVeneneux)
            {
                // Si les effets de l'attaque sont ImmuGivre ou ProtectVeneneux, on affiche la phrase ci-dessous car le joueur d'auto-cible 
                Console.WriteLine($"Ah ! {Nom} a lancé {attaque.Nom} ({attaque.Type}) !\n");
            }
            else if (attaque.Type == TypeElement.Veneneux && cible.EstProtege)
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
                case EffetType.Empoisonnement:
                    // le type Givré est immunisé à l'empoisonnement
                    if (cible.Type == TypeElement.Givre)
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

                case EffetType.ImmuGivre:
                    // Ne peut pas utiliser si l'effet est encore actif
                    if (this.EstImmunise && this.DureeImmuniteGivre > 0)
                    {
                        Console.WriteLine($"L'immunité givre est encore active ! Cela n'aurait aucun effet !\n");
                        break;
                    }

                    // Immunise aux attaques Givrées pendant 3 tour
                    this.EstImmunise = true;
                    this.DureeImmuniteGivre = 3;
                    Console.WriteLine($"{Nom} produit un pigment rouge protecteur qui l'immunise au givre pendant 3 tours\n");

                    break;

                case EffetType.ConversionPenombre:
                    // Convertit le type du personnage en type Pénombre jusqu'à la fin du combat
                    if (cible.Type != TypeElement.Penombre)
                    {
                        cible.Type = TypeElement.Penombre;
                        Console.WriteLine($"{cible.Nom} ne se sent pas bien...il prend le type Pénombre !\n");
                    }
                    break;

                case EffetType.Engluage:
                    // Englue la cible, elle a une chance sur deux de pouvoir attaquer à chaque tour, jusqu'à la fin du combat. 
                    if(!cible.EstEnglue) // si la cible n'est pas (déjà) engluée
                    {
                        cible.EstEnglue = true;
                        Console.WriteLine($"{cible.Nom} est englué! Comment va t-il se sortir de là?...\n");
                    }                   
                    break;
                                                                                                                                                                                      

                case EffetType.ProtectVeneneux:
                    // Diminue de moitié la puissance des attaques vénéneuses pendant 3 tours
                    if(this.EstProtege && this.DureeProtectionVeneneux > 0)
                    {
                        Console.WriteLine($"La protection est encore active ! Cela n'aurait aucun effet !\n");
                        break;
                    }
                    
                    this.EstProtege = true;
                    this.DureeProtectionVeneneux = 3;
                    Console.WriteLine($"{this.Nom} se pourvoit d'un film protecteur qui diminue de moitié les attaques vénéneuses pendant 3 tours\n");
                    break;

                case EffetType.PochetteSurprise:
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

                case EffetType.Aucun:
                default:
                    break;
            }



        }
    }   static void LancerJeu()
        {
        // Étape 1 : créer les attaques 

        // 1) Attaques de Cohibou
        Attaque feuilleRouge = new Attaque
        {
            Nom = "Feuille rouge",
            Type = TypeElement.Automne,
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
            Effet = EffetType.Engluage              // Englue la cible dans une mélasse noire, elle a 1 chance sur 2 ne peut d'attaquer (pile ou face) jusqu'à la fin du combat.
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
                confirmation = ChoixOuiNon("Cohibou? C'est bien toi, n'est-ce pas?");   // ChoixOuiNon(...) affiche un mini-menu Oui/Non (avec curseur) et retourne un bool : true si le joueur choisit Oui, false si Non
                break;                                                                  // Le résultat retourné est stocké dans confirmation.
            case "Sbire pétrole":
                confirmation = ChoixOuiNon("Happeur de décombres, confondeur des méandres... Es-tu bien sûr ?");
                break;
            case "Sbire suintant":
                confirmation = ChoixOuiNon("Ce sbire est vénéneux, tu es sûr de toi ?");
                break;
            case "Grêlon":
                confirmation = ChoixOuiNon("Brr, il fait frisqué ici. C'est toi?");
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
                TourJoueur(joueur, ennemi);
                if (ennemi.EstVivant())
                    TourEnnemi(ennemi, joueur);
            }
            else if(!joueurCommence)
            {
                TourEnnemi(ennemi, joueur);
                if (joueur.EstVivant())
                    TourJoueur(joueur, ennemi);
            }

            // Effets 
            AppliquerEffets(joueur);
            AppliquerEffets(ennemi);

            if (!joueur.EstVivant() || !ennemi.EstVivant())
                break;


            Console.WriteLine("\n Fin du tour");
            Console.ReadKey();
        }

    }

    static void Main()
    {
        
        while(true)
        {
            LancerJeu();
        }



    }
}