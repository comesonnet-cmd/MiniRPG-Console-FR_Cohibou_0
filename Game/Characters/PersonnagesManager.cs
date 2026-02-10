using Game.Attacks;
using Game.Characters;
using Game.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Characters
{
    internal class PersonnagesManager
{
    // 1. Initialisation des personnages:

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


}
}
