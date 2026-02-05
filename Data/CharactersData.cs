using Game;
using MiniRPG_Console_FR_Cohibou_0.Characters;

static class CharactersData
{
    public static List<Personnage> CreerTousLesPersonnages()
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

        return tousLesPersonnages;
    }
}
