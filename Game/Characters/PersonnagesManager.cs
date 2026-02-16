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
            AttaqueImprevue = AtkManager.GetAtkByName("égarement")!,
            Vitesse = 4

        };

            Cohibou.Attaques.Add(AtkManager.GetAtkByName("Feuille rouge")!);
            Cohibou.Attaques.Add(AtkManager.GetAtkByName("Feuille dorée")!);
            Cohibou.Attaques.Add(AtkManager.GetAtkByName("Jet de pomme de pin")!);
            Cohibou.Attaques.Add(AtkManager.GetAtkByName("Jet de bogue")!);

            Personnage SbirePetrole = new Personnage
        {
            Nom = "Sbire pétrole",
            Type = Element.TYPE.Penombre,
            PointsDeVie = 50,
            Vitesse = 5

        };

            SbirePetrole.Attaques.Add(AtkManager.GetAtkByName("Conversion nocturne")!);
            SbirePetrole.Attaques.Add(AtkManager.GetAtkByName("Obscure mélasse")!);
            SbirePetrole.Attaques.Add(AtkManager.GetAtkByName("Mots toxiques")!);
           

            Personnage SbireSuintant = new Personnage
        {
            Nom = "Sbire suintant",
            Type = Element.TYPE.Veneneux,
            PointsDeVie = 50,
            Vitesse = 3
        };
            
            SbireSuintant.Attaques.Add(AtkManager.GetAtkByName("Méthylène gun")!);
            SbireSuintant.Attaques.Add(AtkManager.GetAtkByName("Poche")!);
          

            Personnage Grelon = new Personnage
        {
            Nom = "Grêlon",
            Type = Element.TYPE.Givre,
            PointsDeVie = 50,
            Vitesse = 2
        };

            Grelon.Attaques.Add(AtkManager.GetAtkByName("Gelée douce")!);
            Grelon.Attaques.Add(AtkManager.GetAtkByName("Martelage")!);

        tousLesPersonnages.Add(Cohibou);
        tousLesPersonnages.Add(SbireSuintant);
        tousLesPersonnages.Add(SbirePetrole);
        tousLesPersonnages.Add(Grelon);

        return tousLesPersonnages;
    }


}
}
