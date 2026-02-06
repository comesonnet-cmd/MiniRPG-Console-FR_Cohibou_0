using Game.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Game.Elements.Element;

namespace Game.Attacks
{
    public static class AtkManager
    {
        public static readonly Attaque[] ListAttaques = new Attaque[]
        {
            // Créer les attaques 

            // 1) Attaques de Cohibou
            new()
            {
                Nom = "Feuille rouge",
                Type = Element.TYPE.Automne,
                Puissance = 0,
                Effet = Effects.EffectsManager.EFFECT.ImmuGivre       // Produit un pigment rouge protecteur qui immunise contre le givre pendant 3 tours         
            },

            new()
            {
                Nom = "Feuille dorée",
                Type = Element.TYPE.Automne,
                Puissance = 0,
                Effet = Effects.EffectsManager.EFFECT.ProtectVeneneux // Diminue de moitié la puissance des attaques vénéneuses pendant 3 tours            
            },

            new()
            {
                Nom = "Jet de pomme de pin",
                Type = Element.TYPE.Automne,
                Puissance = 5,
                Effet = Effects.EffectsManager.EFFECT.Aucun
            },

            new()
            {
                Nom = "Jet de bogue",
                Type = Element.TYPE.Automne,
                Puissance = 15,
                Effet = Effects.EffectsManager.EFFECT.Aucun
            },
       
            // 2) Attaques du Grelon
            new()
            {
                Nom = "Gelée douce",
                Type = Element.TYPE.Givre,
                Puissance = 5,
                Effet = Effects.EffectsManager.EFFECT.Aucun
            },

             new()
             {
                Nom = "Martelage",
                Type =  Element.TYPE.Penombre,
                Puissance = 5,
                Effet = Effects.EffectsManager.EFFECT.Aucun
             },

             //3) Attaques du Sbire suintant
             new()
             {
                Nom = "Méthylène gun",
                Type = Element.TYPE.Veneneux,
                Puissance = 5,
                Effet = Effects.EffectsManager.EFFECT.Empoisonnement       // Applique "Empoisonnement", inflige 5 dégâts à chaque tour jusqu'au KO  
             },

             new()
             {
                Nom = "Smog bleu",
                Type = Element.TYPE.Veneneux,
                Puissance = 10,
                Effet = Effects.EffectsManager.EFFECT.Aucun
             },

             new()
             {
                Nom = "Poche",
                Type = Element.TYPE.Veneneux,
                Puissance = 0,                          // pas de dégâts directs
                Effet = Effects.EffectsManager.EFFECT.PochetteSurprise,     // Tire à pile ou face, si pile = "Poche vide", n'inflige aucun dégât, si face = "Smog bleu", enlève 10 PV.
                TirePileFace = true,
                AttaqueBonus = AtkManager.GetAtkByName("Smog bleu")     // "Smog Bleu" (attaque à déclencher si Face)
             },

            // 4) Attaques du Sbire Pétrole
            new()
            {
                Nom = "Conversion nocturne",
                Type = Element.TYPE.Penombre,
                Puissance = 5,
                Effet = Effects.EffectsManager.EFFECT.ConversionPenombre    // Convertit la cible en type Pénombre
            },

            new()
            {
                Nom = "Obscure mélasse",
                Type = Element.TYPE.Penombre,
                Puissance = 5,
                Effet = Effects.EffectsManager.EFFECT.Engluage
            },

            //ATTAQUES IMPREVUES:

            // Attaque imprévue de Cohibou:
            new()
            {
                Nom = "égarement",
                Type = Element.TYPE.Penombre,
                Puissance = 15,
                Effet = Effects.EffectsManager.EFFECT.Aucun
            },
                                                      
        };

      public static Attaque? GetAtkByName(string nameAtk)           // le ? indique que la méthode peut soit retourner une attaque, soit null
        {
            if (ListAttaques == null || string.IsNullOrEmpty(nameAtk))
                return null;
            
            for(int i = 0; i < ListAttaques.Length; i++)
            {
                Attaque? atk = ListAttaques[i];
                
                if (atk != null && atk.Nom == nameAtk)
                {
                    return atk;
                }

            }

            return null;
        }  
        


    }
}
